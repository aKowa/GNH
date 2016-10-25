using UnityEngine;
using PolyDev.Geometry;

[RequireComponent(typeof( MeshFilter ) )]
public class CubeController : MonoBehaviour, ISliceable
{
	public Mesh GetMesh()
	{
		return this.GetComponent<MeshFilter>().sharedMesh;
	}
}
