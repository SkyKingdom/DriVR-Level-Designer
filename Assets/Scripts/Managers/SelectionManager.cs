using System;
using Actions;
using Objects;
using TransformHandles;
using UnityEngine;

namespace Managers
{
    public class SelectionManager : MonoBehaviour
    {
        #region Depenedencies

        private TransformHandleManager _transformHandleManager;

        #endregion
        
        public ObjectBase SelectedObject { get; private set; }
        
        public event Action<ObjectBase> OnObjectSelected;
        public event Action OnObjectDeselected;

        private Handle _handle;
        private TransformActionData _startTransform;

        private void Start()
        {
            _transformHandleManager = DesignerManager.Instance.TransformHandleManager;
        }

        public void SelectObject(ObjectBase objectBase)
        {
            SelectedObject = objectBase;
            var objTransform = SelectedObject.transform;
            objTransform.tag = "Selected";
            CreateHandle(objTransform);
            OnObjectSelected?.Invoke(SelectedObject);
        }

        public void DeselectObject()
        {
            RemoveHandle();
            SelectedObject.transform.tag = "Untagged";
            SelectedObject = null;
            OnObjectDeselected?.Invoke();
        }
        
        private void OnInteractionStart(Handle obj)
        {
            _startTransform = new TransformActionData(SelectedObject.transform.position, SelectedObject.transform.eulerAngles);
        }

        private void OnInteractionEnd(Handle obj)
        {
            var endTransform = new TransformActionData(SelectedObject.transform.position, SelectedObject.transform.eulerAngles);
            var action = new ObjectTransformAction(_startTransform, endTransform, SelectedObject);
            ActionRecorder.Instance.Record(action);
            _startTransform = endTransform;
        }
        
        public void UpdateHandlePosition()
        {
            RemoveHandle();
            CreateHandle(SelectedObject.transform);
        }

        private void CreateHandle(Transform t)
        {
            _handle = _transformHandleManager.CreateHandle(t);
            _handle.OnInteractionStartEvent += OnInteractionStart;
            _handle.OnInteractionEndEvent += OnInteractionEnd;
        }
        
        private void RemoveHandle()
        {
            if (_handle == null) return;
            _handle.OnInteractionStartEvent -= OnInteractionStart;
            _handle.OnInteractionEndEvent -= OnInteractionEnd;
            _transformHandleManager.RemoveTarget(SelectedObject.transform, _handle);
        }
    }
}