using UnityEngine;
using UnityEngine.UI;


public class LevelTimer : MonoBehaviour
{
    [Tooltip("Level time in seconds")]
    [SerializeField] float baseTime = 10f;
    [SerializeField] float preparationTime = 30f;
    [SerializeField] Text countdownTimerText;

    bool triggeredLevelFinish;
    bool levelStarted;

    // cached references
    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
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

    private void UpdateLevelTimer()
    {
        slider.value = (Time.timeSinceLevelLoad - preparationTime) / baseTime;
        bool timerFinished = (Time.timeSinceLevelLoad >= baseTime);
        if (timerFinished)
        {
            FindObjectOfType<LevelController>().LevelTimerFinished();
            triggeredLevelFinish = true;
            GetComponent<Animator>().enabled = false;
        }
    }

    private void CountdownToStart()
    {
        preparationTime -= 1 * Time.deltaTime;
        countdownTimerText.text = preparationTime.ToString("00");
        if (preparationTime <= 0)
        {
            levelStarted = true;
            //GetComponentInChildren<Canvas>().enabled = false;
            //var enemySpawners = FindObjectsOfType<AttackerSpawner>();
            //foreach(AttackerSpawner spawner in enemySpawners)
            //{
            //    spawner.StartSpawning();
            //}
        }
    }
}
