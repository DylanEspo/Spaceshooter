using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : IHealth
{
    [SerializeField] int playerLives = 1;
    [SerializeField] int maxHealth = 100;
    [SerializeField] ParticleSystem hitEffect;

    bool isInvulnerable = false;
    [SerializeField] float invulerabilityDuration = 0.2f;

    CameraShake cameraShake;
    AudioPlayer mAudioPlayer;
    ScoreKeeper mScoreKeeper;
    LevelManager mLevelManager;
    WeaponsManagement mWeaponsManagement;

    private void Awake()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        mWeaponsManagement = GetComponent<WeaponsManagement>();
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
        Debug.Log("Restoring Health");
        health += pValue;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
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
        else
        {
            mWeaponsManagement.dropLaser();
            StartCoroutine(Invulerablity());
        }
    }

    public override void Die()
    {
        if(playerLives > 0)
        {
            //Respawn Player
            //Play white animation
            Debug.Log("Set player invulnerability");
            StartCoroutine(Invulerablity());
            health = maxHealth;
            playerLives--;
        }
        else
        {
            mLevelManager.GameOver();
            Destroy(gameObject);
        }
    }

    /*
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "HealthPack")
        {
            Debug.Log("Collided with health pack");
            RestoreHealth(100);
            mAudioPlayer.PlayHealingClip();
            collision.gameObject.SetActive(false);
        }
        else if (!isInvulnerable)
        {
            IDamageDealer damageDealer = collision.gameObject.GetComponent<IDamageDealer>();
            if (damageDealer != null)
            {
                //Take some damage
                TakeDamage(damageDealer.GetDamage());
                //Tell damage dealer that it hit something and should be destroyed
                damageDealer.Hit();
                PlayHitEffect();
                mAudioPlayer.PlayDamageClip();
                ShakeCamera();
            }
        }
        else
        {
            Debug.Log("Detecing hit, but not taking damage");
        }
    }*/

    //TO DO - This will be affected by Rigidbody's sleep state, if never sleep will always run continousouly
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "HealthPack")
        {
            Debug.Log("Collided with health pack");
            RestoreHealth(100);
            mAudioPlayer.PlayHealingClip();
            collision.gameObject.SetActive(false);
        }
        else if (!isInvulnerable)
        {
            if (collision.tag == "Mine")
            {
                Debug.Log("Crossed paths with mine");
            }
            IDamageDealer damageDealer = collision.GetComponent<IDamageDealer>();
            if (damageDealer != null)
            {
                //Take some damage
                TakeDamage(damageDealer.GetDamage());
                //Tell damage dealer that it hit something and should be destroyed
                damageDealer.Hit();
                PlayHitEffect();
                mAudioPlayer.PlayDamageClip();
                ShakeCamera();
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "HealthPack")
        {
            Debug.Log("Collided with health pack");
            RestoreHealth(50);
            mAudioPlayer.PlayHealingClip();
            other.gameObject.SetActive(false);
        }
        else if (!isInvulnerable)
        {
            if(other.tag == "Mine")
            {
                Debug.Log("Crossed paths with mine");
            }
            else
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
                    ShakeCamera();
                }
            }
        }
        else
        {
            Debug.Log("Detecing hit, but not taking damage");
        }
    }

    IEnumerator Invulerablity()
    {
        isInvulnerable = true;
        Debug.Log("Not taking damage");
        yield return new WaitForSeconds(invulerabilityDuration);
        isInvulnerable = false;
    }

    void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            ParticleSystem instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    void ShakeCamera()
    {
        if (cameraShake != null)
        {
            cameraShake.Play();
        }
    }

    public void SetIsInvulnerable(bool pValue)
    {
        isInvulnerable = pValue;
    }

    public bool isInvulerable()
    {
        return isInvulnerable;
    }
}
