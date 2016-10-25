using UnityEngine;
using UDebug = UnityEngine.Debug;

namespace PolyDev.Geometry
{
	[RequireComponent( typeof (MeshFilter) )]
	public abstract class ASpheroidGenerator : MonoBehaviour
	{
		private Mesh mesh;

		protected Mesh Mesh
		{
			get
			{
				if ( mesh != null )
				{
					return mesh;
				}
				return mesh = this.GetComponent<MeshFilter>().sharedMesh;
			}
		}

		private GameObject spheres;

		protected GameObject Spheres
		{
			get
			{
				if ( spheres != null )
				{
					return spheres;
				}
				spheres = new GameObject( "Spheres" );
				spheres.transform.parent = this.transform;
				spheres.transform.localPosition = Vector3.zero;
				return spheres;
			}
		}


		public abstract void Generate();


		public virtual void Start()
		{
			Generate();
		}


		// ADebug Methods
		protected virtual void Update()
		{
			//ShowNormals( Mesh );
		}

		protected void ShowNormals( Mesh normalMesh )
		{
			for ( int i = 0; i < normalMesh.vertexCount; i++ )
			{
				UDebug.DrawRay( normalMesh.vertices[i], normalMesh.normals[i] * 0.1f, Color.red );
			}
		}

		public void Reset()
		{
			var sphereGO = GameObject.CreatePrimitive( PrimitiveType.Sphere );
			this.GetComponent<MeshFilter>().sharedMesh = sphereGO.GetComponent<MeshFilter>().sharedMesh;
			DestroyImmediate( sphereGO );

			if ( Spheres == null ) return;

			DestroyImmediate( Spheres );
			spheres = null;
		}

		protected Mesh CopyMesh( Mesh passedMesh )
		{
			return new Mesh()
			{
				vertices = passedMesh.vertices,
				bindposes = passedMesh.bindposes,
				normals = passedMesh.normals,
				boneWeights = passedMesh.boneWeights,
				bounds = passedMesh.bounds,
				colors = passedMesh.colors,
				colors32 = passedMesh.colors32,
				hideFlags = passedMesh.hideFlags,
				name = passedMesh.name,
				subMeshCount = passedMesh.subMeshCount,
				triangles = passedMesh.triangles
			};
		}
	}
}
