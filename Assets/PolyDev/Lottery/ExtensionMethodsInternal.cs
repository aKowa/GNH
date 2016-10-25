using System.Collections.Generic;
using System.Linq;

namespace PolyDev.Lottery
{
	/// <summary>
	/// ExtensionMethods that are called internally by the LotteryManager
	/// </summary>
	public static class ExtensionMethodsInternal
	{
		/// <summary>
		/// Checks if the passed list is in the dictionary this method is called from.
		/// </summary>
		/// <param name="dictionary">The dictionary to check from.</param>
		/// <param name="collection">The collection to check if its in the dictionary.</param>
		/// <returns>Returns True, if the dictionary contains the passed list.</returns>
		public static bool Contains<T>(this IEnumerable<Lottery<T>> dictionary, IEnumerable<T> collection)
		{
			return dictionary.Any(t => collection.Equals(t.Collection));
		}

		/// <summary>
		/// Gets the Lottery associated with the passed collection in this dictionary.
		/// </summary>
		/// <param name="dictionary">Dictionary to get the Lottery from.</param>
		/// <param name="collection">collection to look for in dictionary (By Reference Type)</param>
		/// <returns>Returns the Lottery associated with the passed collection</returns>
		public static Lottery<T> Get<T>(this IEnumerable<Lottery<T>> dictionary, IEnumerable<T> collection)
		{
			return dictionary.FirstOrDefault(t => collection.Equals(t.Collection));
		}

		/// <summary>
		/// Removes all collections from the dictionary.
		/// </summary>
		public static void Clear<T> ( this IList<T> collection )
		{
			LotteryManager<T>.Clear ();
		}
	}
}
