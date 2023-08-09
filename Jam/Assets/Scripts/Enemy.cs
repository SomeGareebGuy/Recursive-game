using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currentHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = 100;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy health: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
