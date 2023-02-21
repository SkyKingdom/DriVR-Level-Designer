using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiEventNav : MonoBehaviour
{
    public GameObject dropdownPanel;

    public void DropdownToggle()
    {
        if (dropdownPanel.activeSelf == false)
        {
            dropdownPanel.SetActive(true);
            gameObject.transform.rotation = new Quaternion(0, 0, 180, 0);
        }
        else
        {
            dropdownPanel.SetActive(false);
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
