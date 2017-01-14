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
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    /// The game manager.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private List<CardData> cardData; 

        /// <summary>
        /// The card.
        /// </summary>
        public CardComponent card;

        /// <summary>
        /// The start.
        /// </summary>
        public void Start()
        {
            this.cardData = CardDataLoader.GetCardDataList();
        }

        /// <summary>
        /// The update.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i <= 1; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    if (this.card == null) // sanity check
                    {
                        return;
                    }

                    var cardPolicy = this.card.policies[i];
                    foreach (var policyComponent in this.GetComponentsInChildren<PolicyUIComponent>())
                    {
                        // if ( policyComponent.policy.name == cardPolicy.name)
                        // {
                        // 	policyComponent.AddPolicy ( cardPolicy );
                        // 	break;
                        // }
                    }

                    this.card.SetRandomPolicy();
                }
            }
        }
    }
}
