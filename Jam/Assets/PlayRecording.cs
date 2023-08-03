using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRecording : MonoBehaviour
{
    public TestingMovement _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<TestingMovement>();
    }

    // Update is called once per frame
    public void ChangeRecording()
    {
        _player.recording = false;
        _player.StartPlayback();
    }
}
