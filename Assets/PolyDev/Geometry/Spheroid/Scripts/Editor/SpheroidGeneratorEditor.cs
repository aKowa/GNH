using UnityEditor;
using UnityEngine;
using PolyDev.Geometry;

[CanEditMultipleObjects]
[CustomEditor(typeof( SpheroidGenerator))]
public class SpheroidGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		var sphereGenerator = target as SpheroidGenerator;

		if (sphereGenerator == null) return;

		if ( GUILayout.Button( "Generate" ) )
		{
			sphereGenerator.Reset();
			sphereGenerator.Generate();
		}

		if ( GUILayout.Button( "Reset" ) )
		{
			sphereGenerator.Reset();
		}
	}
}
