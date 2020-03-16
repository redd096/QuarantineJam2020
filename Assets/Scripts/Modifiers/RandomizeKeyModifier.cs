using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    [CreateAssetMenu(fileName = "RandomizeKeyModifier", menuName = "QuarantineShopping/Modifiers/Randomize Key")]
    public class RandomizeKeyModifier : ModifierRule
    {
        public override ModifiersId Id { get { return ModifiersId.RandomizeKey; } }

        public override string Description { get { return "Randomized Keys"; } }
        protected override void ApplyRule(GameManager gameManager)
        {
            gameManager.Player.RandomizeKeyCodes();
        }

        public override void RevertRule(GameManager gameManager)
        {
            gameManager.Player.DefaultKeyCodes();
        }
    }
}