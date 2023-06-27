using System;
using Actions;
using Objects;
using TransformHandles;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Handles the selection of objects and the creation of the transform handle.
    /// </summary>
    public class SelectionManager : MonoBehaviour
    {
        #region Depenedencies
        
        private TransformHandleManager _transformHandleManager;

        #endregion
        
        // Object references
        public ObjectBase SelectedObject { get; private set; }
        public ObjectBase LastSelectedObject { get; private set; }
        
        // Road reference
        public RoadPointContainer SelectedRoadPoint { get; private set; }
        
        // Object path reference
        public PathPointContainer SelectedPathPoint { get; private set; }

        // Selected transform reference
        private Transform _targetTransform;
        
        // Events
        public event Action<ObjectBase> OnObjectSelected;
        public event Action OnObjectDeselected;

        // Current handle
        private Handle _handle;
        
        // Transform data on selection start
        private TransformActionData _startTransform;

        // Interaction delegates
        private Action<Handle> _onInteractionStart;
        private Action<Handle> _onInteractionEnd;

        private void Start()
        {
            // Get dependencies
            _transformHandleManager = DesignerManager.Instance.TransformHandleManager;
        }

        private void OnEnable()
        {
            // Subscribe to events
            DesignerManager.Instance.OnEditTypeChange += OnEditTypeChange;
        }

        private void OnDisable()
        {
            // Unsubscribe from events
            DesignerManager.Instance.OnEditTypeChange -= OnEditTypeChange;
        }

        private void OnEditTypeChange(EditMode oldValue, EditMode value)
        {
            switch (oldValue)
            {
                case EditMode.Object:
                    // Save last selected object when switching from object mode
                    LastSelectedObject = SelectedObject;
                    SelectedObject = null;
                    break;
                case EditMode.Path:
                    // Select last selected object when switching from path mode
                    if (LastSelectedObject)
                    {
                        SelectedObject = LastSelectedObject;
                    }
                    break;
                case EditMode.Road:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(oldValue), oldValue, null);
            }
            
            // Remove handle
            if (_handle != null)
                RemoveHandle();
            
            switch (value)
            {
                // Setup delegates based on edit mode
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
            LastSelectedObject = null;
            RemoveHandle();
            
            if (SelectedObject != null)
                SelectedObject.transform.tag = "Untagged";
            SelectedObject = null;
            _targetTransform = null;
            OnObjectDeselected?.Invoke();
        }
        
        private void OnObjectInteractionStart(Handle obj)
        {
            // Save transform data on interaction start
            _startTransform = new TransformActionData(SelectedObject.transform.position, SelectedObject.transform.eulerAngles);
        }

        private void OnObjectInteractionEnd(Handle obj)
        {
            // Create transform action and record it
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