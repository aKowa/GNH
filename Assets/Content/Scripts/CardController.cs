using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour
{
	public float maxAngle = 15f;

	/// <summary>
	/// Rotates the card towards the input position when dragged.
	/// </summary>
	public void OnDrag()
	{
		var targetDirection = ( Input.mousePosition - this.transform.position ).normalized;
		var angle = Vector3.Angle( targetDirection, Vector3.up );
		angle = Mathf.Clamp( angle, 0, maxAngle );
		if ( Input.mousePosition.x > this.transform.position.x )
		{
			angle *= -1;
		}
		this.transform.rotation = Quaternion.Euler( 0.0f, 0.0f, angle );
	}


	public void OnEndDrag()
	{
		
	}
}
