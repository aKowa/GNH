using UnityEngine;

namespace PolyDev.Geometry
{
	public class SpheroidGenerator : ASpheroidGenerator
	{
		[Range( 0, 10 )] public int pointAmount = 3;
		[Range( 0.01f, 1f )] public float offsetToOrigin = 0.25f;
		public float threshold = 0.5f;
		public Vector2 roughnessMinMax = new Vector2( 0.9f, 1f );

		public override void Generate()
		{
			var mesh = base.CopyMesh( Mesh );

			var randPoints = new Vector3[pointAmount];
			for ( int i = 0; i < randPoints.Length; i++ )
			{
				randPoints[i] = RandomPointOnSphere();
			}

			var vertices = mesh.vertices;
			for ( int i = 0; i < vertices.Length; i++ )
			{
				var delta = vertices[i] * offsetToOrigin;
				foreach ( var randPoint in randPoints )
				{
					var distance = Vector3.Distance( randPoint, vertices[i] );
					if ( distance < threshold )
					{
						vertices[i] = ( vertices[i] - delta ) * distance / threshold + delta;
					}
				}

				vertices[i] *= UnityEngine.Random.Range( roughnessMinMax.x, roughnessMinMax.y );
			}

			mesh.vertices = vertices;
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			mesh.name = "Spheroid";
			this.GetComponent<MeshFilter>().sharedMesh = mesh;
		}



		// ADebug Sphere
		private void DebugCreateSphere( Vector3 point )
		{
			DebugCreateSphere( point, Color.red );
		}

		private void DebugCreateSphere( Vector3 point, Color color )
		{
			DebugCreateSphere( point, color, Vector3.one * 0.1f );
		}


		private void DebugCreateSphere( Vector3 point, Color color, Vector3 scale )
		{
			var sphere = GameObject.CreatePrimitive( PrimitiveType.Sphere );
			sphere.GetComponent<MeshRenderer>().sharedMaterial.color = color;
			sphere.transform.position = point;
			sphere.transform.localScale = scale;
			sphere.transform.parent = base.Spheres.transform;
		}


		// Random point
		private Vector3 RandomPointOnSphere( float radius = 0.5f )
		{
			return RandomPointOnSphere( Vector3.zero );
		}

		private Vector3 RandomPointOnSphere( Vector3 position, float radius = 0.5f )
		{
			var targetPoint = new Vector3( UnityEngine.Random.Range( -1f, 1f ), 
											UnityEngine.Random.Range( -1f, 1f ), 
											UnityEngine.Random.Range( -1f, 1f ) );
			return targetPoint.normalized * radius + position;
		}

		private Vector3 RandomPointOnHalfSphere( Vector3 axis, float radius = 0.5f )
		{
			var targetPoint = RandomPointOnSphere( radius );
			if ( Vector3.Dot( targetPoint, axis ) < 0 )
			{
				targetPoint *= -1;
			}
			DebugCreateSphere( targetPoint );
			return targetPoint;
		}
	}
}