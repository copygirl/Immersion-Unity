using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

[CustomEditor(typeof(Equipment))]
public class EquipmentEditor : Editor {

	public override void OnInspectorGUI() {
		var slots = serializedObject.FindProperty("_slots");

		if (slots.isExpanded = EditorGUILayout.Foldout(slots.isExpanded, "Equipment Slots")) {
			EditorGUI.indentLevel++;

			for (var i = 0; i < slots.arraySize; i++)
				EditorGUILayout.PropertyField(slots.GetArrayElementAtIndex(i));

			if (GUILayout.Button("Add new Equipment Slot"))
				slots.arraySize++;
				
			EditorGUI.indentLevel--;
		}

		if (GUI.changed)
			serializedObject.ApplyModifiedProperties();
	}

}
