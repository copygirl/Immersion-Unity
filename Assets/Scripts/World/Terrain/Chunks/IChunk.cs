
public interface IChunk : IRawBlockAccess {

	Terrain terrain { get; }

	ChunkPos position { get; }

}

public static class ChunkExtensions {

	public static IChunk Relative(this IChunk chunk, int x, int y, int z) {
		return chunk.terrain[chunk.position.Relative(x, y, z)];
	}

}
