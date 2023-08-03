using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestingMovement : MonoBehaviour
{
    public float moveSpeed = 4;
    public float zoneSize = 1024;
    public bool recording;
    
    private Vector2 _inputVector;
    private Vector3 _position;
    private readonly List<Vector3> _recordedPositions = new List<Vector3>();

    private void Update()
    {
        if (recording)
        {
            _position = transform.position;
            Vector3 movement = new Vector3(_inputVector.x, _inputVector.y, 0);
            _position += movement * (moveSpeed * Time.fixedDeltaTime);
        
            float clampedX = Mathf.Clamp(_position.x, -zoneSize / 2, zoneSize / 2);
            float clampedY = Mathf.Clamp(_position.y, -zoneSize / 2, zoneSize / 2);
            transform.position = new Vector3(clampedX, clampedY, _position.z);
        
            if (recording)
            {
                _recordedPositions.Add(transform.position);
            }
        }
    }

    public void Movement(InputAction.CallbackContext context)
    {
        Debug.Log("Hi");
        
        if (recording)
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

    public void StartPlayback()
    {
        transform.position = new Vector3(-0.24f, -0.8f, -12.8f);
        StartCoroutine(PlaybackRoutine());
    }

    private IEnumerator PlaybackRoutine()
    {
        foreach (Vector3 recordedPosition in _recordedPositions)
        {
            Vector3 targetPosition = new Vector3(recordedPosition.x, recordedPosition.y, transform.position.z);
            float timeElapsed = 0f;
            float playbackDuration = 0.000001f; // Adjust this value for playback speed.

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
}
