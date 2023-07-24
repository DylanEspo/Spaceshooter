using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeptuneBossManager : SimpleBossManager
{
    [SerializeField] ShooterHoming myShooter = null;
    [SerializeField] BeamShooter myAltFire = null;
    [SerializeField] float altFireLimit = 200f;
    [SerializeField] float altFireLength = 5f;
    PolygonCollider2D myCollider = null;

    private bool isAltFiring = false;

    void Awake()
    {
        mAudioPlayer = FindObjectOfType<AudioPlayer>();
        mScoreKeeper = FindObjectOfType<ScoreKeeper>();
        mLevelManager = FindObjectOfType<LevelManager>();
        mAnimator = GetComponent<Animator>();
        myCollider = GetComponent<PolygonCollider2D>();
        myShooter = GetComponent<ShooterHoming>();
        myAltFire = GetComponent<BeamShooter>();
    }

    private void Start()
    {
        waypoints = GetWaypoints();
        if(myShooter != null)
        {
            myShooter.isFiring = false;
        }
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
        if (health <= altFireLimit && !isAltFiring)
        {
            Debug.Log("Start alt fire");
            isAltFiring = true;
            mAnimator.SetTrigger("Alt_Fire");
        }
    }

    public void StartAltFire()
    {
        Debug.Log("NeptuneBossManager start firing");
        myAltFire.BeamFire(altFireLength);
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
        Destroy(myShooter);
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
        mLevelManager.Winner();
        Destroy(gameObject);
    }
}

