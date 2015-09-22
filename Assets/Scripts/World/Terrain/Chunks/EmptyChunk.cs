
public class EmptyChunk : Chunk {

	readonly BlockData _default;

	public override BlockData this[int index] {
		get { return _default; }
		set { /* Zhe chunks, zhey do nozhing! */ }
	}

	public EmptyChunk(Terrain terrain, ChunkPos pos, BlockData @default)
		: base(terrain, pos) { _default = @default; }

}
