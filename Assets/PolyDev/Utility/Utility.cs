using System;
using UnityEngine;
using System.Collections;

namespace PolyDev
{
	public static class Utility
	{
		/// <summary>
		/// Returns a string from end to front, ending with specified exitChar. IgnoreChar will be ignored.
		/// </summary>
		public static string ClearString( string s, char exitChar, char ignoreChar )
		{
			var targetString = "";
			var stringBinder = new string[2];
			var charArray = s.ToCharArray();
			for ( int i = charArray.Length; i > 0; i-- )
			{
				if ( charArray[i-1] == exitChar )
				{
					break;
				}
				if ( charArray[i-1] == ignoreChar )
				{
					continue;
				}
				stringBinder[0] = charArray[i - 1].ToString (); 
				stringBinder[1] = targetString;
				targetString = string.Join( "", stringBinder );
			}
			return targetString;
		}

		/// <summary>
		/// Returns a string array, where each element is Cleared from end to front, ending with specified exitChar. IgnoreChar will be ignored.
		/// </summary>
		public static string[] ClearString ( string[] s, char exitChar, char ignoreChar )
		{
			var targetArray = s;
			for ( int i = 0; i < s.Length; i++ )
			{
				targetArray[i] = ClearString( s[i], exitChar, ignoreChar );
			}
			return targetArray;
		}
	}
}
