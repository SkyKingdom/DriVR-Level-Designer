using UnityEngine;

namespace Objects
{
  public class ObjectBase : MonoBehaviour
  {
    public string objectName;
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

    public void Select()
    {
      // TODO: Subscribing to events
    }
    
    public void Deselect()
    {
      // TODO: Unsubscribing from events
    }
  }
}