using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayRecording : MonoBehaviour
{
    public TestingMovement player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TestingMovement>();
    }

    // Update is called once per frame
    public void ChangeRecording()
    {
        player.recording = false;
        player.StartPlayback();
    }
}
