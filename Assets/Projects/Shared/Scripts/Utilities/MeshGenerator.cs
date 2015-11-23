﻿using UnityEngine;
using System.Collections;

public class MeshGenerator  {

	public static Mesh GenerateTorus(float height,int nbSides,float innerRadius,float outterRadius)
	{
		Mesh mesh = new Mesh();
		mesh.Clear();

		
		// Outter shell is at radius1 + radius2 / 2, inner shell at radius1 - radius2 / 2
		float bottomRadius1 = outterRadius;
		float bottomRadius2 = innerRadius; 
		float topRadius1 = outterRadius;
		float topRadius2 = innerRadius;
		
		int nbVerticesCap = nbSides * 2 + 2;
		int nbVerticesSides = nbSides * 2 + 2;
		#region Vertices
		
		// bottom + top + sides
		Vector3[] vertices = new Vector3[nbVerticesCap * 2 + nbVerticesSides * 2];
		int vert = 0;
		float _2pi = Mathf.PI * 2f;
		
		// Bottom cap
		int sideCounter = 0;
		while( vert < nbVerticesCap )
		{
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			float cos = Mathf.Cos(r1);
			float sin = Mathf.Sin(r1);
			vertices[vert] = new Vector3( cos * (bottomRadius1 - bottomRadius2 * .5f), 0f, sin * (bottomRadius1 - bottomRadius2 * .5f));
			vertices[vert+1] = new Vector3( cos * (bottomRadius1 + bottomRadius2 * .5f), 0f, sin * (bottomRadius1 + bottomRadius2 * .5f));
			vert += 2;
		}
		
		// Top cap
		sideCounter = 0;
		while( vert < nbVerticesCap * 2 )
		{
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			float cos = Mathf.Cos(r1);
			float sin = Mathf.Sin(r1);
			vertices[vert] = new Vector3( cos * (topRadius1 - topRadius2 * .5f), height, sin * (topRadius1 - topRadius2 * .5f));
			vertices[vert+1] = new Vector3( cos * (topRadius1 + topRadius2 * .5f), height, sin * (topRadius1 + topRadius2 * .5f));
			vert += 2;
		}
		
		// Sides (out)
		sideCounter = 0;
		while (vert < nbVerticesCap * 2 + nbVerticesSides )
		{
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			float cos = Mathf.Cos(r1);
			float sin = Mathf.Sin(r1);
			
			vertices[vert] = new Vector3(cos * (topRadius1 + topRadius2 * .5f), height, sin * (topRadius1 + topRadius2 * .5f));
			vertices[vert + 1] = new Vector3(cos * (bottomRadius1 + bottomRadius2 * .5f), 0, sin * (bottomRadius1 + bottomRadius2 * .5f));
			vert+=2;
		}
		
		// Sides (in)
		sideCounter = 0;
		while (vert < vertices.Length )
		{
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			float cos = Mathf.Cos(r1);
			float sin = Mathf.Sin(r1);
			
			vertices[vert] = new Vector3(cos * (topRadius1 - topRadius2 * .5f), height, sin * (topRadius1 - topRadius2 * .5f));
			vertices[vert + 1] = new Vector3(cos * (bottomRadius1 - bottomRadius2 * .5f), 0, sin * (bottomRadius1 - bottomRadius2 * .5f));
			vert += 2;
		}
		#endregion
		
		#region Normales
		
		// bottom + top + sides
		Vector3[] normales = new Vector3[vertices.Length];
		vert = 0;
		
		// Bottom cap
		while( vert < nbVerticesCap )
		{
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			
			normales[vert] = new Vector3(Mathf.Cos(r1), 0f, Mathf.Sin(r1));
			normales[vert+1] = -normales[vert];
			vert+=2;
		}
		
		// Top cap
		while( vert < nbVerticesCap * 2 )
		{
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			
			normales[vert] = new Vector3(Mathf.Cos(r1), 0f, Mathf.Sin(r1));
			normales[vert+1] = -normales[vert];
			vert+=2;
		}
		
		// Sides (out)
		sideCounter = 0;
		while (vert < nbVerticesCap * 2 + nbVerticesSides )
		{
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			
			normales[vert] = new Vector3(Mathf.Cos(r1), 0f, Mathf.Sin(r1));
			normales[vert+1] = normales[vert];
			vert+=2;
		}
		
		// Sides (in)
		sideCounter = 0;
		while (vert < vertices.Length )
		{
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			
			normales[vert] = -(new Vector3(Mathf.Cos(r1), 0f, Mathf.Sin(r1)));
			normales[vert+1] = normales[vert];
			vert+=2;
		}
		#endregion
		
		#region UVs
		Vector2[] uvs = new Vector2[vertices.Length];
		
		vert = 0;
		// Bottom cap
		sideCounter = 0;
		while( vert < nbVerticesCap )
		{
			float t = (float)(sideCounter++) / nbSides;
			uvs[ vert++ ] = new Vector2( 0f, t );
			uvs[ vert++ ] = new Vector2( 1f, t );
		}
		
		// Top cap
		sideCounter = 0;
		while( vert < nbVerticesCap * 2 )
		{
			float t = (float)(sideCounter++) / nbSides;
			uvs[ vert++ ] = new Vector2( 0f, t );
			uvs[ vert++ ] = new Vector2( 1f, t );
		}
		
		// Sides (out)
		sideCounter = 0;
		while (vert < nbVerticesCap * 2 + nbVerticesSides )
		{
			float t = (float)(sideCounter++) / nbSides;
			uvs[ vert++ ] = new Vector2( t, 0f );
			uvs[ vert++ ] = new Vector2( t, 1f );
		}
		
		// Sides (in)
		sideCounter = 0;
		while (vert < vertices.Length )
		{
			float t = (float)(sideCounter++) / nbSides;
			uvs[ vert++ ] = new Vector2( t, 0f );
			uvs[ vert++ ] = new Vector2( t, 1f );
		}
		#endregion
		
		#region Triangles
		int nbFace = nbSides * 4;
		int nbTriangles = nbFace * 2;
		int nbIndexes = nbTriangles * 3;
		int[] triangles = new int[nbIndexes];
		
		// Bottom cap
		int i = 0;
		sideCounter = 0;
		while (sideCounter < nbSides)
		{
			int current = sideCounter * 2;
			int next = sideCounter * 2 + 2;
			
			triangles[ i++ ] = next + 1;
			triangles[ i++ ] = next;
			triangles[ i++ ] = current;
			
			triangles[ i++ ] = current + 1;
			triangles[ i++ ] = next + 1;
			triangles[ i++ ] = current;
			
			sideCounter++;
		}
		
		// Top cap
		while (sideCounter < nbSides * 2)
		{
			int current = sideCounter * 2 + 2;
			int next = sideCounter * 2 + 4;
			
			triangles[ i++ ] = current;
			triangles[ i++ ] = next;
			triangles[ i++ ] = next + 1;
			
			triangles[ i++ ] = current;
			triangles[ i++ ] = next + 1;
			triangles[ i++ ] = current + 1;
			
			sideCounter++;
		}
		
		// Sides (out)
		while( sideCounter < nbSides * 3 )
		{
			int current = sideCounter * 2 + 4;
			int next = sideCounter * 2 + 6;
			
			triangles[ i++ ] = current;
			triangles[ i++ ] = next;
			triangles[ i++ ] = next + 1;
			
			triangles[ i++ ] = current;
			triangles[ i++ ] = next + 1;
			triangles[ i++ ] = current + 1;
			
			sideCounter++;
		}
		
		
		// Sides (in)
		while( sideCounter < nbSides * 4 )
		{
			int current = sideCounter * 2 + 6;
			int next = sideCounter * 2 + 8;
			
			triangles[ i++ ] = next + 1;
			triangles[ i++ ] = next;
			triangles[ i++ ] = current;
			
			triangles[ i++ ] = current + 1;
			triangles[ i++ ] = next + 1;
			triangles[ i++ ] = current;
			
			sideCounter++;
		}
		#endregion
		
		mesh.vertices = vertices;
		mesh.normals = normales;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		
		mesh.RecalculateBounds();
		mesh.RecalculateNormals ();
		mesh.Optimize();

		return mesh;
	}

	public static void ScaleTorus(Mesh mesh,float height,float innerRadius,float outterRadius,int nbSides)
	{
		Vector3[] vertices = mesh.vertices;
		
		
		// Outter shell is at radius1 + radius2 / 2, inner shell at radius1 - radius2 / 2
		float bottomRadius1 = outterRadius;
		float bottomRadius2 = innerRadius; 
		float topRadius1 = outterRadius;
		float topRadius2 = innerRadius;
		
		int nbVerticesCap = nbSides * 2 + 2;
		int nbVerticesSides = nbSides * 2 + 2;
		
		// bottom + top + sides
		int vert = 0;
		float _2pi = Mathf.PI * 2f;
		
		// Bottom cap
		int sideCounter = 0;
		while( vert < nbVerticesCap )
		{
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			float cos = Mathf.Cos(r1);
			float sin = Mathf.Sin(r1);
			vertices[vert] = new Vector3( cos * (bottomRadius1 - bottomRadius2 * .5f), 0f, sin * (bottomRadius1 - bottomRadius2 * .5f));
			vertices[vert+1] = new Vector3( cos * (bottomRadius1 + bottomRadius2 * .5f), 0f, sin * (bottomRadius1 + bottomRadius2 * .5f));
			vert += 2;
		}
		
		// Top cap
		sideCounter = 0;
		while( vert < nbVerticesCap * 2 )
		{
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			float cos = Mathf.Cos(r1);
			float sin = Mathf.Sin(r1);
			vertices[vert] = new Vector3( cos * (topRadius1 - topRadius2 * .5f), height, sin * (topRadius1 - topRadius2 * .5f));
			vertices[vert+1] = new Vector3( cos * (topRadius1 + topRadius2 * .5f), height, sin * (topRadius1 + topRadius2 * .5f));
			vert += 2;
		}
		
		// Sides (out)
		sideCounter = 0;
		while (vert < nbVerticesCap * 2 + nbVerticesSides )
		{
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			float cos = Mathf.Cos(r1);
			float sin = Mathf.Sin(r1);
			
			vertices[vert] = new Vector3(cos * (topRadius1 + topRadius2 * .5f), height, sin * (topRadius1 + topRadius2 * .5f));
			vertices[vert + 1] = new Vector3(cos * (bottomRadius1 + bottomRadius2 * .5f), 0, sin * (bottomRadius1 + bottomRadius2 * .5f));
			vert+=2;
		}
		
		// Sides (in)
		sideCounter = 0;
		while (vert < vertices.Length )
		{
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			float cos = Mathf.Cos(r1);
			float sin = Mathf.Sin(r1);
			
			vertices[vert] = new Vector3(cos * (topRadius1 - topRadius2 * .5f), height, sin * (topRadius1 - topRadius2 * .5f));
			vertices[vert + 1] = new Vector3(cos * (bottomRadius1 - bottomRadius2 * .5f), 0, sin * (bottomRadius1 - bottomRadius2 * .5f));
			vert += 2;
		}
		mesh.vertices=vertices;
		
		mesh.RecalculateBounds();
		mesh.RecalculateNormals ();
		mesh.Optimize();
	}
	
	public static Mesh GenerateRing(int nbSides,float innerRadius,float outterRadius)
	{
		Mesh mesh = new Mesh();
		mesh.Clear();
		
		
		// Outter shell is at radius1 + radius2 / 2, inner shell at radius1 - radius2 / 2
		float bottomRadius1 = outterRadius;
		float bottomRadius2 = innerRadius; 
		float topRadius1 = outterRadius;
		float topRadius2 = innerRadius;
		
		int nbVerticesCap = nbSides * 2 + 2;
		int nbVerticesSides = nbSides * 2 + 2;
		#region Vertices
		
		// bottom + top + sides
		Vector3[] vertices = new Vector3[nbVerticesCap ];
		int vert = 0;
		float _2pi = Mathf.PI * 2f;
		
		// Bottom cap
		int sideCounter = 0;
		while( vert < nbVerticesCap )
		{
			sideCounter = sideCounter == nbSides ? 0 : sideCounter;
			
			float r1 = (float)(sideCounter++) / nbSides * _2pi;
			float cos = Mathf.Cos(r1);
			float sin = Mathf.Sin(r1);
			vertices[vert] = new Vector3( cos * (bottomRadius1 - bottomRadius2 * .5f), 0f, sin * (bottomRadius1 - bottomRadius2 * .5f));
			vertices[vert+1] = new Vector3( cos * (bottomRadius1 + bottomRadius2 * .5f), 0f, sin * (bottomRadius1 + bottomRadius2 * .5f));
			vert += 2;
		}

		#endregion
		
		#region Normales
		
		// bottom + top + sides
		Vector3[] normales = new Vector3[vertices.Length];
		vert = 0;
		
		// Bottom cap
		while( vert < nbVerticesCap )
		{
			normales[vert++] = Vector3.down;
		}
		#endregion
		
		#region UVs
		Vector2[] uvs = new Vector2[vertices.Length];
		
		vert = 0;
		// Bottom cap
		sideCounter = 0;
		while( vert < nbVerticesCap )
		{
			float t = (float)(sideCounter++) / nbSides;
			uvs[ vert++ ] = new Vector2( 0f, t );
			uvs[ vert++ ] = new Vector2( 1f, t );
		}

		#endregion
		
		#region Triangles
		int nbIndexes = nbSides * 6;
		int[] triangles = new int[nbIndexes];
		
		// Bottom cap
		int i = 0;
		sideCounter = 0;
		while (sideCounter < nbSides )
		{
			int current = sideCounter  + 2;
			int next = sideCounter + 4;
			
			triangles[ i++ ] = current;
			triangles[ i++ ] = next;
			triangles[ i++ ] = next + 1;
			
			triangles[ i++ ] = current;
			triangles[ i++ ] = next + 1;
			triangles[ i++ ] = current + 1;
			
			sideCounter++;
		}

		#endregion
		
		mesh.vertices = vertices;
		mesh.normals = normales;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		
		mesh.RecalculateBounds();
		mesh.Optimize();
		
		return mesh;
	}
}