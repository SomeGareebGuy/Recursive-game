using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;


    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
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
                    _spriteRenderer.enabled = false;
                    _rigidbody.velocity = Vector2.zero;
                    _rigidbody.simulated = false;
                    this.enabled = false;
                }
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
