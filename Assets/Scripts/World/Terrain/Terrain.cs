using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Terrain : MonoBehaviour, IBlockStorage, IBlockMaterialLookup {

	readonly BlockMaterial[] _idToMaterial =
		new BlockMaterial[BlockData.MAX_MATERIAL_IDS];
	readonly Dictionary<BlockMaterial, byte> _materialToId =
		new Dictionary<BlockMaterial, byte>(BlockData.MAX_MATERIAL_IDS);

	readonly Dictionary<ChunkPos, TerrainChunk> _chunks =
		new Dictionary<ChunkPos, TerrainChunk>();


	public IChunk this[ChunkPos pos] {
		get {
			TerrainChunk chunk;
			return (_chunks.TryGetValue(pos, out chunk)
			        ? (IChunk)chunk : new EmptyChunk(this, pos, default(BlockData)));
		}
	}


	public Terrain() {
		RegisterMaterial(BlockMaterial.AIR);
		RegisterMaterial(BlockMaterial.EARTH);
		RegisterMaterial(BlockMaterial.SAND);
	}


	public TerrainChunk CreateChunk(ChunkPos pos) {
		if (_chunks.ContainsKey(pos))
			throw new InvalidOperationException(string.Format(
				"Chunk at {0} already exists", pos));

		var chunkObject = new GameObject("Chunk: " + pos);
		var chunk = chunkObject.AddComponent<TerrainChunk>();

		chunk.terrain = this;
		chunk.position = pos;

		chunk.transform.parent = transform;
		chunk.transform.localPosition = pos.ToVector3();
		chunk.transform.localRotation = Quaternion.identity;
		chunk.transform.localScale = Vector3.one;

		_chunks[pos] = chunk;
		return chunk;
	}


	#region IBlockStorage implementation
	
	public BlockRegion region { get; set; }
	
	public IBlock this[BlockPos pos] {
		get {
			var chunk = this[ChunkPos.FromBlockPos(pos)];
			return new TerrainBlock(this, chunk, pos);
		}
	}
	
	#endregion

	#region IBlockMaterialLookup implementation

	public int GetMaterialId(BlockMaterial material) {
		return _materialToId[material];
	}

	public BlockMaterial GetMaterial(int materialId) {
		return _idToMaterial[materialId];
	}

	public int RegisterMaterial(BlockMaterial material) {
		var id = (byte)_materialToId.Count;
		_idToMaterial[id] = material;
		_materialToId[material] = id;
		return id;
	}

	#endregion

}
