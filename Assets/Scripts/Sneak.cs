using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sneak : MonoBehaviour
{
    public GameObject SneakoWrap;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
            if (Input.GetKey(KeyCode.N))
                if (Input.GetKey(KeyCode.E))
                    if (Input.GetKey(KeyCode.A))
                        if (Input.GetKey(KeyCode.K))
                            Sneako();
    }

    private void Sneako()
    {
        SneakoWrap.SetActive(true);
    }
}
