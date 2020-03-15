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

        protected override void ApplyRule(GameManager gameManager)
        {
            gameManager.LevelTimer.elapsedTime -= timerGainOrLoss;
        }

        protected override void RevertRule(GameManager gameManager)
        {
            gameManager.LevelTimer.elapsedTime += timerGainOrLoss;
        }
    }
}

