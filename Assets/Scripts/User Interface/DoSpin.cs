using UnityEngine;
using DG.Tweening;

/// <summary>
/// Spins the gears in the main menu
/// </summary>
public class DoSpin : MonoBehaviour
{
    public float duration;
    public bool spinLeft;
    void Start()
    {
        var _direction = 1;
        if (spinLeft == false)
        {
            _direction *= -1;
        }
        
        gameObject.transform.DORotate(new Vector3(0, 0, 360 * _direction), duration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

}
