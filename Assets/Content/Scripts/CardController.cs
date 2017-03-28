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

    /// <summary>
    /// The card controller.
    /// </summary>
    public class CardController : MonoBehaviour
    {
        /// <summary>
        /// The back rotation speed.
        /// </summary>
        [SerializeField]
        private float backRotationSpeed = 1f;

        /// <summary>
        /// The card hand.
        /// </summary>
        private List<CardData> cardHand;

        /// <summary>
        /// The card lists.
        /// </summary>
        private Dictionary<string, List<CardData>> cardLists;

        /// <summary>
        /// The card stacks.
        /// </summary>
        private Dictionary<string, Stack<CardData>> cardStacks;

        /// <summary>
        /// The deviation to help.
        /// </summary>
        [SerializeField]
        private int deviationToHelp = 20;

        /// <summary>
        /// The game manager.
        /// </summary>
        [SerializeField]
        private GameManager gameManager;

        /// <summary>
        /// The last category.
        /// </summary>
        private int lastCategory;

        /// <summary>
        /// The max angle.
        /// </summary>
        [SerializeField]
        private float maxAngle = 15f;

        /// <summary>
        /// The max cards in hand.
        /// </summary>
        [SerializeField]
        private int maxCardsInHand = 20;

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
        /// The threshold angle.
        /// </summary>
        [SerializeField]
        private float thresholdAngle = 10f;

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
            if (this.gameManager.BlockInput)
            {
                return;
            }

            // handle rotation
            var targetDirection = (Input.mousePosition - this.transform.position).normalized;
            var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle - 90, -this.maxAngle, this.maxAngle);
            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // handle threshold angle logic
            if (Mathf.Abs(this.EulerZ) > this.thresholdAngle)
            {
                this.gameManager.PreviewResults(this.ChosenPolicy);
            }
            else
            {
                this.gameManager.RevertPreview();
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

                // TODO: get follow up cards when swipe was right ("yes") with if (this.EulerZ < 0)
                this.GetNextCard();
                this.transform.rotation = Quaternion.identity; // TODO: play next card animation
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

            // if category was not added to dictionary
            if (!this.cardLists.ContainsKey(category))
            {
                // create and add list for this category
                this.cardLists.Add(category, new List<CardData>());
            }

            // add to list for this category
            this.cardLists[category].Add(card);
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
                Debug.LogWarningFormat("DeviatedCardHandFill got null as card, aborting. PolicyType was {0}.", mostDeviatedValue.ToString());
                return;
            }

            this.cardHand.Insert(0, card);
            this.NormalCardHandFill();
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

            Debug.LogWarningFormat("GetFirstMostDeviatedValue() returns index with {0}, returning PolicyType.Culture instead.", index);
            return PolicyType.Culture;
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
                "You tried to get a card from a category, where no card stack is present (not even an empty one, so that might be a wrong category or your card data is corrupt/incorrect).");
            return null;
        }

        /// <summary>
        /// Gets next card.
        /// </summary>
        /// TODO: integrate happiness parameter and set overwrite card text
        private void GetNextCard()
        {
            if (this.cardHand == null)
            {
                Debug.LogWarning("The cardHand list is null, aborting GetNextCard()");
                return;
            }

            if (this.cardHand.Count == 0)
            {
                Debug.LogWarning("The cardHand list is empty, aborting GetNextCard()");
                return;
            }

            var card = this.cardHand[0].CardAttributes;

            this.policyValuesL[0] = card.CultureL;
            this.policyValuesL[1] = card.EconomyL;
            this.policyValuesL[2] = card.EnvironmentL;
            this.policyValuesL[3] = card.SecurityL;
            this.policyValuesL[4] = card.TreasuryL;

            this.policyValuesR[0] = card.CultureR;
            this.policyValuesR[1] = card.EconomyR;
            this.policyValuesR[2] = card.EnvironmentR;
            this.policyValuesR[3] = card.SecurityR;
            this.policyValuesR[4] = card.TreasuryR;

            this.cardHand.RemoveAt(0);
            this.FillCardHand();
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
                t += this.backRotationSpeed * Time.deltaTime;
                yield return null;
            }

            this.transform.rotation = Quaternion.identity;
        }

        /// <summary>
        /// The setup card lists.
        /// </summary>
        private void SetupCardLists()
        {
            // init dictionary and load card data
            this.cardLists = new Dictionary<string, List<CardData>>();
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
        /// The add card list to card stack.
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
                this.cardLists[category] = new List<CardData>(this.cardLists[category].OrderByDescending(card => card.CardId));
            }
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
        }
    }
}
