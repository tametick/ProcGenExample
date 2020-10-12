using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleRandomGenerator))]
public class GeneratorEditor : Editor
{
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		var gen = (SimpleRandomGenerator)target;

		if (GUILayout.Button("Clear")) {
			gen.Clear();
		}

		if (GUILayout.Button("Generate")) {
			gen.Generate();
		}

	}
}
