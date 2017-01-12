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
    using UnityEngine;

    /// <summary>
    /// The data to create a card
    /// </summary>
    public class CardData
    {
        /// <summary>
        /// The card id.
        /// </summary>
        private string cardId;

        /// <summary>
        /// The background color.
        /// </summary>
        private Color backgroundColor;

        /// <summary>
        /// The character.
        /// </summary>
        private Character character;

        /// <summary>
        /// The culture r.
        /// </summary>
        private int cultureR;

        /// <summary>
        /// The economy r.
        /// </summary>
        private int economyR;

        /// <summary>
        /// The environment r.
        /// </summary>
        private int environmentR;

        /// <summary>
        /// The security r.
        /// </summary>
        private int securityR;

        /// <summary>
        /// The treasury r.
        /// </summary>
        private int treasuryR;

        /// <summary>
        /// The culture l.
        /// </summary>
        private int cultureL;

        /// <summary>
        /// The economy l.
        /// </summary>
        private int economyL;

        /// <summary>
        /// The environment l.
        /// </summary>
        private int environmentL;

        /// <summary>
        /// The security l.
        /// </summary>
        private int securityL;

        /// <summary>
        /// The treasury l.
        /// </summary>
        private int treasuryL;

        /// <summary>
        /// The text.
        /// </summary>
        private string text;

        /// <summary>
        /// The text r.
        /// </summary>
        private string textR;

        /// <summary>
        /// The text l.
        /// </summary>
        private string textL;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardData"/> class.
        /// </summary>
        /// <param name="cardId">
        /// The card id.
        /// </param>
        /// <param name="backgroundColor">
        /// The background color.
        /// </param>
        /// <param name="character">
        /// The character.
        /// </param>
        /// <param name="cultureR">
        /// The culture r.
        /// </param>
        /// <param name="economyR">
        /// The economy r.
        /// </param>
        /// <param name="environmentR">
        /// The environment r.
        /// </param>
        /// <param name="securityR">
        /// The security r.
        /// </param>
        /// <param name="treasuryR">
        /// The treasury r.
        /// </param>
        /// <param name="cultureL">
        /// The culture l.
        /// </param>
        /// <param name="economyL">
        /// The economy l.
        /// </param>
        /// <param name="environmentL">
        /// The environment l.
        /// </param>
        /// <param name="securityL">
        /// The security l.
        /// </param>
        /// <param name="treasuryL">
        /// The treasury l.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="textR">
        /// The text r.
        /// </param>
        /// <param name="textL">
        /// The text l.
        /// </param>
        public CardData(string cardId, Color backgroundColor, Character character, int cultureR, int economyR, int environmentR, int securityR, int treasuryR, int cultureL, int economyL, int environmentL, int securityL, int treasuryL, string text, string textR, string textL)
        {
            this.cardId = cardId;
            this.backgroundColor = backgroundColor;
            this.character = character;
            this.cultureR = cultureR;
            this.economyR = economyR;
            this.environmentR = environmentR;
            this.securityR = securityR;
            this.treasuryR = treasuryR;
            this.cultureL = cultureL;
            this.economyL = economyL;
            this.environmentL = environmentL;
            this.securityL = securityL;
            this.treasuryL = treasuryL;
            this.text = text;
            this.textR = textR;
            this.textL = textL;
        }
    }
}
