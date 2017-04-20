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
		private Image gameOverImage = null;

		/// <summary>
		/// Win screen sprite.
		/// </summary>
		[Tooltip ( "This Screen is set on Game Over, if the player has won." )]
		[SerializeField]
		private Sprite winScreen = null;

		/// <summary>
		/// Lose screens, when policy is too low.
		/// </summary>
		[Tooltip("ID determines which screen is shown, when corresponding policy value is too low.")]
		[SerializeField]
		private Sprite[] loseScreensTooLow = null;

		/// <summary>
		/// Lose screens, when policy is too low.
		/// </summary>
		[SerializeField]
		private Sprite[] loseScreensTooHigh = null;

		/// <summary>
		/// Checks Happiness Threshold Win every set round.
		/// </summary>
		[Tooltip ( "Checks Happiness Threshold Win every set round." )]
		[SerializeField]
		private int winCheck = 5;

		/// <summary>
		/// External win check count
		/// </summary>
		public int WinCheck
		{
			get { return this.winCheck; }
		}

		/// <summary>
		/// The happiness threshold win.
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
		/// The maximum deviatedd policy id.
		/// </summary>
		private int maxDeviatiedPolicyID = -1;

		/// <summary>
		/// Determines if an election takes place
		/// </summary>
		public bool IsElection
		{
			get { return this.Round % this.WinCheck == 0; }
		}

		/// <summary>
		/// Internal Round count
		/// </summary>
		private int round = 1;

		/// <summary>
		/// External round count
		/// </summary>
		public int Round
		{
			get { return this.round; }
		}
		
		/// <summary>
		/// The revert speed.
		/// </summary>
		[SerializeField]
		private float revertSpeed = 1.5f;

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
		/// The negative apply color.
		/// </summary>
		[SerializeField]
		private Color negativeApplyColor = Color.red;

		/// <summary>
		/// The positive apply color.
		/// </summary>
		[SerializeField]
		private Color positiveApplyColor = Color.green;

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

			this.RevertPreview ( values, this.revertSpeed );
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
			{
				policy.RevertPreviewValue ( speed );
			}
		}

		/// <summary>
		/// Overload. Starts reverting preview color.
		/// </summary>
		/// <param name="speed">The speed at which the color lerps back</param>
		public void RevertPreview( int[] values, float speed)
		{
			for ( int i = 0; i < values.Length; i++ )
			{
				if ( values[i] > 0 )
				{
					this.policies[i].RevertPreviewValue ( speed, this.positiveApplyColor );
				}
				else if ( values[i] < 0 )
				{
					this.policies[i].RevertPreviewValue( speed, this.negativeApplyColor );
				}
			}
		}

		/// <summary>
		/// The start. Sets initial parameter
		/// </summary>
		public void Start ()
		{
			this.gameOverImage.gameObject.SetActive ( false );
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
		/// </summary>
		private void CheckforGameOver ()
		{
			if ( this.IsElection )
			{
				if ( this.policies[5].Value <= this.winThreshold )
				{
					//this.GetGameOverText ( 5 ).text = "";
					this.SetWin ();
					return;
				}
			}

			// Check for lose by happiness
			// TODO: determine which policy is too high
			if ( this.policies[ (int)PolicyType.Happiness ].Value >= this.loseDeviationThreshold && this.loseDeviationThreshold > 0 )
			{
				//this.GetGameOverText ( 5 ).text += " was too damn low!";
				this.SetLose ();
				return;
			}
			
			// check policy bounds
			for ( var i = 0; i < 4; i++ )
				if ( this.policies[i].Value <= 0 )
				{
					//this.GetGameOverText ( i ).text += " was too damn low!";
					this.SetLose ( i );
					return;
				}
		}

		/// <summary>
		/// Returns GameOver text object and sets parameter for blocking input.
		/// </summary>
		/// <param name="policyId"> The policyID responsible for the game over. </param>
		/// <returns> The <see cref="Text" />. </returns>
		private Text GetGameOverText ( int policyId )
		{
			this.blockInput = true;
			this.gameOverImage.gameObject.SetActive ( true );
			var gameOverText = this.gameOverImage.GetComponentInChildren <Text> ( true );
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
			this.gameOverImage.gameObject.SetActive ( true );
			this.gameOverImage.sprite = this.winScreen;

			// play audio
			AudioController.Instance.Play ( 1 );
		}

		private void SetLose ()
		{
			this.gameOverImage.gameObject.SetActive(true);
			if ( this.policies[this.maxDeviatiedPolicyID].Value > this.Average )
			{
				this.gameOverImage.sprite = this.loseScreensTooHigh[ this.maxDeviatiedPolicyID ];
			}
			else
			{
				this.SetLose(  this.maxDeviatiedPolicyID );
			}
		}

		/// <summary>
		/// Sets the lose screen to the correxponding too low policy
		/// </summary>
		/// <param name="id">Policy id</param>
		private void SetLose ( int id )
		{
			this.gameOverImage.gameObject.SetActive(true);
			try
			{
				this.gameOverImage.sprite = this.loseScreensTooLow[id];
			}
			catch ( IndexOutOfRangeException )
			{
				Debug.LogWarning ( "IndexOutOfRange! No loseToLowScreen set at id: " + id );
				this.gameOverImage.sprite = this.loseScreensTooLow[0] ?? this.winScreen;
			}

			// play audio
			AudioController.Instance.Play( 2 );
		}

		/// <summary>
		/// The average of all 4 policies.
		/// </summary>
		private int Average
		{
			get
			{
				var average = 0;
				for ( var i = 0; i < 4; i++ )
				{
					average += this.policies[i].Value;
				}
				return average / 4;
			}
		}

		/// <summary>
		/// Sets happiness value to max deviation
		/// </summary>
		private void SetHappiness ()
		{
			// set max deviation from average
			var maxDeviation = 0;
			for ( var j = 0; j < 4; j++ )
			{
				var tempDeviation = Math.Abs ( this.policies[j].Value - this.Average );
				if ( tempDeviation > maxDeviation )
				{
					maxDeviation = tempDeviation;
					this.maxDeviatiedPolicyID = j;
				}
			}
			
			// set happiness value
			this.policies[(int)PolicyType.Happiness].SetValue( maxDeviation, this.loseDeviationThreshold );
		}
	}
}
