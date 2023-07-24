using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SimpleBossManager : MonoBehaviour
{
    [SerializeField] protected int health = 500;
    [SerializeField] protected int healthLimit = 400;
    [SerializeField] protected ParticleSystem hitEffect;
    protected Animator mAnimator;
    [SerializeField] protected float deathDelay = 2.5f;
    [SerializeField] protected Transform pathPrefab;
    [SerializeField] protected float regMoveSpeed = 5f;
    [SerializeField] protected float enrageMoveSpeed = 10f;
    [SerializeField] public bool isInvincible = false;

    //Waypoint management
    protected List<Transform> waypoints;
    protected int waypointIndex = 0;

    protected AudioPlayer mAudioPlayer;
    protected ScoreKeeper mScoreKeeper;
    protected LevelManager mLevelManager;
    protected bool isDead = false;

    public abstract void TakeDamage(int pDamage);

    public abstract void Die();

    public abstract void DisableWeapons();

    public abstract void StartFiring();

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
        Debug.Log("Player hit");
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (damageDealer != null && !isInvincible)
        {
            //Take some damage
            TakeDamage(damageDealer.GetDamage());
            //Tell damage dealer that it hit something and should be destroyed
            damageDealer.Hit();
            PlayHitEffect();
            mAudioPlayer.PlayDamageClip();
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

    protected void FollowPath()
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

    public void SetIsInvincible(bool pState)
    {
        isInvincible = pState;
    }
}

