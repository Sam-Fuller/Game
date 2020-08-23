using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

using static MeshBuilder;

[RequireComponent(typeof(MeshFilter))]
public class Chunk_Mesh_Generator : MonoBehaviour {

    public static int levelSize = 200;
    public static int perlinRoofHeight = 10;

    Mesh mesh; 

    bool[][] map;

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

    // int[] generateRoofPerlin(int startNoise, int size){
    //     double[] PerlinSeed = new double[size];
	// 	double[] perlin = new double[size];

    //     for (int i = 0; i < size; i++) {
    //         PerlinSeed[i] = UnityEngine.Random.value;
    //         perlin[i] = 0;
    //     }

    //     PerlinSeed[0] = startNoise;

    //     for (int stepSize = 1; stepSize < size; stepSize *= 2) {
    //         for (int i = 0; i < size; i++) {
    //             double low = PerlinSeed[stepSize*(int)(i/stepSize)];

    //             int highPos = stepSize*(int)(i/stepSize) + stepSize;
    //             highPos = highPos > size-1? size-1: highPos; 
    //             double high = PerlinSeed[highPos];

    //             double noise = (high - low) * (i%stepSize) + low;

    //             perlin[i] += noise;
    //         }
    //     }

    //     int[] outPerlin = new int[size];
    //     for (int i = 0; i < size; i++) {
    //         outPerlin[i] = (int) perlin[i];
    //     }

    //     return outPerlin;
    // }

    // int[] generateFloorPerlin(int startNoise, int size){
    //     double[] PerlinSeed = new double[size];
	// 	double[] perlin = new double[size];

    //     for (int i = 0; i < size; i++) {
    //         PerlinSeed[i] = UnityEngine.Random.value;
    //         perlin[i] = 0;
    //     }

    //     PerlinSeed[0] = startNoise;

    //     for (int stepSize = 1; stepSize < size; stepSize *= 2) {
    //         for (int i = 0; i < size; i++) {
    //             double low = PerlinSeed[stepSize*(int)(i/stepSize)];

    //             int highPos = stepSize*(int)(i/stepSize) + stepSize;
    //             highPos = highPos > size-1? size-1: highPos; 
    //             double high = PerlinSeed[highPos];

    //             double noise = (high - low) * (i%stepSize) + low;

    //             perlin[i] += noise;
    //         }
    //     }

    //     int[] outPerlin = new int[size];
    //     for (int i = 0; i < size; i++) {
    //         outPerlin[i] = (int) perlin[i];
    //     }

    //     return outPerlin;
    // }


    void generateMap(int prevExit, int prevExitPerlinHeight) {
        map = new bool[levelSize*2][levelSize];

        for (int x = 0; x < levelSize * 2; x++) {
            for (int y = 0; y < levelSize; y++) {
                map[x][y] = UnityEngine.Random.value > 0.5;
            }
        }
    }
}
