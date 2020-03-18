using System.Collections;
using UnityEngine;

namespace Quaranteam
{
    public enum ModifiersId
    {
        PlayerSpeed,
        TimePerception,
        Timer,
        ScoreMultiplier,
        RandomizeKey
    }

    public abstract class ModifierRule : ScriptableObject
    {
        /// <summary>
        /// The duration of the effect.
        /// </summary>
        [Header("Duration of the effect (-1 for permanent)"), SerializeField]
        protected float effectDuration = 5f;
        public float Duration { get { return effectDuration; } }

        /// <summary>
        /// Applies the rule on the given game manager.
        /// </summary>
        /// <param name="gameManager"></param>
        protected abstract void ApplyRule(GameManager gameManager);

        /// <summary>
        /// Reverts the rule, causing the game manager to return to the initial state.
        /// </summary>
        /// <param name="gameManager"></param>
        public virtual void RevertRule(GameManager gameManager) { }

        public abstract ModifiersId Id { get; }

        public abstract string Description { get; }

        private float remainingTime;
        public float RemainingTime { get { return remainingTime; } set { remainingTime = value; } }

        public void Apply(GameManager gameManager)
        {
            ApplyRule(gameManager);
            remainingTime = effectDuration;
            if (remainingTime > 0f)
                gameManager.AddModifier(this);
        }

    }
}

