using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TestingMovement : MonoBehaviour
{
    public float moveSpeed = 4;
    public float zoneSize = 1024;
    public bool recording;

    private int _frameCount = 1;
    
    public Vector2 inputVector;
    private Vector3 _position;

    public PlayRecording recorder;
    public MovementManager manager;
    
    private Dictionary<KeyCode, string> keyMap = new Dictionary<KeyCode, string>()
    {
        { KeyCode.UpArrow, "Up" },
        { KeyCode.DownArrow, "Down" },
        { KeyCode.LeftArrow, "Left" },
        { KeyCode.RightArrow, "Right" },
        { KeyCode.Z, "Z" }
    };

    private void Start()
    {
        StartCoroutine(ResetBoolAfterDelay(5f));
    }
    
    IEnumerator ResetBoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.position = new Vector3(-0.24f, -0.8f, -12.8f);
        inputVector = Vector2.zero;
        Debug.Log("Start!");
        recording = false;
    }

    private void Update()
    {
        if (recording)
        {
            _frameCount++;

            foreach (var pair in keyMap)
            {
                if (Input.GetButtonDown(pair.Key.ToString()))
                {
                    recorder.keyDownList.Add(pair.Value);
                    recorder.keyDownFrame.Add(_frameCount);
                }
                
                if (Input.GetButtonUp(pair.Key.ToString()))
                {
                    recorder.keyUpList.Add(pair.Value);
                    recorder.keyUpFrame.Add(_frameCount);
                }
            }
        }

        _position = transform.position;
        Vector3 movement = new Vector3(inputVector.x, inputVector.y, 0);
        _position += movement * (moveSpeed * Time.fixedDeltaTime);

        float clampedX = Mathf.Clamp(_position.x, -zoneSize / 2, zoneSize / 2);
        float clampedY = Mathf.Clamp(_position.y, -zoneSize / 2, zoneSize / 2);
        transform.position = new Vector3(clampedX, clampedY, _position.z);
        
    }

    public void Movement(InputAction.CallbackContext context)
    {
        int walkingDirection = manager.direction;
        
        if (recording)
        {
            if (context.performed)
            {
                inputVector = context.ReadValue<Vector2>();
                manager.isWalking = true;
                
                if (inputVector.x > 0)
                {
                    walkingDirection = 1;
                }
                else if (inputVector.x < 0)
                {
                    walkingDirection = -1;
                }
                else if (inputVector.y > 0)
                {
                    walkingDirection = 3;
                }
                else if (inputVector.y < 0)
                {
                    walkingDirection = -2;
                }

                manager.direction = walkingDirection;
            }

            else if (context.canceled)
            {
                inputVector = Vector2.zero;
                manager.isWalking = false;
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            manager.animator.SetTrigger("Attack");
        }
    }
}
