
/// <summary> IBlock implementation for Terrain / TerrainChunk blocks. </summary>
internal class TerrainBlock : IBlock {

	readonly Terrain _terrain;
	readonly IChunk _chunk;
	
	public IBlockStorage storage { get { return _terrain; } }
	public BlockPos position { get; private set; }
	
	
	public BlockMaterial material {
		get { return _terrain.GetMaterial(_chunk[_chunk.GetIndex(position)].material); }
		set { var index = _chunk.GetIndex(position);
		      _chunk[index] = new BlockData(_terrain.GetMaterialId(value),
		                                     _chunk[index].amount); }
	}
	
	public int amount {
		get { return _chunk[_chunk.GetIndex(position)].amount; }
		set { var index = _chunk.GetIndex(position);
		      _chunk[index] = new BlockData(_chunk[index].material, value); }
	}
	
	
	public TerrainBlock(Terrain terrain, IChunk access, BlockPos pos) {
		_terrain = terrain;
		_chunk = access;
		position = pos;
	}


	#region ToString, Equals and GetHashCode

	public override string ToString() {
		return string.Format(
			"[Block {1}, {2}, {3}/{4}]",
			position, material, amount, BlockData.MAX_AMOUNT);
	}

	public override bool Equals(object obj) {
		var block = (obj as IBlock);
		return ((block != null) &&
		        (block.storage == storage) &&
		        (block.position == position));
	}

	public override int GetHashCode() {
		return HashHelper.GetHashCode(storage, position);
	}

	#endregion
	
}
