using System.Collections.Generic;
using Unity.Splines.Examples;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

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
    [Range(0f, 1f)]
    public float textureScaleToLength = 0.9f;
    public SplineContainer currentSpline;
    public LoftRoadBehaviour roadBehaviour;

    private void Awake()
    {
        _lmb = actionAsset.FindActionMap("Default").FindAction("LMB");
        
        _lmb.performed += OnPointerClick;
    }

    private void Start()
    {
        currentSpline.Spline.SetTangentMode(TangentMode.AutoSmooth);
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
        var pivot = cameraRenderRectTransform.pivot;
        localPoint.x = ( localPoint.x / rect.width ) + pivot.x;
        localPoint.y = ( localPoint.y / rect.height ) + pivot.x;
        var ray = camera.ViewportPointToRay(localPoint);
        if (Physics.Raycast(ray, out var hit))
        {
            points.Add(hit.point);
            currentSpline.Spline.Add(new BezierKnot(hit.point));
            float length = currentSpline.Spline.GetLength();
            roadBehaviour.UpdateTextureScale(length * textureScaleToLength);
            
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
