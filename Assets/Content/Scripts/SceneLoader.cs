using UnityEngine;
using UnityEngine.SceneManagement;

namespace Content.Scripts
{
	public class SceneLoader : MonoBehaviour
	{
		/// <summary>
		/// The scene ID to be Loaded.
		/// </summary>
		[Tooltip ( "The scene ID to be Loaded. Make this " )]
		public int id = 1;

		/// <summary>
		/// Loads the scene with the specified ID.
		/// </summary>
		public void LoadScene ()
		{
			AudioController.Instance.PlaySound ( Resources.Load ( "button" ) as AudioClip );
			SceneManager.LoadScene ( id );
		}
	}
}
