using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PolyDev.UI
{
	/// <summary>
	/// Draws a Bind class with any serializable Type and offers components and properties as Popups.
	/// </summary>
	[CustomPropertyDrawer ( typeof ( BindFloat ) )]
	[CustomPropertyDrawer ( typeof ( BindColor ) )]
	[CustomPropertyDrawer ( typeof ( BindInt ) )]
	public class BindDrawer : PropertyDrawer
	{
		private const float FieldHeight = 16;
		
		public override void OnGUI ( Rect position, SerializedProperty property, GUIContent label )
		{
			EditorGUI.BeginProperty ( position, label, property );
			
			var target = property.serializedObject.targetObject;

			EditorGUI.BeginChangeCheck ();

			// value
			var valueRect = new Rect ( position.x + 15, position.y + 5, position.width, FieldHeight );
			var serializedValue = property.FindPropertyRelative ( "valueUnbound" );
			EditorGUI.PropertyField( valueRect, serializedValue, new GUIContent( label.text + " (Unbound)") );

			// rect
			var objectRect = valueRect;
			objectRect.y += 5 + FieldHeight;
			objectRect.width = objectRect.width / 3 - 5;

			var componentRect = objectRect;
			componentRect.x += objectRect.width;

			var propertyRect = componentRect;
			propertyRect.x += componentRect.width;
			

			// game object
			var serializedGameObject = property.FindPropertyRelative( "targetGameObject" );
			EditorGUI.PropertyField ( objectRect, serializedGameObject, GUIContent.none );
			var gameObj = serializedGameObject.objectReferenceValue as GameObject;


			// component
			var serializedComponent = property.FindPropertyRelative( "targetComponent" );
			var serializedComponentID = property.FindPropertyRelative ( "componentID" );

			Component[] componentArray = new Component[0];
			string[] componentNames = { "None" };
			if (gameObj != null)
			{
				componentArray = gameObj.GetComponents<Component> ();
				componentNames = GetTypeNames( componentArray );
				componentNames = Utility.ClearString( componentNames, '.', ')' );
			}
			serializedComponentID.intValue = EditorGUI.Popup ( componentRect, serializedComponentID.intValue, componentNames );
			var component = serializedComponent.objectReferenceValue as Component;

			if ( serializedComponentID.intValue > componentArray.Length)
			{
				serializedComponentID.intValue = 0;
			}

			if ( componentArray.Length > 0)
			{
				component = componentArray[serializedComponentID.intValue];
			}

			// properties
			var serializedPropertyID = property.FindPropertyRelative ( "propertyID" );

			string[] propertyNames = { "None" };
			if ( component != null )
			{
				propertyNames = GetNames( component.GetType().GetProperties() );
				propertyNames = Utility.ClearString ( propertyNames, '.', ')' );
			}
			serializedPropertyID.intValue = EditorGUI.Popup( propertyRect, serializedPropertyID.intValue, propertyNames );
			
			if ( serializedPropertyID.intValue > propertyNames.Length)
			{
				serializedPropertyID.intValue = 0;
			}

			if ( GUI.changed )
			{
				EditorUtility.SetDirty( target );
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
		{
			return (FieldHeight + 5) * 2;
		}

		private string[] GetTypeNames<T>( IList<T> list )
		{
			var targetStringArray = new string[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				targetStringArray[i] = list[i].GetType ().ToString ();
			}
			return targetStringArray;
		}

		private string[] GetNames<T> ( IList<T> list )
		{
			var targetStringArray = new string[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				targetStringArray[i] = list[i].ToString ();
			}
			return targetStringArray;
		}
	}
}
