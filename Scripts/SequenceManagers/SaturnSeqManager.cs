using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaturnSeqManager : MonoBehaviour
{
    LevelManager mLevelManager;
    PlayerHealth mPlayerHealth;
    SaturnTimelineManager mTimeManager;
    [SerializeField] public EnemyAdvancedSpawner myEnemySpawner;
    [SerializeField] List<WaveConfig> myWaves = null;
    [SerializeField] List<WaveConfig> phaseTwoWave = null;
    [SerializeField] public int numberOfWaves = 1;
    [SerializeField] public GameObject Boss = null;
    [SerializeField] AudioClip levelMusic;
    AudioPlayer myAudioPlayer;
    SaturnBossManager myBossManager = null;

    int numOfPhases = 0;

    private void Awake()
    {
        mPlayerHealth = FindObjectOfType<PlayerHealth>();
        mLevelManager = FindObjectOfType<LevelManager>().GetInstance();
        mTimeManager = FindObjectOfType<SaturnTimelineManager>();
        Boss.SetActive(false);
        myBossManager = Boss.GetComponent<SaturnBossManager>();
        myAudioPlayer = FindObjectOfType<AudioPlayer>();
    }

    private void Start()
    {
        //Start intro cutscene, should be first in sequence
        Debug.Log("DEEM Starting cutscene");
        mTimeManager.StartCutsceneByIndex(0);

        if(levelMusic != null)
        {
            myAudioPlayer.StopTheMusic();
            myAudioPlayer.SetAudioSource(levelMusic);
            myAudioPlayer.StartTheMusic();
        }
    }

    public void StartUpEnemySpawning()
    {
        if (myWaves.Count > 0)
        {
            numOfPhases = 0;
            StartCoroutine(PhaseInEnemySpawner(myWaves, numberOfWaves));
        }
        else
        {
            SpawnInBoss();
        }
    }

    IEnumerator PhaseInEnemySpawner(List<WaveConfig> pWaves, int pNumWaves)
    {
        Debug.Log("Phasing in spanwer");
        myEnemySpawner.SetUpWave(pWaves, pNumWaves);
        yield return new WaitForSeconds(3f);
        myEnemySpawner.BeginWave(pNumWaves);
        numOfPhases++;
    }

    public void SpawnInBoss()
    {
        mTimeManager.StartCutsceneByIndex(1);
    }

    public void SetBossActive()
    {
        Boss.SetActive(true);
        myBossManager.StartFiring();
    }

    public void UnityEventOver()
    {
        Debug.Log("UnityEvent is over");
        if (numOfPhases == 1 && phaseTwoWave.Count > 0)
        {
            Debug.Log("Begin wave phase 2");
            StartCoroutine(PhaseInEnemySpawner(phaseTwoWave, numberOfWaves));
        }
        else
        {
            myEnemySpawner.enabled = false;
            mTimeManager.StartCutsceneByIndex(1);
        }
        //Load in next phase
    }
}
