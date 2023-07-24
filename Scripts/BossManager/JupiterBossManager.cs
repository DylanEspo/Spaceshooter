using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JupiterBossManager : MonoBehaviour
{

    [SerializeField] BossHealthManager enemyHealth;
    [SerializeField] Transform pathPrefab;

    [SerializeField] List<Shooter> weapons;

    List<Transform> waypoints;

    [SerializeField] float healthLimit = 400f;
    [SerializeField] float stunTime = 2f;
    private bool bossInvulnerable = true;

    Vector3 startingPosition = new Vector3(0, 4, 0);

    private bool isStunned = false;

    //Current place in waypoint list
    int waypointIndex = 0;

    void Start()
    {
        waypoints = GetWaypoints();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth.GetHealth() <= healthLimit)
        {
            FollowPath();
        }
        else
        {
            //Need to move enemy to center
            float distance = Vector2.Distance(transform.position, startingPosition);
            if (distance < -0.1 || distance > 0.1)
            {
                RelocateToStartingPosition();
            }
        }
        if(enemyHealth.GetHealth() < 40)
        {
            DisableAllWeapons();
            //Activate big guns
        }
    }

    private void RelocateToStartingPosition()
    {
        float delta = 2f * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, startingPosition, delta);
    }

    public List<Transform> GetWaypoints()
    {
        List<Transform> pWaypoints = new List<Transform>();
        foreach (Transform child in pathPrefab)
        {
            pWaypoints.Add(child);
        }
        return pWaypoints;
    }

    public void DisableAllWeapons()
    {
        Debug.Log("Disable weapons");
        foreach (Shooter pShooter in weapons)
        {
            Debug.Log("Turning off this weapon");
            pShooter.useAI = false;
            pShooter.isFiring = false;
        }
    }

    public void EnableAllWeapons()
    {
        Debug.Log("Enable weapons");
        foreach (Shooter pShooter in weapons)
        {
            pShooter.useAI = true;
            pShooter.isFiring = true;
        }
    }

    void EnableBigGun()
    {

    }

    void FollowPath()
    {
        if (waypointIndex < waypoints.Count)
        {
            Vector3 targetPosition = waypoints[waypointIndex].position;
            //float delta = waveConfig.GetMoveSpeed() * Time.deltaTime;
            float delta = 2f * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);

            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            waypointIndex = 0;
        }
    }

    IEnumerator EnemyStunned(float stunnedTime)
    {
        isStunned = true;
        foreach (Shooter currWeapon in weapons)
        {
            currWeapon.useAI = false;
            currWeapon.isFiring = false;
        }
        Debug.Log("Enemy is stunned");
        yield return new WaitForSeconds(stunnedTime);
        foreach (Shooter currWeapon in weapons)
        {
            currWeapon.useAI = true;
            currWeapon.isFiring = false;
        }
        Debug.Log("Enemy is not stunned");
        isStunned = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Electric")
        {
            //Eventually should retrieve value from stunned item
            StartCoroutine(EnemyStunned(stunTime));
        }
    }

    public void SetIsInvulnerable(bool pState)
    {
        Debug.Log("Deem Setting boss state");
        enemyHealth.SetVulnerability(pState);
    }

}
