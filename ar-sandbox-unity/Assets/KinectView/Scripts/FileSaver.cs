using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSaver {

    private int fileIndexCounter = 0;

    public void SaveDepthMap(ushort[] map)
    {
        MonoBehaviour.print("written file no: " + fileIndexCounter.ToString());
        string[] foo = map.OfType<object>().Select(o => o.ToString()).ToArray();
        //System.IO.File.WriteAllLines(@"C:\Users\s127578\Desktop\testFolder\HeightMap" + fileIndexCounter + ".txt", foo);
        System.IO.File.WriteAllLines(@"C:\Users\s127578\PycharmProjects\untitled\map", foo);
        fileIndexCounter++; 
    }

    public void SaveCalibration(Vector3[] list)
    {
        //string[] foo = list.OfType<object>().Select(o => o.ToString()).ToArray();

        string[] foo = Vector3ToString(list);

        System.IO.File.WriteAllLines(@"C:\Users\s127578\Desktop\testFolder\positions", foo);
        foreach(string item in foo)
        {
            MonoBehaviour.print(item);
        }
    }

    public Vector3[] ReadCalibration()
    {
        string[] lines = System.IO.File.ReadAllLines(@"C:\Users\s127578\Desktop\testFolder\positions");
        Vector3[] positions = new Vector3[4];
        for(int i = 0; i < 4; i++)
        {
            positions[i] = StringToVector3(lines[i]);
        }

        return positions;
    }

    public void saveCameraPosition(Vector3 position, Quaternion rotation)
    {

        System.IO.File.WriteAllLines(@"C:\Users\s127578\Desktop\testFolder\TEST.txt", new string[2] { position.ToString("G4"), rotation.ToString("G4") });
        MonoBehaviour.print("Saved camera: " + position.ToString("G4") + rotation.ToString("G4"));
    }

    public ArrayList loadCameraPosition()
    {
        string[] lines = System.IO.File.ReadAllLines(@"C:\Users\s127578\Desktop\testFolder\TEST.txt");
        Vector3 position = StringToVector3(lines[0]);
        Quaternion rotation = StringToQuanterion(lines[1]);
        ArrayList list = new ArrayList();
        list.Add(position);
        list.Add(rotation);
        MonoBehaviour.print("Loaded camera: " + position.ToString("G4") + rotation.ToString("G4"));
        return list;
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    public static Quaternion StringToQuanterion(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Quaternion result = new Quaternion(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]),
            float.Parse(sArray[3]));
        return result;
    }

    public string[] Vector3ToString(Vector3[] list)
    {
        string[] pos = new string[4];
        for (int i = 0; i < 4; i++)
        {
            pos[i] = list[i].ToString("G4");
        }
        return pos;
    }


}
