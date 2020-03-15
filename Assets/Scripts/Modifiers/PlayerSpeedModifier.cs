using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    [CreateAssetMenu(fileName = "PlayerSpeedModifier", menuName = "QuarantineShopping/Modifiers/PlayerSpeed")]
    public class PlayerSpeedModifier : ModifierRule
    {
        /// <summary>
        /// The value that must be added to the current speed.
        /// </summary>
        [Header("First iteration"), SerializeField]
        private float valueToAddToSpeed;

        /// <summary>
        /// The value that must be added to the current speed.
        /// </summary>
        [Header("Second iteration"), SerializeField]
        private float valueToAddToAcceleration;

        /// <summary>
        /// The duration of the effect.
        /// </summary>
        [Header("Duration of the effect (-1 for permanent)"), SerializeField]
        private float effectDuration = 5f;

        public override void ApplyRule(GameManager gameManager)
        {
            gameManager.Player.speed += valueToAddToSpeed;
            gameManager.Player.acceleration += valueToAddToAcceleration;

            if (effectDuration > 0.0f)
            {
                gameManager.Player.StartCoroutine(RevertEffectAfterDelay(gameManager.Player));
            }
        }

        private IEnumerator RevertEffectAfterDelay(Player player)
        {
            yield return new WaitForSeconds(effectDuration);

            player.speed -= valueToAddToSpeed;
            player.acceleration -= valueToAddToAcceleration;

            yield break;
        }
    }
}

