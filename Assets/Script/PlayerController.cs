using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;

    //public Animator animator;

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

    Vector3 relativeVector;

    public float velocityY;
    public float groundedGravity = -.5f;
    public float gravityMultiplier = 2;
    public float gravity = 9.81f;

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
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();
        ApplyMovement();
        ApplyRotation();
        HandleGravityAndJump();
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

        relativeVector = (speedZ * forward) + (speedX * right);

        if (Direction.magnitude >= 0.1f)
        {
            print(speedZ);
            controller.Move(relativeVector * _Speed * Time.deltaTime);
        }
    }

    private void ApplyRotation()
    {
        float targetAngle = Mathf.Atan2(Side, Forward) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
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
