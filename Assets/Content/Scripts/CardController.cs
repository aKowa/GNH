using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Content.Scripts;

using UnityEngine;

public class CardController : MonoBehaviour
{
    public float backRotationSpeed = 1f;

    public GameManager gameManager;

    public float maxAngle = 15f;

    public int maxCardsInHand = 20;

    public int minMaxPolicyValue = 10;

    public int[] policyValuesL = new int[5];

    public int[] policyValuesR = new int[5];

    public string[] theFourCardCategories = new string[4];

    public float thresholdAngle = 10f;

    private List<CardData> cardHand;

    private Dictionary<string, Stack<CardData>> cardStacks;

    private Dictionary<string, List<CardData>> cardLists; 

    private int lastCategory;

    private int[] ChosenPolicy
    {
        get
        {
            if (this.EulerZ > 0)
            {
                return this.policyValuesL;
            }
            else if (this.EulerZ < 0)
            {
                return this.policyValuesR;
            }

            Debug.LogWarning("Do not call ChosenPolicy, when EulerZ equals zero!");
            return new int[5];
        }
    }
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
            this.gameManager.ApplyResults(this.ChosenPolicy); // TODO: get follow up cards when swipe was right ("yes") with if (this.EulerZ < 0)
            this.GetNextCard();
            this.transform.rotation = Quaternion.identity; // TODO: play next card animation
        }
    }

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

    private void FillCardHand()
    {
        if (this.cardHand == null)
        {
            this.cardHand = new List<CardData>();
        }

        var cardsInHand = this.cardHand.Count;

        if (cardsInHand >= this.maxCardsInHand)
        {
            return;
        }

        for (var i = 0; i < this.maxCardsInHand - cardsInHand; i++)
        {
            var card = this.GetCardFromStack(this.theFourCardCategories[this.lastCategory++]);

            if (card != null)
            {
                this.cardHand.Add(card);
            }
        }
    }

    private CardData GetCardFromStack(string category)
    {
        if (this.cardStacks.ContainsKey(category))
        {
            var card = this.cardStacks[category].Pop();
            if (card != null)
            {
                return card;
            }

            // TODO: implement refilling stack with cards from list? big four new shuffle?
            Debug.LogWarning("You tried to get a card from a category, where the card stack is empty.");
            return null;
        }

        Debug.LogWarning(
            "You tried to get a card from a category, where no card stack is present (not even an empty one, so that might be a wrong category).");
        return null;
    }

    private void GetNextCard()
    {
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

    private void SetupCardStacks()
    {
        // init dictionary
        this.cardStacks = new Dictionary<string, Stack<CardData>>();

        foreach (var key in this.cardLists.Keys.ToList())
        {
            // if the category is one of the big four
            if (this.theFourCardCategories.Contains(key))
            {
                // shuffle cards
                this.cardLists[key].Shuffle();
            }
            else
            {
                // sort list by descending id, because they are later pushed to a stack in order. the big ones need to go down first
                this.cardLists[key] = new List<CardData>(this.cardLists[key].OrderByDescending(card => card.CardId));
            }

            foreach (var card in this.cardLists[key])
            {
                this.AddToCardStacks(card);
            }
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
