using System;
using Objects;
using UnityEngine;

namespace Managers
{
    public class SelectionManager : MonoBehaviour
    {
        public ObjectBase SelectedObject { get; private set; }
        
        public event Action<ObjectBase> OnObjectSelected;
        public event Action OnObjectDeselected;
        
        public void SelectObject(ObjectBase objectBase)
        {
            SelectedObject = objectBase;
            OnObjectSelected?.Invoke(SelectedObject);
        }
        
        public void DeselectObject()
        {
            SelectedObject = null;
            OnObjectDeselected?.Invoke();
        }
    }
}