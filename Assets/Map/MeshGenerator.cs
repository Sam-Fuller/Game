using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

using static MeshBuilder;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {

    Mesh mesh;

    // Start is called before the first frame update
    void Start() {
        mesh = new Mesh();
        // GetComponent<MeshFilter>().mesh = mesh;

        // MeshBuilder meshBuilder = new MeshBuilder();

        // meshBuilder.addTriangle(0, 0, Color.red, 0, 1, Color.red, 1, 1, Color.red);

        // meshBuilder.build(mesh);
        
        Vector3[] vertices = new Vector3[3];
        Color[] colors = new Color[3];
        int[] triangles = new int[3];

        vertices[0] = new Vector3(1,1,0);
        vertices[1] = new Vector3(1,0,1);
        vertices[2] = new Vector3(0,1,1);

        colors[0] = Color.red;
        colors[0] = Color.green;
        colors[0] = Color.blue;


        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.triangles = triangles;
        
        }

    // Update is called once per frame
    void Update() {
        
    }
}
