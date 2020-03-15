using UnityEngine;

namespace Quaranteam
{
    public abstract class ModifierRule : ScriptableObject
    {
        public abstract void ApplyRule(GameManager gameManager);
    }
}

