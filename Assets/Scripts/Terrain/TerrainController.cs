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
		var mesh = new SurfaceNetsMeshGenerator().Generate(new Mesh(), terrain);

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

}
