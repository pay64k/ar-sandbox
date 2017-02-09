using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class KinectDepth : MonoBehaviour {
    private KinectSensor _Sensor;
    private ushort[] _DepthData;

    public GameObject _DepthManagerGameObject;

    private DepthFrame DFrame;
    private DepthFrameReader _Reader;

    private Gradient gradient;

    private DepthSourceManager _Manager;

    private int imageWidth, imageHeight;

    private Color minColor = Color.green;
    private Color maxColor = Color.red;
    private Color lerpedColor = Color.white;

    void Start () {

        _Sensor = KinectSensor.GetDefault();
        _Manager = _DepthManagerGameObject.GetComponent<DepthSourceManager>();
        imageWidth = _Sensor.DepthFrameSource.FrameDescription.Width;
        imageHeight = _Sensor.DepthFrameSource.FrameDescription.Height;


        //_Reader = _Sensor.DepthFrameSource.OpenReader();
        //print(_Reader.DepthFrameSource.FrameDescription.BytesPerPixel);
        //print(_Reader.DepthFrameSource.FrameDescription.DiagonalFieldOfView);
        //print(_Reader.DepthFrameSource.FrameDescription.HorizontalFieldOfView);
        //print(_Reader.DepthFrameSource.FrameDescription.VerticalFieldOfView);
        //print(_Reader.DepthFrameSource.FrameDescription.Height);
        //print(_Reader.DepthFrameSource.FrameDescription.Width);

        if (!_Sensor.IsOpen)
        {
            _Sensor.Open();
            print("----Kinect started----");
        }
    }
	
	void Update () {
        _DepthData = _Manager.GetData();
        print(_DepthData.Length);
        lerpedColor = Color.Lerp(minColor, maxColor, );

    }
}

