using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerSpawner : MonoBehaviour
{

    //[SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] float minSpawnDelay = 1f;
    [SerializeField] float maxSpawnDelay = 5f;
    [SerializeField] bool spawn = false;
    //[SerializeField] Item[] attackerPrefabArray;

    //cached reference
    LevelTimer levelTimer;

    // Start is called before the first frame update
    //void Start()
    IEnumerator Start()
    {
        while (spawn)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            //yield return StartCoroutine(SpawnAllWaves());
            SpawnAttacker();
        }
    }

    private void SpawnAttacker()
    {
        //var attackerIdx = Random.Range(0, attackerPrefabArray.Length);
        //Spawn(attackerPrefabArray[attackerIdx]);
    }

    public void StopSpawning()
    {
        spawn = false;
    }

    public void StartSpawning()
    {
        spawn = true;
        StartCoroutine(Start());
    }

    //private void Spawn(Attacker nextAttacker)
    //{
    //    var newEnemy = Instantiate(nextAttacker, transform.position, transform.rotation);
    //    newEnemy.transform.parent = transform;
    //}


}
