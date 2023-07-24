using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IHealth : MonoBehaviour
{
    [SerializeField] protected int health = 50;

    // Start is called before the first frame update
    public abstract int GetHealth();

    public abstract void TakeDamage(int pDamage);

    public abstract void Die();

}
