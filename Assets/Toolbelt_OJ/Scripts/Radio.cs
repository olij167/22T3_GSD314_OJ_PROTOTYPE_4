using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Radio : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource audioSource;

    public List<AudioClip> audioToCycle;
    int count;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayAudioClip()
    {
        StopAudioSource();

        audioSource.PlayOneShot(audioClip);
    }
    public void PlayAudioSource()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void PlayNewAudioCountUp()
    {
        audioSource.clip = audioToCycle[count];
        if (count < audioToCycle.Count)
        {
            count++;
        }
        else
        {
            count = 0;
        }
    }

    public void PlayNewAudioCountDown()
    {
        audioSource.clip = audioToCycle[count];
        if (count > 0)
        {
            count--;
        }
        else
        {
            count = audioToCycle.Count - 1;
        }
    }

    public void StopAudioSource()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
