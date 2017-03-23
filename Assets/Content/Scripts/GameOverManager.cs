using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour 
{
	public void OnTouch()
	{
		Debug.Log( "Reload Game!" );
		SceneManager.LoadScene( 0 );
	}
}
