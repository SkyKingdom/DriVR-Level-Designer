using DG.Tweening;
using UnityEngine;

/// <summary>
/// Sneak class for sneaking presented by Sneako and Sfadi
/// </summary>
public class Sneak : MonoBehaviour
{
    public GameObject SneakoWrap;
    public GameObject SfadiWrap;

    private Vector3 sneakPos = new(-8.5f, 0, -6);
    private Vector3 sfadiPos = new(8.25f, 0, 6);

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.S)) return;
        if (!Input.GetKey(KeyCode.N)) return;
        if (!Input.GetKey(KeyCode.E)) return;
        if (!Input.GetKey(KeyCode.A)) return;
        if (!Input.GetKey(KeyCode.K)) return;
        if (!Input.GetKey(KeyCode.O)) return;
        Sneako();
        Sfadi();
    }

    private void Sneako()
    {
        SneakoWrap.SetActive(true);
        SneakoWrap.transform.DOMove(sneakPos, 2f);
    }

    private void Sfadi()
    {
        SfadiWrap.SetActive(true);
        SfadiWrap.transform.DOMove(sfadiPos, 2f);
    }
}
