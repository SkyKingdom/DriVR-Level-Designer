using System;
using Objects;
using UnityEngine;
using UnityEngine.UI;
using User_Interface;

namespace Managers
{
    public class DesignerInterfaceManager : MonoBehaviour
    {
        [field: SerializeField, Header("Dependencies")] public TooltipManager TooltipManager { get; private set; }
        
        [Space(20)]
        [SerializeField] private GameObject canvas;
        [field: SerializeField] public LevelSaveModal SaveModal { get; private set; }
        
        #region Blankets

        [Header("Blankets")] 
        [SerializeField] private GameObject objectsDrawerBlanket;

        // Object Options Panel
        [SerializeField] private GameObject objectDetailsBlanket;
        [SerializeField] private GameObject objectPathBlanket;
        [SerializeField] private GameObject objectInteractableBlanket;
        [SerializeField] private GameObject objectPlayableBlanket;
        
        #endregion

        #region Mode Buttons
        
        [Header("Mode Buttons")]
        [SerializeField] private Button mapModeButton;
        [SerializeField] private Button editModeButton;
        [SerializeField] private Button previewModeButton;

        #endregion

        [Header("Objects"), SerializeField] private GameObject objectButtons;
        [SerializeField] private GameObject objectPanel;

        #region Path Buttons

        [Header("Path Edit Buttons")]
        [SerializeField] private GameObject editPathButton;
        [SerializeField] private GameObject stopEditPathButton;

        #endregion
        
        [Header("Map"), SerializeField] private Toggle mapToggle;
        

        private void Start()
        {
            // Subscribe to events
            DesignerManager.Instance.OnModeChange += HandleModeChange;
            DesignerManager.Instance.MapManager.OnMapStatusChange += HandleMapStatusChange;
            DesignerManager.Instance.OnEditTypeChange += HandleEditTypeChange;
        }

        // Handles the edit type change
        private void HandleEditTypeChange(EditMode oldValue, EditMode value)
        {
            switch (oldValue)
            {
                case EditMode.Object:
                    break;
                case EditMode.Path:
                    stopEditPathButton.SetActive(false);
                    break;
                case EditMode.Road:
                    objectButtons.SetActive(true);
                    objectPanel.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(oldValue), oldValue, null);
            }
            
            switch (value)
            {
                case EditMode.Object:
                    break;
                case EditMode.Path:
                    stopEditPathButton.SetActive(true);
                    break;
                case EditMode.Road:
                    objectButtons.SetActive(false);
                    objectPanel.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        // Handles the map status change
        private void HandleMapStatusChange(bool value)
        {
            mapModeButton.interactable = value;
            mapToggle.SetIsOnWithoutNotify(value);
        }

        // Handles the mode change
        private void HandleModeChange(Mode oldValue, Mode value)
        {
            // Show UI if exiting first person mode
            if (oldValue == Mode.FirstPerson)
                ShowUI();
            
            switch (value)
            {
                case Mode.View:
                    objectsDrawerBlanket.SetActive(true); // Disable object selection in view mode
                    break;
                case Mode.Edit:
                    objectsDrawerBlanket.SetActive(false); // Enable object selection in edit mode
                    break;
                case Mode.Map:
                    objectsDrawerBlanket.SetActive(true); // Disable object selection in map mode
                    break;
                case Mode.FirstPerson:
                    HideUI(); // Hide UI when entering first person mode
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        /// <summary>
        /// Updates the details panel based on the selected object.
        /// </summary>
        public void UpdateDetailsPanelBlankets(ObjectBase selectedObject)
        {
            objectDetailsBlanket.SetActive(!selectedObject);
            if (!selectedObject)
            {
                objectPathBlanket.SetActive(true);
                objectInteractableBlanket.SetActive(true);
                objectPlayableBlanket.SetActive(true);
                return;
            }
            
            objectPathBlanket.SetActive(!selectedObject.Path);
            objectInteractableBlanket.SetActive(!selectedObject.Interactable);
            objectPlayableBlanket.SetActive(!selectedObject.Playable);
        }
        
        // Hides the UI for the first person mode
        private void HideUI() => canvas.SetActive(false);

        // Shows the UI for when exiting first person mode
        public void ShowUI() => canvas.SetActive(true);

    }
}