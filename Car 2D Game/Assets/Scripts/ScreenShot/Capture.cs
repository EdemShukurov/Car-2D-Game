using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class Capture : MonoBehaviour
{
    public RectTransform panel;

    // Store more screenshots...
    private int _screenShotCount = 0;

    // Check the Shot Taken/Not.
    private bool _isShotTaken = false;

    // Name of the File, path name.
    private string _screenShot_FileName, _path;

    private byte[] _bytesFile;

    private Texture2D _screenshot;
    private Rect _rect;

    private Vector2 _initRectPosition;


    #region Singleton

    public static Capture Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion


    private void Start()
    {
        _initRectPosition = panel.anchoredPosition;

        float width = Screen.width / 1.5f;
        float height = Screen.height / 1.5f;

        _rect = new Rect(Screen.width / 2f - width/2, Screen.height / 2f - height/2f, width, height);
    }

    /// <summary>
    /// Make screenshot: it will appear in Car-2D-Game folder
    /// </summary>
    public void MakeScreenShot()
    {
        _screenShotCount++;

        // Save the screenshot name as Screenshot_1.png, Screenshot_2.png, with date format...
        _screenShot_FileName = "Screenshot__" + _screenShotCount + System.DateTime.Now.ToString("__yyyy-MM-dd") + ".png";
        ScreenCapture.CaptureScreenshot(_screenShot_FileName);

        _path = Path.GetFullPath(_screenShot_FileName);

        _isShotTaken = true;

    }    

    private void OnGUI()
    {
        if(_isShotTaken)
        {
            try
            {
                _bytesFile = File.ReadAllBytes(_path);
                _screenshot = new Texture2D(0, 0, TextureFormat.DXT1/*ATF_RGB_DXT1*/, false);
                _screenshot.LoadImage(_bytesFile);

            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }


        if (_screenshot != null)
        {
            SwitchPanel(null);

            GUI.DrawTexture(_rect, _screenshot);
            _isShotTaken = false;

            RemoveScreensFromFolder();
        }
    }

    public void SwitchPanel(Vector2? targetPosition)
    {
        Vector2 _targetPosition = targetPosition ?? Vector2.zero;
        panel.anchoredPosition = _targetPosition;
    }

    /// <summary>
    /// Remove all files with .png extensions
    /// </summary>
    private void RemoveScreensFromFolder()
    {
        DirectoryInfo di = new DirectoryInfo(@"C:\Users\1\Documents\GitHub\Car-2D-Game\Car 2D Game");
        FileInfo[] files = di.GetFiles("*.png")
                             .Where(p => p.Extension == ".png").ToArray();

        foreach (FileInfo file in files)
        {
            try
            {
                file.Attributes = FileAttributes.Normal;
                File.Delete(file.FullName);
            }
            catch { }
        }

    }
}