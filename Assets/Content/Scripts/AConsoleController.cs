using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts
{
	/// <summary>
	/// Abstract console controller. provides parameter as properties and basic functionality
	/// </summary>
	public abstract class AConsoleController : MonoBehaviour
	{
		/// <summary>
		/// Internal static instance
		/// </summary>
		private static AConsoleController instance;

		/// <summary>
		/// External static instance (Readonly)
		/// </summary>
		public static AConsoleController Instance
		{
			get
			{
				if (instance == null)
				{
					Debug.LogWarning("ConsoleController.Instance is null!");
				}
				return instance;
			}
		}

		/// <summary>
		/// Sets instance on awake and init param
		/// </summary>
		private void Awake()
		{
			AConsoleController.instance = this;
			this.ToggleActive();
			this.transform.parent.gameObject.SetActive ( Debug.isDebugBuild );
		}

		/// <summary>
		/// Internal debug text.
		/// </summary>
		private Text debugMessage;

		/// <summary>
		/// External Debug text (Readonly)
		/// </summary>
		public Text DebugMessage
		{
			get
			{
				if (this.debugMessage == null)
				{
					return this.debugMessage = this.GetComponentInChildren<Text>();
				}
				return this.debugMessage;
			}
		}

		/// <summary>
		/// Call to add text to debug message.
		/// </summary>
		/// <param name="targetText"></param>
		public abstract void AddToConsole(string targetText);

		/// <summary>
		/// Function called, when console is clicked or tabed
		/// </summary>
		public abstract void OnDown();


		/// <summary>
		/// Disabels console and empties debug message
		/// </summary>
		protected void ToggleActive()
		{
			this.DebugMessage.text = "";
			this.gameObject.SetActive(!this.gameObject.activeSelf);
		}
	}
}
