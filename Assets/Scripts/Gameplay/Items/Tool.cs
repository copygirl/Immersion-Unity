using UnityEngine;

/// <summary> Extension component for items that
///           may be used as tools or weapons. </summary>
[RequireComponent(typeof(Item))]
public class Tool : MonoBehaviour {

	Item _item;


	/// <summary> Gets or sets the weight of the tool in kilograms. </summary>
	public float weight {
		get { return _item.weight; }
		set { _item.weight = value; }
	}


	void Start() {
		_item = GetComponent<Item>();
	}


	public static implicit operator Item(Tool tool) {
		return ((tool != null) ? tool._item : null);
	}

}
