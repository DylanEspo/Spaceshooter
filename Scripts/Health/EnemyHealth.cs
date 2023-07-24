using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : IHealth
{
    [SerializeField] int killScore = 500;
    [SerializeField] ParticleSystem hitEffect;
    AudioPlayer mAudioPlayer;
    ScoreKeeper mScoreKeeper;
    LevelManager mLevelManager;

    void Awake()
    {
        mAudioPlayer = FindObjectOfType<AudioPlayer>();
        mScoreKeeper = FindObjectOfType<ScoreKeeper>();
        mLevelManager = FindObjectOfType<LevelManager>();
    }

    public override int GetHealth()
    {
        return health;
    }

    public void RestoreHealth(int pValue)
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage(int pDamage)
    {
        //Take some damage
        Debug.Log("This is the damage " + pDamage);
        Debug.Log("This is the health before damage " + health);
        health -= pDamage;
        Debug.Log("This is the health after damage " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IDamageDealer damageDealer = other.GetComponent<IDamageDealer>();
        if (damageDealer != null)
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

    public override void Die()
    {
        Debug.Log("This is the health " + health);
        if (mScoreKeeper != null)
        {
            mScoreKeeper.UpdateScore(killScore);
        }

        //GameObject[] myChildren = gameObject.GetComponentsInChildren<GameObject>(true);

        //for(int i = 0; i < myChildren.Length; i++)
        //{
        //    Destroy(myChildren[i]);
        //}
        Destroy(gameObject);
    }
}
