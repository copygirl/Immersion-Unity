using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EquipmentSlotInternal))]
public class EquipmentSlotInternalDrawer : PropertyDrawer {

	float height { get { return EditorGUIUtility.singleLineHeight; } }
	float separation = 2;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return (property.isExpanded ? (5 + property.FindPropertyRelative("tags").arraySize) : 1)
			* (height + separation) - separation;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		var region = property.FindPropertyRelative("region");
		var tags   = property.FindPropertyRelative("tags");
		var attach = property.FindPropertyRelative("attachment");
		var item   = property.FindPropertyRelative("item");

		var rect = new Rect(position.position, new Vector2(position.width, height));

		EditorGUI.BeginProperty(rect, label, property);
		var str = region.enumDisplayNames[region.enumValueIndex];
		if (tags.arraySize > 0) {
			str += " (";
			for (var i = 0; i < tags.arraySize; i++) {
				var tag = tags.GetArrayElementAtIndex(i);
				str += tag.enumDisplayNames[tag.enumValueIndex];
				if (i < tags.arraySize - 1) str += ", ";
			}
			str += ")";
		}
		property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, str);
		rect.y += height + separation;
		EditorGUI.EndProperty();

		if (property.isExpanded) {
			EditorGUI.indentLevel++;
			EditorGUI.PropertyField(rect, region); rect.y += height + separation;

			EditorGUI.LabelField(rect, new GUIContent("Tags"));
			for (var i = 0; i < tags.arraySize; i++) {
				var tag = tags.GetArrayElementAtIndex(i);
				EditorGUI.PropertyField(rect, tag, new GUIContent(" ")); rect.y += height + separation;
			}

			var buttonRect = new Rect(rect.x + EditorGUIUtility.labelWidth, rect.y,
			                          rect.width - EditorGUIUtility.labelWidth, rect.height);
			if (GUI.Button(buttonRect, "Add new Equipment Tag"))
				tags.arraySize++;
			rect.y += height + separation;

			EditorGUI.PropertyField(rect, attach); rect.y += height + separation;
			EditorGUI.PropertyField(rect, item);
			EditorGUI.indentLevel--;
		}
	}

}
