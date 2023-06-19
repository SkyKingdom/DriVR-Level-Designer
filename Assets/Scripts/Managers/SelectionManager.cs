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
        public RoadPointContainer SelectedRoadPoint { get; private set; }
        
        public PathPointContainer SelectedPathPoint { get; private set; }

        private Transform _targetTransform;
        
        public event Action<ObjectBase> OnObjectSelected;
        public event Action OnObjectDeselected;

        private Handle _handle;
        private TransformActionData _startTransform;

        private Action<Handle> _onInteractionStart;
        private Action<Handle> _onInteractionEnd;

        private void Start()
        {
            _transformHandleManager = DesignerManager.Instance.TransformHandleManager;
        }

        private void OnEnable()
        {
            DesignerManager.Instance.OnEditTypeChange += OnEditTypeChange;
        }

        private void OnDisable()
        {
            DesignerManager.Instance.OnEditTypeChange -= OnEditTypeChange;
        }

        private void OnEditTypeChange(EditMode oldValue, EditMode value)
        {
            if (_handle != null)
                RemoveHandle();
            switch (value)
            {
                case EditMode.Object:
                    _onInteractionStart = OnObjectInteractionStart;
                    _onInteractionEnd = OnObjectInteractionEnd;
                    break;
                case EditMode.Path:
                    _onInteractionStart = OnPathPointInteractionStart;
                    _onInteractionEnd = OnPathPointInteractionEnd;
                    break;
                case EditMode.Road:
                    _onInteractionStart = OnRoadPointInteractionStart;
                    _onInteractionEnd = OnRoadPointInteractionEnd;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        #region Object Methods

        public void SelectObject(ObjectBase objectBase)
        {
            SelectedObject = objectBase;
            _targetTransform = SelectedObject.transform;
            _targetTransform.tag = "Selected";
            CreateHandle(_targetTransform);
            OnObjectSelected?.Invoke(SelectedObject);
        }
        
        public void DeselectObject()
        {
            RemoveHandle();
            SelectedObject.transform.tag = "Untagged";
            SelectedObject = null;
            _targetTransform = null;
            OnObjectDeselected?.Invoke();
        }
        
        private void OnObjectInteractionStart(Handle obj)
        {
            _startTransform = new TransformActionData(SelectedObject.transform.position, SelectedObject.transform.eulerAngles);
        }

        private void OnObjectInteractionEnd(Handle obj)
        {
            var endTransform = new TransformActionData(SelectedObject.transform.position, SelectedObject.transform.eulerAngles);
            var action = new ObjectTransformAction(_startTransform, endTransform, SelectedObject);
            ActionRecorder.Instance.Record(action);
            _startTransform = endTransform;
        }

        #endregion

        #region Road Methods

        public void SelectRoadPoint(RoadPointContainer roadPoint)
        {
            SelectedRoadPoint = roadPoint;
            _targetTransform = SelectedRoadPoint.transform;
            _targetTransform.tag = "Selected";
            CreateHandle(_targetTransform);
        }
        
        public void DeselectRoadPoint()
        {
            RemoveHandle();
            SelectedRoadPoint.transform.tag = "Untagged";
            SelectedRoadPoint = null;
            _targetTransform = null;
        }
        
        private void OnRoadPointInteractionStart(Handle obj)
        {
            _startTransform = new TransformActionData(SelectedRoadPoint.transform.position, SelectedRoadPoint.transform.eulerAngles);
        }
        
        private void OnRoadPointInteractionEnd(Handle obj)
        {
            var endTransform = new TransformActionData(SelectedRoadPoint.transform.position, SelectedRoadPoint.transform.eulerAngles);
            var action = new RoadTransformAction(_startTransform, endTransform, SelectedRoadPoint.RoadPoint);
            ActionRecorder.Instance.Record(action);
            _startTransform = endTransform;
        }

        #endregion

        #region Path Methods

        public void SelectPathPoint(PathPointContainer pathPoint)
        {
            SelectedPathPoint = pathPoint;
            _targetTransform = SelectedPathPoint.transform;
            _targetTransform.tag = "Selected";
            CreateHandle(_targetTransform);
            OnObjectSelected?.Invoke(pathPoint.PathPoint.Owner);
        }
        
        public void DeselectPathPoint()
        {
            RemoveHandle();
            SelectedPathPoint.transform.tag = "Untagged";
            SelectedPathPoint = null;
            _targetTransform = null;
        }
        
        private void OnPathPointInteractionStart(Handle obj)
        {
            _startTransform = new TransformActionData(SelectedPathPoint.transform.position, SelectedPathPoint.transform.eulerAngles);
        }

        private void OnPathPointInteractionEnd(Handle obj)
        {
            var endTransform = new TransformActionData(SelectedPathPoint.transform.position, SelectedPathPoint.transform.eulerAngles);
            var action = new PathTransformAction(_startTransform, endTransform, SelectedPathPoint.PathPoint);
            ActionRecorder.Instance.Record(action);
            _startTransform = endTransform;
        }

        #endregion
        
        public void UpdateHandlePosition()
        {
            RemoveHandle();
            CreateHandle(_targetTransform);
        }

        private void CreateHandle(Transform t)
        {
            _handle = _transformHandleManager.CreateHandle(t);
            _handle.OnInteractionStartEvent += _onInteractionStart;
            _handle.OnInteractionEndEvent += _onInteractionEnd;
        }
        
        private void RemoveHandle()
        {
            if (_handle == null) return;

            _handle.OnInteractionStartEvent -= _onInteractionStart;
            _handle.OnInteractionEndEvent -= _onInteractionEnd;
            _transformHandleManager.RemoveTarget(_targetTransform, _handle);
            _handle = null;
        }
    }
}