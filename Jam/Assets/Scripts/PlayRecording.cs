using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayRecording : MonoBehaviour
{
    public List<String> keyDownList;
    public List<String> keyUpList;
    public List<int> keyDownFrame;
    public List<int> keyUpFrame;
    
    private int _frameCount;
    private int _keyDownFrameIndex;
    private int _keyUpFrameIndex;

    public TestingMovement movement;
    public MovementManager manager;
    public Text timerText;

    private string _down = "None";
    private string _up = "None";

    public Vector2 inputVector;
    public int walkingDirection;

    public IEnumerator ResetBoolAfterDelay(float delay)
    {
        float timeRemaining = delay;

        while (timeRemaining > 0f)
        {
            // Update the timer display on the UI
            timerText.text = timeRemaining.ToString("F1") + "s";

            yield return new WaitForSeconds(0.1f); // Update the timer every 0.1 seconds
            timeRemaining -= 0.1f;
        }

        movement.inputVector = Vector2.zero;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    
    private void Update()
    {
        if (!movement.recording)
        {
            _frameCount ++;

            if (_frameCount == keyDownFrame[_keyDownFrameIndex])
            {
                _down = keyDownList[_keyDownFrameIndex];
                Debug.Log("1 "+_frameCount);
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
                Debug.Log("2 "+_frameCount);
                
                if ( keyDownFrame[_keyDownFrameIndex] != 0 && 
                     keyUpFrame[_keyUpFrameIndex] > keyDownFrame[_keyDownFrameIndex] || 
                     keyUpList[_keyUpFrameIndex] != keyDownList[_keyDownFrameIndex - 1])
                {
                    
                }
                else
                {
                    Debug.Log("O " + _frameCount);
                    movement.inputVector = Vector2.zero;
                }
                
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


            if (_down != "None")
            {
                if (_down == "Up")
                {
                    Debug.Log("3 "+_frameCount);
                    movement.inputVector = new Vector2(0, 1);
                }
                else if (_down == "Down")
                {
                    Debug.Log("4 "+_frameCount);
                    movement.inputVector = new Vector2(0, -1);
                }
                else if (_down == "Left")
                {
                    Debug.Log("5 "+_frameCount);
                    movement.inputVector = new Vector2(-1, 0);
                }
                else if (_down == "Right")
                {
                    Debug.Log("6 "+_frameCount);
                    movement.inputVector = new Vector2(1, 0);
                }
                else if (_down == "Z")
                {
                    Debug.Log("7 "+_frameCount);
                    manager.animator.SetTrigger("Attack");
                    manager.Attack();
                }
                else if (_down == "Space" && !movement.isTrailActive)
                {
                    Debug.Log("Dash "+_frameCount);
                    movement.dashTime = movement.startDashTime;
                    movement.isTrailActive = true;
                    StartCoroutine(movement.ActivateTrail(movement.startDashTime));
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

