using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraFollow;

    Rigidbody rb;
    CapsuleCollider capsuleCol;
    [SerializeField] PlayerLandInput input;
    [SerializeField] CameraController camController;

    Vector3 playerMoveInput;

    Vector3 playerLookInput;
    Vector3 previousPlayerLookInput;
    float cameraPitch;

    [Header("PlayerLook")]
    [SerializeField] private float normalSensitivity = 1f;
    [SerializeField] private float aimSensitivity = 0.5f;
    [SerializeField] float playerLookInputLerpTime = 0.35f;
    [HideInInspector] public float sensitivity;

    [Header("Movement")]
    [SerializeField] float movementMultiplier = 30f;
    [SerializeField] float airMovementMultieplier = 1.25f;
    [SerializeField] float runMultiplier = 2.5f;
    [SerializeField] float rotationSpeedMultiplier = 100f;
    [SerializeField] float pitchSpeedMultiplier = 180f;

    [Header("Ground Check")]
    [SerializeField] bool isGrounded = false;
    [SerializeField][Range(0f, 1.8f)] float groundCheckRadiusMultiplier = 0.9f;
    [SerializeField][Range(-0.95f, 1.05f)] float groundCheckDistance = 0.05f;
    RaycastHit groundCheckHit = new RaycastHit();

    [Header("Gravity")]
    [SerializeField] float gravityFallCurrent = -100f;
    [SerializeField] float gravityFallMin = -100f;
    [SerializeField] float gravityFallMax = -500f;
    [SerializeField][Range(-5f, -35f)] float graviteFallIncrementAmount = -20f;
    [SerializeField] float graviteFallIncrementTime = 0.05f;
    [SerializeField] float playerFallTimer = 0.0f;
    [SerializeField] float gravityGrounded = -1f;
    [SerializeField] float maxSlopeAngle = 47.5f;

    [Header("Jumping")]
    [SerializeField] float initialJumpForceMultiplier = 750;
    [SerializeField] float continueJumpForceMultiplier = 0.1f;
    [SerializeField] float jumpTime = 0.175f;
    [SerializeField] float jumpTimeCounter = 0;
    [SerializeField] float cayoteTime = 0.15f;
    [SerializeField] float cayoteTimeCounter = 0;
    [SerializeField] float jumpBufferTime = 0.2f;
    [SerializeField] float jumpBufferTimeCounter = 0;
    [SerializeField] bool playerIsJumping;
    [SerializeField] bool jumpWasPressedLastFrame;

    [Header("Vault Settings")]
    [SerializeField] LayerMask vaultLayer;
    private float playerHeight = 3f;
    private float playerRadius = 0.5f;
    private float vaultDelay = 0.5f;
    private float vaultCounter;

    [Header("Camera Effects")]
    [SerializeField] float defaultCamFov = 60f;
    [SerializeField] float runCamFov = 80f;
    [SerializeField] float camLerpSpeed = 1f;

    [Header("Headbob Settings")]
    [SerializeField] float bobFrequency = 1.5f;
    [SerializeField] float bobHeight = 0.05f;
    [SerializeField] float bobWidth = 0.05f;
    [SerializeField] Transform followTarget;

    [Header("VFX")]
    [SerializeField] GameObject vaultVfx;

    private Vector3 initialPosition;
    private float timer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCol = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetupHeadbobbing();
    }

    private void FixedUpdate()
    {
        sensitivity = UpdateSensitivity();
        if (!camController.usingOrbitCamera)
        {
            playerLookInput = GetLookInput();
            PlayerLook();
            PitchCamera();
        }
        Vault();

        HeadBobbing();

        playerMoveInput = GetMoveInput();
        isGrounded = PlayerGroundCheck();

        playerMoveInput = PlayerMove();
        playerMoveInput = PlayerSlope();
        playerMoveInput = PlayerRun();

        playerMoveInput.y = PlayerFallGravity();
        playerMoveInput.y = PlayerJump();

        playerMoveInput *= rb.mass;

        rb.AddRelativeForce(playerMoveInput, ForceMode.Force);
    }

    private float UpdateSensitivity()
    {
        return Mathf.Lerp(sensitivity, input.aimIsPressed ? aimSensitivity : normalSensitivity, playerLookInputLerpTime);
    }

    private Vector3 GetLookInput()
    {
        previousPlayerLookInput = playerLookInput;
        Vector3 playerLookInputTemp = new Vector3(input.lookInput.x, -input.lookInput.y, 0.0f);
        return Vector3.Lerp(previousPlayerLookInput, playerLookInputTemp * sensitivity * Time.deltaTime, playerLookInputLerpTime);
    }

    private void PlayerLook()
    {
        rb.rotation = Quaternion.Euler(0.0f, rb.rotation.eulerAngles.y + (playerLookInput.x * rotationSpeedMultiplier), 0.0f);
    }

    private void PitchCamera()
    {
        Vector3 rotationValues = cameraFollow.rotation.eulerAngles;
        cameraPitch += playerLookInput.y * pitchSpeedMultiplier;
        cameraPitch = Mathf.Clamp(cameraPitch, -89f, 89f);

        cameraFollow.rotation = Quaternion.Euler(cameraPitch, rotationValues.y, rotationValues.z);
    }

    private Vector3 GetMoveInput()
    {
        return new Vector3(input.moveInput.x, 0, input.moveInput.y);
    }

    private Vector3 PlayerMove()
    {
        return ((isGrounded) ? (playerMoveInput * movementMultiplier) : (playerMoveInput * movementMultiplier * airMovementMultieplier));
    }

    private bool PlayerGroundCheck()
    {
        float sphereCastRadius = capsuleCol.radius * groundCheckRadiusMultiplier;
        float sphereCastTravelDistance = capsuleCol.bounds.extents.y - sphereCastRadius + groundCheckDistance;
        return Physics.SphereCast(rb.position, sphereCastRadius, Vector3.down, out groundCheckHit, sphereCastTravelDistance);
    }

    private Vector3 PlayerSlope()
    {
        Vector3 calculatedPlayerMovement = playerMoveInput;

        if (isGrounded)
        {
            Vector3 localGroundCheckHitNormal = rb.transform.InverseTransformDirection(groundCheckHit.normal);

            float groundSlopeAngle = Vector3.Angle(localGroundCheckHitNormal, rb.transform.up);

            if (groundSlopeAngle == 0.0f)
            {
                if (input.moveIsPressed)
                {
                    RaycastHit rayHit;
                    float rayHeightFromGround = 0.1f;
                    float rayCalculatedRayHeight = rb.position.y - capsuleCol.bounds.extents.y + rayHeightFromGround;
                    Vector3 rayOrgin = new Vector3(rb.position.x, rayCalculatedRayHeight, rb.position.z);
                    if (Physics.Raycast(rayOrgin, rb.transform.TransformDirection(calculatedPlayerMovement), out rayHit, 0.75f))
                    {
                        if (Vector3.Angle(rayHit.normal, rb.transform.up) > maxSlopeAngle)
                        {
                            calculatedPlayerMovement.y = -movementMultiplier;
                        }
                    }
                }

                if (calculatedPlayerMovement.y == 0)
                {
                    calculatedPlayerMovement.y = gravityGrounded;
                }
            }
            else
            {
                Quaternion slopeAnlgeRotation = Quaternion.FromToRotation(rb.transform.up, localGroundCheckHitNormal);
                calculatedPlayerMovement = slopeAnlgeRotation * calculatedPlayerMovement;

                float relativeSlopeAngle = Vector3.Angle(calculatedPlayerMovement, rb.transform.up) - 90f;
                calculatedPlayerMovement += calculatedPlayerMovement * (relativeSlopeAngle / 90);

                if (groundSlopeAngle > maxSlopeAngle)
                {
                    if (input.moveIsPressed)
                    {
                        calculatedPlayerMovement.y += gravityGrounded;
                    }
                }
                else
                {
                    float calculatedSlopeGravity = groundSlopeAngle * -0.2f;
                    if (calculatedSlopeGravity < calculatedPlayerMovement.y)
                    {
                        calculatedPlayerMovement.y = calculatedSlopeGravity;
                    }
                }
            }
#if UNITY_EDITOR
            Debug.DrawRay(rb.position, rb.transform.TransformDirection(calculatedPlayerMovement), Color.red, 0.5f);
#endif
        }

        return calculatedPlayerMovement;
    }
    private Vector3 PlayerRun()
    {
        Vector3 calculatedPlayerRunSpeed = playerMoveInput;
        if (input.moveIsPressed && input.runIsPressed && input.moveInput.y > 0.4f)
        {
            calculatedPlayerRunSpeed *= runMultiplier;
            CameraFovControl(runCamFov);
        }
        else
        {
            CameraFovControl(defaultCamFov);
        }
        return calculatedPlayerRunSpeed;
    }

    private void CameraFovControl(float fov)
    {
        camController.activeCam.m_Lens.FieldOfView = Mathf.Lerp(camController.activeCam.m_Lens.FieldOfView, fov, camLerpSpeed);
    }

    private float PlayerFallGravity()
    {
        float gravity = playerMoveInput.y;
        if (isGrounded)
        {
            gravityFallCurrent = gravityFallMin;
        }
        else
        {
            playerFallTimer -= Time.deltaTime;
            if (playerFallTimer < 0)
            {
                if (gravityFallCurrent > gravityFallMax)
                {
                    gravityFallCurrent += graviteFallIncrementAmount;
                }
            }
            gravity = gravityFallCurrent;
        }
        return gravity;
    }

    private float PlayerJump()
    {
        float calculatedJumpinput = playerMoveInput.y;

        SetJumpTimeCounter();
        SetCayoteTimeCounter();
        SetJumpBufferTimeCounter();

        if (jumpBufferTimeCounter > 0.0f && !playerIsJumping && cayoteTimeCounter > 0.0f)
        {
            if (Vector3.Angle(rb.transform.up, groundCheckHit.normal) < maxSlopeAngle)
            {
                cayoteTimeCounter = 0.0f;
                calculatedJumpinput = initialJumpForceMultiplier;
                playerIsJumping = true;
                jumpBufferTimeCounter = 0.0f;
            }
        }
        else if (input.jumpIsPressed && playerIsJumping && !isGrounded && jumpTimeCounter > 0.0f)
        {
            calculatedJumpinput = initialJumpForceMultiplier * continueJumpForceMultiplier;
        }
        else if (playerIsJumping && isGrounded)
        {
            playerIsJumping = false;
        }
        return calculatedJumpinput;
    }

    private void SetJumpTimeCounter()
    {
        if (playerIsJumping && !isGrounded)
        {
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            jumpTimeCounter = jumpTime;
        }
    }

    private void SetCayoteTimeCounter()
    {
        if (isGrounded)
        {
            cayoteTimeCounter = cayoteTime;
        }
        else
        {
            cayoteTimeCounter -= Time.deltaTime;
        }
    }

    private void SetJumpBufferTimeCounter()
    {
        if (!jumpWasPressedLastFrame && input.jumpIsPressed)
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferTimeCounter -= Time.deltaTime;
        }
        jumpWasPressedLastFrame = input.jumpIsPressed;
    }

    private void SetupHeadbobbing()
    {
        initialPosition = followTarget.localPosition;
    }

    private void HeadBobbing()
    {
        Vector3 velocity = rb.velocity;
        float horizontalSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        if (horizontalSpeed > 0.1f)
        {
            timer += Time.deltaTime * bobFrequency * horizontalSpeed;
            float offsetY = Mathf.Sin(timer) * bobHeight;
            float offsetX = Mathf.Cos(timer) * bobWidth;

            followTarget.localPosition = initialPosition + new Vector3(offsetX, offsetY, 0);
        }
        else
        {
            timer = 0;
            followTarget.localPosition = Vector3.Lerp(followTarget.localPosition, initialPosition, Time.deltaTime * 5f);
        }
    }

    private void Vault()
    {
        if (vaultCounter <= 0)
        {
            if (Physics.Raycast(cameraFollow.transform.position, cameraFollow.transform.forward, out var firstHit, 1.5f, vaultLayer))
            {
                if (Physics.Raycast(firstHit.point + (cameraFollow.transform.forward * playerRadius) + (Vector3.up * 0.8f * playerHeight), Vector3.down, out var secondHit, playerHeight))
                {
                    vaultCounter = vaultDelay;
                    Vector3 desiredPos = secondHit.point;
                    desiredPos.y += 0.3f;
                    transform.position = desiredPos;
                    Instantiate(vaultVfx, transform.position, Quaternion.identity);
                }
            }
        }

        vaultCounter -= Time.deltaTime;
    }
}
