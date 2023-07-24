using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public abstract class BaseTimelineManager : MonoBehaviour
{
    protected bool fix = false;
    [SerializeField] public PlayableDirector director;
    [SerializeField] protected List<PlayableAsset> myCutscenes;
    protected PlayerHealth playerHealth;
    [SerializeField] protected SimpleBossManager bossManager;
    // Start is called before the first frame update

    public UnityEvent introCutsceneOver;
    public UnityEvent bossCutsceneOver;

    public abstract void EndBossCutscene();
    public abstract void SetBossState(bool pState);

    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    public void StartCutsceneByIndex(int index)
    {
        Debug.Log("DEEM Start cutscene");
        if (myCutscenes.Count > 0)
        {
            StartCutscene(myCutscenes[index]);
        }
    }

    public void StartCutscene(PlayableAsset pCutscene)
    {
        if (playerHealth != null)
        {
            playerHealth.SetIsInvulnerable(true);
        }
        director.playableAsset = pCutscene;
        director.Play();
    }

    public void EndIntroCutscene()
    {
        Debug.Log("Ending intro cutscene");
        if (playerHealth != null && playerHealth.isInvulerable())
        {
            Debug.Log("Sending alert");
            Debug.Log("Player is no longer invulnerable");
            playerHealth.SetIsInvulnerable(false);
            //GameEvents.instance.SendEnemyAlert();
        }
        introCutsceneOver.Invoke();
    }

    public void StartLevel()
    {
        if (playerHealth != null && playerHealth.isInvulerable())
        {
            Debug.Log("Player is no longer invulnerable");
            playerHealth.SetIsInvulnerable(false);
        }
        //GameEvents.instance.SendEnemyAlert();
        //Destroy(gameObject);
    }
}

