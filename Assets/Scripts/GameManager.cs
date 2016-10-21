using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public CardComponent card;

	public void Update ()
	{
		for (int i = 0; i <= 1; i++)
		{
			if (Input.GetMouseButtonDown ( i ))
			{
				var cardPolicy = card.policies[i];
				foreach (var policyComponent in this.GetComponentsInChildren<PolicyUIComponent> ())
				{
					if ( policyComponent.policy.name == cardPolicy.name)
					{
						policyComponent.AddPolicy ( cardPolicy );
						break;
					}
				}

				card.SetRandomPolicy ();
			}
		}
	}
}
