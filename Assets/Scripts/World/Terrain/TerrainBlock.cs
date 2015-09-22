
/// <summary> IBlock implementation for Terrain / TerrainChunk blocks. </summary>
internal class TerrainBlock : IBlock {

	readonly Terrain _terrain;
	readonly IRawBlockAccess _access;
	
	public IBlockStorage storage { get { return _terrain; } }
	public BlockPos position { get; private set; }
	
	
	public BlockMaterial material {
		get { return _terrain.GetMaterial(_access[Chunk.GetIndex(position)].material); }
		set { var index = Chunk.GetIndex(position);
		      _access[index] = new BlockData(_terrain.GetMaterialId(value),
		                                     _access[index].amount); }
	}
	
	public int amount {
		get { return _access[Chunk.GetIndex(position)].amount; }
		set { var index = Chunk.GetIndex(position);
		      _access[index] = new BlockData(_access[index].material, value); }
	}
	
	
	public TerrainBlock(Terrain terrain, IRawBlockAccess access, BlockPos pos) {
		_terrain = terrain;
		_access = access;
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
