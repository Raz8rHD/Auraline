using UnityEngine;

public class Auraline_ButtonGlow : MonoBehaviour
{
    public enum ButtonState { Initial, Playing, Paused }
    private ButtonState currentState = ButtonState.Initial;

    [System.Serializable]
    public struct GlowSettings
    {
        [ColorUsage(true, true)] public Color color;
        public float pulseSpeed;
        public float minIntensity;
        public float maxIntensity;
    }

    [Header("State Settings")]
    public GlowSettings initialSettings = new GlowSettings { color = new Color(1f, 0f, 1f, 1f), pulseSpeed = 1.0f, minIntensity = 0.5f, maxIntensity = 2.5f };
    public GlowSettings playingSettings = new GlowSettings { color = Color.green,  pulseSpeed = 2.5f, minIntensity = 0.8f, maxIntensity = 3.0f };
    public GlowSettings pausedSettings  = new GlowSettings { color = Color.yellow, pulseSpeed = 0.2f, minIntensity = 1.2f, maxIntensity = 1.5f };

    private Material buttonMat;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError($"[Auraline_ButtonGlow] No Renderer found on '{gameObject.name}'. Disabling script.", this);
            enabled = false;
            return;
        }

        // Create a per-instance material copy and track it for cleanup.
        buttonMat = rend.material;
        buttonMat.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        if (buttonMat == null) return;

        GlowSettings settings = GetCurrentSettings();

        float lerp             = (Mathf.Sin(Time.time * settings.pulseSpeed) + 1.0f) / 2.0f;
        float currentIntensity = Mathf.Lerp(settings.minIntensity, settings.maxIntensity, lerp);

        buttonMat.SetColor(EmissionColor, settings.color * currentIntensity);
    }

    void OnDestroy()
    {
        // Release the instanced material to avoid GPU memory leaks in both
        // the Editor and standalone builds.
        if (buttonMat != null)
            Destroy(buttonMat);
    }

    private GlowSettings GetCurrentSettings()
    {
        return currentState switch
        {
            ButtonState.Playing => playingSettings,
            ButtonState.Paused  => pausedSettings,
            _                   => initialSettings
        };
    }

    public void UpdateVisuals(ButtonState newState)
    {
        currentState = newState;
    }
}