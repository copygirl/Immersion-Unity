
public interface IChunk : IRawBlockAccess {

	Terrain terrain { get; }

	ChunkPos position { get; }

}
