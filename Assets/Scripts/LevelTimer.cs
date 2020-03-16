using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

namespace Quaranteam
{
    public class LevelTimer : MonoBehaviour
    {
        [HideInInspector] public bool endGame;

        [Tooltip("Game rules")]
        [SerializeField] GameRules gameRules;

        [Header("Player Prefab")]
        [SerializeField] GameObject player;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI countdownTimer = default;
        [SerializeField] private TextMeshProUGUI levelTimer = default;
        [SerializeField] private TextMeshProUGUI bonusTimer = default;
        [SerializeField] private GameObject preparationTimePanel = default;
        [Min(0f)]
        [SerializeField] private float whenToChangeMusicSpeed = default;

        [Header("Debugging Purposes only")]
        [SerializeField] float baseTime = 10f;
        [SerializeField] float preparationTime = 30f;

        private bool triggeredLevelFinish;
        private bool levelStarted;
        private float pTime;

        // cached references
        //private Slider slider;
        private GameManager gameManager;

        protected internal float elapsedTime = 0f;

        private void OnEnable()
        {
            //slider = GetComponentInChildren<Slider>();
            //slider.gameObject.SetActive(false);

            if (gameRules != null)
            {
                preparationTime = gameRules.PreparationTime;
                baseTime = gameRules.GameTime;
            }
            pTime = preparationTime;
            gameManager = FindObjectOfType<GameManager>();
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
            //slider.value = elapsedTime / baseTime;

            TimeSpan time = TimeSpan.FromSeconds(baseTime - elapsedTime);
            levelTimer.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
            if ((baseTime - elapsedTime) > whenToChangeMusicSpeed)
            {
                gameManager.changeMusicSpeed(1f);
            }
            else
            {
                gameManager.changeMusicSpeed(1.2f);
            }
            bool timerFinished = (elapsedTime >= baseTime);
            if (timerFinished || endGame)
            {
                // A chi comunico che ho finito?
                triggeredLevelFinish = true;
                gameManager.OnTimerEnd(endGame);
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
                //slider.gameObject.SetActive(true);

                // A chi comunico che ho iniziato?
                gameManager.OnGameStarted();

                player.SetActive(true);
                player.GetComponentInChildren<Cart>().SetRequiredItems(gameRules);

                // Fai partire lo spawn degli oggetti


                //GetComponentInChildren<Canvas>().enabled = false;
                //var enemySpawners = FindObjectsOfType<AttackerSpawner>();
                //foreach(AttackerSpawner spawner in enemySpawners)
                //{
                //    spawner.StartSpawning();
                //}

            }
        }

        public void ApplyTimeModifier(float timeModifier)
        {
            elapsedTime += timeModifier;

            StartCoroutine(ShowTimeBonus(-timeModifier));
        }

        private IEnumerator ShowTimeBonus(float timeModifier)
        {
            if (timeModifier > 0)
            {
                bonusTimer.text = "+" + timeModifier.ToString();
                bonusTimer.color = Color.green;
            }
            else
            {
                bonusTimer.text = timeModifier.ToString();
                bonusTimer.color = Color.red;
            }
            bonusTimer.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            bonusTimer.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            levelStarted = false;
            preparationTimePanel.SetActive(true);
            //slider.gameObject.SetActive(false);
            preparationTime = pTime;
            elapsedTime = 0f;
            triggeredLevelFinish = false;
        }
    }

}