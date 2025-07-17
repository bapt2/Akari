using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimplePlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    [Header("Vitesse")]
    public float speed = 5f;
    public float sprintSpeed = 8f;
    private float currentSpeed;

    [Header("Saut")]
    public float jumpHeight = 2f;
    public int maxJump = 2;
    private int jumpCount = 0;

    [Header("Gravité")]
    public float gravity = -9.81f;
    private Vector3 velocity;

    private bool isGrounded;
    private bool wasGroundedLastFrame;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Détection du sol
        isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            jumpCount = 0; // réinitialise les sauts
            if (velocity.y < 0)
                velocity.y = -2f; // garde le joueur collé au sol
        }

        // Déplacement horizontal
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? sprintSpeed : speed;

        controller.Move(move * currentSpeed * Time.deltaTime);

        // Saut
        if (Input.GetButtonDown("Jump") && jumpCount < maxJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;
        }

        // Gravité
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Stocke l’état du sol pour la prochaine frame
        wasGroundedLastFrame = isGrounded;
    }
}
