using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    private float currentSpeed;
    private Vector3 moveDirection;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 5.5f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 velocity;
    private int jumpCount = 0;
    [SerializeField] private int maxJump = 2;
    private bool isGrounded;
    private bool wasGroundedLastFrame = true;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Camera")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private CameraSwitcher cameraSwitcher;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && !wasGroundedLastFrame)
            animator.SetBool("isJumping", false);

        ReadInput();      // Lecture des touches
        HandleRotation(); // Rotation TPS
        HandleAnimation();// Animation de déplacement
        HandleJumpInput();// Détection de saut

        wasGroundedLastFrame = isGrounded;
    }

    void FixedUpdate()
    {
        ApplyMovement(); // Déplacement physique
        ApplyGravity();  // Gravité
    }

    //  Lecture Input + Calcul direction
    void ReadInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (cameraSwitcher != null && cameraSwitcher.IsFPS())
        {
            Vector3 forward = cameraPivot.forward;
            Vector3 right = cameraPivot.right;

            forward.y = 0;
            right.y = 0;

            moveDirection = (forward * moveZ + right * moveX);
        }
        else
        {
            moveDirection = new Vector3(moveX, 0f, moveZ);
        }

        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? runSpeed : walkSpeed;
    }

    //  Rotation du perso en TPS
    void HandleRotation()
    {
        if (!cameraSwitcher.IsFPS() && moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            cameraPivot.forward = transform.forward;
        }
    }

    //  Animation selon les inputs
    void HandleAnimation()
    {
        if (!cameraSwitcher.IsFPS())
        {
            animator.SetBool("isMoving", moveDirection.magnitude > 0.1f);
            animator.SetFloat("moveSpeed", moveDirection.magnitude * currentSpeed);
            animator.SetFloat("x", moveDirection.x);
            animator.SetFloat("z", moveDirection.z);
        }
    }

    //  Détection du saut (input uniquement)
    void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < maxJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;

            if (!cameraSwitcher.IsFPS())
            {
                animator.SetTrigger("Jump");
                animator.SetBool("isJumping", true);
            }
        }

        if (isGrounded)
            jumpCount = 0;
    }

    //  Déplacement physique
    void ApplyMovement()
    {
        controller.Move(moveDirection * currentSpeed * Time.fixedDeltaTime);
    }

    //  Gravité persistante
    void ApplyGravity()
    {
        velocity.y += gravity * Time.fixedDeltaTime;
        controller.Move(velocity * Time.fixedDeltaTime);
    }
}
