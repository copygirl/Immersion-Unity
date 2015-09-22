using System;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : IBlockStorage, IRawBlockAccess {

	internal readonly BlockMaterial[] _idToMaterial =
		new BlockMaterial[BlockData.MAX_MATERIAL_IDS];
	internal readonly Dictionary<BlockMaterial, byte> _materialToId =
		new Dictionary<BlockMaterial, byte>(BlockData.MAX_MATERIAL_IDS);


	#region IBlockStorage implementation

	public BlockRegion region { get; private set; }

	public IBlock this[BlockPos pos] {
		get {
			return (region.Contains (pos)
			              ? (IBlock)new TerrainBlock (this, pos)
			              : (IBlock)new EmptyBlock (this, pos));
		}
	}

	#endregion

	#region IRawBlockAccess implementation
	
	public int width { get { return region.width; } }
	public int depth { get { return region.depth; } }
	public int height { get { return region.height; } }
	
	public BlockData[] blockData { get; private set; }

	#endregion


	public Terrain(int width, int depth, int height) {
		int x = -width / 2;
		int y = -depth / 2;
		int z = -height / 2;
		region = new BlockRegion(x, y, z, width + x - 1, depth + y - 1, height + z - 1);

		blockData = new BlockData[width * depth * height];

		_idToMaterial[0] = BlockMaterial.AIR;   _materialToId[BlockMaterial.AIR]   = 0;
		_idToMaterial[1] = BlockMaterial.EARTH; _materialToId[BlockMaterial.EARTH] = 1;
		_idToMaterial[2] = BlockMaterial.SAND;  _materialToId[BlockMaterial.SAND]  = 2;
	}


	#region TerrainBlock struct definition

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


		public BlockMaterial material {
			get { return _terrain._idToMaterial[_terrain.blockData[index].material]; }
			set { _terrain.blockData[index] = new BlockData(_terrain._materialToId[value], _terrain.blockData[index].amount); }
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

	#endregion

}
