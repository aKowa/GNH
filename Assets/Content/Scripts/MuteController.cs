using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts
{
	public class MuteController : MonoBehaviour
	{
		private Toggle toggle = null;

		private void Start ()
		{
			this.toggle = this.GetComponent <Toggle> ();
			this.toggle.isOn = AudioController.IsMuted;
			this.Mute ();
		}

		public void Mute ()
		{
			AudioController.Instance.Mute ( this.toggle.isOn );
		}
	}
}
