using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Content.Scripts
{
    public static class ExtensionMethods
    {
        private static Random rng = new System.Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

		/// <summary>
		/// Gets a color with rgb values from newColor and alpha value from targetColor.
		/// </summary>
		/// <param name="targetColor">Color to get the alpha from</param>
		/// <param name="newColor">Color with new rgb values.</param>
		/// <returns>Color with rgb and alpha from targetColor</returns>
	    public static Color GetRGB( this Color targetColor, Color newColor )
	    {
		    newColor.a = targetColor.a;
		    return newColor;
	    }
    }
}
