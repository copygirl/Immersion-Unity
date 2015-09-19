using System.Collections.Generic;
using UnityEngine;

public class Terrain : IBlockStorage {

	public static readonly BlockMaterial AIR = new BlockMaterial(new Color(1.0f, 1.0f, 1.0f, 0.0f));
	public static readonly BlockMaterial EARTH = new BlockMaterial(new Color(0.75f, 0.5f, 0.1f, 1.0f));


	public const int MAX_BLOCK_MATERIALS = 32;

	readonly BlockMaterial[] _idToMaterial = new BlockMaterial[MAX_BLOCK_MATERIALS];
	readonly Dictionary<BlockMaterial, byte> _materialToId = new Dictionary<BlockMaterial, byte>(MAX_BLOCK_MATERIALS);

	readonly byte[] _blockData;


	public BlockRegion region { get; private set; }

	public IBlock this[BlockPos pos] {
		get { return (region.Contains(pos) ? new TerrainBlock(this, pos) : (IBlock)null); }
	}


	public Terrain(int width, int depth, int height) {
		int x = -width / 2;
		int y = -depth / 2;
		int z = -height / 2;
		region = new BlockRegion(x, y, z, width + x - 1, depth + y - 1, height + z - 1);

		_blockData = new byte[width * depth * height];

		_idToMaterial[0] = AIR;   _materialToId[AIR]   = 0;
		_idToMaterial[1] = EARTH; _materialToId[EARTH] = 1;
	}


	struct TerrainBlock : IBlock {

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
		
		byte data {
			get { return _terrain._blockData[index]; }
			set { _terrain._blockData[index] = value; }
		}


		public BlockMaterial material {
			get { return _terrain._idToMaterial[data >> 2]; }
			set { data = (byte)((_terrain._materialToId[value] << 2) | 0x03); }
		}

		public int amount {
			get { return (data & 0x03) + 1; }
			set { data = (byte)((data & 0xFD) | ((value - 1) & 0x03)); }
		}


		public TerrainBlock(Terrain terrain, BlockPos pos) : this() {
			_terrain = terrain;
			this.position = pos;
		}
		
	}

}
