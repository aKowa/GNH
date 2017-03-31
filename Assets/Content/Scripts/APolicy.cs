using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts
{
	/// <summary>
	/// Abstract Policy class providing readonly properties and setting init param
	/// </summary>
	[RequireComponent(typeof(Image))]
	public abstract class APolicy : MonoBehaviour
	{
		/// <summary>
		/// The Policy type. 
		/// </summary>
		[SerializeField]
		public PolicyType type;

		/// <summary>
		/// Internal text of the policy.
		/// </summary>
		private Text text;

		/// <summary>
		/// The external text value. Returns internal text, when != null.
		/// </summary>
		public Text Text
		{
			get
			{
				if (this.text != null)
				{
					return this.text;
				}
				return this.text = this.gameObject.GetComponentInChildren<Text>();
			}
		}

		/// <summary>
		/// Internal icon of the policy.
		/// </summary>
		private Image icon;

		/// <summary>
		/// The external Icon. Returns internal icon, when != null.
		/// </summary>
		public Image Icon
		{
			get
			{
				if (this.icon != null)
				{
					return this.icon;
				}
				return this.icon = this.gameObject.GetComponent<Image>();
			}
		}

		/// <summary>
		/// Start Icon Color.
		/// </summary>
		[Tooltip("Start Icon Color.")]
		[SerializeField]
		private Color startColor = Color.black;

		/// <summary>
		/// External start color (readonly)
		/// </summary>
		public Color StartColor
		{
			get { return this.startColor; }
		}


		/// <summary>
		/// Internal value of the policy.
		/// </summary>
		protected int value = 50;

		/// <summary>
		/// External value of the policy (readonly)
		/// </summary>
		public int Value
		{
			get { return this.value; }
		}

		/// <summary>
		/// Start setting init param.
		/// </summary>
		private void Start()
		{
			this.startColor = this.Icon.color;
		}

		public abstract void AddValue(int summand, Color minColor, Color maxColor);
		public abstract void SetValue(int targetValue, Color minColor, Color maxColor);
		public abstract void SetIconColor(Color targetColor);
		public abstract void RevertPreviewValue(float speed);
		protected abstract IEnumerator RevertPreviewAnimation(float speed);
	}
}
