
/// <summary> Interface for IBlockStorage classes to
///           provide access to block information. </summary>
public interface IBlock {

	/// <summary> Returns the IBlockStorage this block is linked to. </summary>
	IBlockStorage storage { get; }

	/// <summary> Returns the position this block is linked to. </summary>
	BlockPos position { get; }


	/// <summary> Gets or sets the material of this block. </summary>
	BlockMaterial material { get; set; }

	/// <summary> Gets or sets the amount of material in this block (1 to 4). </summary>
	int amount { get; set; }
	
}

/// <summary> Empty implementation of IBlock interface.
///           References a block outside the boundary of the IBlockStorage.
///           Returns AIR as its material, setting the material does nothing. </summary>
public struct EmptyBlock : IBlock {
	
	public IBlockStorage storage { get; private set; }
	
	public BlockPos position { get; private set; }


	public BlockMaterial material {
		get { return BlockMaterial.AIR; }
		set { /* No operation */ }
	}

	public int amount {
		get { return 4; }
		set { /* No operation */ }
	}


	public EmptyBlock(IBlockStorage storage, BlockPos pos) : this() {
		this.storage = storage;
		this.position = pos;
	}
	
}

public static class BlockExtensions {

	/// <summary> Returns another block from the same
	///           IBlockStorage relative to this block. </summary>
	public static IBlock Relative(this IBlock block, int x, int y, int z) {
		return block.storage[block.position.Relative(x, y, z)];
	}

	/// <summary> Returns a neighboring block from the same IBlockStorage. </summary>
	public static IBlock Neighbor(this IBlock block, BlockFacing face) {
		return block.storage[face.MoveRelative(block.position)];
	}

}
