using System.Collections;
using System;
using UnityEngine;

namespace Content.Scripts
{
	public class AudioController : MonoBehaviour
	{
		/// <summary>
		/// Background music source
		/// </summary>
		[SerializeField]
		private AudioSource bgMusic = null;

		/// <summary>
		/// Sound source
		/// </summary>
		[SerializeField]
		private AudioSource sound = null;

		/// <summary>
		/// internal instance
		/// </summary>
		private static AudioController instance;

		/// <summary>
		/// external instance
		/// </summary>
		public static AudioController Instance
		{
			get { return instance; }
		}

		/// <summary>
		/// Sets static instance 
		/// </summary>
		private void Awake ()
		{
			instance = this;
		}


		private void Mute ( bool state )
		{
			this.gameObject.SetActive ( state );
		}
	}
}
