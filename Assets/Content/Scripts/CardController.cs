using UnityEngine;
using System.Collections;
using Content.Scripts;

public class CardController : MonoBehaviour
{
	public GameManager gameManager;
	public float thresholdAngle = 10f;
	public float maxAngle = 15f;
	public float backRotationSpeed = 1f;
	private float EulerZ
	{
		get
		{
			var eulerZ = this.transform.rotation.eulerAngles.z;
			if (eulerZ > 180)
			{
				eulerZ -= 360f;
			}
			return eulerZ;
		}
	}

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

		// handle threshold angle logic
		if ( Mathf.Abs( EulerZ ) > thresholdAngle )
		{
			gameManager.PreviewResults();
		}
		else
		{
			gameManager.RevertPreview();
		}
	}

	/// <summary>
	/// Called when drag ended. Starts back rotation or ends this round depending on thresholdAngle.
	/// </summary>
	public void OnEndDrag()
	{
		if ( Mathf.Abs( EulerZ ) < thresholdAngle )
		{
			StartCoroutine ( RotateBack ( EulerZ ) );
		}
		else
		{
			gameManager.ApplyResults();
			this.transform.rotation = Quaternion.identity;
		}
	}

	/// <summary>
	/// Rotates the card smoothly back, when drag ended.
	/// </summary>
	private IEnumerator RotateBack( float currentRotation )
	{
		float t = 0;
		while ( t < 1f )
		{
			var angle = Mathf.Lerp( currentRotation, 0.0f, t );
			this.transform.rotation = Quaternion.Euler( 0.0f, 0.0f, angle );
			t += backRotationSpeed * Time.deltaTime;
			yield return null;
		}
		this.transform.rotation = Quaternion.identity;
	}
}
