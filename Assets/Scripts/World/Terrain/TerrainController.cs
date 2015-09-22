using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class TerrainController : MonoBehaviour {

	public int width = 1;
	public int depth = 1;
	public int height = 1;


	void Start() {
		var terrain = GenerateTerrain(width, depth, height);
		var chunk = terrain[new ChunkPos(0, 0, 0)];
		var mesh = new SurfaceNetsMeshGenerator().Generate(new Mesh(), chunk, terrain);

		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	Terrain GenerateTerrain(int width, int depth, int height) {
		var terrain = new Terrain(width * Chunk.WIDTH, depth * Chunk.DEPTH, height * Chunk.HEIGHT);
		GenerateChunk(terrain, new ChunkPos(0, 0, 0));
		return terrain;
	}

	TerrainChunk GenerateChunk(Terrain terrain, ChunkPos pos) {
		var earth = terrain.GetMaterialId(BlockMaterial.EARTH);
		var sand  = terrain.GetMaterialId(BlockMaterial.SAND);

		var chunk = new TerrainChunk(terrain, pos);

		for (var x = 0; x < Chunk.WIDTH; x++)
		for (var z = 0; z < Chunk.DEPTH; z++) {
			var blockPos = pos.ToBlockPos().Relative(x, 0, z);
			var h = Mathf.PerlinNoise(blockPos.x / 40.0F, blockPos.z / 40.0F) * 8 - blockPos.y;
			for (var y = 0; y < Mathf.Min(h, Chunk.HEIGHT); y++) {
				var material = ((h > 3.5F) ? earth : sand);
				var amount = 1 + (int)Mathf.Min(BlockData.MAX_AMOUNT - 1,
				                                (h - y) * BlockData.MAX_AMOUNT);
				chunk[Chunk.GetIndex(x, y, z)] = new BlockData(material, amount);
			}
		}

		terrain[pos] = chunk;
		return chunk;
	}
	
}
