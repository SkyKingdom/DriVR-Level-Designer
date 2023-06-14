using UnityEngine;

// ReSharper disable once CheckNamespace
namespace TransformHandles
{
    public class RotationHandle : MonoBehaviour
    {
        public RotationAxis xAxis;
        public RotationAxis yAxis;
        public RotationAxis zAxis;

        private Handle _parentHandle;

        private bool _handleInitialized;

        public void Initialize(Handle handle)
        {
            if (_handleInitialized) return;
            
            _parentHandle = handle;
            transform.SetParent(_parentHandle.transform, false);

            yAxis.gameObject.SetActive(true);
            yAxis.Initialize(_parentHandle, Vector3.up);
            
            _handleInitialized = true;
        }
    }
}