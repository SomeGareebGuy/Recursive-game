using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlavhAttacks : MonoBehaviour
{
    public List<float> xCoord = new List<float>
    {
        -1.05f, -0.745f, -0.45f, -0.155f, 0.14f, 0.43f, 0.71f, 1.01f
    };

    public List<float> yCoord = new List<float>
    {
        0.65f, 0.43f, 0.22f, -0.01f, -0.23f, -0.44f, -0.66f, -0.88f
    };

    private readonly List<string> _attacks = new List<string>
    {
        "Pawn"
    };

    private int _index;
    private int _randomIndex;
    private float _appearPosX;
    private float _appearPosY;

    private SpriteRenderer _spriteRenderer;
    
    public Animator animator;
    public bool isAttacking = true;
    public float randomYCoord;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            animator.SetTrigger("Attack");
            animator.SetBool("isAttacking", true);

            if (_attacks[_index] == "Pawn")
            {
                _randomIndex = Random.Range(0, 3);
                randomYCoord = yCoord[_randomIndex];
                
                animator.SetTrigger("pawnAttack");
            }
            
            isAttacking = false;
        }
    }

    public void Disappear()
    {
        animator.SetTrigger("Disappear");
        if (_randomIndex == 0)
        {
            _appearPosY = yCoord[1];
            _randomIndex = Random.Range(0, 7);
            _appearPosX = xCoord[_randomIndex];
        }
        else
        {
            _randomIndex = Random.Range(0, _randomIndex - 1);
            _appearPosY = yCoord[_randomIndex];
            _randomIndex = Random.Range(0, 7);
            _appearPosX = xCoord[_randomIndex];
        }
        StartCoroutine(Appear());
    }

    IEnumerator Appear()
    {
        yield return new WaitForSeconds(2);
        _spriteRenderer.enabled = false;
        transform.position = new Vector3(_appearPosX, _appearPosY, transform.position.z);
        yield return new WaitForSeconds(2);
        animator.SetTrigger("Appear");
        _spriteRenderer.enabled = true;
        animator.SetBool("isAttacking", false);
    }
}
