using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTestBehavior : MonoBehaviour
{
    // private GameObject[] cubes;

    private const int N_FFT_BINS = 64;
    private const int N_SAMPLE_REGIONS = 64;
    private const double SCALE_FACTOR = 0.04;

    private string audio_file_loc = "Assets/chanceonus_bar.wav"; // intentionally not git-ed

    private AudioSource source;

    private AudioClip clip;

    private float duration_seconds;
    private int sample_rate;
    private int duration_samples;

    private GameObject[,] amplitude_surface;

    void Start()
    {
        // clip  = Resources.Load<AudioClip>(audio_file_loc);
        source = GetComponent<AudioSource>();
        // source.clip = clip;
        // source.Play(0);
        // Debug.Log("Started playback");

        // duration_seconds = clip.length;
        // sample_rate = clip.frequency;
        // duration_samples = clip.samples;

        amplitude_surface = new GameObject[N_FFT_BINS, N_SAMPLE_REGIONS];

        for (int i = 0; i < N_FFT_BINS; i++)
        {
            for (int j = 0; j < N_SAMPLE_REGIONS; j++)
            {
                amplitude_surface[i, j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // amplitude_surface[i, j].name = "Cube" + (i + 1).ToString() + "_" + (j + 1).ToString();
                // amplitude_surface[i, j].transform.Rotate(0.0f, 1f, 0.0f);
                // amplitude_surface[i, j].transform.position = new Vector3((float)(SCALE_FACTOR * i + SCALE_FACTOR * N_FFT_BINS / 2), (float)(SCALE_FACTOR * j - SCALE_FACTOR * N_SAMPLE_REGIONS / 2), 0);
                // amplitude_surface[i, j].GetComponent<Renderer>().material.color = new Color(16, 16, 16);
                amplitude_surface[i, j].GetComponent<Renderer>().material.color = new Color((((float)i) / N_FFT_BINS) * 6, (((float)j) / N_FFT_BINS) * 6, 0);
                amplitude_surface[i, j].transform.position = new Vector3((float)(SCALE_FACTOR * i - SCALE_FACTOR * N_FFT_BINS / 2) * 2, -4, (float)(SCALE_FACTOR * j - SCALE_FACTOR * N_FFT_BINS / 2) * 2);
                amplitude_surface[i, j].transform.localScale = new Vector3((float)SCALE_FACTOR, 1, (float)SCALE_FACTOR);
            }
            if (i % 128 == 0)
            {
                Debug.Log("Creating a cube");
            }
        }

        Debug.Log("Created some cubes!");

        // Transform myTransform = 

        // Camera.transform.position = new Vector3(0, 0, 0);
        // Camera.main.GetComponent<Transform>().Rotate(45, 0, 0);
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