using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TestingMovement : MonoBehaviour
{
    public float moveSpeed = 4;
    public float zoneSize = 1024;
    public bool recording;
    public float dashSpeed;
    public bool isTrailActive;
    public bool isHit;

    private int _frameCount;
    public float duration;
    public float dashTime;
    private float _speed;
    public float startDashTime;

    public Vector2 inputVector;
    private Vector3 _position;
    private readonly List<Vector3> _recordedPositions = new List<Vector3>();

    public PlayRecording recorder;
    public MovementManager manager;
    public Text timerText;
    public float time;
    

    private void Start()
    {
        StartCoroutine(ResetBoolAfterDelay(20f));
        Application.targetFrameRate = 60;
        time = Time.deltaTime;
    }

    IEnumerator ResetBoolAfterDelay(float delay)
    {
        float timeRemaining = delay;
        delay = 4f;
        
        while (timeRemaining > 0f)
        {
            if (isHit)
            {
                while (delay > 0f)
                {
                    yield return new WaitForSeconds(0.2f);
                    delay -= 0.2f;
                    timeRemaining -= 0.1f;
                }
                isHit = false;
            }
            else
            {
                yield return new WaitForSeconds(0.1f); // Update the timer every 0.1 seconds
                timeRemaining -= 0.1f;   
            }
            
            // Update the timer display on the UI
            timerText.text = timeRemaining.ToString("F1") + "s";
        }

        transform.position = new Vector3(0, -1, -12.8f);
        inputVector = Vector2.zero;
        Debug.Log("Start!");
        recording = false;
        manager.direction = 0;
        StartPlayback();
        StartCoroutine(recorder.ResetBoolAfterDelay(20f));
    }

    private void Update()
    {
        if (recording)
        {
            _frameCount++;
            int walkingDirection = manager.direction;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                inputVector = new Vector2(0, 1);
                walkingDirection = 3;
                recorder.keyDownList.Add("Up");
                recorder.keyDownFrame.Add(_frameCount);
                manager.isWalking = true;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                inputVector = new Vector2(0, -1);
                walkingDirection = 2;
                recorder.keyDownList.Add("Down");
                recorder.keyDownFrame.Add(_frameCount);
                manager.isWalking = true;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                inputVector = new Vector2(1, 0);
                walkingDirection = 1;
                recorder.keyDownList.Add("Right");
                recorder.keyDownFrame.Add(_frameCount);
                manager.isWalking = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                inputVector = new Vector2(-1, 0);
                walkingDirection = -1;
                recorder.keyDownList.Add("Left");
                recorder.keyDownFrame.Add(_frameCount);
                manager.isWalking = true;
            }

            manager.direction = walkingDirection;

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                recorder.keyUpList.Add("Up");
                recorder.keyUpFrame.Add(_frameCount);
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                recorder.keyUpList.Add("Down");
                recorder.keyUpFrame.Add(_frameCount);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                recorder.keyUpList.Add("Right");
                recorder.keyUpFrame.Add(_frameCount);
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                recorder.keyUpList.Add("Left");
                recorder.keyUpFrame.Add(_frameCount);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                manager.animator.SetTrigger("Attack");
                recorder.keyDownList.Add("Z");
                manager.Attack();
                recorder.keyDownFrame.Add(_frameCount);
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isTrailActive)
            {
                isTrailActive = true;
                StartCoroutine(ActivateTrail(startDashTime));
                dashTime = startDashTime;
                recorder.keyDownList.Add("Space");
                recorder.keyDownFrame.Add(_frameCount);
            }

            if (!Input.anyKey)
            {
                inputVector = Vector2.zero;
                manager.isWalking = false;
            }

            _position = transform.position;
            Vector3 movement = new Vector3(inputVector.x, inputVector.y, 0);
            _position += movement * (_speed * time);

            float clampedX = Mathf.Clamp(_position.x, -zoneSize / 2, zoneSize / 2);
            float clampedY = Mathf.Clamp(_position.y, -zoneSize / 2, zoneSize / 2);
            _position = new Vector3(clampedX, clampedY, _position.z);
            transform.position = _position;
            
            _recordedPositions.Add(_position);
        }
        
        if (dashTime <= 0)
        {
            _speed = moveSpeed;
        }
        else
        {
            if (!manager.isWalking)
            {
                if (manager.direction == 1)
                {
                    inputVector = new Vector2(1, 0);
                }
                else if (manager.direction == -1)
                {
                    inputVector = new Vector2(-1, 0);
                }
                else if (manager.direction == 2)
                {
                    inputVector = new Vector2(0, -1);
                }
                else if (manager.direction == 3)
                {
                    inputVector = new Vector2(0, 1);
                }
            }
            dashTime -= Time.deltaTime;
            _speed = dashSpeed;
        }
    }

    private void StartPlayback()
    {
        transform.position = new Vector3(0, -1, -12.8f);
        StartCoroutine(PlaybackRoutine());
    }

    private IEnumerator PlaybackRoutine()
    {
        foreach (Vector3 recordedPosition in _recordedPositions)
        {
            Vector3 targetPosition = new Vector3(recordedPosition.x, recordedPosition.y, transform.position.z);
            float timeElapsed = 0f;
            float playbackDuration = 0.001f; // Adjust this value for playback speed.

            while (timeElapsed < playbackDuration)
            {
                float t = timeElapsed / playbackDuration;
                transform.position = Vector3.Lerp(transform.position, targetPosition, t);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
        }
        Debug.Log("Done");
    }
    
    public IEnumerator ActivateTrail(float trailTime)
    {
        CreateTrail();
        yield return new WaitForSeconds(trailTime/2);
        CreateTrail();
        yield return new WaitForSeconds(trailTime/2);
        CreateTrail();
        isTrailActive = false;
    }

    private void CreateTrail()
    {
        GameObject gObj = new GameObject();
        gObj.transform.position = transform.position;
        gObj.transform.localScale = new Vector3(1, 1, 1);
        SpriteRenderer targetRenderer = gObj.AddComponent<SpriteRenderer>();
        targetRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        StartCoroutine(FadeOut(targetRenderer.color, targetRenderer, gObj));

        if (manager.direction < 0)
        {
            targetRenderer.flipX = true;
        }
        
        targetRenderer.sortingOrder = 2;
    }

    private IEnumerator FadeOut(Color originalColor, SpriteRenderer spriteRenderer, GameObject gObj)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float newOpacity = Mathf.Lerp(1.0f, 0.0f, elapsedTime / duration);
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, newOpacity);

            spriteRenderer.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final opacity is fully transparent
        Color finalColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.0f);
        spriteRenderer.color = finalColor;
        Destroy(gObj);
    }
}
