using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeptuneSeqManager : MonoBehaviour
{
    LevelManager mLevelManager;
    [SerializeField] public EnemyAdvancedSpawner myEnemySpawner;
    [SerializeField] List<WaveConfig> myWaves = null;
    [SerializeField] List<WaveConfig> phaseTwoWave = null;
    [SerializeField] List<WaveConfig> phaseThreeWave = null;
    [SerializeField] public int numberOfWaves = 1;
    [SerializeField] public GameObject Boss = null;

    int numOfPhases = 0;

    private void Awake()
    {
        mLevelManager = FindObjectOfType<LevelManager>().GetInstance();
        Boss.SetActive(false);
    }

    void Start()
    {
        //currentPhase = mWaveOne;
        foreach (WaveConfig wave in myWaves)
        {
            AddWaveConfigs(wave);
        }
        if(myWaves.Count > 0)
        {
            StartCoroutine(PhaseInEnemySpawner(numberOfWaves));
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if(firstPhase != true)
        {
            StartCoroutine(PhaseInEnemySpawner(numberOfWaves));
            firstPhase = true;
            return;
        }*/
        /*
        else if(firstPhase == true && secondPhase != true)
        {
            Debug.Log("Starting second phase");
            StartCoroutine(PhaseInEnemySpawner(2));
            return;
        }
        else if(waveStatus == false && firstPhase && secondPhase && !thirdPhase)
        {
            StartCoroutine(PhaseInEnemySpawner(2));
        }
        if(firstPhase && secondPhase && thirdPhase)
        {

        }*/
    }

    void StartSequence()
    {
        numOfPhases = 0;
        StartCoroutine(PhaseInEnemySpawner(numberOfWaves));
    }

    public void AddWaveConfigs(WaveConfig pWaveConfig)
    {
        myEnemySpawner.AddWaveConfigs(pWaveConfig);
    }

    public void ClearWaveConfigs()
    {
        myEnemySpawner.ClearWaveConfigs();
    }

    public void ModifyEnemySpawner(List<WaveConfig> pWaveConfigs)
    {

    }

    public void UnityEventOver()
    {
        Debug.Log("UnityEvent is over");
        if (numOfPhases == 1)
        {
            Debug.Log("Begin wave phase 2");
            ClearWaveConfigs();
            foreach (WaveConfig waveConfig in phaseTwoWave)
            {
                AddWaveConfigs(waveConfig);
            }
            StartCoroutine(PhaseInEnemySpawner(2));
        }
        else if (numOfPhases == 2)
        {
            Debug.Log("Begin wave phase 3");
            ClearWaveConfigs();
            foreach (WaveConfig waveConfig in phaseThreeWave)
            {
                AddWaveConfigs(waveConfig);
            }
            StartCoroutine(PhaseInEnemySpawner(2));
        }
        else if (numOfPhases == 3)
        {
            Debug.Log("Begin boss");
            Boss.SetActive(true);
        }
        //Load in next phase
    }



    public void StartUpEnemySpawner(int pNumWaves)
    {
        //Debug.Log("Start up enemey spawnwer");
        //myEnemySpawner.StartUpEnemySpawner(3); 
        myEnemySpawner.BeginWave(pNumWaves);
    }

    IEnumerator PhaseInEnemySpawner(int pNumWaves)
    {
        yield return new WaitForSeconds(3f);
        StartUpEnemySpawner(pNumWaves);
        numOfPhases++;
    }

    public void SpawnInBoss()
    {

    }
}
