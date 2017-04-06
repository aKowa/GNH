// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameManager.cs" company="-">
//   André Kowalewski & Bent Nürnberg
// </copyright>
// <summary>
//   Defines the GameManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using PolyDev.UI;

namespace Content.Scripts
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The game manager.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// The block input.
        /// </summary>
        [HideInInspector]
        public bool blockInput = false;
	
		/// <summary>
		/// The bound policy values.
		/// </summary>
		[SerializeField]
		private Policy[] policies = new Policy[6];

		/// <summary>
		/// The game over object.
		/// </summary>
		[SerializeField]
        private GameObject gameOverObject = null;

        /// <summary>
        /// Bound round count
        /// </summary>
        [SerializeField]
        public BindInt round;

        /// <summary>
        /// Checks Happiness Threshold Win every set round.
        /// </summary>
        [Tooltip("Checks Happiness Threshold Win every set round.")]
        [SerializeField]
        private int winCheck = 5;

        /// <summary>
        /// The happiness threshold win.
        /// </summary>
        [Tooltip("Threshold policyType of happines used to determine when player lost! Use negative policyType to disable.")]
        [SerializeField]
        private int happinessThresholdWin = 90;

        /// <summary>
        /// The happiness threshold lose.
        /// </summary>
        [Tooltip("Threshold policyType of happines used to determine when player lost! Use negative policyType to disable.")]
        [SerializeField]
        private int happinessThresholdLose = 20;

		/// <summary>
		/// Win screen sprite.
		/// </summary>
		[Tooltip("This Screen is set on Game Over, if the player has won.")]
		[SerializeField]
		private Sprite winScreen;

		/// <summary>
		/// Lose screens, when policy is too high.
		/// </summary>
		[Tooltip("ID determines which screen is shown, when corresponding policy value is too high.")]
		[SerializeField]
		private Sprite[] loseTooHighScreens;

		/// <summary>
		/// Lose screens, when policy is too low.
		/// </summary>
		[Tooltip("ID determines which screen is shown, when corresponding policy value is too low.")]
		[SerializeField]
		private Sprite[] loseTooLowScreens;

		/// <summary>
		/// Fales, when values on policies should be shown.
		/// </summary>
		[Tooltip ( "Fales, when values on policies should be shown." )]
		[SerializeField]
		private bool hideValues = true;

		/// <summary>
		/// An policy icons target color, when reaching its max value.
		/// </summary>
		[Tooltip( "An policy icons target color, when reaching its max value." )]
		[SerializeField]
		private Color maxColor = Color.black;

		/// <summary>
		/// An policy icons target color, when reaching its min value.
		/// </summary>
		[Tooltip ( "An policy icons target color, when reaching its min value." )]
		[SerializeField]
		private Color minColor = Color.black;

		/// <summary>
		/// The preview color negative.
		/// </summary>
		[SerializeField]
        private Color negativPreviewColor = Color.red;

        /// <summary>
        /// The preview color positive.
        /// </summary>
        [SerializeField]
        private Color positivePreviewColor = Color.green;

        /// <summary>
        /// The revert speed.
        /// </summary>
        [SerializeField]
        private float revertSpeed = 1f;

        /// <summary>
        /// The get policy value.
        /// </summary>
        /// <param name="policyType">
        /// The policy value.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int GetPolicyValue( PolicyType policyType )
        {
            switch (policyType)
            {
                case PolicyType.Culture:
                case PolicyType.Economy:
                case PolicyType.Environment:
                case PolicyType.Security:
                case PolicyType.Treasury:
				case PolicyType.Happiness:
					return this.policies[(int)policyType].Value;
                default:
                    Debug.LogWarning("passed PolicyValue enum not valid, please think about what you are doing here. returned 0.");
                    return 0;
            }
        }

        /// <summary>
        /// The apply results.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        public void ApplyResults(int[] values)
        {
	        var targetHappiness = 0;
            for (var i = 0; i < values.Length; i++)
            {
                this.policies[i].AddValue(values[i], minColor, maxColor);
	            targetHappiness += this.policies[i].Value;
            }

			// set happiness to average of 4 main policies.
			this.policies[(int)PolicyType.Happiness].SetValue( targetHappiness / 4, this.minColor, this.maxColor );

			this.RevertPreview(this.revertSpeed);
            this.round.Value ++;
            this.CheckforGameOver();
        }

        /// <summary>
        /// Shows a preview of the possible result.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        public void PreviewResults( int[] values )
        {
            for (var i = 0; i < values.Length; i++)
            {
				if ( values[i] > 0 )
	            {
					this.policies[i].SetIconColor( this.positivePreviewColor );
	            }
	            else if ( values[i] < 0 )
	            {
					this.policies[i].SetIconColor( this.negativPreviewColor );
				}
            }
        }

		/// <summary>
		/// Starts reverting colors;
		/// </summary>
		public void RevertPreview()
        {
	        this.RevertPreview( this.revertSpeed );
        }

		/// <summary>
		/// Overload. Starts reverting preview color.
		/// </summary>
		/// <param name="speed">The speed at which the color lerps back</param>
		public void RevertPreview( float speed )
		{
			foreach (var policy in this.policies)
			{
				policy.RevertPreviewValue( speed );
			}
		}

		/// <summary>
		/// The start. Sets initial parameter
		/// </summary>
		public void Start()
        {
            this.gameOverObject.SetActive(false);
            this.blockInput = false;
			this.SetValuesActive( !this.hideValues );

	        foreach ( var policy in this.policies )
	        {
				policy.SetValue(50, this.minColor, this.maxColor);
			}
		}

		/// <summary>
		/// Shows/ hides values on interface
		/// </summary>
		/// <param name="state"></param>
		public void SetValuesActive( bool state )
		{
			foreach (var policy in this.policies)
			{
				policy.Text.enabled = state;
			}
		    this.round.targetGameObject.GetComponent<Text>().enabled = state;
		}

        /// <summary>
        /// Checks policy values and determines as well as executes a game over. 
        /// NOTE: reload logic is on the GameOverScreenObject
        /// TODO: refine win logic, when card count is implemented
        /// </summary>
        private void CheckforGameOver()
        {
            if (this.round.valueUnbound % this.winCheck == 0)
            {
                // Check for victory by happiness
                if (this.policies[5].Value >= this.happinessThresholdWin && this.happinessThresholdWin > 0)
                {
                    this.GetGameOverText(5).text =
                        "Victory! \n \n Your happiness exceeds all expectations! \n \n Party hard!!!";
                    this.SetWinImage();
                    return;
                }
            }

            // Check for lose by happines
            if (this.policies[5].Value <= this.happinessThresholdLose && this.happinessThresholdLose > 0)
            {
                this.GetGameOverText(5).text += " was too damn low!";
	            this.SetLoseToLowImage(5);
                return;
            }

            // check policy bounds
            for (var i = 0; i < 4; i++)
            {
                // Check if policy is too low
                if (this.policies[i].Value <= 0)
                {
                    this.GetGameOverText(i).text += " was too damn low!";
					this.SetLoseToLowImage(i);
                    return;
                }

                // Check if policy is too high
                if (this.policies[i].Value >= 100)
                {
                    this.GetGameOverText(i).text += " was too damn high!";
					this.SetLoseToHighImage(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Returns GameOver text object and sets parameter for blocking input.
        /// </summary>
        /// <param name="policyId">
        /// The policyID responsible for the game over.
        /// </param>
        /// <returns>
        /// The <see cref="Text"/>.
        /// </returns>
        private Text GetGameOverText(int policyId)
        {
            this.blockInput = true;
            this.gameOverObject.SetActive(true);
            var gameOverText = this.gameOverObject.GetComponentInChildren<Text>(true);
	        var policyName = this.policies[policyId].type.ToString();
			gameOverText.text = "You Lost! \n \n Your " + policyName;
            return gameOverText;
        }

		/// <summary>
		/// Sets the win screen
		/// </summary>
	    private void SetWinImage()
	    {
		    this.gameOverObject.SetActive(true);
		    var gameOverImage = this.gameOverObject.GetComponent<Image>();
		    gameOverImage.sprite = this.winScreen;
	    }

		/// <summary>
		/// Sets the lose screen to the correxponding too high policy
		/// </summary>
		/// <param name="id">Policy id</param>
	    private void SetLoseToHighImage( int id )
	    {
			this.gameOverObject.SetActive(true);
			var gameOverImage = this.gameOverObject.GetComponent<Image>();
		    try
		    {
				gameOverImage.sprite = this.loseTooHighScreens[id];
			}
		    catch (IndexOutOfRangeException)
		    {
				Debug.LogWarning("No loseToHighScreen set at id: " + id);
			    gameOverImage.sprite = this.winScreen;
		    }
		}

		/// <summary>
		/// Sets the lose screen to the correxponding too low policy
		/// </summary>
		/// <param name="id">Policy id</param>
		private void SetLoseToLowImage(int id)
		{
			this.gameOverObject.SetActive(true);
			var gameOverImage = this.gameOverObject.GetComponent<Image>();
			try
			{
				gameOverImage.sprite = this.loseTooLowScreens[id];
			}
			catch (IndexOutOfRangeException)
			{
				Debug.LogWarning("No loseToLowScreen set at id: " + id);
				gameOverImage.sprite = this.winScreen;
			}
		}
	}
}
