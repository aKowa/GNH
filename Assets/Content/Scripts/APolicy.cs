using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

				var images = this.gameObject.GetComponentsInChildren <Image> ();
				return images.FirstOrDefault ( image => image.tag == "OverlayIcon" );
			}
		}

		/// <summary>
		/// The preview Icon
		/// </summary>
		private Image previewIcon;

		/// <summary>
		/// External preview Icon.
		/// </summary>
		public Image PreviewIcon
		{
			get
			{
				if ( this.previewIcon != null )
				{
					return this.previewIcon;
				}

				var images = this.gameObject.GetComponentsInChildren<Image>();
				return this.previewIcon = images.FirstOrDefault(image => image.tag == "PreviewIcon");
			}
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
			this.icon = this.gameObject.GetComponentsInChildren<Image>()[1];
			this.PreviewIcon.color = Color.clear;
			this.text = this.gameObject.GetComponentInChildren<Text>();
		}

		public abstract void AddValue( int summand );
		public abstract void SetValue( int targetValue );
		public abstract void SetValue( int targetValue, float maximum );
		public abstract void Preview( int value, Color minColor,  Color maxColor );
		public abstract void RevertPreviewValue( float speed );
		protected abstract IEnumerator RevertPreviewAnimation(Image icon, float speed );
	}
}
