using System;
using Objects;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Interfaces
{
    public interface IEditorInteractable
    {
        public event Action OnObjectDeleted;
        public bool IsSelected { get; }
        public void OnPointerEnter();
        public void OnPointerExit();
        
        public void Select();
        
        public void Deselect();
        
        public Transform GetTransform();
        
    }
}