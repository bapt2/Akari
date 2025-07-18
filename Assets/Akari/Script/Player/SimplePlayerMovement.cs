using UnityEngine;

public class SimplePlayerMouvement : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    private float currentSpeed;

    [SerializeField] private float jumpHeight = 5.5f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 velocity;

    private bool isGrounded;
    private bool wasGroundedLastFrame = true;
    private int jumpCount = 0;
    [SerializeField] private int maxJump = 2;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        // Check if grounded
        isGrounded = controller.isGrounded;
        if (isGrounded) jumpCount = 0;

        // Landing detection
        if (isGrounded && !wasGroundedLastFrame)
        {
            animator.SetBool("isJumping", false);
        }

        // Inputs
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Sprint toggle
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? runSpeed : walkSpeed;

        // Normalize movement speed (0 = idle, 1 = walk, 2 = run)
        float normalizedSpeed = 0f;
        if (move.magnitude > 0.1f)
        {
            normalizedSpeed = isSprinting ? 2f : 1f;
        }

        // Animator parameters
        animator.SetBool("isMoving", move.magnitude > 0.1f);
        animator.SetFloat("moveSpeed", normalizedSpeed);
        animator.SetFloat("x", moveX);
        animator.SetFloat("z", moveZ);

        // Apply movement
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && jumpCount < maxJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;
            animator.SetTrigger("Jump");
            animator.SetBool("isJumping", true);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        wasGroundedLastFrame = isGrounded;
    }
}
