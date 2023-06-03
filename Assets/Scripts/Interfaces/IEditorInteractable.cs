using Objects;
using Vector3 = UnityEngine.Vector3;

namespace Interfaces
{
    public interface IEditorInteractable
    {
        public bool IsSelected { get; }
        public void OnPointerEnter();
        public void OnPointerExit();
        
        public void OnDrag(Vector3 position);
        
        public void OnDragRelease();
        
        public void OnRotate(float angle);
        
        public void OnRotateRelease();
        
        public void Select();
        
        public void Deselect();
        
        public ObjectBase GetObject();
        
    }
}