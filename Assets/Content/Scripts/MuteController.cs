using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts
{
	public class MuteController : MonoBehaviour
	{
		/// <summary>
		/// Toggle button
		/// </summary>
		private Toggle toggle = null;

		/// <summary>
		/// Sets init param
		/// </summary>
		private void Start ()
		{
			this.toggle = this.GetComponent <Toggle> ();
			this.toggle.isOn = AudioController.Instance.Mute;
			this.Mute ();
		}

		/// <summary>
		/// Sets mute state in audio controller
		/// </summary>
		public void Mute ()
		{
			AudioController.Instance.Mute = this.toggle.isOn;
		}
	}
}
