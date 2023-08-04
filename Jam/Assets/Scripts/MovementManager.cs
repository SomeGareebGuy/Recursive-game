using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public Animator animator;
    public int direction;

    public bool isWalking = false;

    private SpriteRenderer _sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _sprite.flipX = false;
        
        if (direction < 0)
        {
            _sprite.flipX = true;
        }
        
        if (isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        
        animator.SetInteger("Direction", Mathf.Abs(direction));
        
    }
}
