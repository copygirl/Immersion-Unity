
public struct TerrainBlock : IBlock {
	
	readonly Terrain _terrain;
	
	
	public IBlockStorage storage { get { return _terrain; } }
	
	public BlockPos position { get; private set; }
	
	
	int index { get {
			var x = position.x - _terrain.region.start.x;
			var y = position.y - _terrain.region.start.y;
			var z = position.z - _terrain.region.start.z;
			return (x + (y * _terrain.region.width) +
			        (z * _terrain.region.width * _terrain.region.depth));
		} }
	
	
	public BlockMaterial material {
		get { return _terrain.GetMaterial(_terrain.blockData[index].material); }
		set { _terrain.blockData[index] = new BlockData(_terrain.GetMaterialId(value), _terrain.blockData[index].amount); }
	}
	
	public int amount {
		get { return _terrain.blockData[index].amount; }
		set { _terrain.blockData[index] = new BlockData(_terrain.blockData[index].material, value); }
	}
	
	
	public TerrainBlock(Terrain terrain, BlockPos pos) : this() {
		_terrain = terrain;
		this.position = pos;
	}
	
}
