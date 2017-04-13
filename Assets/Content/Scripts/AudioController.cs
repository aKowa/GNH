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
		/// internal instance
		/// </summary>
		private static AudioController instance = null;

		/// <summary>
		/// external instance
		/// </summary>
		public static AudioController Instance
		{
			get { return instance; }
		}

		private static GameObject soundController = null;

		private static bool isMuted;

		public static bool IsMuted
		{
			get { return isMuted; }
		}

		/// <summary>
		/// Sets static instance 
		/// </summary>
		private void Start ()
		{
			if ( instance == null )
			{
				instance = this;
				soundController = Instantiate(new GameObject(), this.transform, false);
				soundController.name = "SoundController";
				DontDestroyOnLoad(this.gameObject);
			}
		}

		/// <summary>
		/// Sets mute state
		/// </summary>
		/// <param name="state">Target mute state.</param>
		public void Mute ( bool state )
		{
			isMuted = state;
			this.bgMusic.mute = state;
		}

		public void PlaySound ( AudioClip clip )
		{
			if ( !isMuted )
			{
				var soundInstance = Instantiate ( new GameObject (), soundController.transform, false );
				soundInstance.name = clip.name;
				var source = soundInstance.AddComponent <AudioSource> ();
				source.clip = clip;
				source.Play ();
				this.StartCoroutine ( this.DestroySoundInstance ( soundInstance, clip.length ) );
			}
		}

		private IEnumerator DestroySoundInstance ( GameObject soundInstance, float time )
		{
			yield return new WaitForSeconds ( time );
			Destroy ( soundInstance );
		}
	}
}
