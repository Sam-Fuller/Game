using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public Chunk_Mesh_Generator chunk;


    public List<Chunk_Mesh_Generator> chunks = new List<Chunk_Mesh_Generator>();

    // Start is called before the first frame update
    void Start() {
        chunks.Add((Chunk_Mesh_Generator) Instantiate(chunk));
    }

    // Update is called once per frame
    void Update() {
        
    }
}
