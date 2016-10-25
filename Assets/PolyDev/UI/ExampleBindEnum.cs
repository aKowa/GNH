using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using PolyDev.UI;

[RequireComponent ( typeof ( Text ) )]
public class ExampleBindEnum : MonoBehaviour
{
	public Bind<States, Text> myStates;

	public void Start ()
	{
		myStates = new Bind<States, Text> ( this.gameObject, "text" );
		StartCoroutine( Wait() );
	}

	private IEnumerator Wait ()
	{
		switch (myStates.Value)
		{
			case States.First:
				myStates.Value = States.Second;
				break;
			case States.Second:
				myStates.Value = States.Third;
				break;
			case States.Third:
				myStates.Value = States.First;
				break;
		}
		yield return new WaitForSeconds ( 1 );
		StartCoroutine ( Wait () );
	}
}


public enum States
{
	Third,
	First,
	Second
}
