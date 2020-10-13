using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GeneratorHelper))]
public class GeneratorEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		var helper = (GeneratorHelper)target;

		if (GUILayout.Button("Clear")) {
			helper.Clear();
		}

		foreach (var gen in helper.GetComponents<BaseGenerator>()) {
			if (GUILayout.Button($"{gen}")) {
				gen.Generate();
			}
		}
	}
}
