using UnityEngine;

namespace Assets.Scripts.Game_Behaviour
{
    public class ScreenShotHandler : MonoBehaviour
    {
        private static ScreenShotHandler instance;

        public Camera camera;
        private bool takeScreenShotOnNextFrame;

        private void Awake()
        {
            instance = this;
        }

        private void OnPostRender()
        {
            if(takeScreenShotOnNextFrame)
            {
                takeScreenShotOnNextFrame = false;
                RenderTexture renderTexture = camera.targetTexture;

                Texture2D renderResult = new Texture2D(
                    renderTexture.width, renderTexture.height,
                    TextureFormat.ARGB32, false);

                Rect rect = new Rect(0f, 0f, renderTexture.width, renderTexture.height);

                renderResult.ReadPixels(rect, 0, 0);

                byte[] byteArray = renderResult.EncodeToPNG();
                System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot.png", byteArray);

                RenderTexture.ReleaseTemporary(renderTexture);
                camera.targetTexture = null;
            }
        }

        private void TakeScreenShot(int width, int height)
        {
            camera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        }

        public static void Static_TakeScreenShot (int width, int height)
        {
            instance.TakeScreenShot(width, height);
        }
    }


}
