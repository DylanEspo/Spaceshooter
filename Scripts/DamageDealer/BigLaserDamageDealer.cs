using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigLaserDamageDealer : IDamageDealer
{
    [SerializeField] int damage = 30;

    public override int GetDamage()
    {
        return damage;
    }

    public override void SetDamage(int pDamage)
    {
        damage = pDamage;
    }

    public override void Hit()
    {
        Debug.Log("DEEM This is getting destroyed ");
        //Destroy(gameObject);
    }
}
