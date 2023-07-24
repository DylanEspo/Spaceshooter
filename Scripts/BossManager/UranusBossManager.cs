using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UranusBossManager : MonoBehaviour
{
    [SerializeField] int health = 500;
    [SerializeField] int healthLimit = 400;
    [SerializeField] ParticleSystem hitEffect;
    Animator mAnimator;
    [SerializeField] float deathDelay = 2.5f;
    [SerializeField] Transform pathPrefab;
    [SerializeField] float regMoveSpeed = 5f;
    [SerializeField] float enrageMoveSpeed = 10f;
    private bool bossInvulnerable = true;
    [SerializeField] ShooterHoming myShooter = null;

    Collider2D myCollider = null;

    //Waypoint management
    List<Transform> waypoints;
    int waypointIndex = 0;

    AudioPlayer mAudioPlayer;
    ScoreKeeper mScoreKeeper;
    LevelManager mLevelManager;
    bool isDead = false;
    void Awake()
    {
        mAudioPlayer = FindObjectOfType<AudioPlayer>();
        mScoreKeeper = FindObjectOfType<ScoreKeeper>();
        mLevelManager = FindObjectOfType<LevelManager>();
        mAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        myShooter = GetComponent<ShooterHoming>();
    }

    private void Start()
    {
        waypoints = GetWaypoints();
    }

    private void Update()
    {
        //Stop player from moving as well
        if (isDead)
        {
            return;
        }
        else if (health <= healthLimit)
        {
            FollowPath();
        }
    }

    public void StartFiring()
    {
        myShooter.useAI = true;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (damageDealer != null && !bossInvulnerable)
        {
            //Take some damage
            TakeDamage(damageDealer.GetDamage());
            //Tell damage dealer that it hit something and should be destroyed
            damageDealer.Hit();
            PlayHitEffect();
            mAudioPlayer.PlayDamageClip();
        }
    }

    void TakeDamage(int pDamage)
    {
        //Take some damage
        health -= pDamage;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            ParticleSystem instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    private void Die()
    {
        DisableWeapons();
        isDead = true;
        myCollider.enabled = false;
        if (mScoreKeeper != null)
        {
            mScoreKeeper.UpdateScore(2000);
        }
        if (mAnimator != null)
        {
            Debug.Log("CAN BLOW UP");
            StartCoroutine(BlowUpBoss());
        }
        else
        {
            mLevelManager.Winner();
            Destroy(gameObject);
        }
    }

    private void DisableWeapons()
    {
        myShooter.useAI = false;
        myShooter.isFiring = false;
    }


    IEnumerator BlowUpBoss()
    {
        mAnimator.SetTrigger("Blow_Up");
        //Destroy(gameObject, deathDelay);
        yield return new WaitForSeconds(deathDelay);
        mAnimator.SetTrigger("MiniExplo");
        yield return new WaitForSeconds(0.5f);
        mLevelManager.LoadWithSimpleBrackeysFade();
        Destroy(gameObject);
    }

    void FollowPath()
    {
        if (waypointIndex < waypoints.Count)
        {
            Vector3 targetPosition = waypoints[waypointIndex].position;
            //float delta = waveConfig.GetMoveSpeed() * Time.deltaTime;
            float delta = regMoveSpeed * Time.deltaTime;
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

    public int GetHealth()
    {
        return health;
    }

    public void SetIsInvulnerable(bool pState)
    {
        bossInvulnerable = pState;
    }
}

