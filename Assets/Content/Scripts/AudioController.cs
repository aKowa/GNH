using System.Collections;
using System.Security.AccessControl;
using UnityEngine;

namespace Content.Scripts
{
	public class AudioController : MonoBehaviour
	{
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
		/// Background musicSource source
		/// </summary>
		[SerializeField]
		private AudioSource musicSource = null;

		/// <summary>
		/// Sound controller holding all soundSource instances
		/// </summary>
		[SerializeField]
		private AudioSource soundSource = null;

		/// <summary>
		/// Audio clips
		/// </summary>
		[SerializeField]
		private AudioClip[] audioClips = null;

		/// <summary>
		/// Mute bool
		/// </summary>
		public bool Mute
		{
			get
			{ return this.musicSource.mute; }
			set
			{
				this.musicSource.mute = value;
				this.soundSource.mute = value;
			}
		}

		/// <summary>
		/// Instantiate audiocontroller
		/// </summary>
		/// <param name="audioController"></param>
		public static void Instantiate ( GameObject audioController )
		{
			if ( instance != null ) return;
			var clone = Instantiate(audioController, Camera.main.transform.position, Quaternion.identity);
			instance = clone.GetComponent <AudioController>();
		}

		/// <summary>
		/// Game start
		/// </summary>
		private void Start ()
		{
			DontDestroyOnLoad( this.gameObject );
			if ( this.musicSource.clip.name != "background" )
			{
				this.Play ( 0 );
			}
			StartCoroutine ( this.LoadMusicFiles () );
		}

		/// <summary>
		/// Resets musicSource to background, after loading
		/// </summary>
		/// <param name="level"></param>
		private void OnLevelWasLoaded(int level)
		{
			if (this.musicSource.clip.name != "background")
			{
				this.Play(0);
			}
		}

		/// <summary>
		/// Plays audio
		/// </summary>
		public void Play ( int id )
		{
			if ( id < 0 )
			{
				Debug.LogWarning ( "Negative id is not allowed!" );
				return;
			}

			if ( id < 3 )
			{
				this.musicSource.clip = this.audioClips[id];
				this.musicSource.Play();
			}
			else
			{
				this.soundSource.clip = this.audioClips[id];
				this.soundSource.Play();
			}
		}

		private IEnumerator LoadMusicFiles ()
		{
			var asyncLoseClip = Resources.LoadAsync<AudioClip>("ending_sad");
			yield return asyncLoseClip;
			yield return this.audioClips[2] = asyncLoseClip.asset as AudioClip;

			var asyncWinClip = Resources.LoadAsync <AudioClip> ( "ending_happy" );
			yield return asyncWinClip;
			yield return this.audioClips[1] = asyncWinClip.asset as AudioClip;
		}
	}
}
