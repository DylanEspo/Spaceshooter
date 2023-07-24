using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineHealth : IHealth
{
    AudioPlayer mAudioPlayer;
    ScoreKeeper mScoreKeeper;
    [SerializeField] int killScore = 200;
    [SerializeField] ParticleSystem hitEffect;

    public void Awake()
    {
        mAudioPlayer = FindObjectOfType<AudioPlayer>();
        mScoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public override void Die()
    {
        Debug.Log("This is the health " + health);
        if (mScoreKeeper != null)
        {
            mScoreKeeper.UpdateScore(killScore);
        }
        Destroy(gameObject);
    }

    public override int GetHealth()
    {
        return health;
    }

    public override void TakeDamage(int pDamage)
    {
        health -= pDamage;
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
        else
        {
            Debug.Log("Detecing hit, but not taking damage");
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
}
