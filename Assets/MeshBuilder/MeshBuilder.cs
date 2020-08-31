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

		//take a as centre
		float bx = b.x - a.x;
		float by = b.y - a.y;

		float cx = c.x - a.x;
		float cy = c.y - a.y;

		 
		//calculate angle of b
		float bth = (float) Math.Atan2(bx, by);

		//calculate angle of c
		float cth = (float) Math.Atan2(cx, cy);

		//find if clockwise
		bool clockwise = (cth - bth) > 0;

		//set the nodes
		if (clockwise) {
			this.a = a;
        	this.b = b;
        	this.c = c;

		} else {
			this.b = a;
			this.a = b;
			this.c = c;
		}
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
	
	public Vector3[] getVerticies(int layer) {
		Vector3[] vertices = new Vector3[nodes.Count];

        int i = 0;
        foreach(KeyValuePair<MeshNode, MeshNode> node in nodes) {
            vertices[i] = new Vector3(node.Value.x, node.Value.y, layer);
            node.Value.pos = i;
            i++;
        }

		return vertices;
	}
	public int[] getTriangles() {
		int[] triangles = new int[nodes.Count * 3];

		for (int i = 0; i < tris.Count; i++) {
            triangles[i * 3] = tris[i].a.pos;
            triangles[i * 3 + 1] = tris[i].b.pos;
            triangles[i * 3 + 2] = tris[i].c.pos;
        }

		return triangles;
	}

    public void build(Mesh mesh, int layer) {
        mesh.Clear();

        if (nodes.Count < 65535) {
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;
        } else {
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        }
        
        Vector3[] vertices = getVerticies(layer);
        int[] triangles = getTriangles();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
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

    /**
	 * draws a sector of a circle
	 * @param x x position of the centre of the circle
	 * @param y y position of the centre of the circle
	 * @param r1 inner radius of the circle
	 * @param r2 outer radius of the circle
	 * @param th1 start angle of the sector
	 * @param th2 end angle of the sector
	 */
	private void drawSector(float x, float y, float r1, float r2, float th1, float th2) {
		float sinTh1 = (float) Math.Sin(th1);
		float sinTh2 = (float) Math.Sin(th2);
		float cosTh1 = (float) Math.Cos(th1);
		float cosTh2 = (float) Math.Cos(th2);

		//left inner corner
		float innerLeftX = (x+cosTh1*r1);
		float innerLeftY = (y+sinTh1*r1);

		//right inner corner
		float innerRightX = (x+cosTh2*r1);
		float innerRightY = (y+sinTh2*r1);

		//left outer corner
		float outerLeftX = (x+cosTh1*r2);
		float outerLeftY = (y+sinTh1*r2);

		//right outer corner
		float outerRightX = (x+cosTh2*r2);
		float outerRightY = (y+sinTh2*r2);

		//1st triangle
		addTriangle(
			innerLeftX, innerLeftY,
			innerRightX, innerRightY,
			outerLeftX, outerLeftY
		);

		//2nd triangle
		addTriangle(
			innerRightX, innerRightY,
			outerLeftX, outerLeftY,
			outerRightX, outerRightY
		);
	}

	/**
	 * draw an arc made of sectors with a colour fade across the radius
	 * @param x x position of the centre of the circle
	 * @param y y position of the centre of the circle
	 * @param r1 inner radius of the circle
	 * @param r2 outer radius of the circle
	 * @param th1 start angle of the arc
	 * @param th2 end angle of the arc
	 * @param slices number of sectors in the circle (poly count will be 2 * slices * Settings.segmentMultiplier)
	 */
	public void drawSectorArc(float x, float y, float r1, float r2, float th1, float th2, int slices) {		
		float maxAngle = th2 - th1;
		float anglePerSlice = maxAngle/slices;

		for(int i = 0; i < slices; th1 += anglePerSlice, i++){
			drawSector(x, y, r1, r2, th1, th1 + anglePerSlice);
		}
	}

	/**
	 * draw a full circle with a colour fade across the radius
	 * @param x x position of the centre of the circle
	 * @param y y position of the centre of the circle
	 * @param r1 inner radius of the circle
	 * @param r2 outer radius of the circle
	 * @param thOffset the offset of the start of the segments
	 * @param slices number of sectors in the circle (poly count will be 2 * slices * Settings.segmentMultiplier)
	 */
	public void drawSectorCircle(float x, float y, float r1, float r2, float thOffset, int slices) {
		drawSectorArc(x, y, r1, r2, thOffset, (float)(2*Math.PI) + thOffset, slices);
	}
}
