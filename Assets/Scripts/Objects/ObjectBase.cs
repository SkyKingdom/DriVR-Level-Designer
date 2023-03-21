using Actions;
using UnityEngine;

namespace Objects
{
  public class ObjectBase : MonoBehaviour
  {
    public string objectName;
    public bool isDeleted;
    private Outline _outline;
    public Vector3 GetPosition()
    {
      return transform.position;
    }
  
    public Vector3 GetRotation()
    {
      return transform.rotation.eulerAngles;
    }
    public void SetPosition(Vector3 position)
    {
      transform.position = position;
    }
  
    public void SetRotation(Quaternion rotation)
    {
      transform.rotation = rotation;
    }

    public virtual void OnSpawn()
    {
      
    }
    
    public virtual void OnReposition()
    {
      
    }

    public virtual void Select()
    {
      if (_outline == null)
      {
        _outline = gameObject.AddComponent<Outline>();
        _outline.OutlineColor = Color.yellow;
        _outline.OutlineWidth = 10f;
        _outline.OutlineMode = Outline.Mode.OutlineVisible;
      }
      
      _outline.enabled = true;
      // TODO: Subscribing to events
    }
    
    public virtual void Deselect()
    {
      _outline.enabled = false;
      // TODO: Unsubscribing from events
    }

    public void Delete()
    {
      var deleteAction = new DeleteAction(this);
      ActionRecorder.Instance.Record(deleteAction);
    }
  }
}