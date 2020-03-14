using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Quaranteam
{
    public class LevelTimer : MonoBehaviour
    {
        [Tooltip("Game rules")]
        [SerializeField] GameRules gameRules;

        [Header("Player Prefab")]
        [SerializeField] GameObject player;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI countdownTimer = default;
        [SerializeField] private TextMeshProUGUI levelTimer = default;
        [SerializeField] private GameObject preparationTimePanel = default;

        [Header("Debugging Purposes only")]
        [SerializeField] float baseTime = 10f;
        [SerializeField] float preparationTime = 30f;

        private bool triggeredLevelFinish;
        private bool levelStarted;
        private float pTime;

        // cached references
        private Slider slider;
        

        private float elapsedTime = 0f;

        private void OnEnable()
        {
            slider = GetComponentInChildren<Slider>();
            slider.gameObject.SetActive(false);

            if(gameRules != null)
            {
                preparationTime = gameRules.PreparationTime;
                baseTime = gameRules.GameTime;
            }
            pTime = preparationTime;
        }


        // Update is called once per frame
        void Update()
        {
            if (triggeredLevelFinish) { return; }
            if (!levelStarted)
            {
                CountdownToStart();
            }
            else
            {
                UpdateLevelTimer();
            }
        }

        internal void SetGameRules(GameRules appliedGameRules)
        {
            gameRules = appliedGameRules; 
        }

        private void UpdateLevelTimer()
        {
            elapsedTime += 1 * Time.deltaTime;
            slider.value = elapsedTime / baseTime;

            TimeSpan time = TimeSpan.FromSeconds(baseTime - elapsedTime);
            levelTimer.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
            bool timerFinished = (elapsedTime >= baseTime);
            if (timerFinished)
            {
                // A chi comunico che ho finito?
                triggeredLevelFinish = true;
            }

        }

        private void CountdownToStart()
        {
            preparationTime -= 1 * Time.deltaTime;
            countdownTimer.text = preparationTime.ToString("00");
            if (preparationTime <= 0)
            {
                levelStarted = true;
                preparationTimePanel.SetActive(false);
                slider.gameObject.SetActive(true);

                // A chi comunico che ho iniziato?
                FindObjectOfType<GameManager>().OnGameStarted();
                player.SetActive(true);

                // Fai partire lo spawn degli oggetti


                //GetComponentInChildren<Canvas>().enabled = false;
                //var enemySpawners = FindObjectsOfType<AttackerSpawner>();
                //foreach(AttackerSpawner spawner in enemySpawners)
                //{
                //    spawner.StartSpawning();
                //}

            }
        }


        private void OnDisable()
        {
            levelStarted = false;
            preparationTimePanel.SetActive(true);
            slider.gameObject.SetActive(false);
            preparationTime = pTime;
            elapsedTime = 0f;
            triggeredLevelFinish = false;
        }
    }

}