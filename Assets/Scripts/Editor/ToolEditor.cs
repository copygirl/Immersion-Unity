using UnityEditor;

[CustomEditor(typeof(Tool))]
public class ToolEditor : Editor {
	
	public override void OnInspectorGUI() {
		var effectiveness = serializedObject.FindProperty("_toolEffectiveness");

		if (effectiveness.isExpanded = EditorGUILayout.Foldout(effectiveness.isExpanded, "Tool Effectiveness")) {
			EditorGUI.indentLevel++;
			for (var i = 0; i < effectiveness.arraySize; i++) {
				var type = (ToolType)(i + 1);
				var eff = effectiveness.GetArrayElementAtIndex(i);
				EditorGUILayout.Slider(eff, 0.0F, 1.0F, type.ToString());
			}
			EditorGUI.indentLevel--;
		}
	}
	
}
