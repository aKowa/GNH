using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	/// <summary>
	/// The scene ID to be Loaded.
	/// </summary>
	[Tooltip( "The scene ID to be Loaded. Make this " )]
	public int id = 1;

	/// <summary>
	/// Loads the scene with the specified ID.
	/// </summary>
	public void LoadScene ()
	{
		SceneManager.LoadScene( id );
	} 
}
