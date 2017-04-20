// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LetterController.cs" company="-">
//   Andr� Kowalewski & Bent N�rnberg
// </copyright>
// <summary>
//   Defines the LetterController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Content.Scripts
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The letter controller.
    /// </summary>
    public class LetterController : MonoBehaviour
    {
        /// <summary>
        /// The game manager.
        /// </summary>
        [SerializeField]
        private GameManager gameManager = null;

        /// <summary>
        /// The letter text.
        /// </summary>
        [SerializeField]
        private Text letterTextUIComponent = null;

		/// <summary>
		/// The adress text.
		/// </summary>
		[SerializeField]
		private Text textAdressee = null;

		/// <summary>
		/// Adressee name
		/// </summary>
		[SerializeField]
	    private string[] adresseeNames = null;

		/// <summary>
		/// The deviation to help.
		/// </summary>
		[SerializeField]
        private int deviationToHelp = 20;

        /// <summary>
        /// The max letters in hand.
        /// </summary>
        [SerializeField]
        private int maxLettersInHand = 20;

        /// <summary>
        /// The max angle.
        /// </summary>
        [Tooltip("The maximum angle in degrees the letter rotates, when dragged.")]
        [SerializeField]
        private float maxAngle = 15f;

        /// <summary>
        /// The threshold angle.
        /// </summary>
        [Tooltip("Angle in degrees when a letter is marked as chosen.")]
        [SerializeField]
        private float thresholdAngle = 10f;

        /// <summary>
        /// The aniamtions back rotation speed.
        /// </summary>
        [Tooltip("The speed used for rotating the letter back, when it was not choosen.")]
        [SerializeField]
        private float animationRevertRotationSpeed = 1f;

        /// <summary>
        /// How fast the letter moves away
        /// </summary>
        [Tooltip("The speed used for moving the letter away after choosing.")]
        [SerializeField]
        private float animationMoveSpeed = 1f;
		
        /// <summary>
        /// How far the letter should move away
        /// </summary>
        [Tooltip("The target distance to move away after chossing")]
        [SerializeField]
        private float animationMoveDistance = 900f;
        
        /// <summary>
        /// Degrees to start with when next letter rotate animation is being played.
        /// </summary>
        [Tooltip("Degrees to start with when next letter rotate animation is being played.")]
        [SerializeField]
        private float animationNextLetterDegree = 720;

        /// <summary>
        /// The speed at which the next letter rotation animation is being played.
        /// </summary>
        [SerializeField]
        private float animationNextLetterSpeed = 1;
		
		/// <summary>
		/// This letters inital position.
		/// </summary>
		private Vector3 initPosition;

        /// <summary>
        /// The letter hand.
        /// </summary>
        private List<LetterData> letterHand;

        /// <summary>
        /// The letter lists.
        /// </summary>
        private Dictionary<string, List<LetterData>> letterLists;

        /// <summary>
        /// The letters by id.
        /// </summary>
        private Dictionary<int, LetterData> lettersById;

        /// <summary>
        /// The letter stacks.
        /// </summary>
        private Dictionary<string, Stack<LetterData>> letterStacks;

        /// <summary>
        /// The current letter.
        /// </summary>
        private LetterAttributes currentLetter;

        /// <summary>
        /// The last category.
        /// </summary>
        private int lastCategory;

        /// <summary>
        /// The policy values l.
        /// </summary>
        private int[] policyValuesL = new int[5];

        /// <summary>
        /// The policy values r.
        /// </summary>
        private int[] policyValuesR = new int[5];

        /// <summary>
        /// The the four letter categories.
        /// </summary>
        private string[] theFourLetterCategories = { "Culture", "Economy", "Environment", "Security" };

		/// <summary>
		/// This letters image, used as raycasting target
		/// </summary>
        private Image image;

	    public Image Image
	    {
		    get
		    {
			    if ( this.image != null )
			    {
				    return this.image;
			    }
			    return this.image = this.GetComponent <Image> ();
		    }
	    }

	    /// <summary>
		/// Flag if angular threshold of letter was crossed
		/// </summary>
		private bool hasCrossedThreshold = false;


		[SerializeField]
		private Sprite[] letterSprite = null;

		[SerializeField]
		private Sprite reelectionLetterSprite = null;

		[SerializeField]
		private int electionMoneyBonus = 30;

		/// <summary>
		/// Gets the chosen policy.
		/// </summary>
		private int[] ChosenPolicy
        {
            get
            {
                if (this.EulerZ > 0)
                {
                    return this.policyValuesL;
                }

                if (this.EulerZ < 0)
                {
                    return this.policyValuesR;
                }

                Debug.LogWarning("Do not call ChosenPolicy, when EulerZ equals zero!");
                return new int[5];
            }
        }

        /// <summary>
        /// Gets the euler z.
        /// </summary>
        private float EulerZ
        {
            get
            {
                var eulerZ = this.transform.rotation.eulerAngles.z;
                if (eulerZ > 180)
                {
                    eulerZ -= 360f;
                }

                return eulerZ;
            }
        }

        /// <summary>
        /// Gets or sets the last category.
        /// </summary>
        private int LastCategory
        {
            get
            {
                return this.lastCategory;
            }

            set
            {
                this.lastCategory = value % 4;
            }
        }

        /// <summary>
        /// Called when drag began.
        /// </summary>
        public void OnBeginDrag()
        {
            this.StopAllCoroutines();
        }

        /// <summary>
        /// Rotates the letter towards the input position when dragged.
        /// </summary>
        public void OnDrag()
        {
            // early out
            if (this.gameManager.blockInput)
            {
                return;
            }

            // handle rotation
            var targetDirection = (Input.mousePosition - this.transform.position).normalized;
            var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle - 90, -this.maxAngle, this.maxAngle);
            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			// handle threshold angle logic (left positive, right negative)
	   
		    if ( Mathf.Abs ( this.EulerZ ) > this.thresholdAngle && !this.hasCrossedThreshold )
		    {
			    this.hasCrossedThreshold = true;
			    this.gameManager.PreviewResults ( this.ChosenPolicy );
			    this.textAdressee.text = "Dear " + this.adresseeNames[1] + ',';
			    this.ShowLetterSwipeText ();
				AudioController.Instance.Play ( 4 );
		    }
		    else if (Mathf.Abs(this.EulerZ) <= this.thresholdAngle && this.hasCrossedThreshold)
			{
				this.hasCrossedThreshold = false;
				this.gameManager.RevertPreview ();
			    this.textAdressee.text = "Dear " + this.adresseeNames[0] + ',';
			    this.ShowLetterText ();
		    }
        }

        /// <summary>
        /// Called when drag ended. Starts back rotation or ends this round depending on thresholdAngle.
        /// </summary>
        public void OnEndDrag()
        {
            if (Mathf.Abs(this.EulerZ) < this.thresholdAngle)
            {
                this.StartCoroutine(this.RotateBack(this.EulerZ));
            }
            else
            {
                this.gameManager.ApplyResults(this.ChosenPolicy);

                // if swipe was left or else if swipe was right
                if (this.EulerZ > this.thresholdAngle)
                {
                    this.CheckForAndInsertFollowUpLetter(this.currentLetter.FollowUpIdL, this.currentLetter.FollowUpStepL);
                }
                else if (this.EulerZ < -this.thresholdAngle)
                {
                    this.CheckForAndInsertFollowUpLetter(this.currentLetter.FollowUpIdR, this.currentLetter.FollowUpStepR);
                }
                this.StartCoroutine( this.MoveAway() );
            }
        }

        /// <summary>
        /// Callback from animation system, when NextLetterAnimation is over.
        /// </summary>
        public void OnAppliedLetterAnimationOver()
        {
            this.StopAllCoroutines();
			this.GetNextLetter();
        }

        /// <summary>
        /// The add letter list to currentLetter stack.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        private void AddLetterListToLetterStack(string category)
        {
            foreach (var letter in this.letterLists[category])
            {
                this.AddToLetterStacks(letter);
            }
        }

        /// <summary>
        /// The add to letter lists.
        /// </summary>
        /// <param name="letter">
        /// The letter.
        /// </param>
        private void AddToLetterLists(LetterData letter)
        {
            var category = letter.LetterAttributes.Category;
            var id = letter.LetterId;

            // if category was not added to dictionary
            if (!this.letterLists.ContainsKey(category))
            {
                // create and add list for this category
                this.letterLists.Add(category, new List<LetterData>());
            }

            // add to list for this category
            this.letterLists[category].Add(letter);
            this.lettersById.Add(id, letter);
        }

        /// <summary>
        /// The add to letter stacks.
        /// </summary>
        /// <param name="letter">
        /// The letter.
        /// </param>
        private void AddToLetterStacks(LetterData letter)
        {
            var category = letter.LetterAttributes.Category;

            // if category was not added to dictionary
            if (!this.letterStacks.ContainsKey(category))
            {
                // create and add stack for this category
                this.letterStacks.Add(category, new Stack<LetterData>());
            }

            // add to stack for this category
            this.letterStacks[category].Push(letter);
        }

        /// <summary>
        /// The check for and insert follow up letter.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="step">
        /// The step.
        /// </param>
        private void CheckForAndInsertFollowUpLetter(int id, int step)
        {
            if (id > 0)
            {
                if (step < 0)
                {
                    step = (int)Random.Range(4, 12);
                }
                this.letterHand.Insert(step, this.lettersById[id]);
                Debug.LogFormat("Follow Up Letter with ID {0} inserted in {1} letters.", id, step + 1);
            }
            else if (id < 0)
            {
                Debug.LogWarningFormat("Follow Up Letter ID was out of range (read: negative). ID was {0}", id);
            }
        }

        /// <summary>
        /// The deviated letter hand fill.
        /// </summary>
        /// <param name="mostDeviatedValue">
        /// The most deviated value.
        /// </param>
        private void DeviatedLetterHandFill(PolicyType mostDeviatedValue)
        {
            var letter = this.GetLetterFromStack(mostDeviatedValue.ToString());
            if (letter == null)
            {
                Debug.LogWarningFormat( "DeviatedLetterHandFill got null as letter, aborting. PolicyType was {0}.", mostDeviatedValue.ToString());
                return;
            }

            this.letterHand.Insert(0, letter);
            this.NormalLetterHandFill();
        }

        /// <summary>
        /// The fill letter hand.
        /// </summary>
        private void FillLetterHand()
        {
            if (this.letterHand == null)
            {
                this.letterHand = new List<LetterData>();
            }

            if (this.letterHand.Count >= this.maxLettersInHand)
            {
                return;
            }

            var deviation = 0;
            var mostDeviatedValue = this.GetFirstMostDeviatedValue(ref deviation);

            if (deviation < this.deviationToHelp)
            {
                this.NormalLetterHandFill();
            }
            else
            {
                this.DeviatedLetterHandFill(mostDeviatedValue);
            }
        }

        /// <summary>
        /// The get letter from stack.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        /// <returns>
        /// The <see cref="LetterData"/>.
        /// </returns>
        private LetterData GetLetterFromStack(string category)
        {
            if (this.letterStacks.ContainsKey(category))
            {
                if (this.letterStacks[category].Count != 0)
                {
                    return this.letterStacks[category].Pop();
                }

                this.PrepareLetterList(category);
                this.AddLetterListToLetterStack(category);
                return this.letterStacks[category].Pop();
            }

            Debug.LogWarning(
                "You tried to get a letter from a category, where no currentLetter stack is present (not even an empty one, so that might be a wrong category or your currentLetter data is corrupt/incorrect).");
            return null;
        }

        /// <summary>
        /// The get first most deviated value.
        /// </summary>
        /// <param name="maxDeviation">
        /// The max deviation.
        /// </param>
        /// <returns>
        /// The <see cref="PolicyType"/>.
        /// </returns>
        private PolicyType GetFirstMostDeviatedValue(ref int maxDeviation)
        {
            var happiness = this.gameManager.GetPolicyValue(PolicyType.Happiness);

            var deviations = new int[4];
            for (var i = 0; i < deviations.Length; i++)
            {
                deviations[i] = Mathf.Abs(happiness - this.gameManager.GetPolicyValue((PolicyType)i));
            }

            var index = -1;
            for (var i = 0; i < deviations.Length; i++)
            {
                var current = deviations[i];
                if (current > maxDeviation)
                {
                    maxDeviation = current;
                    index = i;
                }
            }

            if (index != -1)
            {
                return (PolicyType)index;
            }
            return PolicyType.Culture;
        }

        /// <summary>
        /// Gets next letter.
        /// </summary>
        private void GetNextLetter()
        {
	        if ( this.gameManager.IsElection )
	        {
		       this.SetElectionLetter ();
	        }
	        else
	        {
				this.GetNextLetterFromList ();
			}
        }

		/// <summary>
		/// Gets letter values for election letter
		/// </summary>
	    private void SetElectionLetter ()
	    {
		    for ( int i = 0; i < this.policyValuesL.Length; i++ )
		    {
			    this.policyValuesL[i] = 0;
			    this.policyValuesR[i] = 0;
		    }
		    this.policyValuesL[(int) PolicyType.Treasury] = this.electionMoneyBonus;
			this.policyValuesR[(int)PolicyType.Treasury] = this.electionMoneyBonus;
			
			this.Image.sprite = this.reelectionLetterSprite;
			this.ShowLetterText ();
		}

		/// <summary>
		/// Gets the next letter from list
		/// </summary>
		private void GetNextLetterFromList ()
	    {
			if (this.letterHand == null)
			{
				Debug.LogWarning("The letterHand list is null, aborting GetNextLetter()");
				if (Debug.isDebugBuild)
				{
					this.SetRandomPolicyValues();
					AConsoleController.Instance.AddToConsole("The letterHand list is null, aborting GetNextLetter()");
				}
				return;
			}

			if (this.letterHand.Count == 0)
			{
				Debug.LogWarning("The letterHand list is empty, aborting GetNextLetter()");
				if (Debug.isDebugBuild)
				{
					this.SetRandomPolicyValues();
					AConsoleController.Instance.AddToConsole("The letterHand list is empty, aborting GetNextLetter()");
				}

				return;
			}

			this.currentLetter = this.letterHand[0].LetterAttributes;

			this.policyValuesL[0] = this.currentLetter.CultureL;
			this.policyValuesL[1] = this.currentLetter.EconomyL;
			this.policyValuesL[2] = this.currentLetter.EnvironmentL;
			this.policyValuesL[3] = this.currentLetter.SecurityL;
			this.policyValuesL[4] = this.currentLetter.TreasuryL;

			this.policyValuesR[0] = this.currentLetter.CultureR;
			this.policyValuesR[1] = this.currentLetter.EconomyR;
			this.policyValuesR[2] = this.currentLetter.EnvironmentR;
			this.policyValuesR[3] = this.currentLetter.SecurityR;
			this.policyValuesR[4] = this.currentLetter.TreasuryR;

			this.ShowLetterText();

			this.letterHand.RemoveAt(0);
			this.FillLetterHand();
		}

        /// <summary>
        /// The normal letter hand fill.
        /// </summary>
        private void NormalLetterHandFill()
        {
            for (var i = 0; i < this.maxLettersInHand - this.letterHand.Count; i++)
            {
                var category = (PolicyType)this.LastCategory++;
                var letter = this.GetLetterFromStack(category.ToString());

                if (letter != null)
                {
                    this.letterHand.Add(letter);
                }
            }
        }

        /// <summary>
        /// The prepare letter list.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        private void PrepareLetterList(string category)
        {
            // if the category is one of the big four
            if (this.theFourLetterCategories.Contains(category))
            {
                // shuffle letters
                this.letterLists[category].Shuffle();
            }
            else
            {
                // sort list by descending id, because they are later pushed to a stack in order. the big ones need to go down first
                this.letterLists[category] = new List<LetterData>(this.letterLists[category].OrderByDescending(card => card.LetterId));
            }
        }

        /// <summary>
        /// Rotates the letter smoothly back, when drag ended.
        /// </summary>
        /// <param name="currentRotation">
        /// The current Rotation.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        private IEnumerator RotateBack(float currentRotation)
        {
            float t = 0;
            while (t < 1f)
            {
                var angle = Mathf.Lerp(currentRotation, 0.0f, t);
                this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
                t += this.animationRevertRotationSpeed * Time.deltaTime;
                yield return null;
            }
            this.transform.rotation = Quaternion.identity;
        }

        /// <summary>
        /// Moves the letter away into the chosen direction
        /// </summary>
        private IEnumerator MoveAway()
		{
			AudioController.Instance.Play ( 3 );
			this.Image.raycastTarget = false;
            float t = 0;
            while ( t <= 1 )
            {
                if (this.EulerZ > 0)
                {
                    this.transform.position += Vector3.left * (this.animationMoveDistance * t);
                }
                else
                {
                    this.transform.position += Vector3.right * (this.animationMoveDistance * t);
                }

                t += this.animationMoveSpeed * Time.deltaTime;

                yield return null;
            }

            this.GetNextLetter();
            this.StartCoroutine( this.NextLetterAnimation() );
        }

		/// <summary>
		/// Plays next letter animation
		/// </summary>
        private IEnumerator NextLetterAnimation()
        {
            this.transform.position = this.initPosition;
            this.transform.rotation = Quaternion.identity;

            var targetTransform = this.transform.parent;

            float t = 0;
            while (t <= 1)
            {
                targetTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
                var targetZ = Mathf.Lerp(this.animationNextLetterDegree, 0, t);
                targetTransform.rotation = Quaternion.Euler(0,0,targetZ);
                t += this.animationNextLetterSpeed * Time.deltaTime;
             
                yield return null;
            }

            targetTransform.rotation = Quaternion.identity;
            this.Image.raycastTarget = true;
            this.StopAllCoroutines();
        }

        /// <summary>
        /// Sets policy values to random integer. Can be used as a default, but removed in real builds.
        /// </summary>
        private void SetRandomPolicyValues()
        {
            for (int i = 0; i < 5; i++)
            {
                this.policyValuesL[i] = (int)Random.Range(-10, 10);
                this.policyValuesR[i] = (int)Random.Range(-10, 10);
            }
        }

        /// <summary>
        /// The setup letter lists.
        /// </summary>
        private void SetupLetterLists()
        {
            // init dictionaries
            this.letterLists = new Dictionary<string, List<LetterData>>();
            this.lettersById = new Dictionary<int, LetterData>();

            var list = LetterDataLoader.GetLetterDataList();

            foreach (var letter in list)
            {
                this.AddToLetterLists(letter);
            }
        }

        /// <summary>
        /// The setup letter stacks.
        /// </summary>
        private void SetupLetterStacks()
        {
            // init dictionary
            this.letterStacks = new Dictionary<string, Stack<LetterData>>();

            foreach (var key in this.letterLists.Keys.ToList())
            {
                this.PrepareLetterList(key);
                this.AddLetterListToLetterStack(key);
            }
        }

        /// <summary>
        /// The show letter swipe text.
        /// </summary>
        private void ShowLetterSwipeText()
        {
	        if ( this.gameManager.IsElection )
	        {
				this.textAdressee.text = "";
				this.letterTextUIComponent.text = "";
				return;
	        }

			// left positive, right negative
			if (this.EulerZ > this.thresholdAngle)
            {
                this.letterTextUIComponent.text = this.currentLetter.TextL;
            }
            else if (this.EulerZ < -this.thresholdAngle)
            {
                this.letterTextUIComponent.text = this.currentLetter.TextR;
            }
        }

        /// <summary>
        /// The show letter text.
        /// </summary>
        private void ShowLetterText()
		{
			this.textAdressee.text = "Dear " + this.adresseeNames[0] + ',';
			if ( this.gameManager.IsElection )
			{
				this.textAdressee.text = "";
				this.letterTextUIComponent.text = "";
		        return;
	        }
			this.letterTextUIComponent.text = this.currentLetter.Text;
			var id = (int)this.currentLetter.Character;
			if (id >= 0 && id < 4)
			{
				this.Image.sprite = this.letterSprite[id];
			}
			else
			{
				Debug.LogWarning ( "Character on Letter not set! Please provide a valid character!" );
			}
		}

		/// <summary>
		/// Sets letter values on start.
		/// </summary>
		private void Start()
        {
			this.initPosition = this.transform.position;
			this.SetupLetterLists();
            this.SetupLetterStacks();
            this.FillLetterHand();
            this.GetNextLetter();
        }
    }
}
