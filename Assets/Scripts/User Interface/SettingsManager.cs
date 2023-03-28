using System;
using System.Collections.Generic;
using Actions;
using Objects;
using Saving;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    //0 name
    //1 path
    //2 interaction
    //3 pov
    [SerializeField] private List<GameObject> blankets;
    private ObjectBase _selectedObject;
    public ObjectBase SelectedObject => _selectedObject;
    
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
    

    private void OnEnable()
    {
        InputManager.Instance.OnObjectSelect += SelectObject;
        InputManager.Instance.OnObjectDeselect += DeselectObject;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnObjectSelect -= SelectObject;
        InputManager.Instance.OnObjectDeselect -= DeselectObject;
    }

    public void SelectObject(ObjectBase obj)
    {
        if (_selectedObject != null)
        {
            SaveData();
            _selectedObject.Deselect();
            ClearData();
        }

        obj.Select();
        _selectedObject = obj;
        UpdateUIBlankets(_selectedObject);
        PathManager.Instance.SelectObject(_selectedObject);
        LoadData();
    }

    public void DeselectObject()
    {
        if (_selectedObject == null) return;
        SaveData();
        ClearData();
        _selectedObject.Deselect();
        _selectedObject = null;
        UpdateUIBlankets(_selectedObject);
        PathManager.Instance.DeselectObject();
    }

    private void LoadData()
    {
        objectName.text = _selectedObject.ObjectName;
        if (_selectedObject.Path)
        {
            objectSpeed.text = _selectedObject.Path.Speed.ToString("F1");
            animateOnStart.isOn = _selectedObject.Path.AnimateOnStart;
            objectPathStart.text = _selectedObject.Path.AnimationStartTime.ToString("F1");
        }
        
        if (_selectedObject.Interactable)
        {
            isCorrect.isOn = _selectedObject.Interactable.Answer;
            objectInteractionStart.text = _selectedObject.Interactable.InteractionStartTime.ToString("F1");
            objectInteractionEnd.text = _selectedObject.Interactable.InteractionEndTime.ToString("F1");
            alwaysInteractable.isOn = _selectedObject.Interactable.AlwaysInteractable;
        }
        
        if (_selectedObject.Playable)
        {
            objectPovStart.text = _selectedObject.Playable.SwitchViewTime.ToString("F1");
        }
        SaveData();
    }
    
    public void SaveData()
    {
        _selectedObject.Rename(objectName.text);
        if (_selectedObject.Path)
        {
            _selectedObject.Path.SetSpeed(float.Parse(objectSpeed.text));
            _selectedObject.Path.SetAnimateOnStart(animateOnStart.isOn);
            _selectedObject.Path.SetAnimationStartTime(float.Parse(objectPathStart.text));
        }

        if (_selectedObject.Interactable)
        {
            _selectedObject.Interactable.SetInteractionValues(
                isCorrect.isOn,
                float.Parse(objectInteractionStart.text),
                float.Parse(objectInteractionEnd.text));
            _selectedObject.Interactable.SetAlwaysInteractable(alwaysInteractable.isOn, isCorrect.isOn);
        }

        if (_selectedObject.Playable)
        {
            float time = float.Parse(objectPovStart.text);
            _selectedObject.Playable.SetPlayOnStart(time < 1f);
            _selectedObject.Playable.SetViewValues(time);

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

    
    private void UpdateUIBlankets(ObjectBase obj)
    {
        blankets[0].SetActive(!obj);
        if (obj)
        {
            blankets[1].SetActive(!obj.Path);
            blankets[2].SetActive(!obj.Interactable);
            blankets[3].SetActive(!obj.Playable);
        }
        else
        {
            blankets[1].SetActive(!obj);
            blankets[2].SetActive(!obj);
            blankets[3].SetActive(!obj);
        }
    }
    
    public void DeleteObject()
    {
        if (_selectedObject == null) return;
        var deleteAction = new DeleteAction(_selectedObject);
        ActionRecorder.Instance.Record(deleteAction);
        DeselectObject();
    }

    public void SaveLevel()
    {
        LevelDataManager.Instance.SaveLevel();
    }
}
