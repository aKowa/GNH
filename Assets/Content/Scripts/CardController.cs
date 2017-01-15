using UnityEngine;
using System.Collections;
using Content.Scripts;

public class CardController : MonoBehaviour
{
	public int[] policyValuesL = new int[4];
	public int[] policyValuesR = new int[4];
	public int minMaxPolicyValue = 10;
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

	private int[] ChosenPolicy
	{
		get
		{
			if (EulerZ > 0)
			{
				return policyValuesL;
			}
			else if (EulerZ < 0)
			{
				return policyValuesR;
			}
			Debug.LogWarning( "Do not call ChosenPolicy, when EulerZ equals zero!" );
			return new int[4];
		}
	}

	/// <summary>
	/// Sets card values on start.
	/// </summary>
	private void Start ()
	{
		SetRandomValues();
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
			gameManager.PreviewResults( ChosenPolicy );
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
			gameManager.ApplyResults( ChosenPolicy );
			SetRandomValues();
			this.transform.rotation = Quaternion.identity;	//TODO: play next card animation
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

	/// <summary>
	/// Sets card values at random.
	/// </summary>
	// TODO: Replace by getting next card from stack
	private void SetRandomValues()
	{
		Debug.LogWarning ( "Getting new Card not implemented! Only sets new random values on the same card." );
		for ( int i = 0; i < policyValuesL.Length; i++ )
		{
			policyValuesL[i] = Random.Range( -minMaxPolicyValue, minMaxPolicyValue );
			policyValuesR[i] = Random.Range ( -minMaxPolicyValue, minMaxPolicyValue );
		}
	}
}
