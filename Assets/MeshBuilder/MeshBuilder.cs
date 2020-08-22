using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.CompilerServices;
using System.Linq;
using System;
using System.Collections.Specialized;

class MeshNode {
    public float x { get; set; }
    public float y { get; set; }

    public int pos;

    public MeshNode(float x, float y) {
        this.x = x;
        this.y = y;
    }

    public override int GetHashCode() {
        return (int)(x * 1000 + y);
    }

    public override bool Equals(object o) {
        if (this.GetType().Equals(o.GetType())) return false;

        MeshNode n = (MeshNode) o;
        if (x == n.x && y == n.y) return true;

        return false;
    }
}

class MeshTri {
    public MeshNode a, b, c;

    public MeshTri(MeshNode a, MeshNode b, MeshNode c) {
        this.a = a;
        this.b = b;
        this.c = c;
    }
}

public class MeshBuilder {
    Dictionary<MeshNode, MeshNode> nodes = new Dictionary<MeshNode, MeshNode>();

    List<MeshTri> tris = new List<MeshTri>();

    MeshNode addMeshNode(float x, float y) {
        MeshNode meshNode = new MeshNode(x, y);

        MeshNode result;
        if (nodes.TryGetValue(meshNode, out result)) {
            return result;
        }

        nodes.Add(meshNode, meshNode);
        return meshNode;
    }

    void addTriangle(MeshTri tri) {
        tris.Add(tri);
    }

    public void addTriangle(float ax, float ay, float bx, float by, float cx, float cy) {
        MeshNode a = addMeshNode(ax, ay);
        MeshNode b = addMeshNode(bx, by);
        MeshNode c = addMeshNode(cx, cy);

        tris.Add(new MeshTri(a, b, c));
    }

    public void build(Mesh mesh) {
        Vector3[] vertices = new Vector3[nodes.Count];
        int[] triangles = new int[nodes.Count * 3];

        int i = 0;
        foreach(KeyValuePair<MeshNode, MeshNode> node in nodes) {
            vertices[i] = new Vector3(node.Value.x, node.Value.y, 1);
            node.Value.pos = i;
            i++;
        }

        for (i = 0; i < tris.Count; i++) {
            triangles[i * 3] = tris[i].a.pos;
            triangles[i * 3 + 1] = tris[i].b.pos;
            triangles[i * 3 + 2] = tris[i].c.pos;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
