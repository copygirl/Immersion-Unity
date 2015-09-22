using UnityEngine;

public enum BlockFacing {

	Invalid,

	/// <summary> -X </summary>
	West,
	/// <summary> +X </summary>
	East,

	/// <summary> -Y </summary>
	Down,
	/// <summary> +Y </summary>
	Up,

	/// <summary> -Z </summary>
	North,
	/// <summary> *Z </summary>
	South

}

public static class BlockFacingExtensions {

	static int[] _relX = { -1, 1,  0, 0,  0, 0 };
	static int[] _relY = {  0, 0, -1, 1,  0, 0 };
	static int[] _relZ = {  0, 0,  0, 0, -1, 1 };

	static BlockFacing[] _opposites = new BlockFacing[]{
		BlockFacing.East,  BlockFacing.East,
		BlockFacing.Up,    BlockFacing.Down,
		BlockFacing.South, BlockFacing.North
	};


	public static bool IsValid(this BlockFacing facing) {
		return ((facing >= BlockFacing.West) && (facing <= BlockFacing.South));
	}

	public static BlockFacing GetOpposite(this BlockFacing facing) {
		return (facing.IsValid() ? _opposites[(int)facing - 1] : BlockFacing.Invalid);
	}

	public static void MoveRelative(this BlockFacing facing, ref int x, ref int y, ref int z) {
		if (!facing.IsValid()) return;
		x += _relX[(int)facing - 1];
		y += _relY[(int)facing - 1];
		z += _relZ[(int)facing - 1];
	}
	public static BlockPos MoveRelative(this BlockFacing facing, BlockPos pos) {
		int x = pos.x, y = pos.y, z = pos.z;
		facing.MoveRelative(ref x, ref y, ref z);
		return new BlockPos(x, y, z);
	}

	public static Vector3 ToVector(this BlockFacing facing) {
		int x = 0, y = 0, z = 0;
		facing.MoveRelative(ref x, ref y, ref z);
		return new Vector3(x, y, z);
	}

}
