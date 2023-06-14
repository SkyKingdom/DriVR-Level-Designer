using TransformHandles;
using UnityEngine;

public class TransformTest : MonoBehaviour
{
    [SerializeField] private TransformHandleManager manager;
    [SerializeField] private Transform target;
    
    private void OnEnable()
    {
        manager.CreateHandle(target);
    }
}
