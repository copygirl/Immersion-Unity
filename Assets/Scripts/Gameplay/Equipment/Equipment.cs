using System.Collections.Generic;
using UnityEngine;

/// <summary> Stores and allows access to a game object's equipment data. </summary>
public class Equipment : MonoBehaviour, IEnumerable<EquipmentSlot> {

	[SerializeField]
	List<EquipmentSlot> _slots = new List<EquipmentSlot>();


	#region IEnumerable implementation

	public IEnumerator<EquipmentSlot> GetEnumerator() {
		return _slots.GetEnumerator();
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

	#endregion

}
