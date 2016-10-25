using UnityEngine;
using PolyDev.Lottery;

public class LotteryRandomInt : MonoBehaviour
{
	public int[] ints = { 0, 1, 2, 3 };

	public int Number { get; private set; }

	public void SetNumber ()
	{
		Number = ints.DrawNext ();
	}

	public bool GetLastEntry ()
	{
		return ints.IsEmpty ();
	}

	public void Remove()
	{
		ints.Remove();
	}

	public void Clear()
	{
		ints.Clear();
	}
}
