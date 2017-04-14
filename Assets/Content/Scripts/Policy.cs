using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts
{
	[RequireComponent(typeof(Image))]
	public class Policy : APolicy
	{
		/// <summary>
		/// Adds the summand and also update text and icon maxColor
		/// </summary>
		/// <param name="summand">Value to be subtracted/ added to value.</param>
		/// <param name="minColor">Color a of icon maxColor lerp</param>
		/// <param name="maxColor">Color b of icon maxColor lerp</param>
		public override void AddValue ( int summand )
		{
			this.SetValue( base.value + summand);
		}
		
		/// <summary>
		/// Sets the value and also update text and icon maxColor
		/// </summary>
		/// <param name="targetValue">Sets this policies value</param>
		public override void SetValue( int targetValue )
		{
			this.SetValue ( targetValue, 100 );
		}

		/// <summary>
		/// Sets the value and also update text and icon maxColor
		/// </summary>
		/// <param name="targetValue">Sets this policies value</param>
		public override void SetValue( int targetValue, float maximum )
		{
			base.value = targetValue;
			base.Text.text = base.type == PolicyType.Treasury ? (base.Value * 1000).ToString() : base.Value.ToString();

			if ( base.type == PolicyType.Happiness )
			{
				base.Icon.fillAmount = (float)base.Value / maximum * -1 + 1;
			}
			else
			{
				base.Icon.fillAmount = (float)base.Value / maximum;
			}
		}

		/// <summary>
		/// Sets Icon color as preview for value change
		/// </summary>
		/// <param name="value">Policy summand value.</param>
		/// <param name="minColor">Min color for value lerp.</param>
		/// <param name="maxColor">Max color for value lerp.</param>
		public override void Preview ( int value, Color minColor ,Color maxColor )
		{
			this.StopAllCoroutines ();
			var normalizedValue = (float)Mathf.Abs ( value ) / 20 ;
			var targetColor = Color.Lerp ( minColor, maxColor, normalizedValue );
			base.PreviewIcon.color = targetColor;
		}
		
		/// <summary>
		/// Starts reverting colors;
		/// </summary>
		public override void RevertPreviewValue( float speed )
		{
			this.RevertPreviewValue ( speed, base.PreviewIcon.color );
		}

		/// <summary>
		/// Starts reverting colors and sets start color;
		/// </summary>
		public override void RevertPreviewValue(float speed, Color startColor)
		{
			this.StopAllCoroutines();
			this.StartCoroutine(this.RevertPreviewAnimation( base.PreviewIcon, speed, startColor ));
		}

		/// <summary>
		/// Lerps image colors back to its init maxColor.
		/// </summary>
		/// <returns>
		/// The <see cref="IEnumerator"/>.
		/// </returns>
		protected override IEnumerator RevertPreviewAnimation( Image icon, float speed, Color startColor )
		{
			float t = 0;
			while (t < 1f)
			{
				icon.color = Color.Lerp ( startColor, Color.clear, t );
				t += speed * Time.deltaTime;
				yield return null;
			}
			icon.color = Color.clear;
		}
	}
}
