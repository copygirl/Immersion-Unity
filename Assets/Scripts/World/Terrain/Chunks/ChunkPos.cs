
public struct ChunkPos {

	public readonly int x, y, z;

	public ChunkPos(int x, int y, int z) : this() {
		this.x = x; this.y = y; this.z = z;
	}


	public static ChunkPos FromBlockPos(BlockPos pos) {
		return new ChunkPos(pos.x >> 4, pos.y >> 4, pos.z >> 4);
	}

	public BlockPos ToBlockPos() {
		return new BlockPos(x << 4, y << 4, z << 4);
	}


	#region ToString, Equals and GetHashCode
	
	public override string ToString() {
		return string.Format("[ChunkPos ({0},{1},{2})]", x, y, z);
	}
	
	public override bool Equals(object obj) {
		ChunkPos pos;
		return ((obj is ChunkPos) &&
		        ((pos = (ChunkPos)obj).x == x) &&
		        (pos.y == y) && (pos.z == z));
	}
	
	public override int GetHashCode() {
		return HashHelper.GetHashCode(x, y, z);
	}
	
	#endregion

}
