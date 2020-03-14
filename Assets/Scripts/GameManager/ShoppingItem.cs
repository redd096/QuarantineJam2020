using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    [CreateAssetMenu(fileName = "ShoppingItem", menuName = "Assets/QuarantineShopping/Item")]
    public class ShoppingItem : ScriptableObject
    {
        [SerializeField]
        private string displayName;
        /// <summary>
        /// The name showed by the UI.
        /// </summary>
        public string DisplayName { get { return displayName; } }

        [SerializeField]
        private Sprite sprite;
        /// <summary>
        /// The sprite that represents the object.
        /// </summary>
        public Sprite Sprite { get { return sprite; } }

        [SerializeField, Min(0)]
        private int baseReward = 0;
        /// <summary>
        /// The reward given by the object when it is NOT in the shopping list.
        /// </summary>
        public int BaseReward { get { return baseReward; } }

        [SerializeField]
        private float weight = 1f;
        /// <summary>
        /// The weight of the object: modifies its falling speed.
        /// </summary>
        public float Weight { get { return weight; } }
    }
}

