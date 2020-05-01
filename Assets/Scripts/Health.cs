using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100;
    public Rigidbody body;

    public void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    public Rigidbody GetRigidBody()
    {
        return body;
    }

    public virtual void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void TakeDamage(float dmg, Vector3 direction, Vector3 position)
    {
        TakeDamage(dmg);
        body.AddForceAtPosition(direction, position);
    }

    public virtual void Die()
    {
        Debug.Log("Old Die");
        Destroy(gameObject);
    }

}
