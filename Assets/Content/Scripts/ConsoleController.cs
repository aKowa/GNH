using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts
{
	public class ConsoleController : AConsoleController
	{
		/// <summary>
		/// Adds debugMessage to debug console
		/// </summary>
		/// <param name="targetText">Text to be added on top of console.</param>
		public override void AddToConsole(string targetText)
		{
			if (!Debug.isDebugBuild)
			{
				return;
			}

			var text = this.gameObject.GetComponentInChildren<Text>();
			text.text = targetText + "\n" + text.text;
		}

		public override void OnDown()
		{
			if (!Debug.isDebugBuild)
			{
				return;
			}

			base.ToggleActive();
		}
	}
}
