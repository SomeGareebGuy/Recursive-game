using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 3;
    public TestingMovement movement;

    public void TakeDamage()
    {
        health -= 1;
        movement.isHit = true;
    }
}
