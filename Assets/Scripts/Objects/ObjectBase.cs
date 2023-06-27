using System;
using Interfaces;
using Saving;
using ThirdPartyAssets.QuickOutline.Scripts;
using UnityEngine;

namespace Objects
{
  /// <summary>
  /// Base class for all objects.
  /// </summary>
  [RequireComponent(typeof(Outline))]
  public class ObjectBase : MonoBehaviour, IEditorInteractable
  {
    public string ObjectName { get; private set; } // The name of the object.
    public string PrefabName { get; private set; } // The name of the prefab. Used for saving and loading the correct prefab.
    
    public Interactable Interactable { get; private set; } // The interactable component.
    public Playable Playable { get; private set; } // The playable component.
    public Path Path { get; private set; } // The path component.
    
    private Outline _outline; // The outline component.
    private Transform _mTransform; // The transform component.
    
    public bool IsDeleted { get; private set; } // Whether the object is marked for deletion.
    
    public event Action OnObjectDeleted; // Event for when the object is deleted.
    
    /// <summary>
    /// Initialize the object.
    /// </summary>
    /// <param name="objectName">Object name that appears in object inspector</param>
    /// <param name="prefabName">Prefab name</param>
    /// <param name="select">Whether to spawn object as selected or not</param>
    public void Initialize(string objectName, string prefabName, bool select = true)
    {
      ObjectName = objectName;
      PrefabName = prefabName;
      SetupComponents();
      if (Interactable != null)
      {
        Interactable.Initialize(this);
      }
      if (Playable != null)
      {
        Playable.Initialize(this);
      }
      if (Path != null)
      {
        Path.Initialize(this);
        Path.SetPath(PathManager.Instance.GetNewPath());
        Path.Spawn(select);
      }
      if (select)
        Select();
      
      RegisterSelf();
    }
    
    private void SetupComponents()
    {
      _mTransform = transform;
      // Setup outline
      _outline = GetComponent<Outline>();
      _outline.OutlineColor = Color.yellow;
      _outline.OutlineWidth = 10f;
      _outline.OutlineMode = Outline.Mode.OutlineVisible;
      _outline.enabled = false;
      
      // Try and get components
      Interactable = GetComponent<Interactable>();
      Playable = GetComponent<Playable>();
      Path = GetComponent<Path>();
    }

    /// <summary>
    /// Register the object with the level data manager for saving and loading.
    /// </summary>
    private void RegisterSelf()
    {
      if (LevelDataManager.Instance)
          LevelDataManager.Instance.RegisterObject(this);
    }

    public Vector3 GetPosition() => _mTransform.position;
    
    public Vector3 GetRotation() => _mTransform.rotation.eulerAngles;
    
    public void SetRotation(Vector3 rotation)
    {
      _mTransform.rotation = Quaternion.Euler(rotation);
    }
    
    public void Rename(string newName)
    {
      ObjectName = newName;
    }
    
    /// <summary>
    /// Sets the object as deleted or not.
    /// </summary>
    public void SetDeleted(bool deleted)
    {
      IsDeleted = deleted;
      if (deleted)
      {
        Hide();
      }
      else
      {
        Show();
      }
    }

    public void SetPosition(Vector3 position)
    {
      transform.position = position;
      if (!Path) return;
      
      // Updates the position of the first path point to the object's position.
      Path.HandleObjectReposition(position);
    }
    
    /// <summary>
    /// Destroy the object as part of the spawn undo action.
    /// </summary>
    public void Delete()
    {
      if (IsSelected && DesignerManager.Instance.InEditType(EditMode.Path))
      {
        DesignerManager.Instance.SetEditMode((int)EditMode.Object);
      }
      OnObjectDeleted?.Invoke();
      if (Path)
      {
        Path.DeletePath();
      }
      LevelDataManager.Instance.DeregisterObject(this);
      OnObjectDeleted = null;
      Destroy(gameObject);
    }

    /// <summary>
    /// Hides the object mesh and path.
    /// </summary>
    private void Hide()
    {
      Deselect();
      if (Path)
      {
        Path.HidePath();
      }

      gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows the object mesh and path.
    /// </summary>
    private void Show()
    {
      if (Path)
      {
        Path.ShowPath();
      }
      gameObject.SetActive(true);
    }

    #region Editor Interactions
    
    public bool IsSelected { get; private set; }

    public void OnPointerEnter()
    {
      if (IsSelected) return;
      _outline.enabled = true;
    }

    public void OnPointerExit()
    {
      if (IsSelected) return;
      _outline.enabled = false;
    }

    public void Select()
    {
      IsSelected = true;
      _outline.enabled = true;
      DesignerManager.Instance.SelectionManager.SelectObject(this);

      if (Path)
      {
        Path.HighlightPath(null);
      }
    }
    
    public void Deselect()
    {
      IsSelected = false;
      _outline.enabled = false;
      if (Path)
      {
        Path.UnhighlightPath(null);
      }
      if (DesignerManager.Instance.SelectionManager.SelectedObject == this)
        DesignerManager.Instance.SelectionManager.DeselectObject();
    }

    public Transform GetTransform()
    {
      return transform;
    }

    #endregion
  }
}