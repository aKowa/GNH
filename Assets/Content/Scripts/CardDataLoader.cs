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
    using System.Linq.Expressions;
    using System.Text;

    using UnityEngine;
    /*
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.RepresentationModel;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;
    */
    /// <summary>
    /// The card data loader.
    /// </summary>
    public class CardDataLoader : MonoBehaviour
    {
        /// <summary>
        /// The card data list.
        /// </summary>
        private List<CardData> cardDataList;

        /// <summary>
        /// The delimiter.
        /// </summary>
        private char[] delimiter = { ':', ' ' };

        /// <summary>
        /// Use this for initialization
        /// </summary>
        private void Start()
        {
            this.cardDataList = new List<CardData>();

            this.LoadCardData("Data/GNH.yaml");

            var output = new StringBuilder();
            output.AppendLine("Loading Result:");
            foreach (var cardData in this.cardDataList)
            {
                output.AppendLine(cardData.ToString());
            }
            Debug.Log(output);
        }

        /// <summary>
        /// Loads the card data from the file with the given file name.
        /// </summary>
        /// <param name="fileName">
        /// The file name of the card data stored in yaml.
        /// </param>
        private void LoadCardData(string fileName)
        {
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
                            this.DeserializeCardData(streamReader);
                        }
                    }
                    while (line != null);
                }
            }
            catch (Exception e)
            {
                Debug.Log(string.Format("Exception message from {0}: {1}", e, e.Message));
            }
        }

        /// <summary>
        /// The parse card data.
        /// </summary>
        /// <param name="streamReader">
        /// The stream reader.
        /// </param>
        private void DeserializeCardData(TextReader streamReader)
        {
            var output = new StringBuilder();
            output.AppendLine("deserialized card data");

            var cardData = new CardData();

            /*
             * CARD ID
             */
            var line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            var splittedLine = line.Split(this.delimiter); // index 2 is the id
            var id = int.Parse(splittedLine[2]);
            cardData.CardId = id;
            output.AppendLine(string.Format("id: {0}", id));

            /*
             * PARENT (ignore)
             */
            streamReader.ReadLine();

            /*
             * ATTRIBUTES (create CardAttributes)
             */
            streamReader.ReadLine();
            var cardAttributes = new CardAttributes();

            /*
             * BACKGROUND COLOR
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the hex code with # in front
            output.AppendLine(string.Format("BackgroundColor: {0}", splittedLine[5]));
            /*
             * do some magic
             *                  here!
             */
            cardAttributes.BackgroundColor = new Color32(0x2B, 0x2B, 0x2B, 0x2B); // pass right byte or float values

            /*
             * CATEGORY
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            output.AppendLine(string.Format("Category: {0}", splittedLine[5]));

            /*
             * CHARACTER
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            output.AppendLine(string.Format("Character: {0}", splittedLine[5]));

            /*
             * CULTURE L
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            cardAttributes.CultureL = int.Parse(splittedLine[5]);
            output.AppendLine(string.Format("Culture L: {0}", splittedLine[5]));

            /*
             * CULTURE R
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            cardAttributes.CultureR = int.Parse(splittedLine[5]);
            output.AppendLine(string.Format("Culture R: {0}", splittedLine[5]));

            /*
             * ECONOMY L
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            cardAttributes.EconomyL = int.Parse(splittedLine[5]);
            output.AppendLine(string.Format("Economy L: {0}", splittedLine[5]));

            /*
             * ECONOMY R
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            cardAttributes.EconomyR = int.Parse(splittedLine[5]);
            output.AppendLine(string.Format("Economy R: {0}", splittedLine[5]));

            /*
             * ENVIRONMENT L
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            cardAttributes.EnvironmentL = int.Parse(splittedLine[5]);
            output.AppendLine(string.Format("Environment L: {0}", splittedLine[5]));

            /*
             * ENVIRONMENT R
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            cardAttributes.EnvironmentR = int.Parse(splittedLine[5]);
            output.AppendLine(string.Format("Environment R: {0}", splittedLine[5]));

            /*
             * SECURITY L
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            cardAttributes.SecurityL = int.Parse(splittedLine[5]);
            output.AppendLine(string.Format("Security L: {0}", splittedLine[5]));

            /*
             * SECURITY R
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            cardAttributes.SecurityR = int.Parse(splittedLine[5]);
            output.AppendLine(string.Format("Security R: {0}", splittedLine[5]));

            /*
             * TEXT
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            output.AppendLine(string.Format("Text: {0}", splittedLine[5]));

            /*
             * TEXT L
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            output.AppendLine(string.Format("Text L: {0}", splittedLine[5]));

            /*
             * TEXT R
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            output.AppendLine(string.Format("Text R: {0}", splittedLine[5]));

            /*
             * TREASURY L
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            cardAttributes.TreasuryL = int.Parse(splittedLine[5]);
            output.AppendLine(string.Format("Treasury L: {0}", splittedLine[5]));

            /*
             * TREASURY R
             */
            line = streamReader.ReadLine();
            if (line == null)
            {
                return;
            }
            splittedLine = line.Split(this.delimiter); // index 5 is the value
            cardAttributes.TreasuryR = int.Parse(splittedLine[5]);
            output.AppendLine(string.Format("Treasury R: {0}", splittedLine[5]));

            this.cardDataList.Add(cardData);

            Debug.Log(output);
        }
    }
}
