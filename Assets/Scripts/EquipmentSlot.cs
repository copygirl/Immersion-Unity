using System;
using System.Linq;
using UnityEngine;

/// <summary> Represents a slot in an entity's equipment. </summary>
public class EquipmentSlot {

	/// <summary> Gets the equipment compound of this slot. </summary>
	public Equipment equipment { get; private set; }

	/// <summary> Gets the equipment region this slot occupies. </summary>
	public EquipmentRegion region { get; private set; }

	/// <summary> Gets the object items will be added to as childs when equipped. </summary>
	public GameObject attachment { get; private set; }


	/// <summary> Gets the item currently equipped in this slot, null if none. </summary>
	public Item item { get; private set; }


	/// <summary> Gets whether an item is currently quipped in this slot. </summary>
	public bool occupied { get { return (item != null); } }


	public EquipmentSlot(Equipment equipment, EquipmentRegion region, GameObject attachment) {
		this.equipment = equipment;
		this.region = region;
		this.attachment = attachment;
	}


	/// <summary> Returns if the specified item can be equipped in this slot. </summary>
	public bool canEquip(Item item) {
		return ((item != null) && !occupied && item.canEquip(this) &&
		        equipment.All(slot => ((slot == this) || slot.region.compatible(region))));
	}

	/// <summary> Returns if the current item can be unequipped from this slot. </summary>
	public bool canUnequip() {
		return (occupied && item.canUnequip(this));
	}


	/// <summary> Equips the specified item in this slot. </summary>
	public void equip(Item item) {
		if (item != null)
			throw new InvalidOperationException(string.Format(
				"{0} is already occupied", this));
		if (!canEquip(item))
			throw new InvalidOperationException(string.Format(
				"Can't equip {0} in {1}", item, this));

		this.item = item;
		item.onEquip(this);
	}

	/// <summary> Unequips the specified item from this slot. </summary>
	public void unequip() {
		if (item == null)
			throw new InvalidOperationException(string.Format(
				"{0} doesn't contain an item", this));
		if (canUnequip())
			throw new InvalidOperationException(string.Format(
				"Can't unequip {0} from {1}", item, this));

		item.onUnequip();
		item = null;
	}

}

