
public static class Chunk {

	public const int SIZE = 16;


	public static int GetIndex(this IChunk chunk, int x, int y, int z) {
		return (x + (y * chunk.width) + (z * chunk.width * chunk.depth));
	}
	public static int GetIndex(this IChunk chunk, BlockPos pos) {
		return chunk.GetIndex(pos.x & 0x0F, pos.y & 0x0F, pos.z & 0x0F);
	}

}
