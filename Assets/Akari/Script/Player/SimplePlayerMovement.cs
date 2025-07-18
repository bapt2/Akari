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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed;
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

        // Mouvement global
        Vector3 move = new Vector3(moveX, 0, moveZ);
        if (move.magnitude > 1f) move.Normalize();

        // Sprint
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? runSpeed : walkSpeed;

        // Vitesse réelle pour l'Animator
        float speedValue = move.magnitude * currentSpeed;

        // Envoie des paramètres à l'Animator
        animator.SetBool("isMoving", move.magnitude > 0.1f);
        animator.SetFloat("moveSpeed", speedValue); // utilisé pour Idle/Walk/Run transitions
        animator.SetFloat("x", moveX);              // Blend Tree X axis
        animator.SetFloat("z", moveZ);              // Blend Tree Y axis

        // Rotation vers la direction du mouvement
        if (move.magnitude > 0.1f)
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
            animator.SetTrigger("Jump");
            animator.SetBool("isJumping", true);
        }

        // Gravité
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Sauvegarde l'état du sol pour la prochaine frame
        wasGroundedLastFrame = isGrounded;
    }
}
