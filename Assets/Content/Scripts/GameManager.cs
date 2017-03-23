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

		public GameObject gameOverObject;

		public Color negativePreviewColor = Color.red;

        public Color positivePreviewColor = Color.green;

        public float revertSpeed = 1f;

        private List<CardData> cardData; // TODO: is this param needed?

        private Text[] textValues;

        private Color initColor;

		[HideInInspector]
	    public bool blockInput = false;

		/// <summary>
		/// The start. Sets initial parameter
		/// </summary>
		public void Start ()
		{
			this.textValues = this.GetComponentsInChildren<Text> ();
			this.initColor = this.textValues[0].color;
			this.gameOverObject.SetActive( false );
			blockInput = false;
		}

		// TODO: Implement next card logic (here or in Card Controller?) -> in Card Controller is fine I guess ;)
		public void ApplyResults(int[] values)
        {
            this.RevertPreview();
            for (int i = 0; i < values.Length; i++)
            {
                this.boundPolicyValues[i].Value += values[i];
            }
			this.CheckforGameOver();
        }

        /// <summary>
        /// Shows a preview of the possible result.
        /// </summary>
        public void PreviewResults(int[] values)
        {
            this.StopAllCoroutines();
            for (int i = 0; i < values.Length; i++)
            {
	            this.textValues[i].color = initColor;
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

	    private void CheckforGameOver()
	    {
		    for ( int i = 0; i < 4; i++ )
		    {
			    if ( boundPolicyValues[i].valueUnbound <= 45 )
			    {
				    blockInput = true;
					this.gameOverObject.SetActive( true );
				    var gameOverText = this.gameOverObject.GetComponentInChildren<Text>();
				    var policyName = boundPolicyValues[i].targetGameObject.GetComponentInParent<Image>().gameObject.name;
					gameOverText.text = "You Lost! \n \n Your " + policyName + " was too damn low!";
			    }
		    }
	    }
	}
}
