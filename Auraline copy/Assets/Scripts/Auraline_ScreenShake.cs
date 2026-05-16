using UnityEngine;
using System;

public class Auraline_ScreenShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [Tooltip("Max positional displacement on a beat hit.")]
    public float shakeIntensity = 0.08f;

    [Tooltip("How long the position shake lasts (seconds).")]
    public float shakeDuration = 0.06f;

    [Tooltip("Scale punch multiplier on a beat hit.")]
    public float scalePunch = 0.12f;

    [Tooltip("How fast scale lerps back to rest.")]
    public float scaleReturnSpeed = 18f;

    [Tooltip("How fast position lerps back to rest.")]
    public float posReturnSpeed = 22f;

    [Header("FMOD Link")]
    [Tooltip("Drag the AuralineController GameObject here.")]
    public AuralineController auralineController;

    // ── Internal ──────────────────────────────────────────────────────────────
    private Vector3 _originalPos;
    private Vector3 _originalScale;
    private float   _shakeTimer;

    // Written on the FMOD native thread, consumed safely on the main thread
    private volatile bool _beatFired;

    private FMOD.Studio.EVENT_CALLBACK _beatCallback;
    private FMOD.Studio.EventInstance  _registeredInstance;

    // Static bridge — FMOD callbacks are static, this lets them reach the instance
    private static Auraline_ScreenShake _instance;

    // FMOD 2.02+: NESTED_TIMELINE_BEAT bubbles beat events from nested events
    // up to the master instance. Defined as a raw cast for version safety.
    private const FMOD.Studio.EVENT_CALLBACK_TYPE NESTED_BEAT =
        (FMOD.Studio.EVENT_CALLBACK_TYPE)0x00040000;

    private FMOD.Studio.EVENT_CALLBACK_TYPE CallbackMask =>
        FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | NESTED_BEAT;

    // ══════════════════════════════════════════════════════════════════════════

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance     = this;
        _beatCallback = new FMOD.Studio.EVENT_CALLBACK(OnBeat);
    }

    void Start()
    {
        _originalPos   = transform.localPosition;
        _originalScale = transform.localScale;
    }

    void Update()
    {
        // Re-register whenever the music instance changes (e.g. after NextTrack)
        if (auralineController != null)
        {
            var live = auralineController.MusicInstance;

            if (live.isValid() && live.handle != _registeredInstance.handle)
            {
                if (_registeredInstance.isValid())
                    _registeredInstance.setCallback(null, CallbackMask);

                live.setCallback(_beatCallback, CallbackMask);
                _registeredInstance = live;
            }
        }

        // Consume the beat flag set by the FMOD thread
        if (_beatFired)
        {
            _beatFired  = false;
            _shakeTimer = shakeDuration;
            transform.localScale = _originalScale * (1f + scalePunch);
        }

        // Position shake
        if (_shakeTimer > 0f)
        {
            transform.localPosition = _originalPos + UnityEngine.Random.insideUnitSphere * shakeIntensity;
            _shakeTimer -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition, _originalPos,
                Time.deltaTime * posReturnSpeed);
        }

        // Lerp scale back to rest
        transform.localScale = Vector3.Lerp(
            transform.localScale, _originalScale,
            Time.deltaTime * scaleReturnSpeed);
    }

    // Runs on a native FMOD thread — keep minimal, no Unity API calls
    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    private static FMOD.RESULT OnBeat(
        FMOD.Studio.EVENT_CALLBACK_TYPE type,
        IntPtr instancePtr,
        IntPtr parameterPtr)
    {
        if (_instance != null)
            _instance._beatFired = true;

        return FMOD.RESULT.OK;
    }

    void OnDestroy()
    {
        if (_registeredInstance.isValid())
            _registeredInstance.setCallback(null, CallbackMask);

        if (_instance == this)
            _instance = null;
    }
}