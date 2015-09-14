using UnityEngine;

public class EquipmentSlot {

	public EquipmentRegion region { get; private set; }

	public GameObject attachment { get; set; }

	public GameObject item { get; set; }


	public EquipmentSlot(EquipmentRegion region, GameObject attachment) {
		this.region = region;
		this.attachment = attachment;
	}

}

