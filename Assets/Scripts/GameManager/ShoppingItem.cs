using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    [CreateAssetMenu(fileName = "ShoppingItem", menuName = "QuarantineShopping/Item")]
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

        [SerializeField, Range(0f, 1f)]
        private float colliderSizeFactor = 0.8f;
        /// <summary>
        /// The factor the sprite size must be multiplied by in order to obtain the collider size.
        /// </summary>
        public float ColliderSizeFactor { get { return colliderSizeFactor; } }

        [SerializeField]
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

        [SerializeField]
        private ModifierRule[] modifiers;
        /// <summary>
        /// The modifiers applied by this shopping items.
        /// </summary>
        public ModifierRule[] Modifiers { get { return modifiers; } }

        [SerializeField]
        private AudioClip pickedSound;
        /// <summary>
        /// The audioclip played when the item is picked in the cart.
        /// </summary>
        public AudioClip PickedSound { get { return pickedSound; } }

        [SerializeField]
        private AudioClip lostSound;
        /// <summary>
        /// The audioclip played when the item is smashed on the floor.
        /// </summary>
        public AudioClip LostSound { get { return lostSound; } }
    }
}

