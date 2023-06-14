using UnityEngine;

namespace TransformHandles
{
    public class PositionHandle : MonoBehaviour
    { 
        [SerializeField] public PositionAxis xAxis;
        [SerializeField] public PositionAxis yAxis;
        [SerializeField] public PositionAxis zAxis;
 
        [SerializeField] public PositionPlane xPlane;
        [SerializeField] public PositionPlane yPlane;
        [SerializeField] public PositionPlane zPlane;

        private Handle _parentHandle;

        private bool _handleInitialized;

        public void Initialize(Handle handle)
        {
            if (_handleInitialized) return;
            
            _parentHandle = handle;

            xAxis.gameObject.SetActive(true);
            xAxis.Initialize(handle);
            
            zAxis.gameObject.SetActive(true);
            zAxis.Initialize(handle);
            
            xPlane.gameObject.SetActive(true);
            xPlane.Initialize(_parentHandle, Vector3.right, Vector3.forward, Vector3.up);
            
            _handleInitialized = true;
        }
    }
}