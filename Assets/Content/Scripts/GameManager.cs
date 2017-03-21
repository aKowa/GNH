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
        public BindInt[] boundPolicyValues = new BindInt[4];

        public Color negativePreviewColor = Color.red;

        public Color positivePreviewColor = Color.green;

        public float revertSpeed = 1f;

        private List<CardData> cardData;

        private Image[] images;

        private Color initColor;

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
                    this.images[i].color = this.positivePreviewColor;
                }
                else if (values[i] < 0)
                {
                    this.images[i].color = this.negativePreviewColor;
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
        /// The start.
        /// </summary>
        public void Start()
        {
            this.images = this.GetComponentsInChildren<Image>();
            this.initColor = this.images[0].color;
        }

        /// <summary>
        /// Lerps image colors back to its init color.
        /// </summary>
        private IEnumerator RevertPreviewAnimation()
        {
            float t = 0;
            var colors = new Color[this.images.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = this.images[i].color;
            }

            while (t < 1f)
            {
                for (int i = 0; i < this.images.Length; i++)
                {
                    this.images[i].color = Color.Lerp(colors[i], this.initColor, t);
                }

                t += this.revertSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }
}
