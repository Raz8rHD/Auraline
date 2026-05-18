```csharp
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayButtonBridge : MonoBehaviour, IPointerDownHandler
{
    [Tooltip("Drag the AuralineController GameObject here.")]
    public AuralineController controller;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (controller != null)
            controller.TogglePlayback();
    }
}
```
