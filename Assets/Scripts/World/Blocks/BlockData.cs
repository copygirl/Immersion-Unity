using System;

/// <summary> Low level data structure representing a single block. </summary>
public struct BlockData {

	public const int MAX_MATERIAL_IDS = 64;
	public const int MAX_AMOUNT = 4;

	byte _data;

	public int material { get { return _data >> 2; } }
	public int amount { get { return 4 - (_data & 0x03); } }

	public bool isSolid { get { return (material > 0); } }

	public BlockData(int material, int amount) {
		if ((material < 0) || (material >= MAX_MATERIAL_IDS))
			throw new ArgumentOutOfRangeException(string.Format(
				"material < 0 or >= {0} ({1})", MAX_MATERIAL_IDS, material), "material");
		if ((amount < 1) || (amount > MAX_AMOUNT))
			throw new ArgumentOutOfRangeException(string.Format(
				"amount < 1 or > {0} ({1})", MAX_AMOUNT, amount), "amount");

		_data = (byte)((material << 2) | (4 - amount));
	}

}
