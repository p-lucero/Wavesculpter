using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTestBehavior : MonoBehaviour
{
    // private GameObject[] cubes;

    private static string audio_file_loc = "Assets/chanceonus_bar.wav" // intentionally not git-ed

    private AudioSource source;

    private AudioClip clip;

    private float duration_seconds;
    private int sample_rate;
    private int duration_samples;

    void Start()
    {
        // clip  = Resources.Load<AudioClip>(audio_file_loc);
        source = GetComponent<AudioSource>();
        // source.clip = clip;
        // source.Play(0);
        // Debug.Log("Started playback");

        duration_seconds = clip.length;
        sample_rate = clip.frequency;
        duration_samples = clip.samples;
    }

    void Awake()
    {
        
    }

    void Update()
    {
        
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 150, 30), "Pause"))
        {
            source.Pause();
            Debug.Log("Pause: " + source.time);
        }

        if (GUI.Button(new Rect(10, 170, 150, 30), "Continue"))
        {
            source.UnPause();
        }
    }
}