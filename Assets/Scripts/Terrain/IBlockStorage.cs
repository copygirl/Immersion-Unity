
/// <summary> Interface for classes that provide block lookup. </summary>
public interface IBlockStorage {

	/// <summary> Gets the region this block storage occupies. </summary>
	BlockRegion region { get; }

	/// <summary> Gets a block reference to the specified position.
	///           If the position is outside the region, an empty block
	///           or one from a connected IBlockStorage is returned. </summary>
	IBlock this[BlockPos pos] { get; }

}
