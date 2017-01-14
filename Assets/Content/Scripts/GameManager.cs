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
	using UnityEngine;
	using UnityEngine.UI;

    /// <summary>
    /// The game manager.
    /// TODO: can be made static?
    /// </summary>
    public class GameManager : MonoBehaviour
    {
	    public float revertSpeed = 1f;
        private List<CardData> cardData;
	    private Image[] images;
	    private Color initColor;

        /// <summary>
        /// The start.
        /// </summary>
        public void Start()
        {
            this.cardData = CardDataLoader.GetCardDataList();
	        this.images = this.GetComponentsInChildren<Image>();
	        initColor = this.images[0].color;
        }

		/// <summary>
		/// Shows a preview of the possible result.
		/// </summary>
	    public void PreviewResults()
	    {
			StopAllCoroutines();
			foreach ( var image in images )
			{
				image.color = Color.red;
			}
	    }

		/// <summary>
		/// Starts reverting colors;
		/// </summary>
	    public void RevertPreview()
		{
			StopAllCoroutines();
			StartCoroutine( RevertPreviewAnimation() );
		}

		/// <summary>
		/// Lerps image colors back to its init color.
		/// </summary>
	    private IEnumerator RevertPreviewAnimation()
	    {
		    float t = 0;
		    while ( t < 1f )
		    {
			    foreach ( var image in images )
			    {
				    image.color = Color.Lerp( Color.red, initColor, t );
				    t += revertSpeed * Time.deltaTime;
			    }
				yield return null;
			}
	    }

		// TODO: Implement next card logic
		public void ApplyResults()
	    {
			RevertPreview ();
			Debug.LogWarning( "Apply Results not implemented!" );
	    }
    }
}
