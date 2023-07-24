using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMainSection : MonoBehaviour
{
    [SerializeField] int health = 50;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] int assignedScore = 200;

    AudioPlayer mAudioPlayer;
    ScoreKeeper mScoreKeeper;
    bool IsInvulnerable = false;

    public void SetAudioPlayer(AudioPlayer pAudioPlayer)
    {
        mAudioPlayer = pAudioPlayer;
    }

    public void SetScoreKeeper(ScoreKeeper pScoreKeeper)
    {
        mScoreKeeper = pScoreKeeper;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (damageDealer != null && !IsInvulnerable)
        {
            //Take some damage
            TakeDamage(damageDealer.GetDamage());
            //Tell damage dealer that it hit something and should be destroyed
            damageDealer.Hit();
            PlayHitEffect();
            PlayDamageClip();
        }
    }
    void TakeDamage(int pDamage)
    {
        //Take some damage
        //Debug.Log("This is the damage " + pDamage);
        //Debug.Log("This is the health before damage " + health);
        health -= pDamage;
        Debug.Log("This is subsection health after damage " + health);
    }

    void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            ParticleSystem instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }


    void PlayDamageClip()
    {
        if (mAudioPlayer != null)
        {
            mAudioPlayer.PlayDamageClip();
        }
    }

    public void Die()
    {
        Debug.Log("This is the health " + health);
        if (mScoreKeeper != null)
        {
            mScoreKeeper.UpdateScore(assignedScore);
        }
        Destroy(gameObject);
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetVulnerability(bool state)
    {
        IsInvulnerable = state;
    }

    public bool CanTakeDamage()
    {
        return IsInvulnerable;
    }
}
