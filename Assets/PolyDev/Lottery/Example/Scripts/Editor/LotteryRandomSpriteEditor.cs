using UnityEngine;
using UnityEditor;

[CustomEditor(typeof( LotteryRandomSprite ) )]
public class LotteryRandomSpriteEditor : Editor
{
	private bool accessed;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		var randomSprite = target as LotteryRandomSprite;

		if (randomSprite == null)
		{
			Debug.LogError( "LotteryRandomSprite not assigned!" );
			return;
		}

		if (GUILayout.Button("Draw Next"))
		{
			randomSprite.SetSprite();
			accessed = true;
		}

		if (GUILayout.Button("Remove"))
		{
			randomSprite.Remove();
			accessed = false;
		}
		
		if (GUILayout.Button("Clear"))
		{
			randomSprite.Clear();
			accessed = false;
		}

		var label = "Picked: ";
		if (!accessed)
			label += "none";
		else if (randomSprite.Sprite != null)
			label += randomSprite.Sprite.name;

		if (randomSprite.IsLastEntry())
			label += " (list is empty)";

		GUILayout.Label(label);
	}
}
