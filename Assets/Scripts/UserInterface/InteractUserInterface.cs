using UnityEngine;

// Used to translate world coordinate to screen point coordinate.
// Allows to keep UI element next to object in interest.
public class InteractUserInterface : MonoBehaviour
{
    [Header("Follow")] 
    [SerializeField] private Transform followObject;
    [SerializeField] private Vector3 offset;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    
    private void Update()
    {
        Vector3 screenPoint = cam.WorldToScreenPoint(followObject.position + offset);
        if (transform.position != screenPoint)
            transform.position = screenPoint;
    }
}
