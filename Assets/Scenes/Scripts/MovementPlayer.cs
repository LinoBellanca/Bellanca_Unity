using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementPlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float crouchSpeed = 3f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Crouch Settings")]
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;

    [Header("Animation Settings")]
    public Animator animator;
    public float jumpButtonGracePeriod = 0.1f;
    public float jumpHorizontalSpeed = 2f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private float ySpeed;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    private bool isJumping;
    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAnimation();
        HandleCameraRotation();
    }

    void HandleMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Crouch handling
        if (Input.GetKey(KeyCode.C))
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }
    }

    void HandleJump()
    {
        if (characterController.isGrounded)
        {
            // Quand le personnage est au sol
            lastGroundedTime = Time.time;
            ySpeed = -0.5f;  // Légère pression vers le sol pour éviter de flotter

            if (Input.GetButtonDown("Jump"))
            {
                // Détecter l'entrée de saut
                jumpButtonPressedTime = Time.time;
            }

            // Vérifier si le saut peut être déclenché après la période de grâce
            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpPower;  // Appliquer la puissance de saut
                animator.SetBool("IsJumping", true);  // Déclencher l'animation de saut
                isJumping = true;
                jumpButtonPressedTime = null;  // Réinitialiser la période de grâce
            }
            else
            {
                // Si on est au sol et que le saut n'est pas déclenché, réinitialiser l'animation de saut
                animator.SetBool("IsJumping", false);
                isJumping = false;
            }
        }
        else
        {
            // Si le personnage est en l'air, appliquer la gravité
            ySpeed -= gravity * Time.deltaTime;

            // Animation de chute quand le personnage est en l'air et descend
            if ((isJumping && ySpeed < 0) || ySpeed < -2)
            {
                animator.SetBool("IsFalling", true);
            }
        }

        // Appliquer la vitesse verticale (y) au mouvement global
        moveDirection.y = ySpeed;

        // Déplacer le personnage
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleAnimation()
    {
        animator.SetFloat("Speed", characterController.velocity.magnitude);

        if (characterController.isGrounded)
        {
            animator.SetBool("IsGrounded", true);
            animator.SetBool("IsFalling", false);

            // Réinitialiser IsJumping lorsqu'on touche le sol
            if (isJumping)
            {
                animator.SetBool("IsJumping", false);
                isJumping = false;
            }
        }
        else
        {
            animator.SetBool("IsGrounded", false);

            // Déclencher l'animation de chute si le personnage est en l'air et descend
            if ((isJumping && ySpeed < 0) || ySpeed < -2)
            {
                animator.SetBool("IsFalling", true);
            }
        }
    }

    void HandleCameraRotation()
    {
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}

