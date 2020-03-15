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
        private float newSpeed;

        /// <summary>
        /// The value that must be added to the current speed.
        /// </summary>
        [Header("Second iteration"), SerializeField]
        private float newAcceleration;

        public override ModifiersId Id { get { return ModifiersId.PlayerSpeed; } }

        public override string Description { get { return "Speed modifier: " + newSpeed.ToString(); } }

        protected override void ApplyRule(GameManager gameManager)
        {
            gameManager.Player.actualSpeed = newSpeed;
            gameManager.Player.actualAcceleration = newAcceleration;
        }

        public override void RevertRule(GameManager gameManager)
        {
            gameManager.Player.actualSpeed = gameManager.Player.speed;
            gameManager.Player.actualAcceleration = gameManager.Player.acceleration;
        }
    }
}

