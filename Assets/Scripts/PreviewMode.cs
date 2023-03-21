using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewMode : MonoBehaviour
{
    private bool _isPreviewMode = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _isPreviewMode = !_isPreviewMode;
            if (_isPreviewMode)
            {
                LevelGeneratorManager.Instance.SetMode(3);
            }
            else
            {
                LevelGeneratorManager.Instance.SetMode(1);
            }
        }
    }

}
