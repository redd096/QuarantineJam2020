using System.Collections;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] GameObject winLabel, loseLabel;
    [SerializeField] float delay4WinCondition;

    //cached reference
    int numberOfAttackers = 0;
    bool levelTimerFinished = false;

    private void Start()
    {
        winLabel.SetActive(false);
        loseLabel.SetActive(false);
    }

    public void AttackerSpawned()
    {
        numberOfAttackers++;
    }

    public void AttackerKilled()
    {
        numberOfAttackers--;
        Debug.Log("Attacker killed: " + numberOfAttackers + " enemies remaining");
        // if no enemies still alive and timer has reached zero, end the level
        if (numberOfAttackers <= 0 && levelTimerFinished)
        {
            Debug.Log("Level can end");
            StartCoroutine(HandleWinCondition());
        }
    }


    public void HandleLoseCondition()
    {
        loseLabel.SetActive(true);
        Time.timeScale = 0f;
    }

    private IEnumerator HandleWinCondition()
    {
        GetComponent<AudioSource>().Play();
        winLabel.SetActive(true);
        yield return new WaitForSeconds(delay4WinCondition);
        GetComponent<ScreenLoader>().LoadNextLevel();
    }

    public void StopSpawners()
    {
        var attackerSpawnerArray = FindObjectsOfType<AttackerSpawner>();
        foreach (AttackerSpawner attackerSpawner in attackerSpawnerArray)
        {
            attackerSpawner.StopSpawning();
        }
    }

    public void LevelTimerFinished()
    {
        StopSpawners();
        levelTimerFinished = true;
    }

    public void Restart()
    {
        levelTimerFinished = false;
    }
}
