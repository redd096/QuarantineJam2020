using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Quaranteam
{
    [RequireComponent(typeof(AudioSource))]
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
        /// <summary>
        /// The timer of the game.
        /// </summary>
        public LevelTimer LevelTimer { get { return levelTimer; } }

        [SerializeField]
        private Player player;
        /// <summary>
        /// The player character.
        /// </summary>
        public Player Player { get { return player; } }


        [SerializeField]
        protected internal GameObject previewIconPrefab;

        [Header("Overlays")]
        public GameObject overlay;

        /// <summary>
        /// The instantiated spawners.
        /// </summary>
        private List<GameObject> spawners = new List<GameObject>();

        /// <summary>
        /// The UI manager for the shopping list UI
        /// </summary>
        private ShoppingListUI shoppingListUI;

        /// <summary>
        /// The overlay displayed at the beginning of the game.
        /// </summary>
        [SerializeField]
        private GameObject waitForEnterButtonOverlay;

        /// <summary>
        /// The time scale applied to all the falling objects.
        /// </summary>
        protected internal float fallingItemsTimeScale = 1.0f;

        /// <summary>
        /// Delegate for methods that are called whenever the score is updated.
        /// </summary>
        /// <param name="newScore"></param>
        public delegate void ScoreUpdateDelegate(int newScore);

        public event ScoreUpdateDelegate onCurrentScoreUpdate;

        private ModifiersPanelUI modifiersPanel;

        public delegate void ModifierRemovedDelegate(ModifiersId id);
        public event ModifierRemovedDelegate onModifierRemoved;


        public delegate void ModifierUpdatedDelegate(ModifiersId id, string newDescription, float newTimer);
        public event ModifierUpdatedDelegate onModifierUpdated;

        protected Dictionary<ModifiersId, ModifierRule> appliedModifiers
            = new Dictionary<ModifiersId, ModifierRule>();

        /// <summary>
        /// The source for music.
        /// </summary>
        private AudioSource audioSource;

        [Header("Audio")]
        [SerializeField] private AudioClip youWinAudioClip;
        [SerializeField] private AudioClip youLoseAudioClip;
        [SerializeField] private AudioClip bgMusic;

        /// <summary>
        /// The player's score.
        /// </summary>
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

        private float currentMultiplier = 1;
        public float CurrentMultiplier
        {
            get { return currentMultiplier; }
            set
            {
                currentMultiplier = value;
                if (currentMultiplier < 0)
                    currentMultiplier = 0;
            }
        }

        #region pause variables

        private GameObject pauseMenu;
        private Button resumeButton;
        private Button exitButton;
        float resumeTime;

        #endregion

        private void Awake()
        {

            // levelTimer = FindObjectOfType<LevelTimer>();   
            shoppingListUI = FindObjectOfType<ShoppingListUI>();
            audioSource = GetComponent<AudioSource>();
            modifiersPanel = FindObjectOfType<ModifiersPanelUI>();

            SetPauseMenu();

            appliedGameRules.SetLists();
        }

        private void Start()
        {
            Debug.Log("Press Enter to start the game");

            StartCoroutine(WaitForEnterButtonAndStartGame());
        }

        private IEnumerator WaitForEnterButtonAndStartGame()
        {
            bool touchInput = false;
            

            while(!(Input.GetKeyDown(KeyCode.Return) || touchInput || Input.GetKeyDown(KeyCode.Escape)))
            {
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                    touchInput = true;
                yield return null;
            }
            //yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Return) && !touchInput);

            levelTimer.SetGameRules(appliedGameRules);
            levelTimer.gameObject.SetActive(true);
            waitForEnterButtonOverlay.gameObject.SetActive(false);

            audioSource.loop = true;
            audioSource.clip = bgMusic;
            audioSource.Play();
        }

        private IEnumerator WaitForEnterButtonAndRestartGame()
        {
            bool touchInput = false;

            while (!(Input.GetKeyDown(KeyCode.Return) || touchInput || Input.GetKeyDown(KeyCode.Escape)))
            {
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                    touchInput = true;
                yield return null;
            }
            //yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Return) && !touchInput);

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
                spawnerComp.previewIconPrefab = previewIconPrefab;
                spawnerComp.previewIconTime = itemInList.previewIconAnticipationTime;
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
            generalSpawnerComp.previewIconPrefab = previewIconPrefab;
            generalSpawnerComp.previewIconTime = appliedGameRules.previewIconAnticipationTime;
        }

        public void OnTimerEnd(bool causeTiltedCart)
        {
            // @todo check shoppint chart
            bool win = FindObjectOfType<Cart>().IsChecklistComplete();
            player.gameObject.SetActive(false);

            audioSource.Stop();
            audioSource.loop = false;
            levelTimer.gameObject.SetActive(false);
            overlay.SetActive(true);
            if (win)
            {
                overlay.GetComponentInChildren<Text>().text = "You win!" + "\n" + "Score: " + CurrentScore;
                audioSource.clip = youWinAudioClip;
                audioSource.Play();
            }
            else
            {
                string loseText = causeTiltedCart ? "You tilted the cart" : "You didn't finish the spesa";
                overlay.GetComponentInChildren<Text>().text = "You lose!" + "\n" + loseText;
                audioSource.clip = youLoseAudioClip;
                audioSource.Play();
            }

            foreach (var spawner in spawners)
            {
                Destroy(spawner);
            }

            // restart game loop
#if !UNITY_ANDROID
            StartCoroutine(WaitForEnterButtonAndRestartGame());
#endif
            
        }

        protected internal void OnItemCollected(ShoppingItem item)
        {
            CurrentScore += (int)(item.BaseReward * CurrentMultiplier);
        }

        private void Update()
        {
            List<ModifierRule> toRemove = new List<ModifierRule>();
            foreach (var modifier in appliedModifiers.Values)
            {
                modifier.RemainingTime -= Time.deltaTime;
                onModifierUpdated?.Invoke(modifier.Id, modifier.Description, modifier.RemainingTime);

                if (modifier.RemainingTime <= 0f)
                {
                    toRemove.Add(modifier);
                }
            }

            foreach (ModifierRule modifier in toRemove)
            {
                RemoveModifier(modifier);
            }
        }

        public void AddModifier(ModifierRule newModifier)
        {
            appliedModifiers[newModifier.Id] = newModifier;
            modifiersPanel.AddModifier(newModifier);
        }

        public void RemoveModifier(ModifierRule modifier)
        {
            modifier.RevertRule(this);
            onModifierRemoved?.Invoke(modifier.Id);
            appliedModifiers.Remove(modifier.Id);
        }

        public void changeMusicSpeed(float trackSpeed)
        {
            if (trackSpeed > 1.5f || trackSpeed < 1f)
            {
                return;
            }

            audioSource.pitch = trackSpeed;
        }

        public void RestartButton()
        {
            SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);
        }

#region pause menu

        void SetPauseMenu()
        {
            //find references
            pauseMenu = GameObject.Find("PauseMenu");
            resumeButton = pauseMenu.transform.Find("ResumeButton").GetComponent<Button>();
            exitButton = pauseMenu.transform.Find("ExitButton").GetComponent<Button>();

            //set buttons function
            resumeButton.onClick.AddListener(Resume);
            exitButton.onClick.AddListener(Exit);

            //set false pauseMenu
            pauseMenu.SetActive(false);
        }

        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = resumeTime;
        }

        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        public void PauseGame()
        {
            bool isPaused = Time.timeScale == 0;

            if (isPaused)
                Resume();
            else
            {
                resumeTime = Time.timeScale;
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
        }

#endregion
    }
}

