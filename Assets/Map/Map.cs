using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public Chunk_Mesh_Generator chunk;
    public List<Chunk_Mesh_Generator> chunks = new List<Chunk_Mesh_Generator>();

    // Start is called before the first frame update
    void Start() {
       chunks.Add((Chunk_Mesh_Generator) Instantiate(chunk, new Vector3(0, 0, 0), Quaternion.identity));
       chunks.Add((Chunk_Mesh_Generator) Instantiate(chunk, new Vector3(1, 0, 0), Quaternion.identity));
       chunks.Add((Chunk_Mesh_Generator) Instantiate(chunk, new Vector3(0, 1, 0), Quaternion.identity));
    }

    bool started = false;
    // Update is called once per frame
    void Update() {
        if (!started) {
            started = true;
            chunks[0].buildLevel(0);
            chunks[1].buildLevel(0);
            chunks[2].buildLevel(0);
        }
    }

}
