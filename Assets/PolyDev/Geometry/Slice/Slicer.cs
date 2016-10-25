using UnityEngine;
using PolyDev.Geometry;

[RequireComponent ( typeof ( Collider ) )]
public class Slicer : MonoBehaviour
{
	public void OnTriggerEnter ( Collider other )
	{
		var sliceable = other.GetComponent<ISliceable>();

		if ( sliceable == null ) return;

		GetCollisionPoints( sliceable.GetMesh() );
	}

	public void GetCollisionPoints( Mesh mesh )
	{
		var triangles = mesh.triangles;
		for ( int i = 0; i < triangles.Length-1; i++ )
		{
			var vert1 = mesh.vertices[ triangles[i] ];
			var vert2 = mesh.vertices[ triangles[i + 1] ];
			var ray = new Ray( vert1, vert2 );

			RaycastHit hit;
			if ( !Physics.Raycast( ray, out hit ) ) continue;

			DebugVisual.Position( hit.point );
		}
	}
}
