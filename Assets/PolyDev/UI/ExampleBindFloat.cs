using PolyDev.UI;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent ( typeof ( Text ) )]
public class ExampleBindFloat : MonoBehaviour
{
	public BindFloat myTime;

	public void Update ()
	{
		myTime.Value = (int)Time.time;
	}
}
