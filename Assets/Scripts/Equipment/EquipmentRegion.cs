using System.Linq;
using UnityEngine;

public class EquipmentRegion {

	public static readonly EquipmentRegion HELD_RIGHT = new EquipmentRegion("Held: Right Hand", "held");
	public static readonly EquipmentRegion HELD_LEFT = new EquipmentRegion("Held: Left Hand", "held");


	public readonly string name;
	public readonly string group;

	EquipmentRegion[] _conflictingSlots;


	public EquipmentRegion(string name, string group = null) {
		this.name = name;
		this.group = group;
	}

	public EquipmentRegion setConflicting(params EquipmentRegion[] conflictingSlots) {
		_conflictingSlots = conflictingSlots;
	}


	public virtual bool compatible(EquipmentRegion other) {
		return ((_conflictingSlots == null) || !_conflictingSlots.Contains(other));
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
