using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayRecording : MonoBehaviour
{
    public List<String> keyDownList = new List<string>{"Up"};
    public List<String> keyUpList = new List<string>{"Up"};
    public List<int> keyDownFrame = new List<int>{0};
    public List<int> keyUpFrame = new List<int>{0};
    
    private int _frameCount = 1;
    private int _keyDownFrameIndex = 1;
    private int _keyUpFrameIndex = 1;

    public TestingMovement movement;
    public MovementManager manager;

    private string _down = "None";
    private string _up = "None";

    public Vector2 inputVector;
    public int walkingDirection;

    private void Update()
    {
        if (!movement.recording)
        {
            _frameCount++;

            if (_frameCount == keyDownFrame[_keyDownFrameIndex])
            {
                _down = keyDownList[_keyDownFrameIndex];
                if (_keyDownFrameIndex == keyDownList.Count - 1)
                {
                    keyDownFrame.Add(0);
                }
                _keyDownFrameIndex++;
            }
            else
            {
                _down = "None";
            }

            if (_frameCount == keyUpFrame[_keyUpFrameIndex])
            {
                _up = keyUpList[_keyUpFrameIndex];
                if (_keyUpFrameIndex == keyUpList.Count - 1)
                {
                    keyUpFrame.Add(0);
                }
                _keyUpFrameIndex++;
            }
            else
            {
                _up = "None";
            }


            if (_down != "None" || _up != "None")
            {
                if (_down == "Up")
                {
                    movement.inputVector = new Vector2(0, 1);
                }
                else if (_down == "Down")
                {
                    movement.inputVector = new Vector2(0, -1);
                }
                else if (_down == "Left")
                {
                    movement.inputVector = new Vector2(-1, 0);
                }
                else if (_down == "Right")
                {
                    movement.inputVector = new Vector2(1, 0);
                }
                else if (_down == "Z")
                {
                    manager.animator.SetTrigger("Attack");
                }

                if (_up == "Up")
                {
                    if (movement.inputVector != new Vector2(0, 1))
                    {
                        movement.inputVector.y -= 1;
                    }
                    else
                    {
                        movement.inputVector = new Vector2(0, 0);
                    }
                }
                else if (_up == "Down")
                {
                    if (movement.inputVector != new Vector2(0, -1))
                    {
                        movement.inputVector.y += 1;
                    }
                    else
                    {
                        movement.inputVector = new Vector2(0, 0);
                    }
                }
                else if (_up == "Left")
                {
                    if (movement.inputVector != new Vector2(-1, 0))
                    {
                        movement.inputVector.x += 1;
                    }
                    else
                    {
                        movement.inputVector = new Vector2(0, 0);
                    }
                }
                else if (_up == "Right")
                {
                    if (movement.inputVector != new Vector2(+1, 0))
                    {
                        movement.inputVector.x -= 1;
                    }
                    else
                    {
                        movement.inputVector = new Vector2(0, 0);
                    }
                }
            }

            if (movement.inputVector != new Vector2(0, 0))
            {
                manager.isWalking = true;
                inputVector = movement.inputVector;
                walkingDirection = manager.direction;
                
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
            else
            {
                manager.isWalking = false;
            }
        }
    }
}

