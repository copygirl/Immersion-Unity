using UnityEngine;

public class BlockMaterial {

	public static readonly BlockMaterial AIR   = new BlockMaterial();
	public static readonly BlockMaterial EARTH = new BlockMaterial(new Color(0.15f,  0.5f, 0.15f, 1.0f));
	public static readonly BlockMaterial SAND  = new BlockMaterial(new Color(0.75f, 0.65f, 0.25f, 1.0f));


	public Color color { get; private set; }

	public bool solid { get; private set; }


	public BlockMaterial(Color color) {
		this.color = color;
		solid = true;
	}

	BlockMaterial() {
		color = Color.clear;
		solid = false;
	}

}
