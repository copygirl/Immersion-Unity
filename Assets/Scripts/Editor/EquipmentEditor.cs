using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Equipment))]
public class EquipmentEditor : Editor {
	
	public override void OnInspectorGUI() {
		var slots = serializedObject.FindProperty("_slots");
		
		if (slots.isExpanded = EditorGUILayout.Foldout(slots.isExpanded, "Equipment Slots")) {
			EditorGUI.indentLevel++;
			
			for (var i = 0; i < slots.arraySize; i++)
				EditorGUILayout.PropertyField(slots.GetArrayElementAtIndex(i));
			
			if (GUILayout.Button("Add new Equipment Slot")) {
				var index = slots.arraySize++;          // Increase size and get last index.
				slots.GetArrayElementAtIndex(index)     // Get EquipmentSlot at that index.
					.FindPropertyRelative("_equipment") // Find _equipment field on slot.
					.objectReferenceValue = target;     // Set reference to this Equipment.
			}
			
			EditorGUI.indentLevel--;
		}
		
		if (GUI.changed)
			serializedObject.ApplyModifiedProperties();
	}
	
}
