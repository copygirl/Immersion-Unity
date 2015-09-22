
// TODO: Currently not being used due to EmptyChunk. Remove?
/// <summary> Empty implementation of IBlock interface.
///           References a block outside the boundary of the IBlockStorage.
///           Returns AIR as its material, setting the material does nothing. </summary>
public class EmptyBlock : IBlock {
	
	public IBlockStorage storage { get; private set; }
	
	public BlockPos position { get; private set; }


	public BlockMaterial material {
		get { return BlockMaterial.AIR; }
		set { /* No operation */ }
	}

	public int amount {
		get { return BlockData.MAX_AMOUNT; }
		set { /* No operation */ }
	}


	public EmptyBlock(IBlockStorage storage, BlockPos pos) {
		this.storage = storage;
		this.position = pos;
	}


	#region ToString, Equals and GetHashCode

	public override string ToString() {
		return string.Format("[Block {0}, {1}, <EMPTY>]", position, material);
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
