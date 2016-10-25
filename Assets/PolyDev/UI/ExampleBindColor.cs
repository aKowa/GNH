using UnityEngine;
using UnityEngine.UI;
using PolyDev.UI;

[RequireComponent(typeof(Image))]
public class ExampleBindColor : MonoBehaviour
{
	public BindColor myColor;

	public void Update ()
	{
		var t = Mathf.Abs( Mathf.Sin( Time.time ) );
		myColor.Value = Color.Lerp( Color.red, Color.yellow, t );
	}
}
