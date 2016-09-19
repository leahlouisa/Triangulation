using UnityEngine;
using System.Collections;

public class Triangle : MonoBehaviour {
	
	private int[] vertices = new int[3];

	public void SetVertices(int[] verts) {
		vertices = verts;
	}

	public int[] GetVertices() {
		return vertices;
	}

}
