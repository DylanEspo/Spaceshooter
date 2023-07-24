using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaitonaryShooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 6f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float baseFiringRate = 0.5f;
    [SerializeField] float fireRateVariance = 0f;
    [SerializeField] float minimumFiringRate = 0.1f;
    [SerializeField] string targetTag = "Player";
    [SerializeField] float yOffset = 1f;
    private State myCurrentState = State.idle;
   
    //Firing info
    private GameObject target = null;
    private Vector3 targetDirection;
    Coroutine firingCoroutine;
    private bool isFiring = false;
    AudioPlayer mAudioPlayer;
    //Will need references to projectile Prefab 
    //Will need to go agro if we're in 

    enum State
    {
        idle,
        engage
    }

    private void Awake()
    {
        mAudioPlayer = FindObjectOfType<AudioPlayer>();
    }

    private void Update()
    {
        if (myCurrentState == State.engage)
        {
            //Debug.Log("Can engage my target of " + targetTag);
            Fire();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == targetTag)
        {
            if (myCurrentState != State.engage)
            {
                target = collision.gameObject;
                myCurrentState = State.engage;
                Debug.Log("Pursuing player");
                targetDirection = target.transform.position - transform.position;
                isFiring = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == targetTag)
        {
            target = collision.gameObject;
            myCurrentState = State.idle;
            Debug.Log("Returning to idle");
            targetDirection = target.transform.position - transform.position;
            isFiring = false;
        }
    }

    void Fire()
    {
        //Don't want to spam a bunch of corountines, only need one running
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuouslyAtTarget());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinuouslyAtTarget()
    {
        while (isFiring)
        {
            GameObject projInstance = Instantiate(projectilePrefab, new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), Quaternion.identity);
            Rigidbody2D projRB = projInstance.GetComponent<Rigidbody2D>();

            if (projRB != null && target != null)
            {
                Vector3 direction = target.transform.position - transform.position;
                Vector3 rotation = transform.position - target.transform.position;
                float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
                //projRB.velocity = transform.up * projectileSpeed;//Reference green arrow in ui, indicates up direction
                Debug.Log("This is the time delta tiem " + Time.deltaTime);
                Debug.Log("This is the fixed delta tiem " + Time.fixedDeltaTime);
                projRB.velocity = new Vector3(direction.x, direction.y, direction.z).normalized * projectileSpeed;
                Debug.Log("This is the projectile's velocity " + projRB.velocity.ToString());
                //projInstance.transform.rotation = Quaternion.Euler(0, 0, rot + 90);
            }
            Destroy(projInstance, projectileLifetime); // Could set a method where the bullet fizzles out

            //Piece manages timing and variance of fire
            float timeToNextProjectile = Random.Range(baseFiringRate - fireRateVariance, baseFiringRate + fireRateVariance);
            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);

            if (mAudioPlayer != null)
            {
                mAudioPlayer.PlayShootingClip();
            }

            //Hold off for predetermined rate
            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
}
