//
// "Surface Nets" mesh generator
// Information: http://0fps.net/2012/07/12/smooth-voxel-terrain-part-2/
// (Adapted from Mikola Lysenko's Javascript version.)
// Source: https://github.com/mikolalysenko/mikolalysenko.github.com/blob/master/Isosurface/js/surfacenets.js
//

using System.Collections.Generic;
using UnityEngine;

public class SurfaceNetsMeshGenerator {

	static readonly int[] _cubeEdges;
	static readonly int[] _edgeTable;

	static SurfaceNetsMeshGenerator() {
		// Precompute edge table, like Paul Bourke does.
		// This saves a bit of time when computing the centroid of each boundary cell.

		// Initialize the cube_edges table.
		// This is just the vertex number of each cube.
		_cubeEdges = new int[24];
		var k = 0;
		for (var i = 0; i < 8; i++)
			for (var j = 1; j <= 4; j <<= 1) {
			var p = i ^ j;
			if (i <= p) {
				_cubeEdges[k++] = i;
				_cubeEdges[k++] = p;
			}
		}

		// Initialize the intersection table.
		// This is a 2^(cube configuration) -> 2^(edge configuration) map.
		// There is one entry for each possible cube configuration, and the
		// output is a 12-bit vector enumerating all edges crossing the 0-level.
		_edgeTable = new int[256];
		for (var i = 0; i < 256; i++) {
			var em = 0;
			for (var j = 0; j < 24; j += 2) {
				var a = (i & (1 << _cubeEdges[j])) != 0;
				var b = (i & (1 << _cubeEdges[j + 1])) != 0;
				em |= (a != b) ? (1 << (j >> 1)) : 0;
			}
			_edgeTable[i] = em;
		}
	}


	public Mesh Generate(Mesh mesh, IBlockStorage storage) {

		var region = storage.region.Expand(1);

		var vertices = new List<Vector3>();
		var normals  = new List<Vector3>();
		var colors   = new List<Color>();
		var indices  = new List<int>();

		var pos = new int[3];
		var R = new int[]{ 1, (region.width + 1), (region.width + 1) * (region.depth + 1) };
		var grid = new IBlock[8];
		var bufNo = 1;

		var vertexBuffer = new Vector3[R[2] * 2];
		
		for (pos[2] = 0; pos[2] < region.height - 1; pos[2]++, bufNo ^= 1, R[2] = -R[2]) {

			// m is the pointer into the buffer we are going to use. 
			var m = 1 + (region.width + 1) * (1 + bufNo * (region.depth + 1));

			for (pos[1] = 0; pos[1] < region.depth - 1; pos[1]++, m += 2)
			for (pos[0] = 0; pos[0] < region.width - 1; pos[0]++, m++) {
				
				// Read in 8 field values around this vertex and store them in an array.
				// Also calculate 8-bit mask, like in marching cubes, so we can speed up sign checks later.
				var mask = 0;
				var g = 0;
				for (var z = 0; z < 2; z++)
				for (var y = 0; y < 2; y++)
				for (var x = 0; x < 2; x++, g++) {
					var blockPos = new BlockPos(region.start.x + pos[0] + x,
					                            region.start.y + pos[1] + y,
					                            region.start.z + pos[2] + z);
					grid[g] = storage[blockPos];
					mask |= !grid[g].material.solid ? (1 << g) : 0;
				}
				
				// Check for early termination if cell does not intersect boundary.
				if ((mask == 0) || (mask == 0xFF))
					continue;
				
				// Sum up edge intersections.
				var edgeMask = _edgeTable[mask];
				var v = new Vector3();
				var eCount = 0;
				
				// For every edge of the cube...
				for (var i = 0; i < 12; i++) {
					
					// Use edge mask to check if it is crossed.
					if((edgeMask & (1 << i)) == 0)
						continue;
					
					// If it did, increment number of edge crossings.
					eCount++;
					
					// Now find the point of intersection.
					var e0 = _cubeEdges[i << 1];           // Unpack vertices.
					var e1 = _cubeEdges[(i << 1) + 1];
					var g0 = (grid[e0].amount - 2) / 3.0F; // Unpack grid values.
					var g1 = (grid[e1].amount - 2) / 3.0F;
					var t  = g0 - g1;                      // Compute point of intersection.

					if (Mathf.Abs(t) <= 1e-6)
						continue;
					t = g0 / t;
					
					// Interpolate vertices and add up intersections (this can be done without multiplying).
					for (int j = 0, k = 1; j < 3; j++, k <<= 1) {
						var a = e0 & k;
						var b = e1 & k;
						if (a != b) v[j] += (a != 0) ? (1.0F - t) : t;
						else v[j] += (a != 0) ? 1.0F : 0.0F;
					}
				}
				
				// Now we just average the edge intersections and add them to coordinate.
				var s = 1.0F / eCount;
				for (var i = 0; i < 3; i++)
					v[i] = pos[i] + s * v[i];
				
				// Add vertex to buffer.
				vertexBuffer[m] = v;
				
				// Now we need to add faces together, to do this we just loop over 3 basic components.
				for (var i = 0; i < 3; i++) {
					// The first three entries of the edge_mask count the crossings along the edge.
					if((edgeMask & (1 << i)) == 0)
						continue;
					
					// i = axes we are point along. iu, iv = orthogonal axes.
					var iu = (i + 1) % 3;
					var iv = (i + 2) % 3;
					
					// If we are on a boundary, skip it.
					if ((pos[iu] < 0) || (pos[iv] < 0))
						continue;
					
					// Otherwise, look up adjacent edges in buffer.
					var du = R[iu];
					var dv = R[iv];

					var v1 = vertexBuffer[m];
					var v2 = vertexBuffer[m - du];
					var v3 = vertexBuffer[m - du - dv];
					var v4 = vertexBuffer[m - dv];

					var normal1 = Vector3.Cross(v2 - v1, v3 - v1).normalized;
					var normal2 = Vector3.Cross(v3 - v1, v4 - v1).normalized;

					// Remember to flip orientation depending on the sign of the corner.
					if ((mask & 1) == 0) {
						var color = grid[0].material.color;
						AddFlatTriangle(vertices, normals, colors, indices, v1, v2, v3, normal1, color);
						AddFlatTriangle(vertices, normals, colors, indices, v1, v3, v4, normal2, color);
					} else {
						var color = grid[1 << i].material.color;
						AddFlatTriangle(vertices, normals, colors, indices, v4, v2, v1, -normal1, color);
						AddFlatTriangle(vertices, normals, colors, indices, v4, v3, v2, -normal2, color);
					}
				}
			}
		}

		mesh.Clear();
		mesh.vertices  = vertices.ToArray();
		mesh.normals   = normals.ToArray();
		mesh.colors    = colors.ToArray();
		mesh.triangles = indices.ToArray();
		mesh.Optimize();

		return mesh;

	}


	static void AddVertex(List<Vector3> vertices, List<Vector3> normals, List<Color> colors, List<int> indices,
	                      Vector3 vertex, Vector3 normal, Color color) {
		var index = vertices.Count;
		vertices.Add(vertex);
		normals.Add(normal);
		colors.Add(color);
		indices.Add(index);
	}

	static void AddFlatTriangle(List<Vector3> vertices, List<Vector3> normals, List<Color> colors, List<int> indices,
	                            Vector3 v1, Vector3 v2, Vector3 v3, Vector3 normal, Color color) {
		AddVertex(vertices, normals, colors, indices, v1, normal, color);
		AddVertex(vertices, normals, colors, indices, v2, normal, color);
		AddVertex(vertices, normals, colors, indices, v3, normal, color);
	}

}
