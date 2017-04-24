using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class depthTerrain : MonoBehaviour {

    public Terrain terr;
    public KinectDepth kinectDepth;
	// Use this for initialization
	void Start () {
        //terr = GetComponent<Terrain>();
	}
	
	// Update is called once per frame
	void Update () {
        //print(terr.terrainData.baseMapResolution);
        //float[,] bla = new float[2,1] { { 0.5f }, { 0.5f } };
        //terr.terrainData.SetHeights(1, 1, bla);

        //bla = new float[2, 1] { { 0.5f }, { 0.5f } };
        //terr.terrainData.SetHeights(10, 10, bla);

        //var x = terr.terrainData.heightmapWidth;
        //var y = terr.terrainData.heightmapHeight ;
        //var x = terr.terrainData.size.x;
        //var y = terr.terrainData.size.z;

        int xResolution = terr.terrainData.heightmapWidth;
        int zResolution = terr.terrainData.heightmapHeight;

        ushort[] depthData = kinectDepth.GetDepthData();

        var max = depthData.Max();

        float[,] heights = terr.terrainData.GetHeights(0, 0, xResolution, zResolution);
        var key = 0;
        for (int z = 0; z < zResolution - 2; z++)
        {
            for (int x = 0; x < 423; x++)
            {
                //float xSin = Mathf.Cos(x);
                //float zSin = -Mathf.Sin(z);
                //heights[x, z] = new Vector3(Mathf.PingPong(Time.time, 1), 0, 0.5f).x;
                //heights[x, z] = (xSin - zSin) * new Vector3(Mathf.PingPong(Time.time, 1), 0, 1f).x;
                //heights[z, x] = Random.Range(0.0f, 1.0f);
                key = z * 513 + x;
                try
                {
                    heights[z, x] = depthData[key]/max;
                    //print(key);
                }
                catch (Exception e)
                {
                    key = 0;
                    //print("aaaaaaaa" + key.ToString());
                }

            }
        }

        terr.terrainData.SetHeights(0, 0, heights);
        //float[,] heightMap = terr.terrainData.GetHeights(0, 0, Mathf.RoundToInt(x), Mathf.RoundToInt(y));

        //print(heightMap.Length);
        //print(depthData.Length);

        //for (int i = 0; i < y; i++)
        //{
        //    for(int j = 0; j < x; j++)
        //    {
        //        //heightMap[i, j] = MapRange(bla[i + j], 0, 1000, 0, 1);
        //        //heightMap[i, j] = 

        //        //heightMap[i, j] = new Vector3(Mathf.PingPong(Time.time, 1), 0, 0.5f).x;
        //    }
        //}
        //terr.terrainData.SetHeights(0, 0, heightMap);
    }

    int MapRange(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return Mathf.RoundToInt((x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min);
    }

}
