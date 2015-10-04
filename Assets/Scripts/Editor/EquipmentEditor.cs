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
			
			if (GUILayout.Button("Add new Equipment Slot"))
				slots.arraySize++;
			
			EditorGUI.indentLevel--;
		}
		
		if (GUI.changed)
			serializedObject.ApplyModifiedProperties();
	}
	
}
