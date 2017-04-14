using System;
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
		private void Awake ()
		{
			if ( AudioController.Instance == null )
			{
				if ( this.audioController == null )
				{
					throw new NullReferenceException ();
				}
				AudioController.Instantiate ( this.audioController );
			}
		}
	}
}
