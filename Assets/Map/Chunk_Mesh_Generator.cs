using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

using static MeshBuilder;

[RequireComponent(typeof(MeshFilter))]
public class Chunk_Mesh_Generator : MonoBehaviour {

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


        BuildMesh();

        // CreateShape();
        // UpdateMesh();
    }

    void CreateShape() {
        vertices = new Vector3[] {
            new Vector3(0,0,1),
            new Vector3(1,0,1),
            new Vector3(0,1,1),
        };

        triangles = new int[]{
            0, 1, 2
        };
    }

    void UpdateMesh() {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }


    void BuildMesh() {
        MeshBuilder meshBuilder = new MeshBuilder();

        meshBuilder.addTriangle(0,0, 1,0, 0,1);

        meshBuilder.build(mesh);
    }


    // Update is called once per frame
    void Update() {
        
    }
}
