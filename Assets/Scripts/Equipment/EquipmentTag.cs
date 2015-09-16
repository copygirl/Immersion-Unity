using System.Collections.Generic;
using System.Linq;

/// <summary> Describes the nature of an equipment slot
///           to differenciate it from other slots. </summary>
public class EquipmentTag {

	public static readonly EquipmentTag HELD = new EquipmentTag("held");

	public static readonly EquipmentTag RIGHT = new EquipmentTag("right");
	public static readonly EquipmentTag LEFT  = new EquipmentTag("left");

	public static readonly EquipmentTag ARMOR     = new EquipmentTag("armor");
	public static readonly EquipmentTag CLOTHING  = new EquipmentTag("clothing");
	public static readonly EquipmentTag ACCESSORY = new EquipmentTag("accessory");
	

	public string value { get; private set; }
	
	
	public EquipmentTag(string value) {
		this.value = value;
	}


	public override string ToString() {
		return string.Format("[EquipmentTag \"{0}\"]", value);
	}
	
	public static string ToString(IEnumerable<EquipmentTag> tags) {
		return string.Format("[EquipmentTags {0}]", string.Join(
			", ", tags.Select(tag => '"' + tag.value + '"').ToArray()));
	}
	
	
	#region Equals / GetHashCode
	
	public override bool Equals(object obj) {
		EquipmentTag tag;
		return (ReferenceEquals(this, obj) ||
		        (((tag = (obj as EquipmentTag)) != null) &&
		 (tag.value == value)));
	}
	
	public override int GetHashCode() {
		return value.GetHashCode();
	}
	
	#endregion

}
