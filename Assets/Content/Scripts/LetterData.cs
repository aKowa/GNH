// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LetterData.cs" company="Morra">
//   Bent Nürnberg
// </copyright>
// <summary>
//   Defines the LetterData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Content.Scripts
{
    using System.Collections;

    using UnityEngine;

    /// <summary>
    /// The data to create a card
    /// </summary>
    public class LetterData
    {
        /// <summary>
        /// Gets or sets the card id.
        /// </summary>
        public int LetterId { get; set; }

        /// <summary>
        /// Gets or sets the card attributes.
        /// </summary>
        public LetterAttributes LetterAttributes { get; set; }
    }
}
