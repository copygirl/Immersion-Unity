using System;
using UnityEngine;

/// <summary> Extension component for items that ay be used as tools or weapons. </summary>
[RequireComponent(typeof(Item))]
public class Tool : MonoBehaviour {

	Item _item;
	
	[SerializeField]
	float[] _toolEffectiveness = new float[Enum.GetValues(typeof(ToolType)).Length - 1];


	/// <summary> Gets or sets the weight of the tool in kilograms. </summary>
	public float weight {
		get { return _item.weight; }
		set { _item.weight = value; }
	}

	/// <summary> Gets or sets the raw effectiveness of
	///           this tool for the specified tool type. </summary>
	public float this[ToolType type] {
		get { return ((Enum.IsDefined(typeof(ToolType), type) && (type != ToolType.None))
			               ? _toolEffectiveness[(int)type - 1] : 0.0F); }
		set {
			if ((value < 0.0F) || (value > 1.0F))
				throw new ArgumentOutOfRangeException("value", value,
					"Effectiveness is not a valid value (0.0 to 1.0)");
			if (!Enum.IsDefined(typeof(ToolType), type) || (type == ToolType.None))
				throw new ArgumentException(string.Format(
					"'{0}' is not a valid ToolType", type), "type");
			_toolEffectiveness[(int)type - 1] = value;
		}
	}


	void Start() {
		_item = GetComponent<Item>();
	}


	public static implicit operator Item(Tool tool) {
		return ((tool != null) ? tool._item : null);
	}

}
