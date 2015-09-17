using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class TerrainController : MonoBehaviour {

	public int width = 64;
	public int depth = 32;
	public int height = 64;


	void Start() {
		var terrain = GenerateTerrain(width, depth, height);
		var mesh = CreateTerrainMesh(terrain);

		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}


	Terrain GenerateTerrain(int width, int depth, int height) {
		var terrain = new Terrain(width, depth, height);

		for (var x = terrain.region.start.x; x <= terrain.region.end.x; x++)
			for (var z = terrain.region.start.z; z <= terrain.region.end.z; z++) {
			terrain[new BlockPos(x, terrain.region.start.y + 10, z)].material = Terrain.EARTH;
			if (Random.value > 0.1f) {
				terrain[new BlockPos(x, terrain.region.start.y + 11, z)].material = Terrain.EARTH;
				if (Random.value > 0.9f)
					terrain[new BlockPos(x, terrain.region.start.y + 12, z)].material = Terrain.EARTH;
			}
		}

		return terrain;
	}

	Mesh CreateTerrainMesh(Terrain terrain) {
		var mesh = new Mesh();

		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<int> triangles = new List<int>();

		for (var x = terrain.region.start.x; x <= terrain.region.end.x; x++)
			for (var y = terrain.region.start.y; y <= terrain.region.end.y; y++)
				for (var z = terrain.region.start.z; z <= terrain.region.end.z; z++) {
				var block = terrain[new BlockPos(x, y, z)];
				if (block.material != Terrain.EARTH) continue;

				int index = vertices.Count;

				vertices.Add(new Vector3(x,     y, z));
				vertices.Add(new Vector3(x + 1, y, z));
				vertices.Add(new Vector3(x + 1, y, z + 1));
				vertices.Add(new Vector3(x,     y, z + 1));

				normals.Add(Vector3.up);
				normals.Add(Vector3.up);
				normals.Add(Vector3.up);
				normals.Add(Vector3.up);
				
				triangles.Add(index);
				triangles.Add(index + 3);
				triangles.Add(index + 1);

				triangles.Add(index + 1);
				triangles.Add(index + 3);
				triangles.Add(index + 2);
			}

		mesh.vertices = vertices.ToArray();
		mesh.normals = normals.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.Optimize();
		
		return mesh;
	}

}
