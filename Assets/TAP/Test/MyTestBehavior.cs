using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class MyTestBehavior : MonoBehaviour
{
    private const int N_FFT_BINS = 64;
    private const int N_SAMPLE_REGIONS = 64;
    private const double HORIZONTAL_CUBE_SCALE = 0.04;
    private const double VERTICAL_CUBE_SCALE = 3;
    private const double FFT_SCALE_BASE = 1.087;

    private string audio_file_loc = "Assets/chanceonus_bar.wav"; // intentionally not git-ed

    private AudioSource source;
    private AudioClip clip;
    private AudioMixer mixer;

    private float duration_seconds;
    private int sample_rate;
    private int duration_samples;
    private int samples_elapsed = 0;
    private int current_sample_set = 0;

    private GameObject[,] amplitude_surface;
    private float[] fft_samples;

    private int m_frameCounter = 0;
    private float m_timeCounter = 0.0f;
    private float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;

    void Start()
    {
        // clip  = Resources.Load<AudioClip>(audio_file_loc);
        source = GetComponent<AudioSource>();
        clip = source.clip;
        // source.clip = clip;
        // source.Play(0);
        // Debug.Log("Started playback");

        duration_seconds = clip.length;
        sample_rate = clip.frequency;
        duration_samples = clip.samples;

        amplitude_surface = new GameObject[N_FFT_BINS, N_SAMPLE_REGIONS];

        for (int i = 0; i < N_FFT_BINS; i++)
        {
            for (int j = 0; j < N_SAMPLE_REGIONS; j++)
            {
                amplitude_surface[i, j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // amplitude_surface[i, j].name = "Cube" + (i + 1).ToString() + "_" + (j + 1).ToString();
                // amplitude_surface[i, j].transform.Rotate(0.0f, 1f, 0.0f);
                // amplitude_surface[i, j].transform.position = new Vector3((float)(HORIZONTAL_CUBE_SCALE * i + HORIZONTAL_CUBE_SCALE * N_FFT_BINS / 2), (float)(HORIZONTAL_CUBE_SCALE * j - HORIZONTAL_CUBE_SCALE * N_SAMPLE_REGIONS / 2), 0);
                // amplitude_surface[i, j].GetComponent<Renderer>().material.color = new Color(16, 16, 16);
                amplitude_surface[i, j].GetComponent<Renderer>().material.color = new Color((((float)i) / N_FFT_BINS) * 6, (((float)j) / N_SAMPLE_REGIONS) * 6, 0);
                amplitude_surface[i, j].transform.position = new Vector3((float)(HORIZONTAL_CUBE_SCALE * i - HORIZONTAL_CUBE_SCALE * N_FFT_BINS / 2) * 2, 0, (float)(HORIZONTAL_CUBE_SCALE * j - HORIZONTAL_CUBE_SCALE * N_SAMPLE_REGIONS / 2) * 2);
                amplitude_surface[i, j].transform.localScale = new Vector3((float)HORIZONTAL_CUBE_SCALE, (float)VERTICAL_CUBE_SCALE, (float)HORIZONTAL_CUBE_SCALE);
            }
            if (i % 128 == 0)
            {
                Debug.Log("Creating a cube");
            }
        }

        Debug.Log("Created some cubes!");

        fft_samples = new float[N_FFT_BINS];

        Camera.main.GetComponent<Transform>().LookAt(amplitude_surface[N_FFT_BINS / 2, N_SAMPLE_REGIONS / 2].GetComponent<Transform>());

        mixer = source.outputAudioMixerGroup.audioMixer;
    }

    void Awake()
    {
        
    }

    Color amplitude_to_RGB(double l)
    {

        l *= 0.29f;
        return Color.HSVToRGB((l < .75f ? .05f + (float)l : .95f), 1, 4);
    }

    void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
        }
        // if (samples_elapsed < duration_samples)
        // {
            // int samples_this_update = (int)(m_lastFramerate * sample_rate);
            if (current_sample_set != N_SAMPLE_REGIONS) 
            {
                source.GetSpectrumData(fft_samples, 0, FFTWindow.Blackman);
                for (int i = 0; i < N_FFT_BINS; i++)
                {
                    amplitude_surface[i, current_sample_set].transform.localScale = new Vector3((float)HORIZONTAL_CUBE_SCALE, (float)(fft_samples[i] * VERTICAL_CUBE_SCALE * (Mathf.Pow((float)FFT_SCALE_BASE, i))), (float)HORIZONTAL_CUBE_SCALE);
                    amplitude_surface[i, current_sample_set].GetComponent<Renderer>().material.color = amplitude_to_RGB((float)(fft_samples[i] * VERTICAL_CUBE_SCALE * (Mathf.Pow((float)FFT_SCALE_BASE, i))));
                }

                // samples_elapsed += samples_this_update;
                current_sample_set++;
                if (current_sample_set == N_SAMPLE_REGIONS)
                    { current_sample_set = 0; }
            }
        // }
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

    public void Play()
    {
        source.UnPause();
    }

    public void Pause()
    {
        source.Pause();
    }

    public void VolumeUp()
    {
        float foofloat = 0;
        mixer.GetFloat("vol", out foofloat);
        mixer.SetFloat("vol", foofloat + 1.5f);
    }

    public void VolumeDown()
    {
        float foofloat = 0;
        mixer.GetFloat("vol", out foofloat);
        mixer.SetFloat("vol", foofloat - 1.5f);
    }
}