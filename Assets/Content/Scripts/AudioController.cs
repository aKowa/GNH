using System.Collections;
using System.Security.AccessControl;
using UnityEngine;

namespace Content.Scripts
{
	public class AudioController : MonoBehaviour
	{
		/// <summary>
		/// Background music source
		/// </summary>
		[SerializeField]
		private static AudioSource bgMusic = null;

		/// <summary>
		/// external bg music
		/// </summary>
		public static AudioSource BgMusic
		{
			get { return bgMusic; }
		}

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

		/// <summary>
		/// Sound controller holding all sound instances
		/// </summary>
		private static GameObject soundController = null;

		/// <summary>
		/// internal mute flag
		/// </summary>
		private static bool isMuted;

		/// <summary>
		/// External mute flag
		/// </summary>
		public static bool IsMuted
		{
			get { return isMuted; }
		}

		/// <summary>
		/// Resets music to background, after loading
		/// </summary>
		/// <param name="level"></param>
		private void OnLevelWasLoaded(int level)
		{
			this.SetBgMusic ();
		}
		
		/// <summary>
		/// Sets static instance 
		/// </summary>
		private void Start ()
		{
			this.SetBgMusic ();
			if ( instance == null )
			{
				instance = this;
				soundController = Instantiate(new GameObject(), this.transform, false);
				soundController.name = "SoundController";
				DontDestroyOnLoad(this.gameObject);
			}
		}

		/// <summary>
		/// Sets audio soruce to background music
		/// </summary>
		private void SetBgMusic ()
		{
			if ( bgMusic == null )
			{
				bgMusic = this.GetComponentInChildren<AudioSource>();
			}
			if ( bgMusic.clip.name != "background" )
			{
				bgMusic.clip = Resources.Load("background") as AudioClip;
				bgMusic.Play();
			}
			bgMusic.loop = true;
		}

		/// <summary>
		/// Sets mute state
		/// </summary>
		/// <param name="state">Target mute state.</param>
		public void Mute ( bool state )
		{
			isMuted = state;
			bgMusic.mute = state;
		}

		/// <summary>
		/// Instantiates a game object and plays audio clip
		/// </summary>
		/// <param name="clip">Audio clip to be played</param>
		public void PlaySound ( AudioClip clip )
		{
			if ( !isMuted )
			{
				var soundInstance = Instantiate ( new GameObject (), soundController.transform, false );
				soundInstance.name = clip.name;
				var source = soundInstance.AddComponent <AudioSource> ();
				source.clip = clip;
				source.Play ();
				Destroy( soundInstance, clip.length);
			}
		}
	}
}
