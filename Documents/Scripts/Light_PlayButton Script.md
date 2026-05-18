```csharp
using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class DrumPad : MonoBehaviour, IPointerDownHandler
{
    public enum FMODEffectType
    {
        None,
        PitchShift,
        ReverbAmount,
        DrawingIntensity,
        SpatialPanning,
        StereoWidth
    }

    [Header("Settings")]
    [Tooltip("Material slot index that contains the emission/glow material.")]
    public int materialIndex = 1;
    public float flashIntensity = 5f;
    [Tooltip("If true, this pad will power on when music starts. If false, it stays off permanently.")]
    public bool canPowerOn = true;

    [Header("Effect Toggle Settings")]
    [Tooltip("If set to anything other than None, this pad will toggle that effect in FMOD.")]
    public FMODEffectType effectToToggle = FMODEffectType.None;
    private bool _isEffectActive = true;

    [Header("Tutorial Glow Settings")]
    public float pulseSpeed = 4f;
    public float minPulseIntensity = 0f;
    public float maxPulseIntensity = 2f;

    private Color    _baseEmission;
    private Material _targetMat;
    private bool     _hasMat;
    private AuralineController _controller;
    private bool     _isPoweredOn;
    private bool     isPulsing = false;

    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

    void Start()
    {
        _controller = FindFirstObjectByType<AuralineController>();
        Renderer rend = GetComponent<Renderer>();
        if (rend == null || rend.materials.Length <= materialIndex) return;

        _targetMat = rend.materials[materialIndex];

        if (_targetMat.HasProperty(EmissionColorId))
        {
            _baseEmission = _targetMat.GetColor(EmissionColorId);
            _hasMat = true;

            // Start powered off
            _targetMat.DisableKeyword("_EMISSION");
            _targetMat.SetColor(EmissionColorId, Color.black);
            _isPoweredOn = false;
            
            if (effectToToggle != FMODEffectType.None && _controller != null)
            {
                _isEffectActive = _controller.GetEffectActive(effectToToggle);
            }
        }
    }

    void Update()
    {
        if (isPulsing && _hasMat)
        {
            float lerp = (Mathf.Sin(Time.time * pulseSpeed) + 1.0f) / 2.0f;
            _targetMat.SetColor(EmissionColorId, _baseEmission * Mathf.Lerp(minPulseIntensity, maxPulseIntensity, lerp));
        }

        // If not allowed to power on, or already powered on, we don't need to do anything anymore
        if (!canPowerOn || _isPoweredOn || !_hasMat || _controller == null || !_controller.MusicInstance.isValid() || !_controller.IsMachineFullyPowered) return;

        _controller.MusicInstance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE state);

        // Power on when music starts playing for the first time
        if (state == FMOD.Studio.PLAYBACK_STATE.PLAYING || state == FMOD.Studio.PLAYBACK_STATE.STARTING)
        {
            PowerOn();
        }
    }

    public void PowerOn()
    {
        if (!_hasMat || _isPoweredOn) return;
        
        _isPoweredOn = true;
        _targetMat.EnableKeyword("_EMISSION");
        UpdateColorBasedOnState();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_hasMat) return;
        if (!_isPoweredOn && !isPulsing) return;

        if (isPulsing)
        {
            _isPoweredOn = true;
            StopPulsing();
            if (_controller != null) _controller.Tutorial_OnPadPressed(this);
        }
        
        if (effectToToggle != FMODEffectType.None)
        {
            _isEffectActive = !_isEffectActive;
            if (_controller != null)
            {
                _controller.SetEffectActive(effectToToggle, _isEffectActive);
            }
        }

        CancelInvoke(nameof(ResetColor));
        Flash();
    }

    public void StartPulsing()
    {
        isPulsing = true;
        if (_hasMat) _targetMat.EnableKeyword("_EMISSION");
    }

    public void StopPulsing()
    {
        isPulsing = false;
        if (_hasMat)
        {
            if (_isPoweredOn)
            {
                UpdateColorBasedOnState();
                _targetMat.EnableKeyword("_EMISSION");
            }
            else
            {
                _targetMat.SetColor(EmissionColorId, Color.black);
                _targetMat.DisableKeyword("_EMISSION");
            }
        }
    }

    void Flash()
    {
        _targetMat.SetColor(EmissionColorId, _baseEmission * flashIntensity);
        Invoke(nameof(ResetColor), 0.1f);
    }

    void ResetColor()
    {
        if (_isPoweredOn)
            UpdateColorBasedOnState();
    }

    void UpdateColorBasedOnState()
    {
        if (effectToToggle != FMODEffectType.None && !_isEffectActive)
        {
            _targetMat.SetColor(EmissionColorId, _baseEmission * 0.2f);
        }
        else
        {
            _targetMat.SetColor(EmissionColorId, _baseEmission);
        }
    }

    void OnDestroy()
    {
        if (_targetMat != null) Destroy(_targetMat);
    }
}
```
