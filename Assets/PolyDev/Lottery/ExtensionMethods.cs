using System.Collections.Generic;
using UnityEngine;

namespace PolyDev.Lottery
{
	/// <summary>
	/// Enables LotteryManager functionality to be called directly on any list. 
	/// </summary>
	public static class ExtensionMethods
	{
		/// <summary>
		/// ExtensionMethod for calling DrawNext() function in LotteryManager. 
		/// </summary>
		/// <param name="collection">The iList from which an entry should be returned.</param>
		/// <returns>Returns next list entry that has not been returned yet.</returns>
		public static T DrawNext<T>(this IList<T> collection)
		{
			return LotteryManager<T>.DrawNext(collection);
		}

		// Returns true, if all elements of the passed IList<T> have been returned once
		public static bool IsEmpty<T>(this IList<T> collection)
		{
			return LotteryManager<T>.IsEmpty(collection);
		}

		// Disables remembering which elements of the passed IList<T> have been already returned
		// by removing from an internal tracking dictionary
		public static void Remove<T>(this IList<T> collection)
		{
			Debug.Log("Removed: " + collection.GetType());
			LotteryManager<T>.Remove(collection);
		}
	}
}
