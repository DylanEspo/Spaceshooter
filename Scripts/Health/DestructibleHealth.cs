using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleHealth : IHealth
{ 
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] ParticleSystem hitEffect;

    public override void Die()
    {
        if(objectToSpawn != null)
        {
            Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    public override int GetHealth()
    {
        return health;
    }

    public void RestoreHealth(int pValue)
    {
        health += pValue;
    }

    public override void TakeDamage(int pDamage)
    {
        health -= pDamage;
        if(health <= 0)
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
            //mAudioPlayer.PlayDamageClip();
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
