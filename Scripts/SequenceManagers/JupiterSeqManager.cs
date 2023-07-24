using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JupiterSeqManager : MonoBehaviour
{
    LevelManager mLevelManager;
    PlayerHealth mPlayerHealth;
    [SerializeField] public EnemyAdvancedSpawner myEnemySpawner;
    [SerializeField] List<WaveConfig> myWaves = null;
    [SerializeField] List<WaveConfig> phaseTwoWave = null;
    [SerializeField] List<WaveConfig> phaseThreeWave = null;
    [SerializeField] public int numberOfWaves = 1;
    JupiterTimelineManager mTimeManager;

    [SerializeField] public GameObject Boss = null;
    JupiterBossManager myBossManager = null;

    [SerializeField] AudioClip levelMusic;
    AudioPlayer myAudioPlayer;

    int numOfPhases = 0;

    private void Awake()
    {
        myAudioPlayer = FindObjectOfType<AudioPlayer>();
        mPlayerHealth = FindObjectOfType<PlayerHealth>();
        mLevelManager = FindObjectOfType<LevelManager>().GetInstance();
        mTimeManager = FindObjectOfType<JupiterTimelineManager>();
        Boss.SetActive(false);
        myBossManager = Boss.GetComponent<JupiterBossManager>();
    }

    private void Start()
    {
        //Start intro cutscene, should be first in sequence
        Debug.Log("DEEM Starting cutscene");
        mTimeManager.StartCutsceneByIndex(0);
        myBossManager.DisableAllWeapons();

        if (levelMusic != null)
        {
            myAudioPlayer.StopTheMusic();
            myAudioPlayer.SetAudioSource(levelMusic);
            myAudioPlayer.StartTheMusic();
        }
    }

    void BeginSpawing()
    {
        Debug.Log("DEEM Begin spawning enemies");
        if (myWaves.Count > 0)
        {
            StartCoroutine(PhaseInEnemySpawner(myWaves, numberOfWaves));
        }
    }    

    public void StartUpEnemySpawning()
    {
        if (myWaves.Count > 0)
        {
            numOfPhases = 0;
            StartCoroutine(PhaseInEnemySpawner(myWaves, 1));
        }
        else
        {
            SpawnInBoss();
        }
    }

    IEnumerator PhaseInEnemySpawner(List<WaveConfig> pWaves, int pNumWaves)
    {
        myEnemySpawner.SetUpWave(pWaves, pNumWaves);
        yield return new WaitForSeconds(3f);
        myEnemySpawner.BeginWave(pNumWaves);
        numOfPhases++;
    }

    public void SpawnInBoss()
    {
        myBossManager.SetIsInvulnerable(true);
        Boss.SetActive(true);
        mTimeManager.StartCutsceneByIndex(1);
    }

    public void SetBossActive()
    {
        Boss.SetActive(true);
        myBossManager.SetIsInvulnerable(false);
        myBossManager.EnableAllWeapons();
    }

    public void UnityEventOver()
    {
        Debug.Log("UnityEvent is over");
        if (numOfPhases == 1 && phaseTwoWave.Count > 0)
        {
            Debug.Log("Begin wave phase 2");
            StartCoroutine(PhaseInEnemySpawner(phaseTwoWave, 1));
        }
        else
        {
            myEnemySpawner.enabled = false;
            SpawnInBoss();
        }
        //Load in next phase
    }
}
