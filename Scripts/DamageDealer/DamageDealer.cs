using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : IDamageDealer
{
    [SerializeField] int damage = 10;
   
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
        Destroy(gameObject);
    }
}
