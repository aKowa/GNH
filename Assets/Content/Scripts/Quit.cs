using System.Collections;
using UnityEngine;


namespace Content.Scripts
{
	public class Quit : MonoBehaviour
	{
		/// <summary>
		/// Quits the game on click
		/// </summary>
		public void OnClick ()
		{
			Application.Quit ();
		}
	}
}
