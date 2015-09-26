using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour, IEnumerable<EquipmentSlot> {

	List<EquipmentSlot> _slots = new List<EquipmentSlot>();


	public EquipmentSlot AddSlot(GameObject attachment, string region, params string[] tags) {
		var slot = new EquipmentSlot(this, attachment, region, tags);
		_slots.Add(slot);
		return slot;
	}


	#region IEnumerable implementation

	public IEnumerator<EquipmentSlot> GetEnumerator() {
		return _slots.GetEnumerator();
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

	#endregion
}
