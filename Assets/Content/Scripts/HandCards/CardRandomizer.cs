using UnityEngine;
using System;

public class CardRandomizer : MonoBehaviour
{
	private CardController card;

	public void OnEnable ()
	{
		card = this.GetComponent<CardController> ();

		for ( int i = 0; i < card.policies.Length; i++)
		{
			var target = SetRandomPolicy ( card.policies[i] );
			card.policies[i] = target;
			card.nameTexts[i].text = target.name.ToString();
			card.valueTexts[i].text = target.value.ToString();

			if ( card.policies[i].value < 0)
			{
				card.nameTexts[i].color = Color.red;
				card.valueTexts[i].color = Color.red;
			}
		}
	}

	private Policy SetRandomPolicy ( Policy policy )
	{
		var e = Enum.GetValues ( typeof ( PolicyName ) );
		policy.name = (PolicyName)e.GetValue ( UnityEngine.Random.Range ( 0, e.Length ) );
		policy.value = UnityEngine.Random.Range ( -20, 20 );
		return policy;
	}
}
