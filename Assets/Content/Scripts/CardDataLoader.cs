// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CardDataLoader.cs" company="Morra">
//   Bent Nürnberg
// </copyright>
// <summary>
//   Defines the CardDataLoader type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Content.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;

    using UnityEngine;

    /// <summary>
    /// The card data loader.
    /// </summary>
    public static class CardDataLoader
    {
        /// <summary>
        /// The delimiters.
        /// </summary>
        private static char[] delimiter = { ':', ' ' };

        /// <summary>
        /// The get card data list.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<CardData> GetCardDataList()
        {
            var cardDataList = new List<CardData>();

            try
            {
                Stream reader = new MemoryStream((Resources.Load("GNH") as TextAsset).bytes);
                var streamReader = new StreamReader(reader);
                using (streamReader)
                {
                    string line;

                    do
                    {
                        line = streamReader.ReadLine();

                        if (line == null)
                        {
                            break;
                        }

                        if (line.StartsWith("---"))
                        {
                            var cardData = GetDeserializedCardData(streamReader);
                            if (cardData != null)
                            {
                                cardDataList.Add(cardData);
                            }
                        }
                    }
                    while (line != null);
                }
            }
            catch (Exception e)
            {
                Debug.Log(string.Format("Exception message from {0}: {1}", e, e.Message));
            }

            return cardDataList;
        }

        /// <summary>
        /// The get deserialized card data.
        /// </summary>
        /// <param name="streamReader">
        /// The stream reader.
        /// </param>
        /// <returns>
        /// The <see cref="CardData"/>.
        /// </returns>
        private static CardData GetDeserializedCardData(TextReader streamReader)
        {
            var cardData = new CardData();
            var cardAttributes = new CardAttributes();
            var output = new StringBuilder();
            output.AppendLine("deserialized card data");

            /*
             * CARD ID
             */
            var line = streamReader.ReadLine();
            cardData.CardId = GetIntegerValue(line, 2);
            output.AppendLine(string.Format("id: {0}", cardData.CardId));

            /*
             * PARENT (ignore)
             */
            streamReader.ReadLine();

            /*
             * ATTRIBUTES (ignore)
             */
            streamReader.ReadLine();

            /*
             * BACKGROUND COLOR
             */
            line = streamReader.ReadLine();
            cardAttributes.BackgroundColor = GetColorValue(line, 5);
            output.AppendLine(string.Format("BackgroundColor: {0}", cardAttributes.BackgroundColor));
            /*
             * do some magic
             *                  here!
             */

            // cardAttributes.BackgroundColor = new Color32(0x2B, 0x2B, 0x2B, 0x2B); // pass right byte or float values

            /*
             * CATEGORY
             */
            line = streamReader.ReadLine();
            cardAttributes.Category = GetStringValue(line, 5);
            output.AppendLine(string.Format("Category: {0}", cardAttributes.Category));

            /*
             * CHARACTER
             */
            line = streamReader.ReadLine();
            var character = GetStringValue(line, 5);
            character = character.Trim(); // trim
            switch (character)
            {
                case "Character1":
                    cardAttributes.Character = Character.Character1;
                    break;
                case "Character2":
                    cardAttributes.Character = Character.Character2;
                    break;
                case "Character3":
                    cardAttributes.Character = Character.Character3;
                    break;
                case "Character4":
                    cardAttributes.Character = Character.Character4;
                    break;
                default:
                    cardAttributes.Character = Character.NotSet;
                    Debug.Log("Error reading Character");
                    break;
            }
            output.AppendLine(string.Format("Character: {0}", cardAttributes.Character));

            /*
             * CULTURE L
             */
            line = streamReader.ReadLine();
            cardAttributes.CultureL = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Culture L: {0}", cardAttributes.CultureL));

            /*
             * CULTURE R
             */
            line = streamReader.ReadLine();
            cardAttributes.CultureR = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Culture R: {0}", cardAttributes.CultureR));

            /*
             * ECONOMY L
             */
            line = streamReader.ReadLine();
            cardAttributes.EconomyL = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Economy L: {0}", cardAttributes.EconomyL));

            /*
             * ECONOMY R
             */
            line = streamReader.ReadLine();
            cardAttributes.EconomyR = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("EconomyR: {0}", cardAttributes.EconomyR));

            /*
             * ENVIRONMENT L
             */
            line = streamReader.ReadLine();
            cardAttributes.EnvironmentL = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Environment L: {0}", cardAttributes.EnvironmentL));

            /*
             * ENVIRONMENT R
             */
            line = streamReader.ReadLine();
            cardAttributes.EnvironmentR = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Environment R: {0}", cardAttributes.EnvironmentR));

            /*
             * FOLLOW UP ID L
             */
            line = streamReader.ReadLine();
            cardAttributes.FollowUpIdL = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Follow Up Id L: {0}", cardAttributes.FollowUpIdL));

            /*
             * FOLLOW UP ID R
             */
            line = streamReader.ReadLine();
            cardAttributes.FollowUpIdR = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Follow Up Id R: {0}", cardAttributes.FollowUpIdR));

            /*
             * FOLLOW UP STEP L
            */
            line = streamReader.ReadLine();
            cardAttributes.FollowUpStepL = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Follow Up Step L: {0}", cardAttributes.FollowUpStepL));

            /*
             * FOLLOW UP STEP R
            */
            line = streamReader.ReadLine();
            cardAttributes.FollowUpStepR = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Follow Up Step R: {0}", cardAttributes.FollowUpStepR));

            /*
             * SECURITY L
             */
            line = streamReader.ReadLine();
            cardAttributes.SecurityL = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Security L: {0}", cardAttributes.SecurityL));

            /*
             * SECURITY R
             */
            line = streamReader.ReadLine();
            cardAttributes.SecurityR = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Security R: {0}", cardAttributes.SecurityR));

            /*
             * TEXT
             */
            line = streamReader.ReadLine();
            cardAttributes.Text = GetStringValue(line, 5);
            output.AppendLine(string.Format("Text: {0}", cardAttributes.Text));

            /*
             * TEXT L
             */
            line = streamReader.ReadLine();
            cardAttributes.TextL = GetStringValue(line, 5);
            output.AppendLine(string.Format("Text L: {0}", cardAttributes.TextL));

            /*
             * TEXT R
             */
            line = streamReader.ReadLine();
            cardAttributes.TextR = GetStringValue(line, 5);
            output.AppendLine(string.Format("Text R: {0}", cardAttributes.TextR));

            /*
             * TREASURY L
             */
            line = streamReader.ReadLine();
            cardAttributes.TreasuryL = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Treasury L: {0}", cardAttributes.TreasuryL));

            /*
             * TREASURY R
             */
            line = streamReader.ReadLine();
            cardAttributes.TreasuryR = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Treasury R: {0}", cardAttributes.TreasuryR));

            //Debug.Log(output);

            cardData.CardAttributes = cardAttributes;
            return cardData;
        }

        /// <summary>
        /// The get color value.
        /// </summary>
        /// <param name="line">
        /// The line.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="Color"/>.
        /// </returns>
        private static Color GetColorValue(string line, int index)
        {
            var hexcode = GetStringValue(line, index);

            if (hexcode.StartsWith("#"))
            {
                hexcode = hexcode.Substring(1);
            }

            if (hexcode.StartsWith("0x"))
            {
                hexcode = hexcode.Substring(2);
            }

            if (hexcode.Length != 6)
            {
                //throw new Exception(string.Format("{0} is not a valid color string.", hexcode));
            }

            var r = byte.Parse(hexcode.Substring(0, 2), NumberStyles.HexNumber);
            var g = byte.Parse(hexcode.Substring(2, 2), NumberStyles.HexNumber);
            var b = byte.Parse(hexcode.Substring(4, 2), NumberStyles.HexNumber);

            return new Color(r, g, b);
        }

        /// <summary>
        /// The get string value.
        /// </summary>
        /// <param name="line">
        /// The line.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetStringValue(string line, int index)
        {
            var splittedLine = line.Split(delimiter);
            var text = new StringBuilder();
            for (var i = index; i < splittedLine.Length; i++)
            {
                text.Append(splittedLine[i]);
                text.Append(" ");
            }
            return text.ToString().Trim();
        }

        /// <summary>
        /// The get integer value.
        /// </summary>
        /// <param name="line">
        /// The line.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private static int GetIntegerValue(string line, int index)
        {
            return int.Parse(line.Split(delimiter)[index]);
        }
    }
}
