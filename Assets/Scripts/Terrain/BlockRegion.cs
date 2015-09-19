using System;

public struct BlockRegion {

	public readonly BlockPos start, end;

	public int width { get { return (end.x - start.x + 1); } }
	public int depth { get { return (end.y - start.y + 1); } }
	public int height { get { return (end.z - start.z + 1); } }


	public BlockRegion(BlockPos start, BlockPos end) {
		this.start = start;
		this.end = end;
		if ((width < 0) || (height < 0) || (depth < 0))
			throw new ArgumentException(string.Format(
				"End position must be larger or equal to start" +
				"position for all dimensions ({0} : {1})", start, end));
	}

	public BlockRegion(int startX, int startY, int startZ,
	                   int endX, int endY, int endZ) :
		this(new BlockPos(startX, startY, startZ),
		     new BlockPos(endX, endY, endZ)) {  }


	/// <summary> Returns if the BlockPos is contained in this region. </summary>
	public bool Contains(BlockPos pos) {
		return ((pos.x >= start.x) && (pos.y >= start.y) && (pos.z >= start.z) &&
		        (pos.x <=   end.x) && (pos.y <=   end.y) && (pos.z <=   end.z));
	}


	/// <summary> Returns a region that is the specified number
	///           of blocks larger in all 6 directions. </summary>
	public BlockRegion Expand(int size){
		return new BlockRegion(start.Relative(-size, -size, -size),
		                       end.Relative(size, size, size));
	}


	#region ToString, Equals, GetHashCode and equality operators

	public override string ToString() {
		return string.Format ("[{0}:{1}]", start, end);
	}

	public override bool Equals(object obj) {
		BlockRegion region;
		return ((obj is BlockRegion) &&
			((region = (BlockRegion)obj).start == start) &&
			(region.end == end));
	}

	public override int GetHashCode() {
		return HashHelper.GetHashCode (start, end);
	}

	public static bool operator ==(BlockRegion lhs, BlockRegion rhs) {
		return lhs.Equals (rhs);
	}
	public static bool operator !=(BlockRegion lhs, BlockRegion rhs) {
		return !lhs.Equals (rhs);
	}

	#endregion

}
