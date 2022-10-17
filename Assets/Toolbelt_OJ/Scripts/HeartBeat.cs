using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HeartBeat : MonoBehaviour
{
    private AudioSource audioSource;

    private GameObject chaser, player;
    [SerializeField] private float heartBeatDistance, maxVolume;
    [SerializeField] private string chaserTag;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = 0f;

        chaser = GameObject.FindGameObjectWithTag(chaserTag);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //Debug.Log("Distance = " + Vector3.Distance(transform.position, chaser.transform.position));

        if (Vector3.Distance(transform.position, chaser.transform.position) <= heartBeatDistance)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            if (audioSource.volume <= maxVolume)
            {
                audioSource.volume += Time.deltaTime;
            }
        }
        else if (Vector3.Distance(transform.position, chaser.transform.position) >= heartBeatDistance && audioSource.isPlaying)
        {
            audioSource.volume -= Time.deltaTime;

            if (audioSource.volume >= 0)
            {
                audioSource.Stop();
            }
        }
    }
}
