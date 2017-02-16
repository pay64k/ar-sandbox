using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class KinectDepth : MonoBehaviour {
    private KinectSensor _Sensor;

    private DepthFrameReader _Reader;

    private Gradient gradient;

    private int imageWidth, imageHeight;

    private Color minColor = Color.green;
    private Color maxColor = Color.red;
    private Color lerpedColor = Color.white;

    private int minDist = 1000;
    private int maxDist = 2000;

    private ushort[] _Data;
    private byte[] pixels;

    Texture2D texture;

    void Start () {
        texture = new Texture2D(512, 424, TextureFormat.RGBA32, false);
        _Sensor = KinectSensor.GetDefault();

        pixels = new byte[512*424*4];

        imageWidth = _Sensor.DepthFrameSource.FrameDescription.Width;
        imageHeight = _Sensor.DepthFrameSource.FrameDescription.Height;

        //ushort minDepth = _Sensor.DepthFrameSource.DepthMinReliableDistance;
        //ushort maxDepth = _Sensor.DepthFrameSource.DepthMinReliableDistance;
        //print(minDepth.ToString() + " " + maxDepth.ToString());


        if (!_Sensor.IsOpen)
        {
            _Sensor.Open();
            _Reader = _Sensor.DepthFrameSource.OpenReader();
            _Data = new ushort[_Sensor.DepthFrameSource.FrameDescription.LengthInPixels];
            print("----Kinect started----");
        }
    }
	
	void Update () {
        Renderer renderer = GetComponent<Renderer>();
        var frame = _Reader.AcquireLatestFrame();

        if (frame != null)
        {
            frame.CopyFrameDataToArray(_Data);
            frame.Dispose();
            frame = null;

            
            for (int colorIndex = 0; colorIndex < 512*424*4; colorIndex+=4)
                {

                int depth = _Data[colorIndex / 4];

                byte[] bla = StringToByteArray(ColorToHex(lerpedColor)); 

                if(depth >= minDist && depth <= maxDist )
                {
                    bla = StringToByteArray(ColorToHex(minColor));
                }
                if(depth >= maxDist)
                {
                   bla=  StringToByteArray(ColorToHex(maxColor));
                }

                pixels[colorIndex] = bla[0];
                pixels[colorIndex + 1] = bla[1];
                pixels[colorIndex + 2] = bla[2];
                pixels[colorIndex + 3] = 0x00;

            }
                
            //lerpedColor = Color.Lerp(minColor, maxColor, );

            texture.LoadRawTextureData(pixels);
            texture.Apply();
            renderer.material.mainTexture = texture;
        }
        //print("Data len:" + _Data.Length);
        //print(pixels.Length/4);
        
    }

    string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }

    public static byte[] StringToByteArray(string hex)
    {
        int NumberChars = hex.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }

    void PrintFrameSpecs()
    {
        //print(_Reader.DepthFrameSource.FrameDescription.BytesPerPixel);         //2
        //print(_Reader.DepthFrameSource.FrameDescription.DiagonalFieldOfView);   //89.5
        //print(_Reader.DepthFrameSource.FrameDescription.HorizontalFieldOfView); //70.6
        //print(_Reader.DepthFrameSource.FrameDescription.VerticalFieldOfView);   //60
        //print(_Reader.DepthFrameSource.FrameDescription.Height);    //424
        //print(_Reader.DepthFrameSource.FrameDescription.Width);     //512
    }
}

