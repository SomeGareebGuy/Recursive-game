using System;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public Animator animator;
    public int direction;

    public Transform attackPointUp;
    public Transform attackPointDown;
    public Transform attackPointLeft;
    public Transform attackPointRight;
    private Transform _attackPoint;
    
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public bool isWalking;

    private SpriteRenderer _sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        _sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        bool currentFlipX = _sprite.flipX;
        _sprite.flipX = direction < 0 || currentFlipX;
        
        animator.SetBool("isWalking", isWalking);
        animator.SetInteger("Direction", Mathf.Abs(direction));
        
    }

    public void Attack()
    {
        if (direction == 0 || direction == 3)
        {
            _attackPoint = attackPointUp;
        }
        else if (direction == 2)
        {
            _attackPoint = attackPointDown;
        }
        else if (direction == 1)
        {
            _attackPoint = attackPointRight;
        }
        else if (direction == -1)
        {
            _attackPoint = attackPointLeft;
        }
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, attackRange, enemyLayers);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(20);
            Debug.Log("Hit " + enemy.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPointUp.position, attackRange);
        Gizmos.DrawWireSphere(attackPointDown.position, attackRange);
        Gizmos.DrawWireSphere(attackPointLeft.position, attackRange);
        Gizmos.DrawWireSphere(attackPointRight.position, attackRange);
    }
}

