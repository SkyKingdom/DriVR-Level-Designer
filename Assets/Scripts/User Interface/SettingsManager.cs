using System;
using System.Collections.Generic;
using Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum ObjectType
{
    Null,
    Decorative,
    Interactable,
    Playable
}

public class SettingsManager : MonoBehaviour
{
    //0 name
    //1 path
    //2 interaction
    //3 pov
    [SerializeField] private List<GameObject> blankets;
    private ObjectType _selectedObjectType;
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
        _selectedObjectType = GetObjectType(_selectedObject);
        UpdateUIBlankets(_selectedObjectType);
        LoadData();
        if (_selectedObjectType is ObjectType.Interactable or ObjectType.Playable)
        {
            PathManager.Instance.SelectObject(_selectedObject as PathObject);
        }
    }

    public void DeselectObject()
    {
        if (_selectedObject == null) return;
        SaveData();
        ClearData();
        _selectedObject.Deselect();
        _selectedObject = null;
        _selectedObjectType = ObjectType.Null;
        UpdateUIBlankets(_selectedObjectType);
        PathManager.Instance.DeselectObject();
    }

    private void LoadData()
    {
        objectName.text = _selectedObject.objectName;
        switch (_selectedObjectType)
        {
            case ObjectType.Interactable:
                var interactableObject = _selectedObject as InteractableObject;
                Debug.Log(interactableObject);
                if (interactableObject == null) return;
                objectSpeed.text = interactableObject.Speed.ToString("F1");
                objectPathStart.text = interactableObject.AnimationStartTime.ToString("F1");
                objectInteractionStart.text = interactableObject.InteractionStartTime.ToString("F1");
                objectInteractionEnd.text = interactableObject.InteractionEndTime.ToString("F1");
                isCorrect.isOn = interactableObject.Answer;
                animateOnStart.isOn = interactableObject.AnimateOnStart;
                alwaysInteractable.isOn = interactableObject.AlwaysInteractable;
                // TODO: load path
                break;
            case ObjectType.Playable:
                var playableObject = _selectedObject as PlayableObject;
                if (playableObject == null) return;
                objectSpeed.text = playableObject.Speed.ToString("F1");
                objectPathStart.text = playableObject.AnimationStartTime.ToString("F1");
                objectPovStart.text = playableObject.SwitchViewTime.ToString("F1");
                animateOnStart.isOn = playableObject.AnimateOnStart;
                // TODO: load path
                break;
        }
    }
    
    private void SaveData()
    {
        _selectedObject.objectName = objectName.text;
        switch (_selectedObjectType)
        {
            case ObjectType.Interactable:
                var interactableObject = _selectedObject as InteractableObject;
                if (interactableObject == null) return;
                if (alwaysInteractable.isOn)
                {
                    interactableObject.SetInteractionValues(isCorrect.isOn, alwaysInteractable.isOn);
                }
                else
                {
                    interactableObject.SetInteractionValues(isCorrect.isOn, float.Parse(objectInteractionStart.text),
                        float.Parse(objectInteractionEnd.text));
                }
                
                if (animateOnStart.isOn)
                {
                    interactableObject.SetAnimationValues(float.Parse(objectSpeed.text), animateOnStart.isOn);
                }
                else
                {
                    interactableObject.SetAnimationValues(float.Parse(objectSpeed.text), float.Parse(objectPathStart.text));
                }
                break;
            case ObjectType.Playable:
                var playableObject = _selectedObject as PlayableObject;
                if (playableObject == null) return;
                if (animateOnStart.isOn)
                {
                    playableObject.SetAnimationValues(float.Parse(objectSpeed.text), animateOnStart.isOn);
                }
                else
                {
                    playableObject.SetAnimationValues(float.Parse(objectSpeed.text), float.Parse(objectPathStart.text));
                }
                
                playableObject.SetViewValues(float.Parse(objectPovStart.text));
                break;
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

    private ObjectType GetObjectType(ObjectBase obj)
    {
        var type = obj.GetType();
        ObjectType objectType = ObjectType.Null;
        switch (type)
        {
            case { } t when t == typeof(DecorativeObject):
                objectType = ObjectType.Decorative;
                break;
            case { } t when t == typeof(InteractableObject):
                objectType = ObjectType.Interactable;
                break;
            case { } t when t == typeof(PlayableObject):
                objectType = ObjectType.Playable;
                break;  
        }

        return objectType;
    }

    private void UpdateUIBlankets(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Decorative:
                LoadDecorationPreset();
                break;
            case ObjectType.Interactable:
                LoadInteractablePreset();
                break;
            case ObjectType.Playable:
                LoadPlayablePreset();
                break;
            case ObjectType.Null:
                LoadDeselectPreset();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    
    private void LoadDeselectPreset()
    {
        blankets[0].SetActive(true);
        blankets[1].SetActive(true);
        blankets[2].SetActive(true);
        blankets[3].SetActive(true);
    }
    
    private void LoadDecorationPreset()
    {
        blankets[0].SetActive(false);
        blankets[1].SetActive(true);
        blankets[2].SetActive(true);
        blankets[3].SetActive(true);

    }

    private void LoadInteractablePreset()
    {
        blankets[0].SetActive(false);
        blankets[1].SetActive(false);
        blankets[2].SetActive(false);
        blankets[3].SetActive(true);
    }

    private void LoadPlayablePreset()
    {
        blankets[0].SetActive(false);
        blankets[1].SetActive(false);
        blankets[2].SetActive(true);
        blankets[3].SetActive(false);
    }
    
    public void DeleteObject()
    {
        if (_selectedObject == null) return;
        _selectedObject.Delete();
        DeselectObject();
    }
    
}
