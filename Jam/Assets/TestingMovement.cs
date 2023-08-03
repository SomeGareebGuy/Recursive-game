using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class TestingMovement : MonoBehaviour
{
    public float moveSpeed = 4;
    private Vector2 _inputVector;

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(_inputVector.x, _inputVector.y, 0);
        transform.position += movement * (moveSpeed * Time.fixedDeltaTime);
    }
    
    public void Movement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _inputVector = context.ReadValue<Vector2>();   
        }
        
        else if (context.canceled)
        {
            _inputVector = Vector2.zero;
        }
    }
}
