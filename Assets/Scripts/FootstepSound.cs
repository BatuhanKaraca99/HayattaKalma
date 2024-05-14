using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footstepSound;

    public void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") >= 0.1f || Input.GetAxis("Vertical") >= 0.1f)
        {
            Step();
        }
    }

    public void Step()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = footstepSound;
            audioSource.Play();
        }
        if (audioSource.isPlaying)
        {
            audioSource.loop = true;   
        }
        audioSource.loop = false;
    }
}
