using  UnityEngine;

namespace PolyDev.Geometry
{
	public static class DebugVisual
	{
		private static GameObject sphere;

		private static GameObject Sphere
		{
			get
			{
				if ( sphere != null )
				{
					return sphere;
				}
				sphere = GameObject.CreatePrimitive ( PrimitiveType.Sphere );
				sphere.GetComponent<SphereCollider> ().isTrigger = true;

				var rend = sphere.GetComponent<Renderer>();
				rend.material.shader = Shader.Find( "Unlit/Color" );
				rend.material.SetColor( "_Color", Color.red );

				sphere.transform.localScale = Vector3.one * 0.1f;
				return sphere;
			}
		}

		private static Transform parent;
		private static Transform Parent
		{
			get
			{
				if ( parent != null )
				{
					return parent;
				}
				return parent = new GameObject("Debug").transform;
			}
		}

		public static void Position( Vector3 position )
		{
			var clone = GameObject.Instantiate( Sphere);
			clone.transform.position = position;
			clone.transform.parent = Parent;
		}

		public static void Position( Vector3[] positions )
		{
			foreach ( var pos in positions )
			{
				Position( pos );
			}
		}
	}
}
