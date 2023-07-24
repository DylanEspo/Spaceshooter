using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] WeaponConfig weapon = null;
    [SerializeField] bool canRespawn = true;
    [SerializeField] float respawnTime = 5;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Collision detected");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Collected by player");
            other.GetComponent<WeaponsManagement>().AddToWeaponList(weapon);
            if (canRespawn)
            {
                StartCoroutine(HideForSeconds(respawnTime));
            }
            else
            {
                Destroy(gameObject);
            }
            //StartCoroutine(HideForSeconds(respawnTime));
        }
    }

    private IEnumerator HideForSeconds(float seconds)
    {
        ShowPickup(false);
        yield return new WaitForSeconds(seconds);
        ShowPickup(true);
    }

    private void ShowPickup(bool shouldShow)
    {
        GetComponent<Collider2D>().enabled = shouldShow;
        GetComponent<SpriteRenderer>().enabled = shouldShow;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(shouldShow);
        }
    }
}
