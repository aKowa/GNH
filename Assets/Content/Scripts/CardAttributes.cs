// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CardAttributes.cs" company="Morra">
//   Bent Nürnberg
// </copyright>
// <summary>
//   Defines the CardAttributes type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Content.Scripts
{
    using UnityEngine;

    /// <summary>
    /// The card attributes.
    /// </summary>
    public class CardAttributes
    {
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the character.
        /// </summary>
        public Character Character { get; set; }

        /// <summary>
        /// Gets or sets the culture r.
        /// </summary>
        public int CultureR { get; set; }

        /// <summary>
        /// Gets or sets the economy r.
        /// </summary>
        public int EconomyR { get; set; }

        /// <summary>
        /// Gets or sets the environment r.
        /// </summary>
        public int EnvironmentR { get; set; }

        /// <summary>
        /// Gets or sets the follow up id.
        /// </summary>
        public int FollowUpId { get; set; }

        /// <summary>
        /// Gets or sets the security r.
        /// </summary>
        public int SecurityR { get; set; }

        /// <summary>
        /// Gets or sets the treasury r.
        /// </summary>
        public int TreasuryR { get; set; }

        /// <summary>
        /// Gets or sets the culture l.
        /// </summary>
        public int CultureL { get; set; }

        /// <summary>
        /// Gets or sets the economy l.
        /// </summary>
        public int EconomyL { get; set; }

        /// <summary>
        /// Gets or sets the environment l.
        /// </summary>
        public int EnvironmentL { get; set; }

        /// <summary>
        /// Gets or sets the security l.
        /// </summary>
        public int SecurityL { get; set; }

        /// <summary>
        /// Gets or sets the treasury l.
        /// </summary>
        public int TreasuryL { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the text r.
        /// </summary>
        public string TextR { get; set; }

        /// <summary>
        /// Gets or sets the text l.
        /// </summary>
        public string TextL { get; set; }
    }
}
