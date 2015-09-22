using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

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

		IBlock block;
		for (var x = terrain.region.start.x; x <= terrain.region.end.x; x++)
		for (var z = terrain.region.start.z; z <= terrain.region.end.z; z++) {
			var h = Mathf.PerlinNoise((x - terrain.region.start.x) / 40.0F,
			                          (z - terrain.region.start.z) / 40.0F) * 8;
			for (int y = terrain.region.start.y; y < terrain.region.start.y + h; y++) {
				block = terrain[new BlockPos(x, y, z)];
				block.material = h > 3.5F ? BlockMaterial.EARTH : BlockMaterial.SAND;
				block.amount = 1 + (int)Mathf.Min(BlockData.MAX_AMOUNT - 1,
						(h - (y - terrain.region.start.y)) * BlockData.MAX_AMOUNT);
			}
		}

		return terrain;
	}

}
