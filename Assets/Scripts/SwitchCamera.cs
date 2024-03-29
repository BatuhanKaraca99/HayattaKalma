using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [Header("Camera to Assign")]
    public GameObject AimCam;
    public GameObject AimCanvas;
    public GameObject ThirdPersonCam;
    public GameObject ThirdPersonCanvas;

    [Header("Camera Animator")]
    public Animator animator;

    private void Update()
    {
        if(Input.GetButton("Fire2") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            AimAnimation(true);
        }
        else if (Input.GetButton("Fire2"))
        {
            AimAnimation(false);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("IdleAim", false);
            animator.SetBool("RifleWalk", false);

            ThirdPersonCam.SetActive(true);
            ThirdPersonCanvas.SetActive(true);
            AimCam.SetActive(false);
            AimCanvas.SetActive(false);
        }
    }

    private void AimAnimation(bool walking) //walking? and aiming
    {
        animator.SetBool("RifleWalk", walking);
        animator.SetBool("Walk", walking);

        animator.SetBool("Idle", false);
        animator.SetBool("IdleAim", true);

        ThirdPersonCam.SetActive(false);
        ThirdPersonCanvas.SetActive(false);
        AimCam.SetActive(true);
        AimCanvas.SetActive(true);
    }

}
