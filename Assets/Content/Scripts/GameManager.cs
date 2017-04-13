// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameManager.cs" company="-">
//   André Kowalewski & Bent Nürnberg
// </copyright>
// <summary>
//   Defines the GameManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Content.Scripts
{
	/// <summary>
	///     The game manager.
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		/// <summary>
		/// Fales, when values on policies should be shown.
		/// </summary>
		[Tooltip ( "Fales, when values on policies should be shown." )]
		[SerializeField]
		private bool showValues = false;

		/// <summary>
		/// The block input.
		/// </summary>
		[HideInInspector]
		public bool blockInput;

		/// <summary>
		/// The bound policy values.
		/// </summary>
		[SerializeField]
		private Policy[] policies = new Policy[6];

		/// <summary>
		/// The start value range.
		/// </summary>
		[SerializeField]
		private Vector2 startValueRange = new Vector2( 45f, 55f );

		/// <summary>
		/// The game over object.
		/// </summary>
		[SerializeField]
		private GameObject gameOverObject = null;

		/// <summary>
		///  Win screen sprite.
		/// </summary>
		[Tooltip ( "This Screen is set on Game Over, if the player has won." )]
		[SerializeField]
		private Sprite winScreen = null;

		/// <summary>
		///  Lose screens, when policy is too low.
		/// </summary>
		[Tooltip("ID determines which screen is shown, when corresponding policy value is too low.")]
		[SerializeField]
		private Sprite[] loseScreens = null;

		/// <summary>
		/// Checks Happiness Threshold Win every set round.
		/// </summary>
		[Tooltip ( "Checks Happiness Threshold Win every set round." )]
		[SerializeField]
		private int winCheck = 5;

		/// <summary>
		///  The happiness threshold win.
		/// </summary>
		[Tooltip ( "Threshold policyType of happines used to determine when player lost! Use negative policyType to disable.")]
		[SerializeField]
		private int winThreshold = 90;

		/// <summary>
		/// The happiness threshold lose.
		/// </summary>
		[Tooltip ( "Threshold policyType of happines used to determine when player lost! Use negative policyType to disable.")]
		[SerializeField]
		private int loseDeviationThreshold = 20;

		/// <summary>
		/// Round count
		/// </summary>
		private int round;

		/// <summary>
		/// The min preview color.
		/// </summary>
		[SerializeField]
		private Color minPreviewColor = Color.white;

		/// <summary>
		/// The max preview color.
		/// </summary>
		[SerializeField]
		private Color maxPreviewColor = Color.blue;

		/// <summary>
		///     The revert speed.
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
		/// The <see cref="int" />.
		/// </returns>
		public int GetPolicyValue ( PolicyType policyType )
		{
			switch ( policyType )
			{
				case PolicyType.Culture:
				case PolicyType.Economy:
				case PolicyType.Environment:
				case PolicyType.Security:
				case PolicyType.Treasury:
				case PolicyType.Happiness:
					return this.policies[(int) policyType].Value;
				default:
					Debug.LogWarning ( "passed PolicyValue enum not valid, please think about what you are doing here. returned 0." );
					return 0;
			}
		}

		/// <summary>
		/// The apply results.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		public void ApplyResults ( int[] values )
		{
			for ( var i = 0; i < values.Length; i++ )
			{
				this.policies[i].AddValue ( values[i] );
			}

			this.SetHappiness ();

			this.RevertPreview ( this.revertSpeed );
			++this.round;
			this.CheckforGameOver ();
		}

		/// <summary>
		/// Shows a preview of the possible result.
		/// </summary>
		/// <param name="values">
		/// The values.
		/// </param>
		public void PreviewResults ( int[] values )
		{
			for ( var i = 0; i < values.Length; i++ )
			{
				var valueAbs = Mathf.Abs ( values[i] );
				if ( valueAbs > 0 )
				{
					this.policies[i].Preview ( valueAbs, this.minPreviewColor, this.maxPreviewColor);
				}
			}
		}

		/// <summary>
		/// Starts reverting colors;
		/// </summary>
		public void RevertPreview ()
		{
			this.RevertPreview ( this.revertSpeed );
		}

		/// <summary>
		/// Overload. Starts reverting preview color.
		/// </summary>
		/// <param name="speed">The speed at which the color lerps back</param>
		public void RevertPreview ( float speed )
		{
			foreach ( var policy in this.policies )
				policy.RevertPreviewValue ( speed );
		}

		/// <summary>
		/// The start. Sets initial parameter
		/// </summary>
		public void Start ()
		{
			this.gameOverObject.SetActive ( false );
			this.blockInput = false;

			this.SetValuesActive ( Debug.isDebugBuild && this.showValues );

			foreach ( var policy in this.policies )
			{
				policy.SetValue ( (int)Random.Range (  this.startValueRange.x, this.startValueRange.y ) );
			}

			this.SetHappiness ();
		}

		/// <summary>
		///  Shows/ hides values on interface
		/// </summary>
		/// <param name="state">The target active state</param>
		public void SetValuesActive ( bool state )
		{
			foreach ( var policy in this.policies )
			{
				policy.Text.enabled = state;
			}
		}

		/// <summary>
		/// Checks policy values and determines as well as executes a game over.
		/// NOTE: reload logic is on the GameOverScreenObject
		/// TODO: Add new elections card at this count
		/// </summary>
		private void CheckforGameOver ()
		{
			if ( this.round % this.winCheck == 0 )
			{
				if ( this.policies[5].Value <= this.winThreshold )
				{
					this.GetGameOverText ( 5 ).text = "Victory! \n \n Your happiness exceeds all expectations! \n \n Party hard!!!";
					this.SetWin ();
					return;
				}
			}
			
			// Check for lose by happiness
			if ( this.policies[ (int)PolicyType.Happiness ].Value >= this.loseDeviationThreshold && this.loseDeviationThreshold > 0 )
			{
				this.GetGameOverText ( 5 ).text += " was too damn low!";
				this.SetLose ( 5 );
				return;
			}
			
			// check policy bounds
			for ( var i = 0; i < 4; i++ )
				if ( this.policies[i].Value <= 0 )
				{
					this.GetGameOverText ( i ).text += " was too damn low!";
					this.SetLose ( i );
					return;
				}
		}

		/// <summary>
		///     Returns GameOver text object and sets parameter for blocking input.
		/// </summary>
		/// <param name="policyId">
		///     The policyID responsible for the game over.
		/// </param>
		/// <returns>
		///     The <see cref="Text" />.
		/// </returns>
		private Text GetGameOverText ( int policyId )
		{
			this.blockInput = true;
			this.gameOverObject.SetActive ( true );
			var gameOverText = this.gameOverObject.GetComponentInChildren <Text> ( true );
			var policyName = this.policies[policyId].type.ToString ();
			gameOverText.text = "You Lost! \n Your " + policyName;
			return gameOverText;
		}

		/// <summary>
		/// Sets the win screen
		/// </summary>
		private void SetWin ()
		{
			// set screen
			this.gameOverObject.SetActive ( true );
			var gameOverImage = this.gameOverObject.GetComponent <Image> ();
			gameOverImage.sprite = this.winScreen;

			// play audio
			AudioController.BgMusic.clip = Resources.Load ("ending_happy") as AudioClip;
			AudioController.BgMusic.Play();
		}

		/// <summary>
		/// Sets the lose screen to the correxponding too low policy
		/// </summary>
		/// <param name="id">Policy id</param>
		private void SetLose ( int id )
		{
			this.gameOverObject.SetActive ( true );
			var gameOverImage = this.gameOverObject.GetComponent <Image> ();
			try
			{
				gameOverImage.sprite = this.loseScreens[id];
			}
			catch ( IndexOutOfRangeException )
			{
				Debug.LogWarning ( "IndexOutOfRange! No loseToLowScreen set at id: " + id );
				gameOverImage.sprite = this.loseScreens[0] ?? this.winScreen;
			}

			// play audio
			AudioController.BgMusic.clip = Resources.Load("ending_sad") as AudioClip;
			AudioController.BgMusic.Play();
		}

		/// <summary>
		/// Sets happiness value to max deviation
		/// </summary>
		private void SetHappiness ()
		{
			// set average
			var average = 0;
			for ( var i = 0; i < 4; i++ )
			{
				average += this.policies[i].Value;
			}
			average = average / 4;

			// set max deviation to average
			var maxDeviation = 0;
			var policyID = 0;
			for ( var j = 0; j < 4; j++ )
			{
				var tempDeviation = Math.Abs ( this.policies[j].Value - average );
				if ( tempDeviation > maxDeviation )
				{
					maxDeviation = tempDeviation;
					policyID = j;
				}
			}
			
			// set happiness value
			this.policies[(int)PolicyType.Happiness].SetValue( maxDeviation, this.loseDeviationThreshold );
			//Debug.Log ("Average: " + average +  "  Deviation: " + maxDeviation + "  of " + this.policies[policyID].name );
		}
	}
}
