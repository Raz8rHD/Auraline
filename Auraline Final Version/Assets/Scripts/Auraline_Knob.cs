using UnityEngine;
using FMODUnity;
using UnityEngine.EventSystems;

public class Auraline_Knob : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [Header("Hardware Link")]
    public Transform knobMesh;

    [Header("Acoustic Mapping")]
    public string fmodParamName = "MasterVolume";
    public float minAngle = -135f;
    public float maxAngle = 135f;

    [Header("Live Data")]
    [Range(0, 1)] public float volumeValue = 0.5f;

    private float  _startMouseAngle;
    private float  _startVolume;
    private Camera _mainCam;

    // ══════════════════════════════════════════════════════════════════════════

    void Start()
    {
        _mainCam = Camera.main;

        if (knobMesh == null) return;

        float startX = knobMesh.localEulerAngles.x;
        if (startX > 180f) startX -= 360f;

        volumeValue = Mathf.InverseLerp(minAngle, maxAngle, startX);
        SyncToFMOD();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _startMouseAngle = GetMouseAngle(eventData.position);
        _startVolume     = volumeValue;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float angleDelta = Mathf.DeltaAngle(_startMouseAngle, GetMouseAngle(eventData.position));
        volumeValue = Mathf.Clamp01(_startVolume + angleDelta / (maxAngle - minAngle));
        UpdateMeshRotation();
        SyncToFMOD();
    }

    private float GetMouseAngle(Vector2 screenPos)
    {
        if (_mainCam == null) return 0f;
        Vector2 dir = screenPos - (Vector2)_mainCam.WorldToScreenPoint(transform.position);
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    private void UpdateMeshRotation()
    {
        if (knobMesh == null) return;
        float angle = Mathf.Lerp(minAngle, maxAngle, volumeValue);
        knobMesh.localEulerAngles = new Vector3(angle, -90f, -90f);
    }

    private void SyncToFMOD()
    {
        RuntimeManager.StudioSystem.setParameterByName(fmodParamName, volumeValue);
    }
}