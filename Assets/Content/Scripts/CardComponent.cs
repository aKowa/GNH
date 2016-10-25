using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class CardComponent : MonoBehaviour
{
	public Policy[] policies = new Policy[2];

	public RectTransform display;
	public GameObject policyCard;

	public void OnEnable ()
	{
		//SetRandomPolicy();
	}

	/// <summary>
	/// Sets the card values at random.
	/// </summary>
	public void SetRandomPolicy ()
	{
		ClearChildren ();

		for(int i = 0; i < policies.Length; i++)
		{
			policies[i] = GetRandomPolicy ();
			var clone = Instantiate ( policyCard );
			clone.transform.SetParent( display );
			var trans = clone.GetComponent<RectTransform> ();
			trans.localScale = Vector3.one;

			var texts = clone.GetComponentsInChildren<Text> ();
			texts[0].text = policies[i].name.ToString();
			texts[1].text = policies[i].value.ToString();

			if ( policies[i].value < 0)
			{
				texts[0].color = Color.red;
				texts[1].color = Color.red;
			}
		}
	}
	
	/// <summary>
	/// Returns a random policy.
	/// </summary>
	/// <returns></returns>
	private Policy GetRandomPolicy ()
	{
		var e = Enum.GetValues ( typeof ( PolicyName ) );
		return new Policy
		{
			name = (PolicyName)e.GetValue ( UnityEngine.Random.Range ( 0, e.Length ) ),
			//value = UnityEngine.Random.Range ( -20, 20 )
		};
	}

	/// <summary>
	/// Deletes Child objects und PolicyCardRoot.
	/// </summary>
	private void ClearChildren ()
	{
		if (display.childCount > 0)
		{
			var children = display.transform.GetComponentsInChildren<Transform> ();
			for (int i = 1; i < children.Length; i++)
			{
				Destroy ( children[i].gameObject );
			}
		}
	}
}
