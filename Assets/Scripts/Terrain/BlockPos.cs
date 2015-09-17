
public struct BlockPos {

	public readonly int x, y, z;

	public BlockPos(int x, int y, int z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}


	public BlockPos Relative(int x, int y, int z) {
		return new BlockPos(this.x + x, this.y + y, this.z + z);
	}


	#region ToString, Equals, GetHashCode and equality operators

	public override string ToString() {
		return string.Format("({0},{1},{2})", x, y, z);
	}

	public override bool Equals(object obj) {
		BlockPos pos;
		return ((obj is BlockPos) &&
		        ((pos = (BlockPos)obj).x == x) &&
		        (pos.y == y) && (pos.z == z));
	}

	public override int GetHashCode() {
		return HashHelper.GetHashCode(x, y, z);
	}

	public static bool operator ==(BlockPos lhs, BlockPos rhs) {
		return lhs.Equals(rhs);
	}
	public static bool operator !=(BlockPos lhs, BlockPos rhs) {
		return !lhs.Equals(rhs);
	}

	#endregion

}
