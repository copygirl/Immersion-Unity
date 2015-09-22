
public class TerrainChunk : Chunk {

	readonly BlockData[] _data = new BlockData[WIDTH * DEPTH * HEIGHT];

	public override BlockData this[int index] {
		get { return _data[index]; }
		set { _data[index] = value; }
	}

	public TerrainChunk(Terrain terrain, ChunkPos pos)
		: base(terrain, pos) {  }

}
