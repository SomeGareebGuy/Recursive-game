using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawnpieces : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision involves an object on the target layer
        if ((enemyLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.GetComponent<TestingMovement>().isTrailActive == false || enemy.GetComponent<TestingMovement>().isHit == true)
                {
                    enemy.GetComponent<Health>().TakeDamage();
                    Debug.Log("Hit " + enemy.name);
                }
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
