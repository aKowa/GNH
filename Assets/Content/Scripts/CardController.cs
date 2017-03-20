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

    public int minMaxPolicyValue = 10;

    public int[] policyValuesL = new int[5];

    public int[] policyValuesR = new int[5];

    public float thresholdAngle = 10f;

    private Dictionary<string, Stack<CardData>> cardStacks;

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
            this.gameManager.ApplyResults(this.ChosenPolicy);
            this.SetRandomValues();
            this.transform.rotation = Quaternion.identity; // TODO: play next card animation
        }
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

    /// <summary>
    /// Sets card values at random.
    /// </summary>
    // TODO: Replace by getting next card from stack
    private void SetRandomValues()
    {
        Debug.LogWarning("Getting new Card not implemented! Only sets new random values on the same card.");
        for (int i = 0; i < this.policyValuesL.Length; i++)
        {
            this.policyValuesL[i] = Random.Range(-this.minMaxPolicyValue, this.minMaxPolicyValue);
            this.policyValuesR[i] = Random.Range(-this.minMaxPolicyValue, this.minMaxPolicyValue);
        }
    }

    /// <summary>
    /// Sets card values on start.
    /// </summary>
    private void Start()
    {
        this.StartCoroutine(this.SetupCardStacks());

        this.SetRandomValues();
    }

    private IEnumerator SetupCardStacks()
    {
        // init dictionary and load card data
        this.cardStacks = new Dictionary<string, Stack<CardData>>();
        var list = CardDataLoader.GetCardDataList();
        yield return new WaitForEndOfFrame(); // wait

        // sort list by descending id, because they are later pushed to a stack in order. the big ones need to go down first
        list = new List<CardData>(list.OrderByDescending(card => card.CardId));
        yield return new WaitForEndOfFrame(); // wait

        // add all cards to the stacks
        foreach (var card in list)
        {
            this.AddToCardStacks(card);
            Debug.Log(card.CardId);
        }

        yield return null;
    }

    private void AddToCardStacks(CardData card)
    {
        var category = card.CardAttributes.Category;
        
        // if a card with this category was added before
        if (this.cardStacks.ContainsKey(category))
        {
            // push to stack for this category
            this.cardStacks[category].Push(card);
        }
        else
        {
            // create stack for this category
            this.cardStacks.Add(category, new Stack<CardData>());

            // and push to stack
            this.cardStacks[category].Push(card);
        }
    }
}
