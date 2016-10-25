using System.Collections.Generic;
using UnityEngine;

namespace PolyDev.Lottery
{
	/// <summary>
	/// Manages all lists that called the lottery functionality.
	/// </summary>
	public static class LotteryManager<T>
	{
		/// <param name="Dictionary">The Dictionary containing all collections, which called LotteryRandom methods.</param>
		private static readonly List<Lottery<T>> Dictionary = new List<Lottery<T>>();

		/// <summary>
		/// Method to be called to get the next random entry from passed collection. Also adds it to the Dictionary on its first call.
		/// </summary>
		/// <param name="collection">The collection to get the next random entry from. Reference Type from dictionary</param>
		/// <returns>Next entry from passed collection, that has not been returned yet.</returns>
		public static T DrawNext(IList<T> collection)
		{ 
			if (Dictionary.Contains(collection))
			{
				return Dictionary.Get(collection).DrawNext();
			}
			Dictionary.Add(new Lottery<T>(collection));
			Debug.Log("Added " + collection.GetType() + " to dictionary");
			return Dictionary.Get(collection).DrawNext();
		}

		/// <summary>
		/// Indicates, when all collections in the passed collection have been returned in DrawNext() once.
		/// </summary>
		/// <param name="collection">The collection to check if its empty.</param>
		/// <returns>True, if all element from the passed collection have been accessed once.</returns>
		public static bool IsEmpty(IList<T> collection)
		{
			var entry = Dictionary.Get(collection);
			return entry != null && entry.IsEmtpy();
		}

		/// <summary>
		/// Removes the passed list from the dictionary.
		/// </summary>
		/// <param name="collection">The list that should be removed.</param>
		public static void Remove(IList<T> collection)
		{
			Dictionary.Remove(Dictionary.Get(collection));
		}

		/// <summary>
		/// Removes all added collection from the dictionary.
		/// </summary>
		public static void Clear()
		{
			Dictionary.Clear();
		}
	}
}
