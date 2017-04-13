using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Content.Scripts
{
	public class AudioInstanceController : MonoBehaviour
	{
		/// <summary>
		/// audio controller prefab
		/// </summary>
		[SerializeField]
		private GameObject audioController = null;

		/// <summary>
		/// Sets audio controller instance
		/// </summary>
		private void Start ()
		{
			if ( AudioController.Instance == null )
			{
				Instantiate ( this.audioController, this.transform.position, Quaternion.identity );
			}
		}
	}
}
