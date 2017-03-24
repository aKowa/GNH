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
        /// <summary>
        /// The block input.
        /// </summary>
        [HideInInspector]
        private bool blockInput = false;

        /// <summary>
        /// The bound policy values.
        /// </summary>
        [SerializeField]
        private BindInt[] boundPolicyValues = new BindInt[6];

        /// <summary>
        /// The game over object.
        /// </summary>
        [SerializeField]
        private GameObject gameOverObject;

        /// <summary>
        /// The happiness threshold lose.
        /// </summary>
        [Tooltip("Threshold policyValue of happines used to determine when player lost! Use negative policyValue to disable.")]
        [SerializeField]
        private int happinessThresholdLose = 20;

        /// <summary>
        /// The happiness threshold win.
        /// </summary>
        [Tooltip("Threshold policyValue of happines used to determine when player lost! Use negative policyValue to disable.")]
        [SerializeField]
        private int happinessThresholdWin = 90;

        /// <summary>
        /// The init color.
        /// </summary>
        private Color initColor;

        /// <summary>
        /// The preview color negative.
        /// </summary>
        [SerializeField]
        private Color previewColorNegative = Color.red;

        /// <summary>
        /// The preview color positive.
        /// </summary>
        [SerializeField]
        private Color previewColorPositive = Color.green;

        /// <summary>
        /// The revert speed.
        /// </summary>
        [SerializeField]
        private float revertSpeed = 1f;

        /// <summary>
        /// The text values.
        /// </summary>
        private Text[] textValues;

        /// <summary>
        /// Gets a policyValue indicating whether to block input.
        /// </summary>
        public bool BlockInput
        {
            get
            {
                return this.blockInput;
            }

            private set
            {
                this.blockInput = value;
            }
        }

        /// <summary>
        /// The get policy value.
        /// </summary>
        /// <param name="policyValue">
        /// The policy value.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int GetPolicyValue(PolicyValue policyValue)
        {
            switch (policyValue)
            {
                case PolicyValue.Culture:
                case PolicyValue.Economy:
                case PolicyValue.Environment:
                case PolicyValue.Security:
                case PolicyValue.Treasury:
                    return this.boundPolicyValues[(int)policyValue].Value;
                case PolicyValue.Happiness:
                    // Gets the calculated happiness by using the average of the big four policy values.
                    var targetHappiness = 0;
                    for (var i = 0; i < 4; i++)
                    {
                        targetHappiness += this.boundPolicyValues[i].Value;
                    }

                    return targetHappiness / 4;
                default:
                    Debug.LogWarning("passed PolicyValue enum not valid, please think about what you are doing here. returned 0.");
                    return 0;
            }
        }

        /// <summary>
        /// The apply results.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        public void ApplyResults(int[] values)
        {
            // TODO: Implement turn count to determine time passed
            this.RevertPreview();
            for (var i = 0; i < values.Length; i++)
            {
                this.boundPolicyValues[i].Value += values[i];
            }

            this.boundPolicyValues[5].Value = this.GetPolicyValue(PolicyValue.Happiness);
            this.CheckforGameOver();
        }

        /// <summary>
        /// Shows a preview of the possible result.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        public void PreviewResults(int[] values)
        {
            this.StopAllCoroutines();
            for (var i = 0; i < values.Length; i++)
            {
                this.textValues[i].color = this.initColor;
                if (values[i] > 0)
                {
                    this.textValues[i].color = this.previewColorPositive;
                }
                else if (values[i] < 0)
                {
                    this.textValues[i].color = this.previewColorNegative;
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
            this.BlockInput = false;
        }

        /// <summary>
        /// Checks policy values and determines as well as executes a game over. 
        /// NOTE: reload logic is on the GameOverScreenObject 
        /// </summary>
        private void CheckforGameOver()
        {
            // Check for victory by happiness
            if (this.boundPolicyValues[5].valueUnbound >= this.happinessThresholdWin && this.happinessThresholdWin > 0)
            {
                this.GetGameOverText(5).text =
                    "Victory! \n \n Your happiness exceeds all expectations! \n \n Party hard!!!";
                return;
            }

            // Check for lose by happines
            if (this.boundPolicyValues[5].valueUnbound <= this.happinessThresholdLose && this.happinessThresholdLose > 0)
            {
                this.GetGameOverText(5).text += " was too damn low!";
                return;
            }

            // check policy bounds
            for (var i = 0; i < 4; i++)
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
        /// <param name="policyId">
        /// The policyID responsible for the game over.
        /// </param>
        /// <returns>
        /// The <see cref="Text"/>.
        /// </returns>
        private Text GetGameOverText(int policyId)
        {
            this.BlockInput = true;
            this.gameOverObject.SetActive(true);
            var gameOverText = this.gameOverObject.GetComponentInChildren<Text>();
            var policyName =
                this.boundPolicyValues[policyId].targetGameObject.GetComponentInParent<Image>().gameObject.name;
            gameOverText.text = "You Lost! \n \n Your " + policyName;
            return gameOverText;
        }

        /// <summary>
        /// Lerps image colors back to its init color.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        private IEnumerator RevertPreviewAnimation()
        {
            float t = 0;
            var colors = new Color[this.textValues.Length];
            for (var i = 0; i < colors.Length; i++)
            {
                colors[i] = this.textValues[i].color;
            }

            while (t < 1f)
            {
                for (var i = 0; i < this.textValues.Length; i++)
                {
                    this.textValues[i].color = Color.Lerp(colors[i], this.initColor, t);
                }

                t += this.revertSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }
}
