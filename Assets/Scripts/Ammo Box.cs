using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [Header("AmmoLoad")]
    public Rifle rifle;
    private int ammoToGive = 50;
    private float radius = 2.5f;

    [Header("Sounds")]
    public AudioClip ammoLoadSound;
    public AudioSource audioSource;

    [Header("AmmoLoad Animator")]
    public Animator animator;

    private void Update()
    {
        if (Vector3.Distance(transform.position, rifle.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.F) && rifle.mag != 50)
            {
                animator.SetBool("Open", true);
                rifle.mag = ammoToGive;
                AmmoCount.occurrence.UpdateMagText(ammoToGive);

                //sound effect
                audioSource.PlayOneShot(ammoLoadSound);

                Object.Destroy(gameObject, 1.5f);
            }
        }
    }
}
