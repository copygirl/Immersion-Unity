
public static class Chunk {

	public const int SIZE = 16;


	public static int GetIndex(this IChunk chunk, int x, int y, int z) {
		return ((x & 0x0F) | ((y & 0x0F) << 4) | ((z & 0x0F) << 8));
	}
	public static int GetIndex(this IChunk chunk, BlockPos pos) {
		return chunk.GetIndex(pos.x, pos.y, pos.z);
	}

}
