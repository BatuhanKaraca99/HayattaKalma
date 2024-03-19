using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 1.9f;
    public float playerSprint = 3f;

    [Header("Player Script Cameras")]
    public Transform playerCamera; //Player Camera Reference

    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;

    [Header("Player Jumping and Velocity")]
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    public float jumpRange = 1f;
    Vector3 velocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;


    private void FixedUpdate()
    {
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask); //Surface check

        if(onSurface && velocity.y < 0)
        {
            velocity.y = - 2f;
        }

        velocity.y += gravity * Time.deltaTime;
        cC.Move(velocity*Time.deltaTime); // Move character with gravity

        playerMove();

        Jump();
        Sprint();
    }

    void playerMove()
    {
        GetAxis(playerSpeed);
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && onSurface)
        {
            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
        }
    }

    void Sprint()
    {
        if(Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && onSurface)
        {
            GetAxis(playerSprint);
        }
    }

    void GetAxis(float speed)
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y; //Find angle from x/z(horizontal/vertical) and rotation of camera
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime); //Gradually rotation
            transform.rotation = Quaternion.Euler(0f, angle, 0f); //Apply transform
                                                                  //
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //get camera angle vector
            cC.Move(moveDirection * speed * Time.deltaTime);
        }
    }
}
