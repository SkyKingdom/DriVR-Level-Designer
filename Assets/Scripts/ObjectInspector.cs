using Actions;
using Interfaces;
using Managers;
using Objects;
using Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInspector : MonoBehaviour
{
    #region Dependencies

    private DesignerInterfaceManager _uiManager;

    #endregion

    [SerializeField] private ObjectBase selectedObject;
    public ObjectBase SelectedObject => selectedObject;

    #region UI References

    [Header("Object Details UI")]
    [SerializeField] private TMP_InputField objectName;
    [SerializeField] private TMP_InputField objectSpeed;
    [SerializeField] private TMP_InputField objectPathStart;
    [SerializeField] private TMP_InputField objectInteractionStart;
    [SerializeField] private TMP_InputField objectInteractionEnd;
    [SerializeField] private TMP_InputField objectPovStart;
    [SerializeField] private Toggle isCorrect;
    [SerializeField] private Toggle animateOnStart;
    [SerializeField] private Toggle alwaysInteractable;

    #endregion


    private void Start()
    {
        _uiManager = DesignerManager.Instance.DesignerUIManager;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    // TODO: Refactor after implementing selection manager
    public void SelectObject(IEditorInteractable interactable)
    {
        var obj = interactable.GetTransform();
        if (selectedObject != null)
        {
            SaveData();
            selectedObject.Deselect();
            ClearData();
        }
        
        if (obj == null)
        {
            selectedObject = null;
            _uiManager.UpdateDetailsPanelBlankets(selectedObject);
            return;
        }
        
        //obj.Select();
        //selectedObject = obj;
        _uiManager.UpdateDetailsPanelBlankets(selectedObject);
        PathManager.Instance.SelectObject(selectedObject);
        LoadData();
    }

    // TODO: Refactor after implementing selection manager
    public void DeselectObject()
    {
        if (selectedObject == null) return;
        SaveData();
        ClearData();
        selectedObject.Deselect();
        selectedObject = null;
        _uiManager.UpdateDetailsPanelBlankets(selectedObject);
        PathManager.Instance.DeselectObject();
    }

    /// <summary>
    /// Loads the data of the selected object into the inspector
    /// </summary>
    private void LoadData()
    {
        objectName.text = selectedObject.ObjectName;
        if (selectedObject.Path)
        {
            objectSpeed.text = selectedObject.Path.Speed.ToString("F1");
            animateOnStart.isOn = selectedObject.Path.AnimateOnStart;
            objectPathStart.text = selectedObject.Path.AnimationStartTime.ToString("F1");
        }
        
        if (selectedObject.Interactable)
        {
            isCorrect.isOn = selectedObject.Interactable.Answer;
            objectInteractionStart.text = selectedObject.Interactable.InteractionStartTime.ToString("F1");
            objectInteractionEnd.text = selectedObject.Interactable.InteractionEndTime.ToString("F1");
            alwaysInteractable.isOn = selectedObject.Interactable.AlwaysInteractable;
        }
        
        if (selectedObject.Playable)
        {
            objectPovStart.text = selectedObject.Playable.SwitchTime.ToString("F1");
        }
        
        // Avoids the data resetting when clicking on a loaded object
        SaveData();
    }
    
    /// <summary>
    /// Saves the data of the selected object into the inspector
    /// </summary>
    public void SaveData()
    {
        selectedObject.Rename(objectName.text);
        if (selectedObject.Path)
        {
            selectedObject.Path.SetSpeed(float.Parse(objectSpeed.text));
            selectedObject.Path.SetAnimateOnStart(animateOnStart.isOn);
            selectedObject.Path.SetAnimationStartTime(float.Parse(objectPathStart.text));
        }

        if (selectedObject.Interactable)
        {
            selectedObject.Interactable.SetInteractionTime(
                float.Parse(objectInteractionStart.text),
                float.Parse(objectInteractionEnd.text));
            selectedObject.Interactable.SetAlwaysInteractable(alwaysInteractable.isOn, isCorrect.isOn);
        }

        if (selectedObject.Playable)
        {
            float time = float.Parse(objectPovStart.text);
            selectedObject.Playable.SetPlayOnStart(time < 1f);
            selectedObject.Playable.SetSwitchTime(time);

        }
    }
    
    /// <summary>
    /// Clears the data of the inspector
    /// </summary>
    private void ClearData()
    {
        objectName.text = "";
        objectSpeed.text = "";
        objectInteractionStart.text = "";
        objectInteractionEnd.text = "";
        objectPovStart.text = "";
        objectPathStart.text = "";
        isCorrect.isOn = false;
        animateOnStart.isOn = false;
        alwaysInteractable.isOn = false;
    }
    
    // TODO: Refactor
    public void DeleteObject()
    {
        if (selectedObject == null) return;
        var deleteAction = new DeleteAction(selectedObject);
        ActionRecorder.Instance.Record(deleteAction);
        DeselectObject();
    }

    /// <summary>
    /// Calls the level data manager to save the level
    /// </summary>
    public void SaveLevel() => LevelDataManager.Instance.SaveLevel();
}
