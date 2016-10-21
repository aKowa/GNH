using UnityEngine;
using UnityEngine.UI;

public class PolicyUIComponent : MonoBehaviour
{
	public Policy policy = new Policy { value = 50 };

	private Text[] texts;

	public void Start ()
	{
		texts = this.GetComponentsInChildren<Text> ();
		texts[0].text = policy.name.ToString ();
	}

	public void AddPolicy ( Policy pol )
	{
		if (pol.name == policy.name)
		{
			policy.value += pol.value;
			texts[1].text = policy.value.ToString ();
		}
	}
}
