using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip footstepSound;

    bool walking = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if(Input.GetKey(KeyCode.W)
          || Input.GetKey(KeyCode.A)
          || Input.GetKey(KeyCode.S)
          || Input.GetKey(KeyCode.D))
        {
            audioSource.volume = 0.1f;
            if(walking == false)
            {
                InvokeRepeating("Step", 1f, 1.3f);
            }
            walking = true;
        }
        else
        {
            audioSource.volume = 0f;
            walking = false;
        }
    }

    public void Step()
    {
        audioSource.PlayOneShot(footstepSound);
    }

}
