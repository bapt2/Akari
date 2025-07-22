using UnityEngine;

public class SimplePlayerMouvement : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    private float currentSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 5.5f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 velocity;

    private bool isGrounded;
    private bool wasGroundedLastFrame = true;
    private int jumpCount = 0;
    [SerializeField] private int maxJump = 2;

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

        // Aligne le joueur au démarrage pour éviter inversion initiale
        if (cameraSwitcher != null && cameraSwitcher.IsFPS())
        {
            Vector3 forward = cameraPivot.forward;
            forward.y = 0;
            if (forward.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        }
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded) jumpCount = 0;

        if (isGrounded && !wasGroundedLastFrame)
        {
            animator.SetBool("isJumping", false);
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move;

        if (cameraSwitcher != null && cameraSwitcher.IsFPS())
        {
            Vector3 forward = cameraPivot.forward;
            Vector3 right = cameraPivot.right;

            forward.y = 0f;
            right.y = 0f;

            move = (forward * moveZ + right * moveX).normalized;
        }
        else
        {
            move = new Vector3(moveX, 0, moveZ);
            if (move.magnitude > 1f) move.Normalize();
        }

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? runSpeed : walkSpeed;

        float speedValue = move.magnitude * currentSpeed;

        if (!cameraSwitcher.IsFPS())
        {
            animator.SetBool("isMoving", move.magnitude > 0.1f);
            animator.SetFloat("moveSpeed", speedValue);
            animator.SetFloat("x", moveX);
            animator.SetFloat("z", moveZ);
        }

        if (!cameraSwitcher.IsFPS() && move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        controller.Move(move * currentSpeed * Time.deltaTime);

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

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        wasGroundedLastFrame = isGrounded;
    }
}
