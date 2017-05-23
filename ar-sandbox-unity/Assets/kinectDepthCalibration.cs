using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class kinectDepthCalibration : MonoBehaviour {
    private KinectSensor _Sensor;

    private DepthFrameReader _Reader;


    private int imageWidth, imageHeight;

    public float minDist = 1000;
    public float maxDist = 1200;

    private ushort[] _Data;
    private float[] _NormalizedData;
    private byte[] pixels;

    private FileSaver fileSaver = new FileSaver();

    Texture2D texture;
    
    void Start()
    {
        texture = new Texture2D(512, 424, TextureFormat.RGB24, false);
        _Sensor = KinectSensor.GetDefault();

        imageWidth = _Sensor.DepthFrameSource.FrameDescription.Width;
        imageHeight = _Sensor.DepthFrameSource.FrameDescription.Height;

        pixels = new byte[imageHeight * imageWidth * (int)TextureFormat.RGB24];

        _NormalizedData = new float[imageHeight * imageWidth];

        if (!_Sensor.IsOpen)
        {
            _Sensor.Open();
            _Reader = _Sensor.DepthFrameSource.OpenReader();
            _Data = new ushort[_Sensor.DepthFrameSource.FrameDescription.LengthInPixels];
            print("----Kinect started----");
        }
        
    }

    void Update()
    {
        

        if (Input.GetKeyDown("c")){fileSaver.SaveDepthMap(_Data);}
        
        if (!_Sensor.IsAvailable){return;}

        Renderer renderer = GetComponent<Renderer>();
        var frame = _Reader.AcquireLatestFrame();

        if (frame != null)
        {
            frame.CopyFrameDataToArray(_Data);
            frame.Dispose();
            frame = null;
            ushort closestDistance = 8000;
            for (int i = 0; i < imageWidth * imageHeight; i += 1)
            {
                if (_Data[i] < closestDistance && _Data[i] != 0) { closestDistance = _Data[i]; }
            }
            print(closestDistance);
            texture.LoadRawTextureData(pixels);
            texture.Apply();
            renderer.material.SetTexture("_MainTex", texture);
        }

    }

    public ushort[] GetDepthData()
    {
        return _Data;
    }
}
