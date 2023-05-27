using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStats playerStats;

    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 2.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmootTime = 0.3f;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmootTime = 0.03f;

    //JUMP
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] float jumpHeight = 1.0f;
    [SerializeField] float groundRaycastDistance = 0.2f;
    bool isJumping = false;

    [SerializeField] bool lockCursor = true;

    public float cameraPitch = 0.0f;
    public float velocityY = 0.0f;

    CharacterController controller = null;
    public bool isRunning = false;
    public bool isCrouching = false;
    public const float StandingHeight = 2.0f;
    public const float CrouchingHeight = 1.0f;

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
        isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);
        isCrouching = Input.GetKey(KeyCode.LeftControl);

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmootTime);

        if (controller.isGrounded)
        {
            velocityY = 0.0f;

            if (Input.GetButtonDown("Jump"))
            {
                velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
                isJumping = true;
            }
            else
            {
                isJumping = false;
            }
        }

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * (isRunning ? walkSpeed * 2 : walkSpeed) + Vector3.up * velocityY;

        if (isCrouching)
        {
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
    }
}
