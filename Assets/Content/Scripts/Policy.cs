using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts
{
	[RequireComponent(typeof(Image))]
	public class Policy : MonoBehaviour
	{
        /// <summary>
		/// The Policy type. 
		/// </summary>
		/// TODO: Needed?
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
                if (text != null)
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
                if (text != null)
                {
                    return this.icon;
                }
                return this.icon = this.gameObject.GetComponent<Image>();
            }
        }

        /// <summary>
        /// Internal value of the policy.
        /// </summary>
        private int value = 50;

		/// <summary>
		/// External value of the policy.
		/// </summary>
		public int Value
		{
			get
			{
				return this.value;
			}
		}

		/// <summary>
		/// Sets the value and also update text and icon color
		/// </summary>
		/// <param name="summand">Value to be subtracted/ added to value.</param>
		/// <param name="minColor">Color a of icon color lerp</param>
		/// <param name="maxColor">Color b of icon color lerp</param>
		public void SetValue ( int summand, Color minColor, Color maxColor )
		{
			this.value += summand;
			this.Text.text = this.Value.ToString ();
			this.Icon.color = Color.Lerp ( minColor, maxColor, (float)this.Value / 100 );
			Debug.Log ( this.Value / 100 );
		}

		/// <summary>
		/// Start setting init param.
		/// </summary>
		private void Start ()
		{
			Color targetColor = Color.black;
			targetColor.a = this.Icon.color.a / 2;
			this.Icon.color = targetColor;
		}
	}
}
