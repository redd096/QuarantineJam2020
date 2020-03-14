using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Quaranteam
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Fill the rules of this level.
        /// </summary>
        public GameRules appliedGameRules;

        /// <summary>
        /// The prefab of the item spawner.
        /// </summary>
        public GameObject itemSpawnerPrefab;

        /// <summary>
        /// The spawn location of the item spawner.
        /// </summary>
        [SerializeField]
        private Transform itemSpawnerLocation;

        private LevelTimer levelTimer;

        private void Awake()
        {       
            
            levelTimer = FindObjectOfType<LevelTimer>();   
        }

        private void Start()
        {
            Debug.Log("Press Enter to start the game");

            StartCoroutine(WaitForEnterButton());
        }

        private IEnumerator WaitForEnterButton()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    levelTimer.SetGameRules(appliedGameRules);
                    levelTimer.gameObject.SetActive(true);
                    yield break;
                }

                yield return null;
            }
        }

        public void OnGameStarted()
        {
            foreach (SpawnRule itemInList in appliedGameRules.ItemsInShoppingList)
            {
                GameObject spawner = Instantiate(itemSpawnerPrefab, itemSpawnerLocation.position, Quaternion.identity);
                ShoppingItemSpawner spawnerComp = spawner.GetComponent<ShoppingItemSpawner>();
                spawnerComp.itemsToSpawn.Add(itemInList.item);
                spawnerComp.minDelay = itemInList.minSpawnDelay;
                spawnerComp.maxDelay = itemInList.maxSpawnDelay;

            }

            // General items spawner
            GameObject generalSpawner = Instantiate(itemSpawnerPrefab, itemSpawnerLocation.position, Quaternion.identity);
            ShoppingItemSpawner generalSpawnerComp = generalSpawner.GetComponent<ShoppingItemSpawner>();
            generalSpawnerComp.minDelay = appliedGameRules.generalMinDelay;
            generalSpawnerComp.maxDelay = appliedGameRules.generalMaxDelay;
            foreach (ShoppingItem generalItem in appliedGameRules.OtherItems)
            {
                generalSpawnerComp.itemsToSpawn.Add(generalItem);
            }
        }
    }
}

