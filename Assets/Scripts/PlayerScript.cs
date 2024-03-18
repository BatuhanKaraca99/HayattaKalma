using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 1.9f;

    [Header("Player Script Cameras")]
    public Transform playerCamera; //Player Camera Reference

    [Header("Player Animator and Gravity")]
    public CharacterController cC;

    [Header("Player Jumping and Velocity")]
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;

    private void FixedUpdate()
    {
        playerMove();
    }

    void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y; //Find angle from x/z(horizontal/vertical) and rotation of camera
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime); //Gradually rotation
            transform.rotation = Quaternion.Euler(0f, angle, 0f); //Apply transform
                                                                  //
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //get camera angle vector
            cC.Move(moveDirection * playerSpeed * Time.deltaTime);
        }
    }
}
