using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDamageDealer : MonoBehaviour
{
    public abstract int GetDamage();

    public abstract void SetDamage(int pDamage);

    public abstract void Hit();
}
