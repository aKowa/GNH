using UnityEngine;
using PolyDev.UI;

public class CardGameManager : MonoBehaviour
{
	public GameObject hand;
	public GameObject field;
	public GameObject card;

	public BindFloat[] stats;

	public void Awake ()
	{
		CardHelper.Hand = hand;
		CardHelper.Field = field;
	}

	public void Start ()
	{
		DrawCards ();
		for (int i = 0; i < stats.Length; i++)
		{
			stats[i].valueUnbound = 50;
		}
	}

	public void OnRoundEnd ()
	{
		ApplyStats ();
		DrawCards ();
	}

	public void DrawCards ()
	{
		var missingCardAmount = 5 - hand.GetComponentsInChildren<CardComponent> ().Length;

		for (int i = 0; i < missingCardAmount; i++)
		{
			var clone = Instantiate ( card );
			clone.transform.SetParent ( hand.transform );
		}
	}

	public void ApplyStats ()
	{
		var fieldCards = CardHelper.Field.GetComponentsInChildren<CardController> ();
		foreach ( var c in fieldCards )
		{
			foreach ( var policy in c.policies )
			{
				stats[(int)policy.name].Value += policy.value;
			}
			Destroy (c.gameObject);
		}
	}
}
