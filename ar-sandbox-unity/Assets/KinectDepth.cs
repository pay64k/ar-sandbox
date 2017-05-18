﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class KinectDepth : MonoBehaviour
{
    private KinectSensor _Sensor;

    private DepthFrameReader _Reader;

    private Gradient gradient;

    private int imageWidth, imageHeight;

    private Color32 minColor;
    private Color32 maxColor;

    private Color32 lerpedColor = Color.white;

    private Color32[] colorScale;

    public float minDist = 1000;
    public float maxDist = 1200;

    private ushort[] _Data;
    private byte[] pixels;
    private float[] heightPixels;

    public bool displayTexture = true;

    private int counter = 0;
    private FileSaver fileSaver = new FileSaver();

    Texture2D texture;
    Texture2D heightTexture;

    void Start()
    {
        texture = new Texture2D(512, 424, TextureFormat.RGB24, false);
        heightTexture = new Texture2D(512, 424, TextureFormat.RFloat, false);
        _Sensor = KinectSensor.GetDefault();
        //if (!_Sensor.IsAvailable)
        //{
        //    print("no kinect v2 connected");
        //    return;
        //}

        pixels = new byte[512 * 424 * 3];
        heightPixels = new float[512 * 424 * 1];

        imageWidth = _Sensor.DepthFrameSource.FrameDescription.Width;
        imageHeight = _Sensor.DepthFrameSource.FrameDescription.Height;

        //ushort minDepth = _Sensor.DepthFrameSource.DepthMinReliableDistance;
        //ushort maxDepth = _Sensor.DepthFrameSource.DepthMinReliableDistance;
        //print(minDepth.ToString() + " " + maxDepth.ToString());


        //http://www.perbang.dk/rgbgradient/
        String[] hexScale6 = { "33E500", "87E808", "D9EC10", "EFB819", "F37421", "F7342A" };
        String[] hexScale6Reverse = { "F7342A", "F37421", "EFB819", "D9EC10", "87E808", "33E500" };
        String[] hexScale10 = { "33E500", "62E704", "90E909", "BEEB0D", "EAED12", "EFC717", "F1A11B", "F37B20", "F55725", "F7342A" };
        String[] hexScale10Reverse = { "F7342A", "F55725", "F37B20", "F1A11B", "EFC717", "EAED12", "BEEB0D", "90E909", "62E704", "33E500" };

        String[] scale14 = {"F73329",
                            "F54C26",
                            "F46423",
                            "F27E1F",
                            "F1981C",
                            "F0B219",
                            "EECD15",
                            "EDE812",
                            "D3EB0F",
                            "B3EA0C",
                            "94E909",
                            "74E706",
                            "53E603",
                            "33E500"};



        String[] currentScale = scale14;

        colorScale = new Color32[currentScale.Length];

        minColor = HexToColor(currentScale[0]);
        maxColor = HexToColor(currentScale[currentScale.Length - 1]);

        for (int i = 0; i < currentScale.Length; i++)
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

    void Update()
    {

        if (Input.GetKeyDown("c"))
        {
            fileSaver.SaveDepthMap(_Data);
        }

        if (!_Sensor.IsAvailable)
        {
            return;
        }

        Renderer renderer = GetComponent<Renderer>();
        var frame = _Reader.AcquireLatestFrame();

        if (frame != null)
        {
            frame.CopyFrameDataToArray(_Data);
            frame.Dispose();
            frame = null;


            for (int colorIndex = 0; colorIndex < 512 * 424 * 3; colorIndex += 3)
            {

                float depth = _Data[colorIndex / 3];

                Color32 currentColor = Color.white;

                if (depth >= minDist && depth <= maxDist)
                {
                    var depthToLevelIndex = MapRange(depth, minDist, maxDist, 0, colorScale.Length - 1);
                    currentColor = colorScale[depthToLevelIndex];
                }
                else if (depth >= maxDist)
                {
                    currentColor = maxColor;
                }
                else if (depth <= maxDist)
                {
                    currentColor = minColor;
                }
                var bla = (byte)MapRange(depth, minDist, maxDist, 255, 0); //255 max
                pixels[colorIndex] = bla;

                //pixels[colorIndex] = currentColor.r;
                pixels[colorIndex + 1] = currentColor.g;
                pixels[colorIndex + 2] = currentColor.b;

                //heightPixels[colorIndex / 3] = 1;

            }

            texture.LoadRawTextureData(pixels);
            //heightTexture.LoadRawTextureData(heightPixels);
            texture.Apply();
            //heightTexture.Apply();
            //renderer.material.mainTexture = texture;
            renderer.material.SetTexture("_MainTex", texture);
            //renderer.material.SetTexture("HeightMap", heightTexture);
            //renderer.material.SetFloatArray("HeightMapFloat", heightPixels);
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


    int MapRange(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return Mathf.RoundToInt((x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min);
    }

    public ushort[] GetDepthData()
    {
        return _Data;
    }

}

