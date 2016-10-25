using UnityEngine;
using UnityEditor;

[CustomEditor(typeof( LotteryRandomAudioClip ) )]
public class LotteryRandomAudioClipEditor : Editor
{
	private bool accessed;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		var randomInt = target as LotteryRandomAudioClip;

		if (randomInt == null)
		{
			Debug.LogError( "LotteryRandomAudioClip not assigned!" );
			return;
		}

		if (GUILayout.Button("Draw Next"))
		{
			randomInt.SetClip();
			accessed = true;
		}

		if (GUILayout.Button("Remove"))
		{
			randomInt.Remove();
			accessed = false;
		}
		
		if (GUILayout.Button("Clear"))
		{
			randomInt.Clear();
			accessed = false;
		}

		var label = "Picked: ";
		if (!accessed)
			label += "none";
		else if (randomInt.Clip != null)
			label += randomInt.Clip.name;

		if (randomInt.IsLastEntry())
			label += " (list is empty)";

		GUILayout.Label(label);
	}
}
