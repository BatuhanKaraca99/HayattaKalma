using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBox : MonoBehaviour
{
    [Header("HealthBoost")]
    public PlayerScript player;
    private float healthToGive = 120;
    private float radius = 2.5f;

    [Header("Sounds")]
    public AudioClip HealthBoostSound;
    public AudioSource audioSource;

    [Header("HealthBox Animator")]
    public Animator animator;

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.F) && player.presentHealth != 120)
            {
                animator.SetBool("Open", true);
                player.presentHealth = healthToGive;
                player.healthBar.SetHealth(healthToGive);

                //sound effect
                audioSource.PlayOneShot(HealthBoostSound);

                Object.Destroy(gameObject, 1.5f);
            }
        }
    }
}
