using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;

    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private float mouseSensitivity = 3.5f;
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed;
    [SerializeField] private float gravity = -6.0f;
    [SerializeField][Range(0.0f, 0.5f)] public float moveSmootTime = 0.3f;
    [SerializeField][Range(0.0f, 0.5f)] public float mouseSmootTime = 0.03f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private bool lockCursor = true;
    [SerializeField] public float CrouchingHeight = 0.1f;

    private CharacterController controller = null;
    private float cameraPitch = 0.0f;
    private float velocityY = 0.0f;
    public bool isJumping = false;
    private bool isRunning = false;
    private bool isCrouching = false;
    private float StandingHeight = 2.0f;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    public void Start()
    {
        controller = GetComponent<CharacterController>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        playerStats.playerController = this;
    }

    public void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    public void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmootTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    public void UpdateMovement()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && !isCrouching;

        isCrouching = Input.GetKey(KeyCode.LeftControl);

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmootTime);

        if (controller.isGrounded)
        {
            velocityY = 0.0f;

            if (Input.GetButtonDown("Jump") && !isCrouching)
            {
                velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
                isJumping = true;
            }
            else
            {
                isJumping = false;
            }
        }

        if (transform.position.y < 9.7f)
        {
            isRunning = false;
            if (Input.GetKey(KeyCode.Space)) 
            {
                velocityY = Mathf.Lerp(velocityY, 2.0f, 0.1f); 
            }
            else if (Input.GetKey(KeyCode.LeftControl)) 
            {
                velocityY = Mathf.Lerp(velocityY, -2.0f, 0.1f); 
            }
            else
            {
                velocityY = Mathf.Lerp(velocityY, 0.0f, 0.1f); 
            }
        }
        else
        {
            velocityY += gravity * Time.deltaTime;
        }

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * (isRunning ? runSpeed : walkSpeed) + Vector3.up * velocityY;

        if (isCrouching)
        {
            isRunning = false;
            isJumping = false;

            velocity *= 0.5f;
        }

        float targetHeight = isCrouching ? CrouchingHeight : StandingHeight;
        float currentHeight = controller.height;
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, 10f * Time.deltaTime);
        controller.height = currentHeight;

        controller.Move(velocity * Time.deltaTime);

        if (isRunning && playerStats.UpdatedStamina > 0)
        {
            playerStats.UpdatedStamina -= Time.deltaTime;
            if (playerStats.UpdatedStamina < 0)
            {
                playerStats.UpdatedStamina = 0;
            }
        }

        if (playerStats.UpdatedStamina <= 0)
        {
            runSpeed = 2;
        }
        else
        {
            runSpeed = 4;
        }
    }
}
