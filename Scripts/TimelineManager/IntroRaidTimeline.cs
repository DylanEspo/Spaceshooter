using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroRaidTimeline : MonoBehaviour
{
    private bool fix = false;
    public PlayableDirector director;
    PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.SetIsInvulnerable(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TO DO - Delete this or have a call to destroy this object
        if (director.state != PlayState.Playing && !fix)
        {
            //Debug.Log("Begin");
            if (playerHealth != null && playerHealth.isInvulerable())
            {
                Debug.Log("Sending alert");
                StartLevel();
            }

        }
    }

    public void StartLevel()
    {
        playerHealth.SetIsInvulnerable(false);
        GameEvents.instance.SendEnemyAlert();
        playerHealth = null;
        //Destroy(gameObject);
    }
}
