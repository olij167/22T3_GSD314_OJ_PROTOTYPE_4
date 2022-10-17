//from tutorial: https://www.youtube.com/watch?v=f473C43s8nE&ab_channel=Dave%2FGameDevelopment


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Toolbelt_OJ;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 25f;
    private float baseSpeed = 25f;
    [SerializeField] private float runSpeed = 40f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private bool canRun;
    [SerializeField] private float stamina = 5f;
    private float maxStamina = 5f;
    [SerializeField] private float staminaDecreaseRate = 1.25f, staminaIncreaseRate = 0.75f;
    [SerializeField] private Transform orientation;


    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float airMultiplier = 0.4f;
    [SerializeField] private bool canJump = true;

    [Header("Key Bindings")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool isGrounded;

    [Header("Audio & UI")]

    [SerializeField] private List<AudioClip> footstepSounds, jumpSounds;
    AudioSource audioSource;
    [SerializeField] private AudioSource staminaSource;

    [SerializeField] private Slider staminaBar;
    [SerializeField] private GameObject zoomText;

    [SerializeField] private UIController staminaUIController;



    private float horizontalInput, verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        audioSource = GetComponent<AudioSource>();

        if (!canRun)
        {
            if(staminaBar != null && staminaBar.gameObject.activeSelf)
            {
                staminaBar.gameObject.SetActive(false);
            }
        }
        else
        {
            baseSpeed = moveSpeed;
            maxStamina = stamina;
            staminaBar.maxValue = maxStamina;
        }
    }

    private void Update()
    {
        //Check if grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MovementInput();
        ControlSpeed();

        //Apply drag
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        } 
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovementInput()
    {

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jump
        if (Input.GetKey(jumpKey) && canJump && isGrounded)
        {
            canJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (canRun)
        {
            Sprint();
        }
    }

    void MovePlayer()
    {
        // Get forward movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);

        if (rb.velocity.magnitude > 2f && !audioSource.isPlaying)
        {
            audioSource.volume = Random.Range(0.25f, 0.35f);
            audioSource.pitch = Random.Range(1f, 1.2f);
            audioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Count)]);
        }

    }

    void ControlSpeed()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity
        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump() //fix jump sounds
    {
        //reset y velocity
        if (canJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Count)]);
        }
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void Sprint()
    {
        switch (canRun && Input.GetKey(KeyCode.LeftShift))
        {
            case true:
                {
                    if (stamina >= 0f)
                    {
                        moveSpeed = runSpeed;
                        stamina -= Time.deltaTime * staminaDecreaseRate;
                        zoomText.SetActive(true);
                        audioSource.pitch = Random.Range(1.5f, 1.7f);
                        staminaUIController.sizeLerp = true;

                    }
                    else
                    {
                        moveSpeed = baseSpeed;
                        zoomText.SetActive(false);
                        staminaUIController.sizeLerp = false;
                        staminaUIController.ResetSize(staminaBar.image, staminaUIController.imageStartScale);

                        if (!staminaSource.isPlaying)
                            staminaSource.Play();
                    }
                    break;
                }
            case false:
                {
                    moveSpeed = baseSpeed;
                    zoomText.SetActive(false);
                    staminaUIController.sizeLerp = false;
                    staminaUIController.ResetSize(staminaBar.image, staminaUIController.imageStartScale);

                    if (stamina <= maxStamina)
                    {
                        stamina += Time.deltaTime * staminaIncreaseRate;
                    }
                    break;
                }
        }

        staminaBar.value = stamina;
    }

    private void ResetMoveSpeed()
    { 

    }

    public float ChangePlayerSpeed(float newSpeed, bool isRunSpeed)
    {
        if (!isRunSpeed)
        {
            return moveSpeed = newSpeed;
        }
        else
        {
            return runSpeed = newSpeed;
        }
    }
}
