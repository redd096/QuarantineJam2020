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

        public override ModifiersId Id { get { return ModifiersId.ScoreMultiplier; } }

        public override string Description { get { return "Score Multiplier: " + multiplierGainOrLoss.ToString(); } }

        protected override void ApplyRule(GameManager gameManager)
        {
            gameManager.CurrentMultiplier += multiplierGainOrLoss;
        }

        public override void RevertRule(GameManager gameManager)
        {
            gameManager.CurrentMultiplier -= multiplierGainOrLoss;
        }
    }
}

