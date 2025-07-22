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
    [SerializeField] private Transform cameraPivot; // FPS camera pivot
    [SerializeField] private CameraSwitcher cameraSwitcher;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed;

        // Aligne le joueur sur la caméra au lancement
        if (cameraSwitcher != null && cameraSwitcher.IsFPS())
        {
            AlignPlayerWithCamera();
        }
    }

    void Update()
    {
        // Vérifie si le joueur est au sol
        isGrounded = controller.isGrounded;
        if (isGrounded) jumpCount = 0;

        // Détection de l'atterrissage
        if (isGrounded && !wasGroundedLastFrame)
        {
            animator.SetBool("isJumping", false);
        }

        // Entrées du joueur
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move;

        if (cameraSwitcher != null && cameraSwitcher.IsFPS())
        {
            // Mouvement basé sur la direction de la caméra FPS
            Vector3 forward = cameraPivot.forward;
            Vector3 right = cameraPivot.right;

            forward.y = 0f;
            right.y = 0f;

            move = (forward * moveZ + right * moveX).normalized;
        }
        else
        {
            // Mouvement classique TPS
            move = new Vector3(moveX, 0, moveZ);
            if (move.magnitude > 1f) move.Normalize();
        }

        // Sprint
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? runSpeed : walkSpeed;

        // Animation (uniquement en TPS)
        if (!cameraSwitcher.IsFPS())
        {
            float speedValue = move.magnitude * currentSpeed;
            animator.SetBool("isMoving", move.magnitude > 0.1f);
            animator.SetFloat("moveSpeed", speedValue);
            animator.SetFloat("x", moveX);
            animator.SetFloat("z", moveZ);
        }

        // Rotation joueur (TPS uniquement)
        if (!cameraSwitcher.IsFPS() && move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Déplacement horizontal
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Saut
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

        // Gravité
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        wasGroundedLastFrame = isGrounded;
    }

    // Aligne le joueur sur la direction de la caméra FPS
    public void AlignPlayerWithCamera()
    {
        if (cameraPivot == null) return;

        Vector3 camForward = cameraPivot.forward;
        camForward.y = 0;
        if (camForward.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(camForward);
    }
}
