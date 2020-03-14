﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        [SerializeField]
        private LevelTimer levelTimer;

        public GameObject overlay;

        /// <summary>
        /// The instantiated spawners.
        /// </summary>
        private List<GameObject> spawners = new List<GameObject>();

        /// <summary>
        /// The UI manager for the shopping list UI
        /// </summary>
        private ShoppingListUI shoppingListUI;

        [Header("Overlays")]
        /// <summary>
        /// The overlay displayed at the beginning of the game.
        /// </summary>
        [SerializeField]
        private GameObject waitForEnterButtonOverlay;

        /// <summary>
        /// Delegate for methods that are called whenever the score is updated.
        /// </summary>
        /// <param name="newScore"></param>
        public delegate void ScoreUpdateDelegate(int newScore);

        public event ScoreUpdateDelegate onCurrentScoreUpdate;

        private int currentScore = 0;
        public int CurrentScore
        {
            get { return currentScore; }
            set
            {
                currentScore = value;
                onCurrentScoreUpdate?.Invoke(currentScore);
            }
        }

        private void Awake()
        {

            // levelTimer = FindObjectOfType<LevelTimer>();   
            shoppingListUI = FindObjectOfType<ShoppingListUI>();
        }

        private void Start()
        {
            Debug.Log("Press Enter to start the game");
            
            StartCoroutine(WaitForEnterButtonAndStartGame());
        }

        private IEnumerator WaitForEnterButtonAndStartGame()
        {
            yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Return));

            levelTimer.SetGameRules(appliedGameRules);
            levelTimer.gameObject.SetActive(true);
            waitForEnterButtonOverlay.gameObject.SetActive(false);
        }

        private IEnumerator WaitForEnterButtonAndRestartGame()
        {
            yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Return));

            SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);
        }

        public void OnGameStarted()
        {
            overlay.SetActive(false);

            foreach (SpawnRule itemInList in appliedGameRules.ItemsInShoppingList)
            {
                GameObject spawner = Instantiate(itemSpawnerPrefab, itemSpawnerLocation.position, Quaternion.identity);
                ShoppingItemSpawner spawnerComp = spawner.GetComponent<ShoppingItemSpawner>();
                spawnerComp.itemsToSpawn.Add(itemInList.item);
                spawnerComp.minDelay = itemInList.minSpawnDelay;
                spawnerComp.maxDelay = itemInList.maxSpawnDelay;
                spawners.Add(spawner);
                shoppingListUI.AddRequiredShoppingItem(itemInList.item);
            }

            // General items spawner
            GameObject generalSpawner = Instantiate(itemSpawnerPrefab, itemSpawnerLocation.position, Quaternion.identity);
            ShoppingItemSpawner generalSpawnerComp = generalSpawner.GetComponent<ShoppingItemSpawner>();
            generalSpawnerComp.minDelay = appliedGameRules.generalMinDelay;
            generalSpawnerComp.maxDelay = appliedGameRules.generalMaxDelay;
            spawners.Add(generalSpawner);
            foreach (ShoppingItem generalItem in appliedGameRules.OtherItems)
            {
                generalSpawnerComp.itemsToSpawn.Add(generalItem);
            }
        }

        public void OnTimerEnd()
        {
            // @todo check shoppint chart
            bool win = FindObjectOfType<Cart>().IsChecklistComplete();

            levelTimer.gameObject.SetActive(false);
            overlay.SetActive(true);
            if (win)
            {
                overlay.GetComponentInChildren<Text>().text = "You win!";
            }
            else
            {
                overlay.GetComponentInChildren<Text>().text = "You lose!";
            }

            foreach (var spawner in spawners)
            {
                Destroy(spawner);
            }

            // restart game loop
            StartCoroutine(WaitForEnterButtonAndRestartGame());
        }

        protected internal void OnItemCollected(ShoppingItem item)
        {
            CurrentScore += item.BaseReward;
        }

        private void Update()
        {
            // Brutally exit the game.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}

