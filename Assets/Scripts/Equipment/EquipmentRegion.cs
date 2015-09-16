
/// <summary> Represents an equipment region on an entity.
///           Used for both bodies (head, chest, legs, feet, etc)
///           and non-living things (belt slots, pouches, etc). </summary>
public class EquipmentRegion {

	public static readonly EquipmentRegion HAND = new EquipmentRegion("hand");


	public string name { get; private set; }


	public EquipmentRegion(string name) {
		this.name = name;
	}


	public override string ToString() {
		return string.Format("[EquipmentRegion \"{0}\"]", name);
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
