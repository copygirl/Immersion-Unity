
public class EmptyChunk : IChunk {

	readonly BlockData _default;
	
	#region IChunk implementation
	
	public Terrain terrain { get; private set; }
	
	public ChunkPos position { get; private set; }
	
	#endregion
	
	#region IRawBlockAccess implementation
	
	public int width { get { return Chunk.SIZE; } }
	public int depth { get { return Chunk.SIZE; } }
	public int height { get { return Chunk.SIZE; } }
	
	public BlockData this[int index] {
		get { return _default; }
		set { /* Zhe chunks, zhey do nozhing! */ }
	}
	
	#endregion

	public EmptyChunk(Terrain terrain, ChunkPos pos, BlockData @default) {
		this.terrain = terrain;
		position = pos;
		_default = @default;
	}

}
