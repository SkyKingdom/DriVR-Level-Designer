using System;
using System.Collections.Generic;
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
    
    [Header("Object Details")]
    [SerializeField] private TMP_InputField objectName;
    [SerializeField] private TMP_InputField objectSpeed;
    [SerializeField] private TMP_InputField objectPathStart;
    [SerializeField] private TMP_InputField objectInteractionStart;
    [SerializeField] private TMP_InputField objectInteractionEnd;
    [SerializeField] private TMP_InputField objectPovStart;
    [SerializeField] private Toggle isCorrect;
    [SerializeField] private Toggle animateOnStart;
    [SerializeField] private Toggle alwaysInteractable;


    private void Start()
    {
        _uiManager = DesignerManager.Instance.DesignerUIManager;
    }

    private void OnEnable()
    {
        InputHandler.Instance.OnSelect += SelectObject;
        InputHandler.Instance.OnDeselect += DeselectObject;
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnSelect -= SelectObject;
        InputHandler.Instance.OnDeselect -= DeselectObject;
    }

    public void SelectObject(IEditorInteractable interactable)
    {
        var obj = interactable.GetObject();
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
        
        obj.Select();
        selectedObject = obj;
        _uiManager.UpdateDetailsPanelBlankets(selectedObject);
        PathManager.Instance.SelectObject(selectedObject);
        LoadData();
    }

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
        SaveData();
    }
    
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
    
    public void DeleteObject()
    {
        if (selectedObject == null) return;
        var deleteAction = new DeleteAction(selectedObject);
        ActionRecorder.Instance.Record(deleteAction);
        DeselectObject();
    }

    public void SaveLevel()
    {
        LevelDataManager.Instance.SaveLevel();
    }
}
