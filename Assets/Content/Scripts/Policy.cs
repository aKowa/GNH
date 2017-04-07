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
		/// <param name="minColor">Color a of icon maxColor lerp</param>
		/// <param name="maxColor">Color b of icon maxColor lerp</param>
		public override void SetValue( int targetValue )
		{
			base.value = targetValue;
			base.Text.text = base.type == PolicyType.Treasury ? (base.Value * 1000).ToString() : base.Value.ToString();
			base.Icon.fillAmount = (float)base.Value / 100;
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
			base.Icon.color = targetColor;
		}
		
		/// <summary>
		/// Starts reverting colors;
		/// </summary>
		public override void RevertPreviewValue( float speed )
		{
			this.StopAllCoroutines();
			this.StartCoroutine( this.RevertPreviewAnimation( speed ) );
		}

		/// <summary>
		/// Lerps image colors back to its init maxColor.
		/// </summary>
		/// <returns>
		/// The <see cref="IEnumerator"/>.
		/// </returns>
		protected override IEnumerator RevertPreviewAnimation( float speed )
		{
			float t = 0;
			var color = base.Icon.color;
			while (t < 1f)
			{
				base.Icon.color = base.Icon.color.GetRGB( Color.Lerp(color, base.StartColor, t) );
				t += speed * Time.deltaTime;
				yield return null;
			}
		}
	}
}
