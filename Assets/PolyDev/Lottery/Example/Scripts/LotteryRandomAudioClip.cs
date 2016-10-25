using UnityEngine;
using PolyDev.Lottery;

public class LotteryRandomAudioClip : MonoBehaviour
{
	public AudioSource Source;

	public AudioClip[] Clips = new AudioClip[2];

	public AudioClip Clip { get; private set; }

	public void SetClip ()
	{
		Clip = Clips.DrawNext ();
		if (Source != null)
		{
			Source.clip = Clip;
			Source.Play();
		}
	}

	public bool IsLastEntry ()
	{
		return Clips.IsEmpty ();
	}

	public void Remove()
	{
		Clips.Remove();
	}

	public void Clear()
	{
		Clips.Clear();
	}
}
