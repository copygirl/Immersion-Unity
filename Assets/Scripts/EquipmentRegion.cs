using System.Linq;
using UnityEngine;

public class EquipmentRegion {

	public static readonly EquipmentRegion HELD_RIGHT = new EquipmentRegion("Held: Right Hand");
	public static readonly EquipmentRegion HELD_LEFT = new EquipmentRegion("Held: Left Hand");
	public static readonly EquipmentRegion HELD_BOTH = new EquipmentRegion("Held: Both Hands", HELD_RIGHT, HELD_LEFT);


	public readonly string name;

	EquipmentRegion[] _conflictingSlots;


	public EquipmentRegion(string name, params EquipmentRegion[] conflictingSlots) {
		this.name = name;
		_conflictingSlots = conflictingSlots;
	}


	public virtual bool Compatible(EquipmentRegion other) {
		return !_conflictingSlots.Contains(other);
	}

	public override string ToString() {
		return string.Format("[EquipmentSlot \"{0}\"]", name);
	}


	#region Equals / GetHashCode

	public override bool Equals(object obj) {
		EquipmentRegion region;
		return (ReferenceEquals(this, obj) ||
		        (((region = (obj as EquipmentRegion)) != null) &&
		         (region.name == name)));
	}

	public override int GetHashCode() {
		return name.GetHashCode();
	}

	#endregion

}
