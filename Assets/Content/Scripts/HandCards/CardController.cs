using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CardController : MonoBehaviour, IPointerClickHandler
{
	public Policy[] policies;
	public Text[] nameTexts;
	public Text[] valueTexts;


	public void OnPointerClick ( PointerEventData eventData )
	{
		if (this.transform.parent == CardHelper.Hand.transform)
		{
			var amountFieldCards = CardHelper.Field.transform.GetComponentsInChildren<CardController> ().Length;
			if (amountFieldCards < 3)
			{
				this.transform.SetParent ( CardHelper.Field.transform );
			}
		}
		else if (this.transform.parent == CardHelper.Field.transform)
		{
			this.transform.SetParent ( CardHelper.Hand.transform );
		}
	}
}
