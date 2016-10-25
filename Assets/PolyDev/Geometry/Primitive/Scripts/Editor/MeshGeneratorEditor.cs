using UnityEngine;
using UnityEditor;
using PolyDev.Geometry;

[CustomEditor( typeof( APrimitiveMeshGenerator ), true )]
public class MeshGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		var generator = target as APrimitiveMeshGenerator;

		if (generator == null) return;
		if ( GUILayout.Button( "Create Mesh" ) )
		{
			generator.Clear();
			generator.CreateMesh();
		}

		if ( GUILayout.Button( "Clear" ) )
		{
			generator.Clear();
		}
	}
}
