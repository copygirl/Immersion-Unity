using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour, IEnumerable<EquipmentSlot> {

	Dictionary<EquipmentRegion, EquipmentSlot> _slots =
		new Dictionary<EquipmentRegion, EquipmentSlot>();


	public EquipmentSlot this[EquipmentRegion region] {
		get {
			EquipmentSlot slot;
			return (_slots.TryGetValue(region, out slot) ? slot : null);
		}
	}


	public EquipmentSlot AddSlot(EquipmentRegion region, GameObject attachment) {
		var slot = new EquipmentSlot(this, region, attachment);
		_slots.Add(region, slot);
		return slot;
	}


	#region IEnumerable implementation

	public IEnumerator<EquipmentSlot> GetEnumerator() {
		return _slots.Values.GetEnumerator();
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

	#endregion
}
