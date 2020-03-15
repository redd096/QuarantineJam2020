﻿using System.Collections;
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

        protected override void ApplyRule(GameManager gameManager)
        {
            gameManager.Player.speed += valueToAddToSpeed;
            gameManager.Player.acceleration += valueToAddToAcceleration;
        }

        protected override void RevertRule(GameManager gameManager)
        {
            gameManager.Player.speed -= valueToAddToSpeed;
            gameManager.Player.acceleration -= valueToAddToAcceleration;
        }
    }
}

