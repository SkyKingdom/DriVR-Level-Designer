using System;
using Objects;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Interfaces
{
    /// <summary>
    /// Interface for objects that can be interacted with in the editor
    /// </summary>
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