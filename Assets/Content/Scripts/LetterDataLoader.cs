// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LetterDataLoader.cs" company="Morra">
//   Bent Nürnberg
// </copyright>
// <summary>
//   Defines the LetterDataLoader type.
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
    /// The letter data loader.
    /// </summary>
    public static class LetterDataLoader
    {
        /// <summary>
        /// The delimiters.
        /// </summary>
        private static char[] delimiter = { ':', ' ' };

        /// <summary>
        /// The get letter data list.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<LetterData> GetLetterDataList()
        {
            var letterDataList = new List<LetterData>();

            try
            {
                Stream reader = new MemoryStream((Resources.Load("GNH") as TextAsset).bytes);
                var streamReader = new StreamReader(reader, Encoding.UTF8);
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
                            var letterData = GetDeserializedLetterData(streamReader);
                            if (letterData != null)
                            {
                                letterDataList.Add(letterData);
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

            return letterDataList;
        }

        /// <summary>
        /// The get deserialized letter data.
        /// </summary>
        /// <param name="streamReader">
        /// The stream reader.
        /// </param>
        /// <returns>
        /// The <see cref="LetterData"/>.
        /// </returns>
        private static LetterData GetDeserializedLetterData(TextReader streamReader)
        {
            var letterData = new LetterData();
            var letterAttributes = new LetterAttributes();
            var output = new StringBuilder();
            output.AppendLine("deserialized letter data");

            /*
             * LETTER ID
             */
            var line = streamReader.ReadLine();
            letterData.LetterId = GetIntegerValue(line, 2);
            output.AppendLine(string.Format("id: {0}", letterData.LetterId));

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
            letterAttributes.BackgroundColor = GetColorValue(line, 5);
            output.AppendLine(string.Format("BackgroundColor: {0}", letterAttributes.BackgroundColor));
            /*
             * do some magic
             *                  here!
             */

            // letterAttributes.BackgroundColor = new Color32(0x2B, 0x2B, 0x2B, 0x2B); // pass right byte or float values

            /*
             * CATEGORY
             */
            line = streamReader.ReadLine();
            letterAttributes.Category = GetStringValue(line, 5);
            output.AppendLine(string.Format("Category: {0}", letterAttributes.Category));

            /*
             * CHARACTER
             */
            line = streamReader.ReadLine();
            var character = GetStringValue(line, 5);
            character = character.Trim(); // trim
            switch (character)
            {
                case "Character1":
                    letterAttributes.Character = Character.Character1;
                    break;
                case "Character2":
                    letterAttributes.Character = Character.Character2;
                    break;
                case "Character3":
                    letterAttributes.Character = Character.Character3;
                    break;
                case "Character4":
                    letterAttributes.Character = Character.Character4;
                    break;
                default:
                    letterAttributes.Character = Character.NotSet;
                    Debug.Log("Error reading Character");
                    break;
            }
            output.AppendLine(string.Format("Character: {0}", letterAttributes.Character));

            /*
             * CULTURE L
             */
            line = streamReader.ReadLine();
            letterAttributes.CultureL = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Culture L: {0}", letterAttributes.CultureL));

            /*
             * CULTURE R
             */
            line = streamReader.ReadLine();
            letterAttributes.CultureR = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Culture R: {0}", letterAttributes.CultureR));

            /*
             * ECONOMY L
             */
            line = streamReader.ReadLine();
            letterAttributes.EconomyL = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Economy L: {0}", letterAttributes.EconomyL));

            /*
             * ECONOMY R
             */
            line = streamReader.ReadLine();
            letterAttributes.EconomyR = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("EconomyR: {0}", letterAttributes.EconomyR));

            /*
             * ENVIRONMENT L
             */
            line = streamReader.ReadLine();
            letterAttributes.EnvironmentL = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Environment L: {0}", letterAttributes.EnvironmentL));

            /*
             * ENVIRONMENT R
             */
            line = streamReader.ReadLine();
            letterAttributes.EnvironmentR = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Environment R: {0}", letterAttributes.EnvironmentR));

            /*
             * FOLLOW UP ID L
             */
            line = streamReader.ReadLine();
            letterAttributes.FollowUpIdL = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Follow Up Id L: {0}", letterAttributes.FollowUpIdL));

            /*
             * FOLLOW UP ID R
             */
            line = streamReader.ReadLine();
            letterAttributes.FollowUpIdR = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Follow Up Id R: {0}", letterAttributes.FollowUpIdR));

            /*
             * FOLLOW UP STEP L
            */
            line = streamReader.ReadLine();
            letterAttributes.FollowUpStepL = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Follow Up Step L: {0}", letterAttributes.FollowUpStepL));

            /*
             * FOLLOW UP STEP R
            */
            line = streamReader.ReadLine();
            letterAttributes.FollowUpStepR = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Follow Up Step R: {0}", letterAttributes.FollowUpStepR));

            /*
             * SECURITY L
             */
            line = streamReader.ReadLine();
            letterAttributes.SecurityL = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Security L: {0}", letterAttributes.SecurityL));

            /*
             * SECURITY R
             */
            line = streamReader.ReadLine();
            letterAttributes.SecurityR = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Security R: {0}", letterAttributes.SecurityR));

            /*
             * TEXT
             */
            line = streamReader.ReadLine();
            letterAttributes.Text = GetStringValue(line, 5);
            output.AppendLine(string.Format("Text: {0}", letterAttributes.Text));

            /*
             * TEXT L
             */
            line = streamReader.ReadLine();
            letterAttributes.TextL = GetStringValue(line, 5);
            output.AppendLine(string.Format("Text L: {0}", letterAttributes.TextL));

            /*
             * TEXT R
             */
            line = streamReader.ReadLine();
            letterAttributes.TextR = GetStringValue(line, 5);
            output.AppendLine(string.Format("Text R: {0}", letterAttributes.TextR));

            /*
             * TREASURY L
             */
            line = streamReader.ReadLine();
            letterAttributes.TreasuryL = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Treasury L: {0}", letterAttributes.TreasuryL));

            /*
             * TREASURY R
             */
            line = streamReader.ReadLine();
            letterAttributes.TreasuryR = GetIntegerValue(line, 5);
            output.AppendLine(string.Format("Treasury R: {0}", letterAttributes.TreasuryR));

            //Debug.Log(output);

            letterData.LetterAttributes = letterAttributes;
            return letterData;
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
