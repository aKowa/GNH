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
		/// Adds the summand and also update text and icon color
		/// </summary>
		/// <param name="summand">Value to be subtracted/ added to value.</param>
		/// <param name="minColor">Color a of icon color lerp</param>
		/// <param name="maxColor">Color b of icon color lerp</param>
		public void AddValue ( int summand, Color minColor, Color maxColor )
		{
			this.SetValue( this.value + summand, minColor, maxColor );
		}

		/// <summary>
		/// Sets the value and also update text and icon color
		/// </summary>
		/// <param name="targetValue">Sets this policies value</param>
		/// <param name="minColor">Color a of icon color lerp</param>
		/// <param name="maxColor">Color b of icon color lerp</param>
		public void SetValue(int targetValue, Color minColor, Color maxColor)
		{
			this.value = targetValue;
			this.Text.text = this.type == PolicyType.Treasury ? (this.Value * 1000).ToString() : this.Value.ToString();
			this.Icon.color = Color.Lerp(minColor, maxColor, (float)this.Value / 100);
		}

		/// <summary>
		/// Sets Icon Color.
		/// </summary>
		/// <param name="targetColor">Target Color</param>
		public void SetIconColor( Color targetColor )
		{
			this.Icon.color = this.Icon.color.GetRGB( targetColor );
		}

		/// <summary>
		/// Starts reverting colors;
		/// </summary>
		public void RevertPreviewValue( float speed )
		{
			this.StopAllCoroutines();
			this.StartCoroutine( this.RevertPreviewAnimation( speed ) );
		}

		/// <summary>
		/// Lerps image colors back to its init color.
		/// </summary>
		/// <returns>
		/// The <see cref="IEnumerator"/>.
		/// </returns>
		private IEnumerator RevertPreviewAnimation( float speed )
		{
			float t = 0;
			var color = this.Icon.color;
			while (t < 1f)
			{
				this.Icon.color = this.Icon.color.GetRGB( Color.Lerp(color, this.startColor, t) );
				t += speed * Time.deltaTime;
				yield return null;
			}
		}

		/// <summary>
		/// Start setting init param.
		/// </summary>
		private void Start ()
		{
			this.startColor = this.Icon.color;
		}
	}
}
