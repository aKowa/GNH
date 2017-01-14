// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameManager.cs" company="-">
//   André Kowalewski & Bent Nürnberg
// </copyright>
// <summary>
//   Defines the GameManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Content.Scripts
{
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    /// The game manager.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private List<CardData> cardData; 

        /// <summary>
        /// The start.
        /// </summary>
        public void Start()
        {
            this.cardData = CardDataLoader.GetCardDataList();
        }
    }
}
