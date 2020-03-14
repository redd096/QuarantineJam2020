using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
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

        //[SerializeField, Min(0)]
        //private int numItemsInShoppingList = 5;

        ///// <summary>
        ///// The number of items picked from DB for the shopping list.
        ///// </summary>
        //public int NumItemsInShoppingList { get { return numItemsInShoppingList; } }

        //[SerializeField, Min(1)]
        //private int numSpawnsForEachItemInShoppingList = 3;

        ///// <summary>
        ///// The minimum number of spawns for each item in the shopping list.
        ///// </summary>
        //public int NumSpawnsForEachItemInShoppingList { get { return numSpawnsForEachItemInShoppingList; } }

        //[SerializeField, Min(1f)]
        //private float baseTimeBetweenItemSpawn = 5f;

        ///// <summary>
        ///// The base amount of time between each item spawn.
        ///// </summary>
        //public float BaseTimeBetweenItemSpawn { get { return baseTimeBetweenItemSpawn; } }

        [SerializeField]
        private ShoppingItem[] availableItems;
        /// <summary>
        /// The items that can be spawned during the game.
        /// </summary>
        public ShoppingItem[] AvailableItems { get { return availableItems; } }
    }
}

