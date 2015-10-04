using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Disable "will always have its default value" warning,
// since the value will be set using the inspector.
#pragma warning disable 0649

/// <summary> Represents a slot in an game object's equipment. Note that
///           this should only be serialized once, in the Equipment script. </summary>
[Serializable]
public class EquipmentSlot {

	#region Private fields (serialized)

	[SerializeField] Equipment _equipment;
	[SerializeField] EquipmentRegion _region;
	[SerializeField] EquipmentTag[] _tags;
	[SerializeField] GameObject _attachment;

	#endregion

	#region Public properties

	/// <summary> Gets the equipment this slot was created for. </summary>
	public Equipment equipment { get { return _equipment; } }

	/// <summary> Gets the equipment region of this slot. </summary>
	public EquipmentRegion region { get { return _region; } }
	
	/// <summary> Gets the equipment tags of this slot, which further describe its use. </summary>
	public IEnumerable<EquipmentTag> tags { get { return _tags.Select(t => t); } }
	
	
	/// <summary> Gets the object items will be added to as childs when equipped. </summary>
	public GameObject attachment { get { return _attachment; } }
	
	/// <summary> Gets the item currently equipped in this slot, null if none. </summary>
	public Item item { get { return _attachment.transform.Cast<Transform>()
			.Select(t => t.GetComponent<Item>()).FirstOrDefault(); } }


	/// <summary> Gets whether an item is currently quipped in this slot. </summary>
	public bool occupied { get { return (item != null); } }
	
	#endregion
	
	#region Public methods
	
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
	}
	
	#endregion
	
	#region ToString
	
	public override string ToString() {
		return string.Format("[EquipmentSlot: {0}{1}]", _region,
		                     ((_tags.Length > 0) ? " (" + _tags.Join(",") + ")" : ""));
	}
	
	#endregion
	
}
