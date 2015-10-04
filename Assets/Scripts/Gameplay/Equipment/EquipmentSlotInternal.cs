using System;
using UnityEngine;

/// <summary> Stores the raw information of a single equipment slot. </summary>
[Serializable]
public class EquipmentSlotInternal {

	// Disable "will always have its default value" warning,
	// since the value will be set using the inspector.
	#pragma warning disable 0649

	public EquipmentRegion region;
	public EquipmentTag[] tags;
	public GameObject attachment;
	public Item item;

	#pragma warning restore 0649
	
}
