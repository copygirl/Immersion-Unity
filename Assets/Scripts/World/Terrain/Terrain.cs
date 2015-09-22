using System;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : IBlockStorage, IBlockMaterialLookup {

	readonly BlockMaterial[] _idToMaterial =
		new BlockMaterial[BlockData.MAX_MATERIAL_IDS];
	readonly Dictionary<BlockMaterial, byte> _materialToId =
		new Dictionary<BlockMaterial, byte>(BlockData.MAX_MATERIAL_IDS);

	readonly Dictionary<ChunkPos, TerrainChunk> _chunks =
		new Dictionary<ChunkPos, TerrainChunk>();


	#region IBlockStorage implementation

	public BlockRegion region { get; private set; }

	public IBlock this[BlockPos pos] {
		get {
			var chunk = this[ChunkPos.FromBlockPos(pos)];
			return new TerrainBlock(this, chunk, pos);
		}
	}

	#endregion


	public Chunk this[ChunkPos pos] {
		get {
			TerrainChunk chunk;
			return (_chunks.TryGetValue(pos, out chunk)
			        ? (Chunk)chunk : new EmptyChunk(this, pos, default(BlockData)));
		}
		set {
			// TODO: Refine this.
			if (value != null)
				_chunks[pos] = (TerrainChunk)value;
			else _chunks.Remove(pos);
		}
	}


	public Terrain(int width, int depth, int height) {
		int x = -width / 2;
		int y = -depth / 2;
		int z = -height / 2;
		region = new BlockRegion(x, y, z, width + x - 1, depth + y - 1, height + z - 1);

		RegisterMaterial(BlockMaterial.AIR);
		RegisterMaterial(BlockMaterial.EARTH);
		RegisterMaterial(BlockMaterial.SAND);
	}


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
