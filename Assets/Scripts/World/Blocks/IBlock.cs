
/// <summary> Interface for IBlockStorage classes to
///           provide access to block information. </summary>
public interface IBlock {

	/// <summary> Returns the IBlockStorage this block is linked to. </summary>
	IBlockStorage storage { get; }

	/// <summary> Returns the position this block is linked to. </summary>
	BlockPos position { get; }


	/// <summary> Gets or sets the material of this block. </summary>
	BlockMaterial material { get; set; }

	/// <summary> Gets or sets the amount of material in this block.
	///           Valid values are 1 to BlockData.MAX_AMOUNT. </summary>
	int amount { get; set; }
	
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
