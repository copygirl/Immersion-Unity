using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TerrainController : MonoBehaviour {

	public int width = 1;
	public int depth = 1;
	public int height = 1;


	Terrain terrain;

	void Start() {
		terrain = GetComponent<Terrain>();

		for (var x = -width / 2; x < width / 2; x++)
		for (var y = -depth / 2; y < depth / 2; y++)
		for (var z = -height / 2; z < height / 2; z++) {
			var chunk = terrain.CreateChunk(new ChunkPos(x, y, z));
			GenerateChunk(chunk);
			chunk.UpdateMesh();
		}
	}

	void GenerateChunk(IChunk chunk) {
		var earth = terrain.GetMaterialId(BlockMaterial.EARTH);
		var sand  = terrain.GetMaterialId(BlockMaterial.SAND);

		for (var x = 0; x < chunk.width; x++)
		for (var z = 0; z < chunk.height; z++) {
			var blockPos = chunk.position.ToBlockPos().Relative(x, 0, z);
			var h = Mathf.PerlinNoise(blockPos.x / 40.0F, blockPos.z / 40.0F) * 8 - blockPos.y;
			for (var y = 0; y < Mathf.Min(h, chunk.depth); y++) {
				var material = ((h > 3.5F) ? earth : sand);
				var amount = 1 + (int)Mathf.Min(BlockData.MAX_AMOUNT - 1,
				                                (h - y) * BlockData.MAX_AMOUNT);
				chunk[chunk.GetIndex(x, y, z)] = new BlockData(material, amount);
			}
		}
	}
	
}
