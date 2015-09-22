
public abstract class Chunk : IRawBlockAccess {

	public const int WIDTH  = 16;
	public const int DEPTH  = 16;
	public const int HEIGHT = 16;


	public readonly Terrain terrain;
	public readonly ChunkPos position;


	#region IRawBlockAccess implementation

	int IRawBlockAccess.width { get { return WIDTH; } }
	int IRawBlockAccess.depth { get { return DEPTH; } }
	int IRawBlockAccess.height { get { return HEIGHT; } }

	public abstract BlockData this[int index] { get; set; }

	#endregion


	public Chunk(Terrain terrain, ChunkPos pos) {
		this.terrain = terrain;
		position = pos;
	}


	public static int GetIndex(int x, int y, int z) {
		return ((x & 0x0F) + ((y & 0x0F) * WIDTH) + ((z & 0x0F) * WIDTH * DEPTH));
	}
	public static int GetIndex(BlockPos pos) {
		return GetIndex(pos.x, pos.y, pos.z);
	}

}
