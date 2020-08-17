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
    public Color color { get; set; }

    public int pos;

    public MeshNode(float x, float y, Color color) {
        this.x = x;
        this.y = y;
        this.color = color;
    }

    public override int GetHashCode() {
        return (int)(x * 1000 + y);
    }

    public override bool Equals(object o) {
        if (this.GetType().Equals(o.GetType())) return false;

        MeshNode n = (MeshNode) o;
        if (x == n.x && y == n.y && color.Equals(n.color)) return true;

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

    MeshNode addMeshNode(float x, float y, Color color) {
        MeshNode meshNode = new MeshNode(x, y, color);

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

    public void addTriangle(float ax, float ay, Color ac, float bx, float by, Color bc, float cx, float cy, Color cc) {
        MeshNode a = addMeshNode(ax, ay, ac);
        MeshNode b = addMeshNode(bx, by, bc);
        MeshNode c = addMeshNode(cx, cy, cc);

        tris.Add(new MeshTri(a, b, c));
    }

    public void build(Mesh mesh) {
        Vector3[] vertices = new Vector3[nodes.Count];
        Color[] colors = new Color[nodes.Count];
        int[] triangles = new int[nodes.Count * 3];

        int i = 0;
        foreach(KeyValuePair<MeshNode, MeshNode> node in nodes) {
            vertices[i] = new Vector3(node.Value.x, node.Value.y, 0);
            colors[i] = node.Value.color;
            node.Value.pos = i;
            i++;
        }

        for (i = 0; i < tris.Count; i++) {
            triangles[i * 3] = tris[i].a.pos;
            triangles[i * 3 + 1] = tris[i].b.pos;
            triangles[i * 3 + 2] = tris[i].c.pos;
        }

        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.triangles = triangles;
    }
}
