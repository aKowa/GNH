using UnityEngine;
using PolyDev.Lottery;

public class LotteryRandomSprite : MonoBehaviour
{
	public SpriteRenderer rend;
	public Sprite[] sprites;

	public Sprite Sprite { get; private set; }

	public void SetSprite ()
	{
		Sprite = sprites.DrawNext ();
		if (rend != null)
			rend.sprite = Sprite;
	}

	public bool IsLastEntry ()
	{
		return sprites.IsEmpty ();
	}

	public void Remove()
	{
		sprites.Remove();
	}

	public void Clear()
	{
		sprites.Clear();
	}
}
