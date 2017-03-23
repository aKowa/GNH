// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameManager.cs" company="-">
//   André Kowalewski & Bent Nürnberg
// </copyright>
// <summary>
//   Defines the GameManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Content.Scripts
{
    using System.Collections;
    using System.Collections.Generic;

    using PolyDev.UI;

    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The game manager.
    /// </summary>
    /// TODO: can be made static?
    public class GameManager : MonoBehaviour
    {
        public BindInt[] boundPolicyValues = new BindInt[6];

        public Color negativePreviewColor = Color.red;

        public Color positivePreviewColor = Color.green;

        public float revertSpeed = 1f;

        private List<CardData> cardData;

        private Text[] textValues;

        private Color initColor;


		/// <summary>
		/// The start.
		/// </summary>
		public void Start ()
		{
			this.textValues = this.GetComponentsInChildren<Text> ();
			this.initColor = this.textValues[0].color;
		}

		// TODO: Implement next card logic (here or in Card Controller?)
		public void ApplyResults(int[] values)
        {
            this.RevertPreview();
            for (int i = 0; i < values.Length; i++)
            {
                this.boundPolicyValues[i].Value += values[i];
            }
        }

        /// <summary>
        /// Shows a preview of the possible result.
        /// </summary>
        public void PreviewResults(int[] values)
        {
            this.StopAllCoroutines();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > 0)
                {
                    this.textValues[i].color = this.positivePreviewColor;
                }
                else if (values[i] < 0)
                {
                    this.textValues[i].color = this.negativePreviewColor;
                }
            }
        }

        /// <summary>
        /// Starts reverting colors;
        /// </summary>
        public void RevertPreview()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.RevertPreviewAnimation());
        }

        /// <summary>
        /// Lerps image colors back to its init color.
        /// </summary>
        private IEnumerator RevertPreviewAnimation()
        {
            float t = 0;
            var colors = new Color[this.textValues.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = this.textValues[i].color;
            }

            while (t < 1f)
            {
                for (int i = 0; i < this.textValues.Length; i++)
                {
                    this.textValues[i].color = Color.Lerp(colors[i], this.initColor, t);
                }

                t += this.revertSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }
}
