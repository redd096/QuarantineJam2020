using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    [Serializable]
    public class SpawnRule
    {
        public ShoppingItem item;
        public float previewIconAnticipationTime = 2f;
        public float minSpawnDelay;
        public float maxSpawnDelay;
    }

    /// <summary>
    /// The rules of a level.
    /// </summary>
    [CreateAssetMenu(fileName = "GameRules", menuName = "QuarantineShopping/GameRules")]
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


        [Header("List of shopping items:"), SerializeField]
        private List<SpawnRule> shoppingList = new List<SpawnRule>();

        [Header("List of MODIFIER"), SerializeField]
        private ShoppingItem[] modifierList;

        private SpawnRule[] itemsInShoppingList;
        /// <summary>
        /// The items that can be spawned during the game.
        /// </summary>
        public SpawnRule[] ItemsInShoppingList { get { return itemsInShoppingList; } }

        private ShoppingItem[] otherItems;
        /// <summary>
        /// Items spawned but not in the shopping list.
        /// </summary>
        public ShoppingItem[] OtherItems { get { return otherItems; } }

        [Header("Minimum time between two spawns")]
        public float generalMinDelay = 0.1f;

        [Header("Duration of the preview icon")]
        public float previewIconAnticipationTime = 2f;

        [Header("Maximum time between two spawns")]
        public float generalMaxDelay = 1.0f;

        public List<ShoppingItem> GetShoppingList()
        {
            List<ShoppingItem> shoppingList = new List<ShoppingItem>();
            foreach (SpawnRule spawnRule in itemsInShoppingList)
            {
                shoppingList.Add(spawnRule.item);
            }

            return shoppingList;
        }

        public void SetLists()
        {
            //copy, so we don't remove nothing from original list
            List<SpawnRule> temp_shoppingList = CopyList(shoppingList);

            //itemsInShoppingList prende 4 random da shoppingList
            {
                //normally 4, but if shoppingList is lesser then 4, then use shoppingList length
                int length = Mathf.Min(4, temp_shoppingList.Count);
                itemsInShoppingList = new SpawnRule[length];

                for (int i = 0; i < length; i++)
                {
                    int random = UnityEngine.Random.Range(0, temp_shoppingList.Count);

                    //add in itemsInShoppingList an object random from shoppingList
                    itemsInShoppingList[i] = temp_shoppingList[random];

                    //remove from shoppingList (so we can't add it anymore)
                    temp_shoppingList.RemoveAt(random);
                }
            }

            //otherItems prende quelli che mancano da shoppingList (prendendo solo item, perché serve uno ShoppingItem e non GameRule)
            //poi prende tutti quelli in modifierList
            {
                //remained shoppingList + modifierList
                int length = temp_shoppingList.Count + modifierList.Length;
                otherItems = new ShoppingItem[length];

                //every item in shoppingList
                for (int i = 0; i < temp_shoppingList.Count; i++)
                {
                    otherItems[i] = temp_shoppingList[i].item;
                }

                //every modifier
                for(int i = 0; i < modifierList.Length; i++)
                {
                    otherItems[temp_shoppingList.Count + i] = modifierList[i];
                }
            }
        }

        List<T> CopyList<T>(List<T> listToCopy)
        {
            List<T> temp = new List<T>();

            for(int i = 0; i < listToCopy.Count; i++)
            {
                temp.Add(listToCopy[i]);
            }

            return temp;
        }
    }
}

