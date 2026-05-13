using UnityEngine;
using FMODUnity;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class AuralineController : MonoBehaviour, IPointerDownHandler
{
    [Header("Hardware Links")]
    public EventReference fmodEvent;
    public Transform islandBG;
    public Transform screenCursor;
    public Collider screenCollider;
    public LineRenderer drawingLine;

    [Header("Hardware Settings")]
    public Transform[] bars;
    public float waveSpeed = 10f;
    public float smoothSpeed = 12f;

    [Header("Drawing & Flow")]
    public float surfaceLift = 0.02f;
    [Tooltip("Maximum number of active strokes to prevent performance drops.")]
    public int maxStrokes = 20;
    public Auraline_DrawHint drawHint;
    private bool userHasStartedDrawing = false;

    [Header("Live Modulations")]
    [Range(0, 1)] public float reverbLevel = 0f;
    [Range(-12, 12)] public float pitchLevel = 0f;
    public float drawingIntensity = 0f;
    public float spatialPanning = 0.5f;
    public float stereoWidth = 0f;

    [Header("Effect Toggles")]
    public bool pitchEnabled = false;
    public bool reverbEnabled = false;
    public bool drawingIntensityEnabled = false;
    public bool spatialPanningEnabled = false;
    public bool stereoWidthEnabled = false;

    [Header("Visual Feedback")]
    public Auraline_ButtonGlow playButtonGlow;
    
    [System.Serializable]
    public struct TutorialPadConfig
    {
        public DrumPad pad;
        [Tooltip("Delay in seconds before this pad starts pulsing AFTER the previous pad was clicked.")]
        public float delay;
    }

    [Header("Tutorial Feedback")]
    public DrumPad tutorialResetPad;
    public DrumPad tutorialNextTrackPad;
    [Tooltip("Delay in seconds before the next track pad lights up after pressing the reset pad.")]
    public float tutorialDelay = 2.0f;
    [Tooltip("Sequential list of modulation pads. The next pad only pulses after the previous is clicked.")]
    public TutorialPadConfig[] tutorialModulationPads;

    private int tutorialState = 0; // 0 = waiting to draw, 1 = prompted reset, 2 = prompted next track, 3 = seq prompting mod pads, 4 = done
    private bool _isWaitingForNextTrack = false;
    private int _currentModPadIndex = 0;
    private bool _isWaitingForModPad = false;

    [Tooltip("5.0x multiplier for aggressive distortion response.")]
    public float velocitySensitivity = 5.0f;

    [Header("Startup Sequence")]
    public Light mainSceneLight;
    public float startupDelay = 5f;
    public float lightFadeDuration = 2f;
    
    private float _originalLightIntensity;
    private float _originalAmbientIntensity;
    private float _startupTimer = 0f;
    private bool _isFadingLight = false;
    private bool _hasStartedBootup = false;
    public bool IsMachineFullyPowered { get; private set; } = false;

    // Private state
    private FMOD.Studio.EventInstance musicInstance;
    private bool isPlaying;
    private int currentTrackIndex;
    private float pulse;
    private Vector3 lastFrameMousePos;
    private bool isNewStroke = true;
    private Camera _mainCam;

    private List<LineRenderer> allStrokes = new List<LineRenderer>();
    private LineRenderer currentStroke;

    /// <summary>
    /// Exposes the live FMOD event instance so other systems (e.g. Auraline_ScreenShake)
    /// can register callbacks that survive track switches.
    /// </summary>
    public FMOD.Studio.EventInstance MusicInstance => musicInstance;

    // ══════════════════════════════════════════════════════════════════════════

    void Start()
    {
        _mainCam = Camera.main;
        
        // Prevent tutorial pads from turning on automatically when music starts
        if (tutorialResetPad != null) tutorialResetPad.canPowerOn = false;
        if (tutorialNextTrackPad != null) tutorialNextTrackPad.canPowerOn = false;
        
        // Bootup Sequence initialization
        _originalAmbientIntensity = RenderSettings.ambientIntensity;
        RenderSettings.ambientIntensity = 0f;

        if (mainSceneLight != null)
        {
            _originalLightIntensity = mainSceneLight.intensity;
            mainSceneLight.intensity = 0f;
        }

        if (!fmodEvent.IsNull)
        {
            musicInstance = RuntimeManager.CreateInstance(fmodEvent);
            musicInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        }

        if (screenCursor != null) screenCursor.gameObject.SetActive(false);
        if (drawingLine != null)
        {
            drawingLine.positionCount = 0;
            drawingLine.enabled = false;
        }

        SetBarsVisible(false);
    }

    void Update()
    {
        // Handle quitting the application
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
            return;
        }

        // Startup sequence logic
        if (_hasStartedBootup && !IsMachineFullyPowered)
        {
            if (_startupTimer > 0)
            {
                _startupTimer -= Time.deltaTime;
                if (_startupTimer <= 0)
                {
                    _isFadingLight = true;
                }
            }
            
            if (_isFadingLight)
            {
                bool lightDone = true;
                bool ambientDone = true;

                if (_originalAmbientIntensity > 0)
                {
                    RenderSettings.ambientIntensity += (_originalAmbientIntensity / lightFadeDuration) * Time.deltaTime;
                    if (RenderSettings.ambientIntensity >= _originalAmbientIntensity)
                        RenderSettings.ambientIntensity = _originalAmbientIntensity;
                    else
                        ambientDone = false;
                }

                if (mainSceneLight != null && _originalLightIntensity > 0)
                {
                    mainSceneLight.intensity += (_originalLightIntensity / lightFadeDuration) * Time.deltaTime;
                    if (mainSceneLight.intensity >= _originalLightIntensity)
                        mainSceneLight.intensity = _originalLightIntensity;
                    else
                        lightDone = false;
                }

                if (lightDone && ambientDone)
                    CompleteBootup();
            }
        }

        if (musicInstance.isValid())
            musicInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

        if (!isPlaying) return;

        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
            isNewStroke = true;

        if (Pointer.current != null && Pointer.current.press.isPressed)
        {
            CalculateVelocity();
            HandleTouch();
        }
        else
        {
            drawingIntensity = Mathf.Lerp(drawingIntensity, 0f, Time.deltaTime * 5f);
            if (screenCursor != null) screenCursor.gameObject.SetActive(false);

            // Tutorial Step 1: User has drawn something and released their finger
            if (userHasStartedDrawing && tutorialState == 0)
            {
                tutorialState = 1;
                if (tutorialResetPad != null) tutorialResetPad.StartPulsing();
            }
        }

        // Sync FMOD parameters every frame while playing
        musicInstance.setParameterByName("PitchShift",       pitchEnabled ? pitchLevel : 0f);
        musicInstance.setParameterByName("ReverbAmount",     reverbEnabled ? reverbLevel : 0f);
        musicInstance.setParameterByName("DrawingIntensity", drawingIntensityEnabled ? drawingIntensity : 0f);
        musicInstance.setParameterByName("SpatialPanning",   spatialPanningEnabled ? spatialPanning : 0.5f);
        musicInstance.setParameterByName("StereoWidth",      stereoWidthEnabled ? stereoWidth : 0f);

        pulse = 0.5f + Mathf.PingPong(Time.time * 4f, 0.5f);
        AnimateIsland();
    }

    void CalculateVelocity()
    {
        Vector3 currentMousePos = Pointer.current.position.ReadValue();
        float distance     = Vector3.Distance(currentMousePos, lastFrameMousePos);
        
        // Prevent division by zero if Time.deltaTime is extremely small or zero
        float dt = Mathf.Max(Time.deltaTime, 0.0001f);
        
        float currentSpeed = (distance / (dt * 1000f)) * velocitySensitivity;
        drawingIntensity   = Mathf.Clamp01(Mathf.Lerp(drawingIntensity, currentSpeed, dt * 8f));
        lastFrameMousePos  = currentMousePos;
    }

    void HandleTouch()
    {
        if (_mainCam == null) return;

        Vector2 pointerPos = Pointer.current.position.ReadValue();
        Ray ray = _mainCam.ScreenPointToRay(pointerPos);

        if (!Physics.Raycast(ray, out RaycastHit hit) || hit.collider != screenCollider) return;

        if (drawingLine != null)
        {
            if (isNewStroke)
            {
                // Create a new empty GameObject to avoid cloning other components
                GameObject strokeObj = new GameObject("Stroke_" + allStrokes.Count);
                strokeObj.transform.SetParent(drawingLine.transform.parent, false);
                strokeObj.transform.localPosition = drawingLine.transform.localPosition;
                strokeObj.transform.localRotation = drawingLine.transform.localRotation;
                strokeObj.transform.localScale = drawingLine.transform.localScale;
                
                currentStroke = strokeObj.AddComponent<LineRenderer>();
                
                // Copy essential LineRenderer properties from the template
                currentStroke.sharedMaterials = drawingLine.sharedMaterials;
                currentStroke.colorGradient = drawingLine.colorGradient;
                currentStroke.widthCurve = drawingLine.widthCurve;
                currentStroke.widthMultiplier = drawingLine.widthMultiplier;
                currentStroke.numCapVertices = drawingLine.numCapVertices;
                currentStroke.numCornerVertices = drawingLine.numCornerVertices;
                currentStroke.useWorldSpace = drawingLine.useWorldSpace;
                currentStroke.alignment = drawingLine.alignment;
                currentStroke.textureMode = drawingLine.textureMode;
                currentStroke.shadowCastingMode = drawingLine.shadowCastingMode;
                currentStroke.receiveShadows = drawingLine.receiveShadows;
                currentStroke.sortingLayerID = drawingLine.sortingLayerID;
                currentStroke.sortingOrder = drawingLine.sortingOrder;
                
                currentStroke.positionCount = 0;
                currentStroke.enabled = true;
                
                allStrokes.Add(currentStroke);
                
                // Enforce max strokes to prevent memory/performance issues
                if (allStrokes.Count > maxStrokes)
                {
                    LineRenderer oldestStroke = allStrokes[0];
                    allStrokes.RemoveAt(0);
                    if (oldestStroke != null && oldestStroke != drawingLine)
                    {
                        Destroy(oldestStroke.gameObject);
                    }
                }
                
                isNewStroke = false;
            }

            if (currentStroke != null)
            {
                currentStroke.enabled = true;
                Vector3 localHitPos = transform.InverseTransformPoint(hit.point);
                currentStroke.positionCount++;
                currentStroke.SetPosition(currentStroke.positionCount - 1, localHitPos + new Vector3(0f, surfaceLift, 0f));
            }
        }

        if (screenCursor != null)
        {
            screenCursor.gameObject.SetActive(true);
            screenCursor.position = hit.point + hit.normal * 0.005f;
        }

        Bounds b   = screenCollider.bounds;
        float xPct = Mathf.Clamp01((hit.point.x - b.min.x) / b.size.x);
        float yPct = Mathf.Clamp01((hit.point.y - b.min.y) / b.size.y);

        spatialPanning = xPct;
        stereoWidth    = Mathf.Abs(xPct - 0.5f) * 2f;
        pitchLevel     = Mathf.Lerp(-12f, 12f, xPct);
        reverbLevel    = Mathf.Lerp(0f, 1f, yPct);

        if (hit.collider == screenCollider) 
        {
            // The user touched the screen!
            if (!userHasStartedDrawing)
            {
                userHasStartedDrawing = true;
                if (drawHint != null) drawHint.DismissForever();
            }
        }
    }

    public void NextTrack()
    {
        if (!IsMachineFullyPowered || tutorialState < 2 || _isWaitingForNextTrack) return;

        currentTrackIndex = (currentTrackIndex + 1) % 4;
        RuntimeManager.StudioSystem.setParameterByName("TrackSelector", (float)currentTrackIndex);
    }

    public void TogglePlayback()
    {
        if (!musicInstance.isValid()) return;

        // Block interaction while booting up
        if (_hasStartedBootup && !IsMachineFullyPowered) return;

        musicInstance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE state);
        musicInstance.getPaused(out bool isPaused);

        // Resume from stopped or paused → Playing (Green)
        if (state == FMOD.Studio.PLAYBACK_STATE.STOPPED || isPaused)
        {
            if (state == FMOD.Studio.PLAYBACK_STATE.STOPPED) 
            {
                if (!_hasStartedBootup)
                {
                    _hasStartedBootup = true;
                    _startupTimer = startupDelay;
                    return; // Stop here, CompleteBootup() will start the music later
                }
                
                musicInstance.start();
            }

            musicInstance.setPaused(false);
            StartPlaybackVisuals();
        }
        else
        {
            musicInstance.setPaused(true);
            isPlaying = false;
            SetBarsVisible(true);

            if (playButtonGlow != null)
                playButtonGlow.UpdateVisuals(Auraline_ButtonGlow.ButtonState.Paused);
        }
    }

    private void StartPlaybackVisuals()
    {
        isPlaying = true;
        SetBarsVisible(true);
        if (drawHint != null && !userHasStartedDrawing) 
            drawHint.ShowHint();

        if (playButtonGlow != null)
            playButtonGlow.UpdateVisuals(Auraline_ButtonGlow.ButtonState.Playing);
    }

    private void CompleteBootup()
    {
        _isFadingLight = false;
        IsMachineFullyPowered = true;
        
        if (musicInstance.isValid())
        {
            musicInstance.start();
            musicInstance.setPaused(false);
        }
        
        StartPlaybackVisuals();
    }

    public void ResetModulations()
    {
        if (!IsMachineFullyPowered || tutorialState < 1) return;

        pitchLevel     = 0f;
        reverbLevel    = 0f;
        drawingIntensity = 0f;
        spatialPanning = 0.5f;
        stereoWidth    = 0f;

        if (musicInstance.isValid())
        {
            musicInstance.setParameterByName("PitchShift",       0f);
            musicInstance.setParameterByName("ReverbAmount",     0f);
            musicInstance.setParameterByName("DrawingIntensity", 0f);
            musicInstance.setParameterByName("SpatialPanning",   0.5f);
            musicInstance.setParameterByName("StereoWidth",      0f);
        }

        if (drawingLine != null)
        {
            drawingLine.positionCount = 0;
            drawingLine.enabled = false;
        }

        foreach (var stroke in allStrokes)
        {
            if (stroke != null && stroke != drawingLine)
            {
                Destroy(stroke.gameObject);
            }
        }
        allStrokes.Clear();
        currentStroke = null;

        isNewStroke = true;
    }

    public void Tutorial_OnPadPressed(DrumPad pad)
    {
        if (tutorialState == 1 && pad == tutorialResetPad)
        {
            tutorialState = 2;
            _isWaitingForNextTrack = true;
            if (tutorialNextTrackPad != null)
            {
                StartCoroutine(DelayedNextTrackPulsing());
            }
        }
        else if (tutorialState == 2 && pad == tutorialNextTrackPad && !_isWaitingForNextTrack)
        {
            tutorialState = 3;
            _currentModPadIndex = 0;
            if (tutorialModulationPads != null && tutorialModulationPads.Length > 0)
            {
                TriggerNextModulationPad();
            }
            else
            {
                tutorialState = 4;
            }
        }
        else if (tutorialState == 3 && !_isWaitingForModPad && tutorialModulationPads != null)
        {
            if (_currentModPadIndex < tutorialModulationPads.Length && pad == tutorialModulationPads[_currentModPadIndex].pad)
            {
                _currentModPadIndex++;
                if (_currentModPadIndex < tutorialModulationPads.Length)
                {
                    TriggerNextModulationPad();
                }
                else
                {
                    tutorialState = 4;
                }
            }
        }
    }

    private void TriggerNextModulationPad()
    {
        _isWaitingForModPad = true;
        StartCoroutine(DelayedModPadPulsing(_currentModPadIndex));
    }

    private System.Collections.IEnumerator DelayedModPadPulsing(int index)
    {
        float delay = tutorialModulationPads[index].delay;
        if (delay > 0f) yield return new WaitForSeconds(delay);
        
        _isWaitingForModPad = false;
        if (tutorialModulationPads[index].pad != null)
        {
            tutorialModulationPads[index].pad.StartPulsing();
        }
    }

    private System.Collections.IEnumerator DelayedNextTrackPulsing()
    {
        yield return new WaitForSeconds(tutorialDelay);
        _isWaitingForNextTrack = false;
        if (tutorialNextTrackPad != null)
        {
            tutorialNextTrackPad.StartPulsing();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isPlaying) TogglePlayback();
    }

    void AnimateIsland()
    {
        for (int i = 0; i < bars.Length; i++)
        {
            if (bars[i] == null) continue;
            Vector3 currentScale = bars[i].localScale;
            float targetHeight = Mathf.PerlinNoise(Time.time * waveSpeed, i * 0.7f) * 1.2f * pulse;
            bars[i].localScale = Vector3.Lerp(
                currentScale,
                new Vector3(currentScale.x, targetHeight, currentScale.z),
                Time.deltaTime * smoothSpeed);
        }
    }

    void SetBarsVisible(bool visible)
    {
        if (bars != null)
            foreach (var bar in bars)
                if (bar != null) bar.gameObject.SetActive(visible);

        if (islandBG != null) islandBG.gameObject.SetActive(visible);
    }

    void OnDestroy()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicInstance.release();
        }
    }

    public void SetEffectActive(DrumPad.FMODEffectType effect, bool isActive)
    {
        switch (effect)
        {
            case DrumPad.FMODEffectType.PitchShift:
                pitchEnabled = isActive;
                break;
            case DrumPad.FMODEffectType.ReverbAmount:
                reverbEnabled = isActive;
                break;
            case DrumPad.FMODEffectType.DrawingIntensity:
                drawingIntensityEnabled = isActive;
                break;
            case DrumPad.FMODEffectType.SpatialPanning:
                spatialPanningEnabled = isActive;
                break;
            case DrumPad.FMODEffectType.StereoWidth:
                stereoWidthEnabled = isActive;
                break;
        }
    }

    public bool GetEffectActive(DrumPad.FMODEffectType effect)
    {
        switch (effect)
        {
            case DrumPad.FMODEffectType.PitchShift: return pitchEnabled;
            case DrumPad.FMODEffectType.ReverbAmount: return reverbEnabled;
            case DrumPad.FMODEffectType.DrawingIntensity: return drawingIntensityEnabled;
            case DrumPad.FMODEffectType.SpatialPanning: return spatialPanningEnabled;
            case DrumPad.FMODEffectType.StereoWidth: return stereoWidthEnabled;
            default: return false;
        }
    }
}