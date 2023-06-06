using System;
using Objects;
using UnityEngine;
using UnityEngine.UI;
using User_Interface;

namespace Managers
{
    public class DesignerInterfaceManager : MonoBehaviour
    {
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
        
        [field: SerializeField, Space(20)] public LevelSaveModal SaveModal { get; private set; }
        [SerializeField] private Toggle mapToggle;
        

        private void Start()
        {
            DesignerManager.Instance.OnModeChange += HandleModeChange;
            DesignerManager.Instance.MapManager.OnMapStatusChange += HandleMapStatusChange;
        }

        private void HandleMapStatusChange(bool value)
        {
            mapModeButton.interactable = value;
            mapToggle.SetIsOnWithoutNotify(value);
        }

        private void HandleModeChange(Mode oldValue, Mode value)
        {
            switch (value)
            {
                case Mode.View:
                    objectsDrawerBlanket.SetActive(true);
                    break;
                case Mode.Edit:
                    objectsDrawerBlanket.SetActive(false);
                    break;
                case Mode.Map:
                    objectsDrawerBlanket.SetActive(true);
                    break;
                case Mode.FirstPerson:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        /// <summary>
        /// Updates the details panel based on the selected object.
        /// </summary>
        /// <param name="selectedObject"></param>
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
    }
}