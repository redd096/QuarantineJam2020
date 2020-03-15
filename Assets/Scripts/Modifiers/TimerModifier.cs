using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    [CreateAssetMenu(fileName = "TimerModifier", menuName = "QuarantineShopping/Modifiers/Timer")]
    public class TimerModifier : ModifierRule
    {

        [Header("To add to the timer"), SerializeField]
        private float timerGainOrLoss = 5;

        public override ModifiersId Id { get { return ModifiersId.Timer; } }

        public override string Description { get { return "Timer boost"; } }
        protected override void ApplyRule(GameManager gameManager)
        {
            gameManager.LevelTimer.elapsedTime -= timerGainOrLoss;
        }

        public override void RevertRule(GameManager gameManager)
        {
            gameManager.LevelTimer.elapsedTime += timerGainOrLoss;
        }
    }
}

