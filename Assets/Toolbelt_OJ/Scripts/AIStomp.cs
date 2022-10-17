//sounds from: https://freesound.org/people/JanKoehl/sounds/85600/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
//[RequireComponent(typeof(CharacterController))]
public class AIStomp : MonoBehaviour
{
    private AudioSource audioSource;
    //private CharacterController controller;
    private Rigidbody rb;

    [SerializeField] private List<AudioClip> footstepSounds;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.velocity.magnitude > 2f && !audioSource.isPlaying)
        {
            audioSource.volume = Random.Range(0.25f, 0.35f);
            audioSource.pitch = Random.Range(0.7f, 0.8f);
            audioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Count)]);
        }
    }
}
