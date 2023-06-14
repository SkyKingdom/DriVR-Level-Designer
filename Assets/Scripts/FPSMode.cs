using UnityEngine;
using UnityEngine.InputSystem;

public class FPSMode : MonoBehaviour
{
    // Needed references for enabling/disabling FPS mode
    [Header("FPS Mode")]
    [SerializeField] private GameObject sceneCamera;
    [SerializeField] private GameObject capsule;
    
    // Check if we are in FPS mode
    private bool IsInFPSMode => DesignerManager.Instance.CurrentMode == Mode.FirstPerson;
    
    [Header("Input"), SerializeField] private InputActionReference escapeAction;
    
    // Stores the previous mode so we can return to it when exiting preview mode
    private Mode previousMode; 

    private void OnEnable()
    {
        DesignerManager.Instance.OnModeChange += OnModeChange;
        escapeAction.action.Enable();
        escapeAction.action.performed += OnEscapePressed;
    }

    // Handle mode change
    private void OnModeChange(Mode oldValue, Mode value)
    {
        previousMode = oldValue;
        if (value == Mode.FirstPerson)
        {
            HandleSwitching(true);
            return;
        }
        HandleSwitching(false);
    }
    
    /// <summary>
    /// Handles switching between FPS mode and normal mode
    /// </summary>
    /// <param name="isEntering">true if entering, false if exiting</param>
    private void HandleSwitching(bool isEntering)
    {
        sceneCamera.SetActive(!isEntering); // Enable/disable scene camera
        capsule.SetActive(isEntering); // Enable/disable FPS capsule
        Cursor.visible = !isEntering; // Show/hide cursor
        Cursor.lockState = isEntering ? CursorLockMode.Locked : CursorLockMode.None; // Lock/unlock cursor
    }

    private void OnDisable()
    {
        escapeAction.action.Disable();
        escapeAction.action.performed -= OnEscapePressed;
        DesignerManager.Instance.OnModeChange -= OnModeChange;
    }

    // Exist FPS mode using the escape key
    private void OnEscapePressed(InputAction.CallbackContext obj)
    {
        if (!IsInFPSMode)
            return;
        ExitPreview();
    }

    // Exit FPS mode
    private void ExitPreview()
    {
        DesignerManager.Instance.SetMode((int)previousMode);
    }

}
