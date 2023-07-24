using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Responsibilities
//Spawn enemy
//Order of the waves
//Time between waves
public class EnemyAdvancedSpawner : MonoBehaviour
{
    [SerializeField] public List<WaveConfig> waveConfigs;
    [SerializeField] public float timeBetweenWaves = 0f;
    [SerializeField] public bool isLooping = true;
    [SerializeField] public int numberOfWaves = 1;
    [SerializeField] public float waveCooldown = 5f;
    public WaveConfig currentWave;
    public UnityEvent endSpawning;

    public void BeginWave(int pNumberOfWaves)
    {
        StartCoroutine(SpawnAdvancedEnemyWaves(pNumberOfWaves));
    }

    public WaveConfig GetCurrentWave()
    {
        return currentWave;
    }

    public void AddWaveConfigs(WaveConfig pWaveConfig)
    {
        waveConfigs.Add(pWaveConfig);
    }

    public void ClearWaveConfigs()
    {
        waveConfigs.Clear();
    }

    public void SetUpWave(List<WaveConfig> pWaveConfigs, int pNumberOfWaves)
    {
        foreach(WaveConfig currWave in pWaveConfigs)
        {
            AddWaveConfigs(currWave);
        }
    }

    IEnumerator SpawnAdvancedEnemyWaves(int pNumberOfWaves)
    {
        int j = 0;
        //Will finish out even if condition changed midway
        Debug.Log("Starting spawning");
        foreach (WaveConfig wave in waveConfigs)
        {
            currentWave = wave;
            for (int i = 0; i < currentWave.GetEnemyCount(); i++)
            {
                Instantiate(currentWave.GetEnemyPrefab(i), currentWave.GetStartingWaypoint().position,
                Quaternion.Euler(0, 0, 180), transform);
                //Loop through one, then give back control to unity and come back based around spawnTime
                yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
            }
            yield return new WaitForSeconds(waveCooldown);
        }
        ClearWaveConfigs();
        yield return new WaitForSeconds(1f);
        endSpawning.Invoke();
        Debug.Log("Deem this is the curernt size " + waveConfigs.Count);
        /*
        do
        {
            //Go through each different wave
            foreach (WaveConfig wave in waveConfigs)
            {
                currentWave = wave;
                for (int i = 0; i < currentWave.GetEnemyCount(); i++)
                {
                    Instantiate(currentWave.GetEnemyPrefab(i), currentWave.GetStartingWaypoint().position,
                    Quaternion.Euler(0, 0, 180), transform);
                    //Loop through one, then give back control to unity and come back based around spawnTime
                    yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
                }
            }
            Debug.Log("Wave # is done " + j);
            yield return new WaitForSeconds(timeBetweenWaves);
            j++;
        } while (j < pNumberOfWaves);*/
    }
}
