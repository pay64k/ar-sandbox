using UnityEngine;
using System.Collections;

public class Plane_Mesh_Script : MonoBehaviour
{
    //void Awake()
    //{
    //    GameObject plane = new GameObject("Plane");
    //    MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
    //    meshFilter.mesh = CreateMesh(1, 0.2f);
    //    MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
    //    renderer.material.shader = Shader.Find("Particles/Additive");
    //    Texture2D tex = new Texture2D(1, 1);
    //    tex.SetPixel(0, 0, Color.green);
    //    tex.Apply();
    //    renderer.material.mainTexture = tex;
    //    renderer.material.color = Color.green;
    //}
    //Mesh CreateMesh(float width, float height)
    //{
    //    Mesh m = new Mesh();
    //    m.name = "ScriptedMesh";
    //    m.vertices = new Vector3[] {
    //     new Vector3(-width, -height, 0.01f),
    //     new Vector3(width, -height, 0.01f),
    //     new Vector3(width, height, 0.01f),
    //     new Vector3(-width, height, 0.01f)
    // };
    //    m.uv = new Vector2[] {
    //     new Vector2 (0, 0),
    //     new Vector2 (0, 1),
    //     new Vector2(1, 1),
    //     new Vector2 (1, 0)
    // };
    //    m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
    //    m.RecalculateNormals();

    //    return m;
    //}

    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        // make changes to the Mesh by creating arrays which contain the new values
        mesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0) };
        mesh.normals = new Vector3[] { new Vector3(0, 0, -1), new Vector3(0, 0, -1), new Vector3(0,0, -1) };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0,.5f), new Vector2(.5f, .5f) };
        mesh.triangles = new int[] { 0, 1, 2 };
    }
}