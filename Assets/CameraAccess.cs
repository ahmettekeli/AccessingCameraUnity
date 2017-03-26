using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraAccess : MonoBehaviour
{

    //for default camera
    private int currentCamIndex = 0;
    public Text info;

    WebCamTexture cameraTexture;
    public RawImage display;

    void Start()
    {
        info.text = "Connected camera count: " + WebCamTexture.devices.Length.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit();
        }
    }

    //for switching between cameras
    //most of smartpohnes have 2 cameras: frontfacing and backfacing camera
    public void OnSwapCamera()
    {
        //if camera count is greater than 0 (if there is a camera)
        if (WebCamTexture.devices.Length > 0)
        {
            currentCamIndex++;
            //clamping currentCamIndex between 0-1
            currentCamIndex %= WebCamTexture.devices.Length;
            OnStartStopCamera(); //stop camera
            OnStartStopCamera(); //start camera
        }
        else
        {
            //handle no camera detected.
            Debug.Log("No camera detected.");
        }
    }

    public void OnStartStopCamera()
    {
        
        //if cameraTexture already has a video to show then we need to clear it.Cause it means we want to stop the camera.
        if (cameraTexture != null)
        {
            display.texture = null;
            cameraTexture.Stop();
            cameraTexture = null;
        }
        //we want to start the camera.
        else
        {
            
            //assigning the active camera to webcamdevice object.
            WebCamDevice device = WebCamTexture.devices[currentCamIndex];

            //creating a webcamera texture with the what the current device captures.
            cameraTexture = new WebCamTexture(device.name);

            //displaying what camera is capturing
            display.texture = cameraTexture;

            if (!device.isFrontFacing) //if it's rear facing rotating accordingly
            {
                //correcting video angle
                float antiRotate = -(360 - cameraTexture.videoRotationAngle) + 90 + 180;
                Quaternion quatRot = new Quaternion();
                quatRot.eulerAngles = new Vector3(0, 0, antiRotate);
                display.transform.rotation = quatRot;
            }

            else if(device.isFrontFacing) // if it's frontfacing rotatin accordingly
            {
                float antiRotate = -(360 - cameraTexture.videoRotationAngle) + 90;
                Quaternion quatRot = new Quaternion();
                quatRot.eulerAngles = new Vector3(0, 0, antiRotate);
                display.transform.rotation = quatRot;
            }

            cameraTexture.Play();
        }



        //if(device.isFrontFacing)
        //{
        //    //show frontfacing camera icon
        //}
        //else
        //{
        //    //show backfacing camera icon
        //}
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
