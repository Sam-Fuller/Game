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


    public int floorPerlinMin;
    public float floorPerlinMultiplier;
    public int floorPerlinWidth;
    public float floorPerlinSharpness;

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

    int[] generatePerlin(int startNoise, int size, int min, float multiplier, int minWidth, float sharpness){
        double[] PerlinSeed = new double[size];
		double[] perlin = new double[size];

        for (int i = 0; i < size; i++) {
            PerlinSeed[i] = UnityEngine.Random.value;
            perlin[i] = 0;
        }

        PerlinSeed[0] = startNoise;

        int count = 0;
        for (int stepSize = size; stepSize > minWidth; stepSize /= 2) {
            for (int i = 0; i < size; i++) {
                int lowPos = stepSize*(int)(i/stepSize);
                double low = PerlinSeed[lowPos];

                int highPos = lowPos + stepSize;
                highPos = highPos > size-1? size-1: highPos; 
                double high = PerlinSeed[highPos];

                double noise = (high - low) * (i%stepSize)/stepSize + low;
                noise *= sharpness*count;

                perlin[i] += noise;
            }
            count++;
        }

        int[] outPerlin = new int[size];
        for (int i = 0; i < size; i++) {
            outPerlin[i] = (int) (perlin[i] * multiplier) + min;
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

        //int[] floorPerlin = generatePerlin(0, levelSize+1, floorPerlinMin, floorPerlinMultiplier, floorPerlinWidth, floorPerlinSharpness);
        int[] roofPerlin = generatePerlin(0, levelSize+1, floorPerlinMin, floorPerlinMultiplier, floorPerlinWidth, floorPerlinSharpness);

        // for (int x = 0; x < levelSize * 2; x+=2) {
        //     for (int y = levelSize/2; y >= 0 && y > levelSize/2-floorPerlin[(int) (x/2)]; y--) {
        //         //map[x, y] = false;
        //         //map[x+1, y] = false;
        //     }
        // }

        for (int x = 0; x < levelSize * 2; x+=2) {
            int y;
            for (y = levelSize/2; y < levelSize && y < levelSize/2+roofPerlin[(int) (x/2)]; y++) {
                map[x, y] = false;
                map[x+1, y] = false;
            }
            map[x+(y%2==0?0:1), y] = false;
        }
    }
}
