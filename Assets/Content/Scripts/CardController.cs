using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour
{
	public float maxAngle = 15f;
	public float speed = 1f;
	private float t = 0;

	/// <summary>
	/// Called when drag began.
	/// </summary>
	public void OnBeginDrag()
	{
		StopAllCoroutines();
	}

	/// <summary>
	/// Rotates the card towards the input position when dragged.
	/// </summary>
	public void OnDrag()
	{
		// handle rotation
		var targetDirection = ( Input.mousePosition - this.transform.position ).normalized;
		var angle = Vector3.Angle( targetDirection, Vector3.up );
		angle = Mathf.Clamp( angle, 0, maxAngle );
		if ( Input.mousePosition.x > this.transform.position.x )
		{
			angle *= -1;
		}
		this.transform.rotation = Quaternion.Euler ( 0.0f, 0.0f, angle );
	}

	/// <summary>
	/// Called when drag ended.
	/// </summary>
	public void OnEndDrag()
	{
		StartCoroutine( RotateBack() );
	}

	/// <summary>
	/// Rotates the card smoothly back, when drag ended.
	/// </summary>
	private IEnumerator RotateBack()
	{
		this.t = 0;
		var startRotation = this.transform.rotation.z * 100;
		while ( this.t < 1 )
		{
			this.t += speed * Time.deltaTime;
			var angle = Mathf.Lerp( startRotation, 0.0f, this.t );
			this.transform.rotation = Quaternion.Euler( 0.0f, 0.0f, angle );
			//Debug.Log( t + "   " + angle );
			yield return null;
		}
		this.transform.rotation = Quaternion.identity;
	}
}
