using UnityEngine;

public class Auraline_DrawHint : MonoBehaviour
{
    [Header("Animation Settings")]
    public float speed = 2f;
    public float width = 0.5f;
    public float height = 0.2f;

    private Vector3 startPos;
    private bool hasBeenDismissed = false;
    private TrailRenderer _trailRenderer;

    void Start()
    {
        startPos = transform.localPosition;
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        // Animate the pen in a small "infinity" or "zigzag" pattern
        float x = Mathf.Sin(Time.time * speed) * width;
        float y = Mathf.Cos(Time.time * speed * 0.5f) * height;
        
        transform.localPosition = startPos + new Vector3(x, y, 0);
    }

    public void ShowHint()
    {
        // Only show if the user hasn't drawn yet in this session
        if (!hasBeenDismissed)
        {
            gameObject.SetActive(true);
            
            if (_trailRenderer != null)
            {
                _trailRenderer.Clear(); // Wipes the old trail history
            }
        }
    }

    public void DismissForever()
    {
        hasBeenDismissed = true;
        gameObject.SetActive(false);
    }
}