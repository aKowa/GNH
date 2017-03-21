// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CardData.cs" company="Morra">
//   Bent Nürnberg
// </copyright>
// <summary>
//   Defines the CardData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Content.Scripts
{
    using System.Collections;

    using UnityEngine;

    /// <summary>
    /// The data to create a card
    /// </summary>
    public class CardData
    {
        /// <summary>
        /// Gets or sets the card id.
        /// </summary>
        public int CardId { get; set; }

        /// <summary>
        /// Gets or sets the card attributes.
        /// </summary>
        public CardAttributes CardAttributes { get; set; }
    }
}
