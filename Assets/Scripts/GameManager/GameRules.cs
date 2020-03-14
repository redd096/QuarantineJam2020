﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    [Serializable]
    public class SpawnRule
    {
        public ShoppingItem item;
        public float minSpawnDelay;
        public float maxSpawnDelay;
    }

    /// <summary>
    /// The rules of a level.
    /// </summary>
    [CreateAssetMenu(fileName = "GameRules", menuName = "Assets/QuarantineShopping/GameRules")]
    public class GameRules : ScriptableObject
    {
        [SerializeField, Min(0f)]
        private float preparationTime = 5f;
        /// <summary>
        /// The countdown before the actual timer starts.
        /// </summary>
        public float PreparationTime { get { return preparationTime; } }

        [SerializeField, Min(0f)]
        private float gameTime = 90f;
        
        /// <summary>
        /// The duration of the game session.
        /// </summary>
        public float GameTime { get { return gameTime; } }

        [SerializeField]
        private SpawnRule[] itemsInShoppingList;
        /// <summary>
        /// The items that can be spawned during the game.
        /// </summary>
        public SpawnRule[] ItemsInShoppingList { get { return itemsInShoppingList; } }

        [Header("Other items"), SerializeField]
        private ShoppingItem[] otherItems;
        /// <summary>
        /// Items spawned but not in the shopping list.
        /// </summary>
        public ShoppingItem[] OtherItems { get { return otherItems; } }

        public float generalMinDelay = 0.1f;
        public float generalMaxDelay = 1.0f;
    }
}
