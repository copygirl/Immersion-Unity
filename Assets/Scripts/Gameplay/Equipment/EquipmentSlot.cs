﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary> Represents a slot in an entity's equipment. </summary>
public class EquipmentSlot {

	#region Public properties

	/// <summary> Gets the equipment compound of this slot. </summary>
	public Equipment equipment { get; private set; }
	
	/// <summary> Gets the object items will be added to as childs when equipped. </summary>
	public GameObject attachment { get; private set; }


	/// <summary> Gets the equipment region of this slot. </summary>
	public string region { get; private set; }
	
	/// <summary> Gets the equipment tags of this slot, which describe its use. </summary>
	public IEnumerable<string> tags { get; private set; }


	/// <summary> Gets the item currently equipped in this slot, null if none. </summary>
	public Item item { get; private set; }


	/// <summary> Gets whether an item is currently quipped in this slot. </summary>
	public bool occupied { get { return (item != null); } }

	#endregion


	public EquipmentSlot(Equipment equipment, GameObject attachment,
	                     string region, params string[] tags) {
		this.equipment = equipment;
		this.attachment = attachment;
		this.region = region;
		this.tags = tags;
	}


	/// <summary> Returns if the specified item can be equipped in this slot. </summary>
	public bool CanEquip(Item item) {
		return ((item != null) && !occupied && item.CanEquip(this));
	}

	/// <summary> Returns if the current item can be unequipped from this slot. </summary>
	public bool CanUnequip() {
		return (occupied && item.CanUnequip(this));
	}


	/// <summary> Equips the specified item in this slot. </summary>
	public void Equip(Item item) {
		if (occupied)
			throw new InvalidOperationException(string.Format(
				"{0} is already occupied", this));
		if (!CanEquip(item))
			throw new InvalidOperationException(string.Format(
				"Can't equip {0} in {1}", item, this));

		this.item = item;
		item.OnEquip(this);
	}

	/// <summary> Unequips the specified item from this slot. </summary>
	public void Unequip() {
		if (!occupied)
			throw new InvalidOperationException(string.Format(
				"{0} doesn't contain an item", this));
		if (!CanUnequip())
			throw new InvalidOperationException(string.Format(
				"Can't unequip {0} from {1}", item, this));

		item.OnUnequip();
		item = null;
	}

}
