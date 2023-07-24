using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaturnBossManager : SimpleBossManager
{
    [SerializeField] ShooterHoming myShooter = null;
    Collider2D myCollider = null;

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
        myShooter.isFiring = false;
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

    public override void TakeDamage(int pDamage)
    {
        //Take some damage
        health -= pDamage;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    public override void Die()
    {
        DisableWeapons();
        myShooter.isFiring = false;
        Debug.Log("This is the health " + health);
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
            mLevelManager.LoadNextLevelInIndex();
            Destroy(gameObject);
        }
    }

    public override void DisableWeapons()
    {
        myShooter.useAI = false;
        myShooter.isFiring = false;
    }

    public override void StartFiring()
    {
        myShooter.useAI = true;
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
}

