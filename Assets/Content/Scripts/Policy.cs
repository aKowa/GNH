using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts
{
	[RequireComponent(typeof(Image))]
	public class Policy : APolicy
	{
		/// <summary>
		/// Adds the summand and also update text and icon color
		/// </summary>
		/// <param name="summand">Value to be subtracted/ added to value.</param>
		/// <param name="minColor">Color a of icon color lerp</param>
		/// <param name="maxColor">Color b of icon color lerp</param>
		public override void AddValue ( int summand, Color minColor, Color maxColor )
		{
			this.SetValue( base.value + summand, minColor, maxColor );
		}
		
		/// <summary>
		/// Sets the value and also update text and icon color
		/// </summary>
		/// <param name="targetValue">Sets this policies value</param>
		/// <param name="minColor">Color a of icon color lerp</param>
		/// <param name="maxColor">Color b of icon color lerp</param>
		public override void SetValue(int targetValue, Color minColor, Color maxColor)
		{
			//Debug.Log( base.type );
			base.value = targetValue;
			base.Text.text = base.type == PolicyType.Treasury ? (base.Value * 1000).ToString() : base.Value.ToString();
			if (base.type == PolicyType.Security)
			{
				base.Icon.fillAmount = (float)base.Value / 100;
			}
			else
			{
				base.Icon.color = Color.Lerp(minColor, maxColor, (float)base.Value / 100);
			}
		}

		/// <summary>
		/// Sets Icon Color.
		/// </summary>
		/// <param name="targetColor">Target Color</param>
		public override void SetIconColor( Color targetColor )
		{
			base.Icon.color = base.Icon.color.GetRGB( targetColor );
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
		/// Lerps image colors back to its init color.
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
