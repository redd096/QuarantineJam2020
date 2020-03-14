using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    public class Cart : MonoBehaviour
    {

        [Tooltip("Required Items (Serialized for debugging purposes only)")]
        [SerializeField] SpawnRule[] requiredItems;

        Dictionary<ShoppingItem, bool> checklist = new Dictionary<ShoppingItem, bool>();

        // Start is called before the first frame update
        void Start()
        {
            foreach (SpawnRule spawn in requiredItems)
            {
                Debug.Log("required Item: " + spawn.item.DisplayName);
                if(!checklist.ContainsKey(spawn.item))
                {
                    checklist.Add(spawn.item, false);
                }
            }
        }

        internal void SetRequiredItems(GameRules appliedGameRules)
        {
            requiredItems = appliedGameRules.ItemsInShoppingList;
        }


        // Update is called once per frame
        void Update()
        {

        }

        public bool IsChecklistComplete()
        {
            return !checklist.ContainsValue(false);

        }

        internal void ItemObtained(ShoppingItem item)
        {
            Debug.Log("Collected");
            if (checklist.ContainsKey(item))
            {
                checklist[item] = true;
            }
        }
    }

}