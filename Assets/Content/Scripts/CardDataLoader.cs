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
            var fileName = "Data/GNH.yaml";
            var cardDataList = new List<CardData>();

            try
            {
                var streamReader = new StreamReader(fileName, Encoding.UTF8);

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
            cardAttributes.BackgroundColor = new Color32(0x2B, 0x2B, 0x2B, 0x2B); // pass right byte or float values

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
            switch (character)
            {
                case "Character1":
                    cardAttributes.Character = 1;
                    break;
                case "Character2":
                    cardAttributes.Character = 2;
                    break;
                case "Character3":
                    cardAttributes.Character = 3;
                    break;
                case "Character4":
                    cardAttributes.Character = 4;
                    break;
                default:
                    cardAttributes.Character = 0;
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

            Debug.Log(output);

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
            var color = new Color();
            /*
             * do some magic here
             */
            Debug.Log("Color not implemented yet");
            return color;
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
            return text.ToString();
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
