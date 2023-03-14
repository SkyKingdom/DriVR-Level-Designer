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
    
    [Header("Object Details")]
    [SerializeField] private TMP_InputField objectName;
    [SerializeField] private TMP_InputField objectSpeed;
    [SerializeField] private TMP_InputField objectPathStart;
    [SerializeField] private TMP_InputField objectInteractionStart;
    [SerializeField] private TMP_InputField objectInteractionEnd;
    [SerializeField] private TMP_InputField objectPovStart;
    private bool _triggerOnStart;
    private bool _alwaysInteractable;
    

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
            _selectedObject.Deselect();
        obj.Select();
        _selectedObject = obj;
        _selectedObjectType = GetObjectType(_selectedObject);
        UpdateUIBlankets(_selectedObjectType);
        LoadData();
    }

    private void DeselectObject()
    {
        if (_selectedObject == null) return;
        
        _selectedObject.Deselect();
        _selectedObject = null;
        _selectedObjectType = ObjectType.Null;
        UpdateUIBlankets(_selectedObjectType);
    }

    private void LoadData()
    {
        objectName.text = _selectedObject.objectName;
        switch (_selectedObjectType)
        {
            case ObjectType.Null:
                break;
            case ObjectType.Decorative:
                break;
            case ObjectType.Interactable:
                var interactableObject = _selectedObject as InteractableObject;
                Debug.Log(interactableObject);
                if (interactableObject != null)
                {
                    objectSpeed.text = interactableObject.Speed.ToString("F1");
                    objectPathStart.text = interactableObject.AnimationStartTime.ToString("F1");
                    objectInteractionStart.text = interactableObject.InteractionStartTime.ToString("F1");
                    objectInteractionEnd.text = interactableObject.InteractionEndTime.ToString("F1");
                }

                break;
            case ObjectType.Playable:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void SaveData()
    {
        
    }
    
    private void ClearData()
    {
        objectName.text = "";
        objectSpeed.text = "";
        objectInteractionStart.text = "";
        objectInteractionEnd.text = "";
        objectPovStart.text = "";
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
