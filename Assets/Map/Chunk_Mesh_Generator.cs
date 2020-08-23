using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

using static MeshBuilder;

[RequireComponent(typeof(MeshFilter))]
public class Chunk_Mesh_Generator : MonoBehaviour {

    Mesh mesh;

    // Start is called before the first frame update
    void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        BuildMesh();
    }

    void BuildMesh() {
        MeshBuilder meshBuilder = new MeshBuilder();

        meshBuilder.addTriangle(0,0, 1,0, 0,1);
        meshBuilder.addTriangle(1,1, 1,0, 0,1);

        meshBuilder.build(mesh);
    }


    // Update is called once per frame
    void Update() {
        
    }

    void generateMap(int prevExit, int prevExitPerlinHeight) {
        map = new bool[levelSize*2][levelSize];

        for (int x = 0; x < levelSize * 2; x++) {
            for (int y = 0; y < levelSize; y++) {
                map[x][y] = UnityEngine.Random.value > 0.5;
            }
        }
    }
}
