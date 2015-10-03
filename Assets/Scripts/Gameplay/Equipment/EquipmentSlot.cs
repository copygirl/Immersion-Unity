using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary> Represents a slot in an game object's equipment. </summary>
[Serializable]
public struct EquipmentSlot : ISerializationCallbackReceiver {
	
	[SerializeField] Equipment _equipment;
	[SerializeField] int _index;


	EquipmentSlotInternal slot { get {
			if (_equipment == null)
				throw new InvalidOperationException(
					"Equipment slot has not been (properly) initialized");
			return _equipment._slots[_index];
		} }

	#region Public properties
	
	/// <summary> Gets the object items will be added to as childs when equipped. </summary>
	public GameObject attachment { get { return _equipment._slots[_index].attachment; } }
	
	/// <summary> Gets the equipment region of this slot. </summary>
	public EquipmentRegion region { get { return _equipment._slots[_index].region; } }
	
	/// <summary> Gets the equipment tags of this slot, which further describe its use. </summary>
	public IEnumerable<EquipmentTag> tags { get { return _equipment._slots[_index].tags.Select(t => t); } }
	
	
	/// <summary> Gets the item currently equipped in this slot, null if none. </summary>
	public Item item {
		get { return _equipment._slots[_index].item; }
		private set { _equipment._slots[_index].item = value; }
	}
	
	/// <summary> Gets whether an item is currently quipped in this slot. </summary>
	public bool occupied { get { return (item != null); } }
	
	#endregion


	internal EquipmentSlot(Equipment equipment, int index) {
		_equipment = equipment;
		_index = index;
	}


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
		
		this.item = item;
		this.item.OnEquip(this);
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
	
	#endregion
	
	#region ISerializationCallbackReceiver implementation
	
	public void OnBeforeSerialize() {
		if ((_equipment == null) || (_equipment._slots[_index] == null)) {
			Debug.LogWarning("Serializing EquipmentSlot which doesn't refer to a valid Equipment / EquipmentSlotInternal");
			_equipment = null;
			_index = 0;
		}
	}
	
	public void OnAfterDeserialize() {  }
	
	#endregion
	
	#region ToString, Equals and GetHashCode
	
	public override string ToString() {
		var tags = _equipment._slots[_index].tags;
		return string.Format("[EquipmentSlot: {0}{1}]", region,
		                     ((tags.Length > 0) ? " (" + tags.Join(",") + ")" : ""));
	}
	
	public override bool Equals(object obj) {
		EquipmentSlot slot;
		return ((obj is EquipmentSlot) &&
		        ((slot = (EquipmentSlot)obj)._equipment == _equipment) &&
		        (slot._index == _index));
	}
	
	public override int GetHashCode() {
		return HashHelper.GetHashCode(_equipment, _index);
	}
	
	#endregion
}
