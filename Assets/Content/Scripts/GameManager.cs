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
    using System.Collections;

    using PolyDev.UI;

    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The game manager.
    /// </summary>
    /// TODO: can be made static?
    public class GameManager : MonoBehaviour
    {
        [HideInInspector]
        public bool blockInput = false;

        public BindInt[] boundPolicyValues = new BindInt[6];

        public GameObject gameOverObject;

        [Tooltip("Threshold value of happines used to determine when player lost! Use negative value to disable.")]
        public int loseHappinesThreshold = 20;

        public Color negativePreviewColor = Color.red;

        public Color positivePreviewColor = Color.green;

        public float revertSpeed = 1f;

        [Tooltip("Threshold value of happines used to determine when player lost! Use negative value to disable.")]
        public int winHappinesThreshold = 90;

        private Color initColor;

        private Text[] textValues;

        /// <summary>
        /// Calculates happiness by using the average of all policy values.
        /// </summary>
        private int Happiness
        {
            get
            {
                int targetHappines = 0;
                for (int i = 0; i < 4; i++)
                {
                    targetHappines += this.boundPolicyValues[i].valueUnbound;
                }

                return targetHappines / 4;
            }
        }

        // TODO: Implement turn count to determine time passed
        public void ApplyResults(int[] values)
        {
            this.RevertPreview();
            for (int i = 0; i < values.Length; i++)
            {
                this.boundPolicyValues[i].Value += values[i];
            }

            this.boundPolicyValues[5].Value = this.Happiness;
            this.CheckforGameOver();
        }

        /// <summary>
        /// Shows a preview of the possible result.
        /// </summary>
        public void PreviewResults(int[] values)
        {
            this.StopAllCoroutines();
            for (int i = 0; i < values.Length; i++)
            {
                this.textValues[i].color = this.initColor;
                if (values[i] > 0)
                {
                    this.textValues[i].color = this.positivePreviewColor;
                }
                else if (values[i] < 0)
                {
                    this.textValues[i].color = this.negativePreviewColor;
                }
            }
        }

        /// <summary>
        /// Starts reverting colors;
        /// </summary>
        public void RevertPreview()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.RevertPreviewAnimation());
        }

        /// <summary>
        /// The start. Sets initial parameter
        /// </summary>
        public void Start()
        {
            this.textValues = this.GetComponentsInChildren<Text>();
            this.initColor = this.textValues[0].color;
            this.gameOverObject.SetActive(false);
            this.blockInput = false;
        }

        /// <summary>
        /// Checks policy values and determines as well as executes a game over. 
        /// NOTE: reload logic is on the GameOverScreenObject 
        /// </summary>
        private void CheckforGameOver()
        {
            // Check for victory by happiness
            if (this.boundPolicyValues[5].valueUnbound >= this.winHappinesThreshold && this.winHappinesThreshold > 0)
            {
                this.GetGameOverText(5).text =
                    "Victory! \n \n Your happiness exceeds all expectations! \n \n Party hard!!!";
                return;
            }

            // Check for lose by happines
            if (this.boundPolicyValues[5].valueUnbound <= this.loseHappinesThreshold && this.loseHappinesThreshold > 0)
            {
                this.GetGameOverText(5).text += " was too damn low!";
                return;
            }

            // check policy bounds
            for (int i = 0; i < 4; i++)
            {
                // Check if policy is too low
                if (this.boundPolicyValues[i].valueUnbound <= 0)
                {
                    this.GetGameOverText(i).text += " was too damn low!";
                    return;
                }

                // Check if policy is too high
                if (this.boundPolicyValues[i].valueUnbound >= 100)
                {
                    this.GetGameOverText(i).text += " was too damn high!";
                    return;
                }
            }
        }

        /// <summary>
        /// Returns GameOver text object and sets parameter for blocking input.
        /// </summary>
        /// <param name="policyID">The policyID responsible for the game over.</param>
        private Text GetGameOverText(int policyID)
        {
            this.blockInput = true;
            this.gameOverObject.SetActive(true);
            var gameOverText = this.gameOverObject.GetComponentInChildren<Text>();
            var policyName =
                this.boundPolicyValues[policyID].targetGameObject.GetComponentInParent<Image>().gameObject.name;
            gameOverText.text = "You Lost! \n \n Your " + policyName;
            return gameOverText;
        }

        /// <summary>
        /// Lerps image colors back to its init color.
        /// </summary>
        private IEnumerator RevertPreviewAnimation()
        {
            float t = 0;
            var colors = new Color[this.textValues.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = this.textValues[i].color;
            }

            while (t < 1f)
            {
                for (int i = 0; i < this.textValues.Length; i++)
                {
                    this.textValues[i].color = Color.Lerp(colors[i], this.initColor, t);
                }

                t += this.revertSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }
}
