using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    public static CameraController instance;

    public float moveSpeed;

    public Transform target;

    public Animator anim;

 
    public void camShake()
    {
        anim.SetTrigger("Shake");
    }

    public void weakCamShake()
    {
        anim.SetTrigger("WeakShake");
    }
    public void weakestCamShake()
    {
        anim.SetTrigger("WeakerShake");
    }
    
    public void shiftCam()
    {
        anim.SetTrigger("ShiftTrigger");
        anim.SetBool("Shift", true);
    }
    public void resetCam()
    {
        anim.SetBool("Shift", false);
    }

    #region Pola
    private int ScreenSizeX = 0;
    private int ScreenSizeY = 0;
    #endregion

    #region metody

    #region rescale camera
    private void RescaleCamera()
    {

        if (Screen.width == ScreenSizeX && Screen.height == ScreenSizeY) return;

        float targetaspect = 16.0f / 9.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;
        Camera camera = GetComponent<Camera>();

        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }

        ScreenSizeX = Screen.width;
        ScreenSizeY = Screen.height;
    }
    #endregion

    #endregion

    #region metody unity

    void OnPreCull()
    {
        if (Application.isEditor) return;
        Rect wp = Camera.main.rect;
        Rect nr = new Rect(0, 0, 1, 1);

        Camera.main.rect = nr;
        GL.Clear(true, true, Color.black);

        Camera.main.rect = wp;
        //Screen.SetResolution(1920, 1080, true);
        //Screen.SetResolution(Screen.width, Screen.height, true);

    }

    // Start is called before the first frame update
    void Start()
    {
        //Screen.SetResolution(1920, 1080, true);
        //RescaleCamera();

    }

    // Update is called once per frame
    void Update()
    {
       // RescaleCamera();
        if (target != null)
        {

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);

        }
        //Screen.SetResolution(Screen.width, Screen.height, true);

    }
}
#endregion
