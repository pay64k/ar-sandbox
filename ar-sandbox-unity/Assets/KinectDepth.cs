﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class KinectDepth : MonoBehaviour {
    private KinectSensor _Sensor;

    private DepthFrameReader _Reader;

    private Gradient gradient;

    private int imageWidth, imageHeight;

    //private Color32 minColor = Color.HSVToRGB(107f / 360f, 90f / 100f, 90f / 100f);
    //private Color32 maxColor = Color.HSVToRGB(2f / 360f, 90f / 100f, 90f / 100f);
    private Color32 minColor;
    private Color32 maxColor;

    private Color32 lerpedColor = Color.white;

    private Color32[] colorScale;

    private float minDist = 500;
    private float maxDist = 2500;

    private ushort[] _Data;
    private byte[] pixels;

    Texture2D texture;

    void Start () {
        texture = new Texture2D(512, 424, TextureFormat.RGB24, false);
        _Sensor = KinectSensor.GetDefault();

        pixels = new byte[512*424*3];

        imageWidth = _Sensor.DepthFrameSource.FrameDescription.Width;
        imageHeight = _Sensor.DepthFrameSource.FrameDescription.Height;

        //ushort minDepth = _Sensor.DepthFrameSource.DepthMinReliableDistance;
        //ushort maxDepth = _Sensor.DepthFrameSource.DepthMinReliableDistance;
        //print(minDepth.ToString() + " " + maxDepth.ToString());

        

        String[] hexScale6 = { "33E500", "87E808", "D9EC10", "EFB819", "F37421", "F7342A"};
        String[] hexScale10 = { "33E500", "62E704", "90E909", "BEEB0D", "EAED12", "EFC717", "F1A11B", "F37B20", "F55725", "F7342A" };

        String[] currentScale = hexScale6;

        colorScale = new Color32[currentScale.Length];

        minColor = HexToColor(currentScale[0]);
        maxColor = HexToColor(currentScale[currentScale.Length-1]);

        for(int i=0; i < currentScale.Length; i++)
        {
            colorScale[i] = HexToColor(currentScale[i]);
        }

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


            for (int colorIndex = 0; colorIndex < 512*424*3; colorIndex+=3)
                {

                float depth = _Data[colorIndex / 3];

                Color32 currentColor = Color.white;

                if (depth >= minDist && depth <= maxDist )
                {
                    var depthToLevelIndex = Map(depth, minDist, maxDist, 0, colorScale.Length-1);
                    currentColor = colorScale[depthToLevelIndex];
                    //float newHue = Mathf.Clamp(depth/maxDist/360f, 2f / 360f, 107f / 360f);

                    //currentColor = Color.HSVToRGB(newHue, 90f / 100f, 90f / 100f);

                    //currentColor = Color.Lerp(minColor, maxColor, depth/maxDist);

                }
                else if(depth >= maxDist)
                {
                    currentColor = maxColor;
                }
                else if(depth <= maxDist)
                {
                    currentColor = minColor;
                }

                pixels[colorIndex] = currentColor.r;
                pixels[colorIndex + 1] = currentColor.g;
                pixels[colorIndex + 2] = currentColor.b;

            }
            //var bla = Mathf.PingPong(Time.time, 1);
            //    lerpedColor = Color.Lerp(Color.white, Color.black, bla);

            //lerpedColor = Color.Lerp(minColor, maxColor, );

            texture.LoadRawTextureData(pixels);
            texture.Apply();
            renderer.material.mainTexture = texture;
        }

        
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

    Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }


    int Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return Mathf.RoundToInt((x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min);
    }

}

