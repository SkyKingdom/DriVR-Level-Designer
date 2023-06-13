using Actions;
using Interfaces;
using Mapbox.Unity;
using Saving;
using ThirdPartyAssets.QuickOutline.Scripts;
using UnityEngine;

namespace Objects
{
  [RequireComponent(typeof(Outline))]
  public class ObjectBase : MonoBehaviour, IEditorInteractable
  {
    public string ObjectName { get; private set; }
    public string PrefabName { get; private set; }
    
    public Interactable Interactable { get; private set; }
    public Playable Playable { get; private set; }
    public Path Path { get; private set; }
    private Outline _outline;
    private Transform _mTransform;
    
    public bool IsDeleted { get; private set; }
    

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

    public void OnReposition(Vector3 position)
    {
      transform.position = position;
      if (!Path) return;
      Path.HandleObjectReposition(position);
    }
    
    public void Delete()
    {
      if (Path)
      {
        Path.DeletePath();
      }
      LevelDataManager.Instance.DeregisterObject(this);
      Destroy(gameObject);
    }

    private void Hide()
    {
      if (Path)
      {
        Path.HidePath();
      }

      gameObject.SetActive(false);
    }

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

    private Vector3 _lastPosition;
    private Vector3 _lastRotation;

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

    public void OnDrag(Vector3 position)
    {
      if (SpawnManager.Instance.EditMode == EditMode.Object && !SpawnManager.Instance.PrefabToSpawn)
        transform.position = position;
    }

    public void OnDragRelease()
    {
      if (transform.position == _lastPosition) return;
      
      // Create action
      var action = new DragAction(_lastPosition, transform.position, this);
      // Add action to undo stack
      ActionRecorder.Instance.Record(action);
      
      _lastPosition = transform.position;
    }

    public void OnRotate(float angle)
    {
      if (SpawnManager.Instance.EditMode == EditMode.Object && !SpawnManager.Instance.PrefabToSpawn)
        transform.Rotate(0f, angle, 0f);
    }

    public void OnRotateRelease()
    {
      if (transform.rotation.eulerAngles == _lastRotation) return;
      
      // Create action
      var action = new RotateAction(_lastRotation, transform.rotation.eulerAngles, this);
      // Add action to undo stack
      ActionRecorder.Instance.Record(action);
      
      _lastRotation = transform.rotation.eulerAngles;
    }

    public void Select()
    {
      IsSelected = true;
      _outline.enabled = true;
      DesignerManager.Instance.SelectionManager.SelectObject(this);
      // Store last position and rotation
      var transform1 = transform;
      _lastPosition = transform1.position;
      _lastRotation = transform1.rotation.eulerAngles;
      
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
    }

    public Transform GetTransform()
    {
      return transform;
    }

    #endregion
  }
}