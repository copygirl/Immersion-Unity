
/// <summary> IBlock implementation for Terrain / TerrainChunk blocks. </summary>
internal class TerrainBlock : IBlock {

	readonly Terrain _terrain;
	readonly IRawBlockAccess _access;
	readonly int _index;
	
	public IBlockStorage storage { get { return _terrain; } }
	public BlockPos position { get; private set; }
	
	
	public BlockMaterial material {
		get { return _terrain.GetMaterial(_access.blockData[_index].material); }
		set { _access.blockData[_index] = new BlockData(_terrain.GetMaterialId(value),
		                                                _access.blockData[_index].amount); }
	}
	
	public int amount {
		get { return _access.blockData[_index].amount; }
		set { _access.blockData[_index] = new BlockData(_access.blockData[_index].material, value); }
	}
	
	
	public TerrainBlock(Terrain terrain, BlockPos pos,
	                    IRawBlockAccess access, int index) {
		_access = terrain;
		position = pos;
		_access = access;
		_index = index;
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
