using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    [CreateAssetMenu(fileName = "TimePerceptionModifier", menuName = "QuarantineShopping/Modifiers/Time Perception")]
    public class TimePerceptionModifier : ModifierRule
    {
        /// <summary>
        /// Time scale factor
        /// </summary>
        [Header("Time will be multiplied by this value"), SerializeField, Range(0.001f, 2.0f)]
        private float timeScaleApplied = 0.5f;

        public override ModifiersId Id { get { return ModifiersId.TimePerception; } }

        public override string Description { get { return "Time dilation: " + timeScaleApplied.ToString("00"); } }

        protected override void ApplyRule(GameManager gameManager)
        {
            gameManager.fallingItemsTimeScale = timeScaleApplied;
        }

        public override void RevertRule(GameManager gameManager)
        {
            gameManager.fallingItemsTimeScale = 1.0f;
        }

        private IEnumerator SmoothlyApplyTimeScale(GameManager gameManager, float target)
        {
            while (Mathf.Abs(Time.timeScale - target) > 0.001f)
            {
                gameManager.fallingItemsTimeScale = Mathf.MoveTowards(
                    gameManager.fallingItemsTimeScale, target, Time.deltaTime);
                yield return null;
            }
        }
    }
}

