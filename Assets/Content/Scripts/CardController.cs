using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour
{
	public float thresholdAngle = 10f;
	public float maxAngle = 15f;
	public float backRotationSpeed = 1f;
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
		var angle = Mathf.Atan2( targetDirection.y, targetDirection.x ) * Mathf.Rad2Deg;
		angle = Mathf.Clamp( angle - 90, -maxAngle, maxAngle );
		this.transform.rotation = Quaternion.AngleAxis( angle, Vector3.forward );
	}

	/// <summary>
	/// Called when drag ended.
	/// </summary>
	public void OnEndDrag()
	{
		var currentRotation = this.transform.rotation.eulerAngles.z;
		if (currentRotation > 180)
		{
			currentRotation -= 360f;
		}
		if ( Mathf.Abs( currentRotation ) < thresholdAngle )
		{
			StartCoroutine ( RotateBack ( currentRotation ) );
		}
		else
		{
			// TODO: Implement next card logic
			Debug.Log( "End Round" );
		}
	}

	/// <summary>
	/// Rotates the card smoothly back, when drag ended.
	/// </summary>
	private IEnumerator RotateBack( float currentRotation )
	{
		this.t = 0;
		while ( this.t < 1 )
		{
			this.t += backRotationSpeed * Time.deltaTime;
			var angle = Mathf.Lerp( currentRotation, 0.0f, this.t );
			this.transform.rotation = Quaternion.Euler( 0.0f, 0.0f, angle );
			yield return null;
		}
		this.transform.rotation = Quaternion.identity;
	}
}
