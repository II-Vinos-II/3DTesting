using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;

    public Animator animator;

    float speedZ;
    float speedX;

    public float _Speed = 3f;
    public float rotateSpeed = 3.0f;

    public float turnSmoothTime = .1f;
    public float turnSmootheVelocity;

    public Transform cam;

    Vector3 Direction;
    float Forward;
    float Side;
    public float smoothSpeed;
    Vector3 relativeVector;

    public float velocityY;
    public float groundedGravity = -.5f;
    public float gravityMultiplier = 2;
    public float gravity = 9.81f;
    public bool inAir;
    public bool isLanded;

    //PlayerIndex playerIndex;

    enum PlayerStatus
    {
        Idle,
        Moving,
        Fall,
        HadrFall
    }
    [SerializeField] PlayerStatus status;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        print(controller.isGrounded);

        if(transform.position.y > 1.1f && !controller.isGrounded)
        {
            animator.Play("Fall");
            isLanded = false;
        }
        if (transform.position.y < 1.1f && controller.isGrounded && !isLanded)
        {
            animator.Play("Landing");
            isLanded = true;
        }


        GatherInput();
        ApplyMovement();
        ApplyRotation();
        HandleGravityAndJump(); 
        animator.SetFloat("Speed", Direction.sqrMagnitude);

    }
    private void GatherInput()
    {
        Forward = Input.GetAxis("Horizontal");
        Side = Input.GetAxis("Vertical");
        Direction = new Vector3(Forward, 0, Side).normalized;
    }
    private void ApplyMovement()
    {
            speedX = (Forward);
            speedZ = (Side);
            
            Vector3 forward = cam.transform.forward;
            Vector3 right = cam.transform.right;
            
            forward.y = 0f;
            right.y = 0f;
            
            forward = forward.normalized;
            right = right.normalized;
            controller.Move(relativeVector * _Speed * Time.deltaTime);
            relativeVector = (speedZ * forward) + (speedX * right);
            Direction.x = Input.GetAxisRaw("Horizontal");
            Direction.z = Input.GetAxisRaw("Vertical");


            if (Direction != Vector3.zero)
            {
                animator.SetFloat("Horizontal", Direction.x);
                animator.SetFloat("Vertical", Direction.z);
            }
            else
            {
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", 0);
            }
          
    }

    private void ApplyRotation()
    {
        float targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, targetAngle, 0), rotateSpeed * Time.deltaTime);
    }

    public void HandleGravityAndJump()
    {
        if (controller.isGrounded && velocityY < 0f)
            velocityY = groundedGravity;

        velocityY -= gravity * gravityMultiplier * Time.deltaTime;
        controller.Move(Vector3.up * velocityY * Time.deltaTime);
    }
}
