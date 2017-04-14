// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CardController.cs" company="-">
//   Andr� Kowalewski & Bent N�rnberg
// </copyright>
// <summary>
//   Defines the CardController type.
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
    /// The card controller.
    /// </summary>
    public class CardController : MonoBehaviour
    {
        /// <summary>
        /// The game manager.
        /// </summary>
        [SerializeField]
        private GameManager gameManager = null;

        /// <summary>
        /// The card text.
        /// </summary>
        [SerializeField]
        private Text cardTextUIComponent = null;

		/// <summary>
		/// The adress text.
		/// </summary>
		[SerializeField]
		private Text textAdressee = null;

		/// <summary>
		/// Adressee name
		/// </summary>
		[SerializeField]
	    private string[] adresseeNames;

		/// <summary>
		/// The deviation to help.
		/// </summary>
		[SerializeField]
        private int deviationToHelp = 20;

        /// <summary>
        /// The max cards in hand.
        /// </summary>
        [SerializeField]
        private int maxCardsInHand = 20;

        /// <summary>
        /// The max angle.
        /// </summary>
        [Tooltip("The maximum angle in degrees the card rotates, when dragged.")]
        [SerializeField]
        private float maxAngle = 15f;

        /// <summary>
        /// The threshold angle.
        /// </summary>
        [Tooltip("Angle in degrees when a card is marked as chosen.")]
        [SerializeField]
        private float thresholdAngle = 10f;

        /// <summary>
        /// The aniamtions back rotation speed.
        /// </summary>
        [Tooltip("The speed used for rotating the card back, when it was not choosen.")]
        [SerializeField]
        private float animationRevertRotationSpeed = 1f;

        /// <summary>
        /// How fast the card moves away
        /// </summary>
        [Tooltip("The speed used for moving the card away after choosing.")]
        [SerializeField]
        private float animationMoveSpeed = 1f;
		
        /// <summary>
        /// How far the card should move away
        /// </summary>
        [Tooltip("The target distance to move away after chossing")]
        [SerializeField]
        private float animationMoveDistance = 900f;
        
        /// <summary>
        /// Degrees to start with when next card rotate animation is being played.
        /// </summary>
        [Tooltip("Degrees to start with when next card rotate animation is being played.")]
        [SerializeField]
        private float animationNextCardDegree = 720;

        /// <summary>
        /// The speed at which the next card rotation animation is being played.
        /// </summary>
        [SerializeField]
        private float aniamtionNextCardSpeed = 1;

		/// <summary>
		/// The swipe audio clip
		/// </summary>
		[SerializeField]
		private AudioClip swipeClip = null;

		/// <summary>
		/// The new card audio clip
		/// </summary>
		[SerializeField]
		private AudioClip newCardClip = null;
		
		/// <summary>
		/// This cards inital position.
		/// </summary>
		private Vector3 initPosition;

        /// <summary>
        /// The card hand.
        /// </summary>
        private List<CardData> cardHand;

        /// <summary>
        /// The card lists.
        /// </summary>
        private Dictionary<string, List<CardData>> cardLists;

        /// <summary>
        /// The cards by id.
        /// </summary>
        private Dictionary<int, CardData> cardsById;

        /// <summary>
        /// The card stacks.
        /// </summary>
        private Dictionary<string, Stack<CardData>> cardStacks;

        /// <summary>
        /// The current card.
        /// </summary>
        private CardAttributes currentCard;

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
        /// The the four card categories.
        /// </summary>
        private string[] theFourCardCategories = { "Culture", "Economy", "Environment", "Security" };

		/// <summary>
		/// This cards image, used as raycasting target
		/// </summary>
        private Image image;

		/// <summary>
		/// Flag if angular threshold of card was crossed
		/// </summary>
		private bool hasCrossedThreshold = false;
		
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
        /// Rotates the card towards the input position when dragged.
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
			    this.ShowCardSwipeText ();
				AudioController.Instance.Play ( 4 );
		    }
		    else if (Mathf.Abs(this.EulerZ) <= this.thresholdAngle && this.hasCrossedThreshold)
			{
				this.hasCrossedThreshold = false;
				this.gameManager.RevertPreview ();
			    this.textAdressee.text = "Dear " + this.adresseeNames[0] + ',';
			    this.ShowCardText ();
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
                    this.CheckForAndInsertFollowUpCard(this.currentCard.FollowUpIdL, this.currentCard.FollowUpStepL);
                }
                else if (this.EulerZ < -this.thresholdAngle)
                {
                    this.CheckForAndInsertFollowUpCard(this.currentCard.FollowUpIdR, this.currentCard.FollowUpStepR);
                }

                this.StartCoroutine(this.MoveAway());
            }
        }

        /// <summary>
        /// Callback from animation system, when NextCardAnimation is over.
        /// </summary>
        public void OnAppliedCardAnimationOver()
        {
            this.StopAllCoroutines();
			this.GetNextCard();
        }

        /// <summary>
        /// The add card list to currentCard stack.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        private void AddCardListToCardStack(string category)
        {
            foreach (var card in this.cardLists[category])
            {
                this.AddToCardStacks(card);
            }
        }

        /// <summary>
        /// The add to card lists.
        /// </summary>
        /// <param name="card">
        /// The card.
        /// </param>
        private void AddToCardLists(CardData card)
        {
            var category = card.CardAttributes.Category;
            var id = card.CardId;

            // if category was not added to dictionary
            if (!this.cardLists.ContainsKey(category))
            {
                // create and add list for this category
                this.cardLists.Add(category, new List<CardData>());
            }

            // add to list for this category
            this.cardLists[category].Add(card);
            this.cardsById.Add(id, card);
        }

        /// <summary>
        /// The add to card stacks.
        /// </summary>
        /// <param name="card">
        /// The card.
        /// </param>
        private void AddToCardStacks(CardData card)
        {
            var category = card.CardAttributes.Category;

            // if category was not added to dictionary
            if (!this.cardStacks.ContainsKey(category))
            {
                // create and add stack for this category
                this.cardStacks.Add(category, new Stack<CardData>());
            }

            // add to stack for this category
            this.cardStacks[category].Push(card);
        }

        /// <summary>
        /// The check for and insert follow up card.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="step">
        /// The step.
        /// </param>
        private void CheckForAndInsertFollowUpCard(int id, int step)
        {
            if (id > 0 && step >= 0)
            {
                this.cardHand.Insert(step, this.cardsById[id]);
                Debug.LogFormat("Follow Up Card with ID {0} inserted in {1} cards.", id, step + 1);
            }
        }

        /// <summary>
        /// The deviated card hand fill.
        /// </summary>
        /// <param name="mostDeviatedValue">
        /// The most deviated value.
        /// </param>
        private void DeviatedCardHandFill(PolicyType mostDeviatedValue)
        {
            var card = this.GetCardFromStack(mostDeviatedValue.ToString());
            if (card == null)
            {
                Debug.LogWarningFormat(
                    "DeviatedCardHandFill got null as card, aborting. PolicyType was {0}.", 
                    mostDeviatedValue.ToString());
                return;
            }

            this.cardHand.Insert(0, card);
            this.NormalCardHandFill();
        }

        /// <summary>
        /// The fill card hand.
        /// </summary>
        private void FillCardHand()
        {
            if (this.cardHand == null)
            {
                this.cardHand = new List<CardData>();
            }

            if (this.cardHand.Count >= this.maxCardsInHand)
            {
                return;
            }

            var deviation = 0;
            var mostDeviatedValue = this.GetFirstMostDeviatedValue(ref deviation);

            if (deviation < this.deviationToHelp)
            {
                this.NormalCardHandFill();
            }
            else
            {
                this.DeviatedCardHandFill(mostDeviatedValue);
            }
        }

        /// <summary>
        /// The get card from stack.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        /// <returns>
        /// The <see cref="CardData"/>.
        /// </returns>
        private CardData GetCardFromStack(string category)
        {
            if (this.cardStacks.ContainsKey(category))
            {
                if (this.cardStacks[category].Count != 0)
                {
                    return this.cardStacks[category].Pop();
                }

                this.PrepareCardList(category);
                this.AddCardListToCardStack(category);
                return this.cardStacks[category].Pop();
            }

            Debug.LogWarning(
                "You tried to get a card from a category, where no currentCard stack is present (not even an empty one, so that might be a wrong category or your currentCard data is corrupt/incorrect).");
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
        /// Gets next card.
        /// </summary>
        private void GetNextCard()
        {
            if (this.cardHand == null)
            {
                Debug.LogWarning("The cardHand list is null, aborting GetNextCard()");
                if (Debug.isDebugBuild)
                {
                    this.SetRandomPolicyValues();
                    AConsoleController.Instance.AddToConsole("The cardHand list is null, aborting GetNextCard()");
                }

                return;
            }
            
            if (this.cardHand.Count == 0)
            {
                Debug.LogWarning("The cardHand list is empty, aborting GetNextCard()");
                if (Debug.isDebugBuild)
                {
                    this.SetRandomPolicyValues();
                    AConsoleController.Instance.AddToConsole("The cardHand list is empty, aborting GetNextCard()");
                }

                return;
            }

            this.currentCard = this.cardHand[0].CardAttributes;

            this.policyValuesL[0] = this.currentCard.CultureL;
            this.policyValuesL[1] = this.currentCard.EconomyL;
            this.policyValuesL[2] = this.currentCard.EnvironmentL;
            this.policyValuesL[3] = this.currentCard.SecurityL;
            this.policyValuesL[4] = this.currentCard.TreasuryL;

            this.policyValuesR[0] = this.currentCard.CultureR;
            this.policyValuesR[1] = this.currentCard.EconomyR;
            this.policyValuesR[2] = this.currentCard.EnvironmentR;
            this.policyValuesR[3] = this.currentCard.SecurityR;
            this.policyValuesR[4] = this.currentCard.TreasuryR;

            this.ShowCardText();
			
			this.cardHand.RemoveAt(0);
            this.FillCardHand();
        }

        /// <summary>
        /// The normal card hand fill.
        /// </summary>
        private void NormalCardHandFill()
        {
            for (var i = 0; i < this.maxCardsInHand - this.cardHand.Count; i++)
            {
                var category = (PolicyType)this.LastCategory++;
                var card = this.GetCardFromStack(category.ToString());

                if (card != null)
                {
                    this.cardHand.Add(card);
                }
            }
        }

        /// <summary>
        /// The prepare card list.
        /// </summary>
        /// <param name="category">
        /// The category.
        /// </param>
        private void PrepareCardList(string category)
        {
            // if the category is one of the big four
            if (this.theFourCardCategories.Contains(category))
            {
                // shuffle cards
                this.cardLists[category].Shuffle();
            }
            else
            {
                // sort list by descending id, because they are later pushed to a stack in order. the big ones need to go down first
                this.cardLists[category] =
                    new List<CardData>(this.cardLists[category].OrderByDescending(card => card.CardId));
            }
        }

        /// <summary>
        /// Rotates the card smoothly back, when drag ended.
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
                t += animationRevertRotationSpeed * Time.deltaTime;
                yield return null;
            }

            this.transform.rotation = Quaternion.identity;
        }

        /// <summary>
        /// Moves the card away into the chosen direction
        /// </summary>
        private IEnumerator MoveAway()
		{
			AudioController.Instance.Play ( 3 );
			this.image.raycastTarget = false;
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

            this.GetNextCard();
            StartCoroutine(this.NextCardAnimation());
        }

        private IEnumerator NextCardAnimation()
        {
            this.transform.position = this.initPosition;
            this.transform.rotation = Quaternion.identity;

            var targetTransform = this.transform.parent;

            float t = 0;
            while (t <= 1)
            {
                targetTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
                var targetZ = Mathf.Lerp(this.animationNextCardDegree, 0, t);
                targetTransform.rotation = Quaternion.Euler(0,0,targetZ);
                t += this.aniamtionNextCardSpeed * Time.deltaTime;
             
                yield return null;
            }

            targetTransform.rotation = Quaternion.identity;
            this.image.raycastTarget = true;
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
        /// The setup card lists.
        /// </summary>
        private void SetupCardLists()
        {
            // init dictionaries
            this.cardLists = new Dictionary<string, List<CardData>>();
            this.cardsById = new Dictionary<int, CardData>();

            var list = CardDataLoader.GetCardDataList();

            foreach (var card in list)
            {
                this.AddToCardLists(card);
            }
        }

        /// <summary>
        /// The setup card stacks.
        /// </summary>
        private void SetupCardStacks()
        {
            // init dictionary
            this.cardStacks = new Dictionary<string, Stack<CardData>>();

            foreach (var key in this.cardLists.Keys.ToList())
            {
                this.PrepareCardList(key);
                this.AddCardListToCardStack(key);
            }
        }

        /// <summary>
        /// The show card swipe text.
        /// </summary>
        private void ShowCardSwipeText()
        {
            // left positive, right negative
            if (this.EulerZ > this.thresholdAngle)
            {
                this.cardTextUIComponent.text = this.currentCard.TextL;
            }
            else if (this.EulerZ < -this.thresholdAngle)
            {
                this.cardTextUIComponent.text = this.currentCard.TextR;
            }
        }

        /// <summary>
        /// The show card text.
        /// </summary>
        private void ShowCardText()
        {
            this.cardTextUIComponent.text = this.currentCard.Text;
			this.textAdressee.text = "Dear " + this.adresseeNames[0] + ',';
		}

		/// <summary>
		/// Sets card values on start.
		/// </summary>
		private void Start()
        {
            this.SetupCardLists();
            this.SetupCardStacks();
            this.FillCardHand();
            this.GetNextCard();
            this.initPosition = this.transform.position;
            this.image = this.GetComponent<Image>();
        }
    }
}
