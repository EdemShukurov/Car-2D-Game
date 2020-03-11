using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class Capture : MonoBehaviour
{
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
        _rect = new Rect(Screen.width / 2f - 250f, Screen.height / 2f - 150f, 500f, 300f);
        _initRectPosition = new Vector2(_rect.x, _rect.y - 800f);
    }

    public void MakeScreenShot()
    {
        _screenShotCount++;

        // Save the screenshot name as Screenshot_1.png, Screenshot_2.png, with date format...
        _screenShot_FileName = "Screenshot__" + _screenShotCount + System.DateTime.Now.ToString("__yyyy-MM-dd") + ".png";
        ScreenCapture.CaptureScreenshot(_screenShot_FileName);

        _path = System.IO.Path.GetFullPath(_screenShot_FileName);

        _isShotTaken = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            MakeScreenShot();
        }

        //if (false /*Close_Screen.HitTest(Input.GetTouch(0).position)*/)
        //{
        //    if (Input.GetTouch(0).phase == TouchPhase.Began)
        //    {
        //        Screenshot = null;
        //        Shot_Taken = true;
        //    }
        //}

        //if (false /*Shot_Taken == true*/)
        //{
        //    Origin_Path = System.IO.Path.Combine(Application.persistentDataPath, Screen_Shot_File_Name);

        //    // This is the path of my folder.
        //    File_Path = "/mnt/sdcard/DCIM/Inde/" + Screen_Shot_File_Name;
        //    if (System.IO.File.Exists(Origin_Path))
        //    {
        //        System.IO.File.Move(Origin_Path, File_Path);
        //        Shot_Taken = false;
        //    }
        //}
    }

    private void OnGUI()
    {
        if(_isShotTaken)
        {
            try
            {
                _bytesFile = System.IO.File.ReadAllBytes(_path);
                _screenshot = new Texture2D(0, 0, TextureFormat.DXT1/*ATF_RGB_DXT1*/, false);
                _screenshot.LoadImage(_bytesFile);

            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
            //  Path_Name = System.IO.Path.Combine(Application.persistentDataPath, Screen_Shot_File_Name);
        }


        if (_screenshot != null)
        {
            //GUI.DrawTexture(rect, _screenshot, ScaleMode.ScaleToFit, true, 200f, Color.red, 50f, 30f);
            GUI.DrawTexture(_rect, _screenshot);
            _isShotTaken = false;

            Invoke("DescendRect", 1f);
        }
    }

    private void DescendRect()
    {
        _screenshot = null;

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