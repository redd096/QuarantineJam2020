using System.Collections;
using UnityEngine;

namespace Quaranteam
{
    public abstract class ModifierRule : ScriptableObject
    {
        /// <summary>
        /// The duration of the effect.
        /// </summary>
        [Header("Duration of the effect (-1 for permanent)"), SerializeField]
        protected float effectDuration = 5f;

        /// <summary>
        /// Applies the rule on the given game manager.
        /// </summary>
        /// <param name="gameManager"></param>
        protected abstract void ApplyRule(GameManager gameManager);

        /// <summary>
        /// Reverts the rule, causing the game manager to return to the initial state.
        /// </summary>
        /// <param name="gameManager"></param>
        protected virtual void RevertRule(GameManager gameManager) { }

        public void Apply(GameManager gameManager)
        {
            ApplyRule(gameManager);

            if (effectDuration > 0.0f)
            {
                gameManager.StartCoroutine(RevertEffectAfterDelay(gameManager));
            }
        }

        protected IEnumerator RevertEffectAfterDelay(GameManager gameManager)
        {
            yield return new WaitForSeconds(effectDuration);

            RevertRule(gameManager);

            yield break;
        }

    }
}

