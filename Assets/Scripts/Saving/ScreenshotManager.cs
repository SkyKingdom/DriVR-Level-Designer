using System.IO;
using UnityEngine;

namespace Saving
{
    public class ScreenshotManager : MonoBehaviour
    {
        [SerializeField] private RenderTexture sceneCameraTexture;
        
        public void TakeScreenShot(string name ,string path)
        {
            Texture2D tex = new Texture2D(sceneCameraTexture.width, sceneCameraTexture.height);
            RenderTexture.active = sceneCameraTexture;
            tex.ReadPixels(new Rect(0, 0, sceneCameraTexture.width, sceneCameraTexture.height), 0, 0);
            RenderTexture.active = null;
            byte[] bytearray = tex.EncodeToPNG();
            File.WriteAllBytes(path + "/" + name + ".png", bytearray);
            Debug.Log( "saved screenshot at:" + path);
        }
    }
}
