using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManagement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] WeaponConfig startingWeapon;
    [SerializeField] int maxNumberOfWeapons = 3;
    [SerializeField] int numLasers = 1;
    [SerializeField] float fireRateVariance = 0f;
    [SerializeField] float minimumFiringRate = 0.1f;
    [SerializeField] float baseFiringRate = 0.2f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] List <GameObject> weaponPowerups;
    private Dictionary<string, GameObject> droppableWeaponPowerups;

    //References to weapon firing
    List<WeaponConfig> OurWeapons;
    private GameObject equippedProjectile;
    private float projectileLifetime = 1f;
    private int currWeaponIndex = 0;
    bool hasFired = false;
    public bool isFiring = false;
    Coroutine firingCoroutine;

    //Reference to audio player
    AudioPlayer mAudioPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        mAudioPlayer = FindObjectOfType<AudioPlayer>();
        OurWeapons = new List<WeaponConfig>();
    }

    void Start()
    {
        OurWeapons.Add(startingWeapon);
        equippedProjectile = startingWeapon.GetProjectilePrefab();
        projectileSpeed = startingWeapon.GetProjectileSpeed();
        projectileLifetime = startingWeapon.GetProjectileLifetime();
        Debug.Log("Projectile lifetime is " + projectileLifetime);
        baseFiringRate = startingWeapon.GetBaseFireRate();
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    public bool CanAddWeapon(WeaponConfig weapon)
    {
        if (OurWeapons.Contains(weapon))
        {
            return false;
        }
        return true;
    }

    public void AddToWeaponList(WeaponConfig weapon)
    {
        if (CanAddWeapon(weapon))
        {
            Debug.Log("Added weapon");
            OurWeapons.Add(weapon);
        }
    }

    public bool canAddLaser()
    {
        if(numLasers < 3)
        {
            return true;
        }
        return false;
    }


    public void addLaser()
    {
        if(numLasers < 3)
        {
            numLasers++;
        }
    }

    public void dropLaser()
    {
        if (numLasers > 1)
        {
            numLasers--;
            DropLaserPickup();
        }
        else
        {
            numLasers = 1;
        }
    }

    public void DropLaserPickup()
    {
        string weaponName = OurWeapons[currWeaponIndex].GetWeaponName();

        if(weaponName == "LaserGun")
        {
            Instantiate(droppableWeaponPowerups["LaserGun"], new Vector3(transform.position.x, transform.position.y -1, transform.position.z), Quaternion.identity);
        }
        if(weaponName == "ElectroBall")
        {
            Instantiate(droppableWeaponPowerups["ElectroBall"], new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Quaternion.identity);
        }
        if(weaponName == "Missle")
        {
            Instantiate(droppableWeaponPowerups["Missle"], new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Quaternion.identity);
        }
    }

    //Should consider switching into retrieving by object instead of index
    public void SetEquipped(int i)
    {
        //Debug.Log("DEE " + OurWeapons[i].GetBaseFireRate());
        Debug.Log("Weapons size " + OurWeapons.Count);
        int j = i - 1;
        Debug.Log(i);
        if(i <= OurWeapons.Count && i != currWeaponIndex)
        {
            equippedProjectile = OurWeapons[j].GetProjectilePrefab();
            projectileSpeed = OurWeapons[j].GetProjectileSpeed();
            projectileLifetime = OurWeapons[j].GetProjectileLifetime();
            Debug.Log("Weapon lifetime is " + projectileLifetime);
            baseFiringRate = OurWeapons[j].GetBaseFireRate();
            currWeaponIndex = i;
            Debug.Log("Current selected weapon is " + currWeaponIndex);
        }
        else
        {
            Debug.Log("Can't equip, slot empty");
        }
    }

    void DropWeapon(int i)
    {
        int j = i - 1;
        if(OurWeapons[i-1] != null)
        {
            OurWeapons.RemoveAt(j);
            StopFiring();
        }
    }

    private void StopFiring()
    {
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
            isFiring = false;
        }
    }

    int GetMaxNumberOfWeapons()
    {
        return maxNumberOfWeapons;
    }

    WeaponConfig GetWeaponAtIndex(int i)
    {
        return OurWeapons[i];
    }

    void Fire()
    {
        //Don't want to spam a bunch of corountines, only need one running
        if (isFiring && firingCoroutine == null && hasFired == false)
        {
            hasFired = true;
            StartCoroutine(FireController());
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {

            FireLaser();
            //Piece manages timing and variance of fire
            float timeToNextProjectile = Random.Range(baseFiringRate - fireRateVariance, baseFiringRate + fireRateVariance);
            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);

            if(mAudioPlayer != null)
            {
                mAudioPlayer.PlayShootingClip();
            }

            //Hold off for predetermined rate
            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }

    void FireLaser()
    {
        GameObject projInstance = Instantiate(equippedProjectile, transform.position, Quaternion.identity);
        Rigidbody2D projRB = projInstance.GetComponent<Rigidbody2D>();
        if (projRB != null)
        {
            projRB.velocity = transform.up * projectileSpeed;//Reference green arrow in ui, indicates up direction
        }
        Destroy(projInstance, projectileLifetime); // Could set a method where the bullet fizzles out

        if(numLasers >= 2)
        {
            Debug.Log("Firing two lasers");
            GameObject projInstanceTwo = Instantiate(equippedProjectile, new Vector3(transform.position.x + 0.3f, transform.position.y - 0.3f, transform.position.z), Quaternion.identity);
            Rigidbody2D projRBTwo = projInstanceTwo.GetComponent<Rigidbody2D>();
            if (projRBTwo != null)
            {
                projRBTwo.velocity = transform.up * projectileSpeed;//Reference green arrow in ui, indicates up direction
            }
            Destroy(projInstanceTwo, projectileLifetime);
        }

        if(numLasers == 3)
        {
            Debug.Log("Firing three lasers");
            GameObject projInstanceThree = Instantiate(equippedProjectile, new Vector3(transform.position.x - 0.3f, transform.position.y - 0.3f, transform.position.z), Quaternion.identity);
            Rigidbody2D projRBThree = projInstanceThree.GetComponent<Rigidbody2D>();
            if (projRBThree != null)
            {
                projRBThree.velocity = transform.up * projectileSpeed;//Reference green arrow in ui, indicates up direction
            }
            Destroy(projInstanceThree, projectileLifetime);
        }
    }

    public void FlipFire()
    {

    }

    IEnumerator FireController()
    {
        yield return new WaitForSeconds(baseFiringRate);
        hasFired = false;
    }

    public float GetFiringRate()
    {
        return baseFiringRate;
    }

    public float GetFiringRateMin()
    {
        return minimumFiringRate;
    }

    public int GetNumberOfLasers()
    {
        return numLasers;
    }


}
