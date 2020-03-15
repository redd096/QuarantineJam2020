using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    [CreateAssetMenu(fileName = "ScoreMultiplier", menuName = "QuarantineShopping/Modifiers/Score Multiplier")]
    public class ScoreMultiplierModifier : ModifierRule
    {

        [SerializeField, Header("To add to the multiplier")]
        private float multiplierGainOrLoss = 0.5f;

        protected override void ApplyRule(GameManager gameManager)
        {
            gameManager.CurrentMultiplier += multiplierGainOrLoss;
        }

        protected override void RevertRule(GameManager gameManager)
        {
            gameManager.CurrentMultiplier -= multiplierGainOrLoss;
        }
    }
}

