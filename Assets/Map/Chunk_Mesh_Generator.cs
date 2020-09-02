using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

using static MeshBuilder;

[RequireComponent(typeof(MeshFilter))]
public class Chunk_Mesh_Generator : MonoBehaviour {
    public int levelSize;

    Mesh mesh; 

    bool[,] map;

    // Start is called before the first frame update
    void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void buildLevel(int level) {
        generateMap(0, 0);
        buildMesh();
    }

    void buildMesh() {
        MeshBuilder meshBuilder = new MeshBuilder();

        for (int x = 0; x < levelSize*2; x++) {
            for (int y = 0; y < levelSize; y++) {
                bool flipped = (y%2==0) ^ (x%2==0);

                float point = (y + (flipped? 0f: 1f)) /levelSize;
                float edge = (y + (flipped? 1f: 0f)) /levelSize;

                float left = (x/2f) /levelSize;
                float centre = (x/2f + 0.5f) /levelSize;
                float right = (x/2f + 1f) /levelSize;

                if (map[x, y]) {
                    meshBuilder.addTriangle(
                        left, edge,
                        right, edge,
                        centre, point
                    );
                }
            }
        }

        meshBuilder.build(mesh, 10);
    }


    // Update is called once per frame
    void Update() {
        
    }

    int[] generateRoofPerlin(int startNoise, int size){
        double[] PerlinSeed = new double[size];
		double[] perlin = new double[size];

        for (int i = 0; i < size; i++) {
            PerlinSeed[i] = UnityEngine.Random.value;
            perlin[i] = 0;
        }

        PerlinSeed[0] = startNoise;

        for (int stepSize = 1; stepSize < size; stepSize *= 2) {
            for (int i = 0; i < size; i++) {
                double low = PerlinSeed[stepSize*(int)(i/stepSize)];

                int highPos = stepSize*(int)(i/stepSize) + stepSize;
                highPos = highPos > size-1? size-1: highPos; 
                double high = PerlinSeed[highPos];

                double noise = (high - low) * (i%stepSize) + low;

                perlin[i] += noise;
            }
        }

        int[] outPerlin = new int[size];
        for (int i = 0; i < size; i++) {
            outPerlin[i] = (int) perlin[i];
        }

        return outPerlin;
    }

    public int floorPerlinMin;
    public float floorPerlinMultiplier;
    int[] generateFloorPerlin(int startNoise, int size){
        double[] PerlinSeed = new double[size];
		double[] perlin = new double[size];

        for (int i = 0; i < size; i++) {
            PerlinSeed[i] = UnityEngine.Random.value;
            perlin[i] = 0;
        }

        PerlinSeed[0] = startNoise;

        for (int stepSize = size; stepSize < 1; stepSize /= 2) {
            for (int i = 0; i < size; i++) {
                double low = PerlinSeed[stepSize*(int)(i/stepSize)];

                int highPos = stepSize*(int)(i/stepSize) + stepSize;
                highPos = highPos > size-1? size-1: highPos; 
                double high = PerlinSeed[highPos];

                double noise = (high - low) * (i%stepSize) + low;

                perlin[i] += noise;
            }
        }

        int[] outPerlin = new int[size];
        for (int i = 0; i < size; i++) {
            outPerlin[i] = (int) (perlin[i] * floorPerlinMultiplier) + floorPerlinMin;
        }

        return outPerlin;
    }

    void generateMap(int prevExit, int prevExitPerlinHeight) {
        map = new bool[levelSize*2, levelSize];

        for (int x = 0; x < levelSize * 2; x++) {
            for (int y = 0; y < levelSize; y++) {
                map[x, y] = true;
            }
        }

        int[] floorPerlin = generateFloorPerlin(0, levelSize * 2);
        int[] roofPerlin = generateFloorPerlin(0, levelSize * 2);

        for (int x = 0; x < levelSize * 2; x++) {
            for (int y = levelSize/2; y >= 0 && y > levelSize/2-floorPerlin[x]; y--) {
                map[x, y] = false;
            }
        }

        for (int x = 0; x < levelSize * 2; x++) {
            for (int y = levelSize/2; y < levelSize && y < levelSize/2+roofPerlin[x]; y++) {
                map[x, y] = false;
            }
        }
    }
}
