using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    [Header("Ray casting")]
    [SerializeField] private RectTransform cameraRenderRectTransform;
    [SerializeField] private Camera camera;
    
    [Header("Input")]
    [SerializeField] private InputActionAsset actionAsset;

    private InputAction _lmb;
    
    [Header("Debugging")]
    public List<Vector3> points = new();

    private void Awake()
    {
        _lmb = actionAsset.FindActionMap("Default").FindAction("LMB");
        
        _lmb.performed += OnPointerClick;
    }

    private void OnEnable()
    {
        _lmb.Enable();
    }

    private void OnDisable()
    {
        _lmb.Disable();
    }

    private void OnPointerClick(InputAction.CallbackContext callbackContext)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cameraRenderRectTransform, Mouse.current.position.ReadValue(), null, out var localPoint);
        Debug.Log(localPoint);
        var rect = cameraRenderRectTransform.rect;
        localPoint.x += cameraRenderRectTransform.pivot.x;
        localPoint.y += cameraRenderRectTransform.pivot.x;
        var ray = camera.ViewportPointToRay(localPoint);
        if (Physics.Raycast(ray, out var hit))
        {
            points.Add(hit.point);
            Debug.Log("Hit");
        }
    }

    private void OnDrawGizmos()
    {
        if (points.Count == 0) return;
        Gizmos.color = Color.red;
        foreach (var point in points)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
}
