using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary> Stores and allows access to a game object's equipment data. </summary>
public class Equipment : MonoBehaviour, IEnumerable<EquipmentSlot> {

	[SerializeField]
	internal List<EquipmentSlotInternal> _slots = new List<EquipmentSlotInternal>();

	#region IEnumerable implementation

	public IEnumerator<EquipmentSlot> GetEnumerator() {
		return _slots.Select((slot, i) => new EquipmentSlot(this, i)).GetEnumerator();
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

	#endregion

}
