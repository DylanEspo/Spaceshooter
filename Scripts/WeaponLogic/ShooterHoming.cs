using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterHoming : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float baseFiringRate = 0.2f;

    [Header("AI")]
    [SerializeField] public bool useAI = false;
    [SerializeField] float fireRateVariance = 0f;
    [SerializeField] float minimumFiringRate = 0.1f;
    [SerializeField] GameObject homingTarget = null;

    public bool isFiring = false;
    Coroutine firingCoroutine;
    AudioPlayer mAudioPlayer;

    private void Awake()
    {
        mAudioPlayer = FindObjectOfType<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (useAI)
        {
            isFiring = true;
            Fire();
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
            GameObject projInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D projRB = projInstance.GetComponent<Rigidbody2D>();

            if (projRB != null && homingTarget != null)
            {
                Vector3 direction = homingTarget.transform.position - transform.position;
                Vector3 rotation = transform.position - homingTarget.transform.position;
                float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
                //projRB.velocity = transform.up * projectileSpeed;//Reference green arrow in ui, indicates up direction
                projRB.velocity = new Vector2(direction.x, direction.y) * projectileSpeed;
                projInstance.transform.rotation = Quaternion.Euler(0, 0, rot + 90);
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

