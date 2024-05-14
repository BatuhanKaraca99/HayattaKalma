using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip gunSound;
    public AudioClip reloadSound;
    public AudioClip pickSound;

    public void Fire()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = gunSound;
            audioSource.Play();
        }
    }

    public void Reload()
    {
        audioSource.clip = reloadSound;
        audioSource.PlayOneShot(reloadSound);
    }

    public void AmmoPick()
    {
        audioSource.clip = pickSound;
        audioSource.PlayOneShot(pickSound);
    }
}
