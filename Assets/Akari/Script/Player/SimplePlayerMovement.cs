using UnityEngine;

public class SimplePlayerMouvement : MonoBehaviour
{
    //  Contrôleur de physique (collision et déplacement)
    private CharacterController controller;

    //  Contrôleur d'animations
    private Animator animator;

    //  Vitesses
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    private float currentSpeed;

    //  Saut et gravité
    [SerializeField] private float jumpHeight = 5.5f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 velocity;

    //  Gestion du sol et des sauts
    private bool isGrounded;
    private bool wasGroundedLastFrame = true;
    private int jumpCount = 0;
    [SerializeField] private int maxJump = 2;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        currentSpeed = speed;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if(isGrounded)
        {
            jumpCount = 0;
        }
        // Détecte l'atterrissage (passage de en l'air à au sol)
        if (isGrounded && !wasGroundedLastFrame)
        {
            // Désactive l'animation de saut quand le joueur touche le sol
            if (animator != null)
            {
                animator.SetBool("isJumping", false);
            }
        }
        
        // Récupère les entrées horizontales (gauche/droite) et verticales (avant/arrière)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calcule le vecteur de déplacement en fonction de l'orientation du joueur
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        
        // Détermine la vitesse actuelle selon si le joueur sprinte ou non
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isSprinting ? sprintSpeed : speed;
        
        // Calcule la magnitude du mouvement pour l'Animator
        float moveMagnitude = move.magnitude;
        
        // Met à jour les paramètres de l'Animator pour gérer les animations automatiquement
        if (animator != null)
        {
            // Utilise moveSpeed pour contrôler les transitions Idle -> Walk -> Run
            if (moveMagnitude > 0.1f) // Le joueur bouge
            {
                float animationSpeed = isSprinting ? sprintSpeed : speed;
                animator.SetFloat("moveSpeed", animationSpeed);
                animator.SetBool("isMoving", true);
            }
            else // Le joueur est immobile
            {
                animator.SetFloat("moveSpeed", 0f);
                animator.SetBool("isMoving", false);
            }
        }
        
        // Applique le mouvement horizontal avec la vitesse actuelle
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Gestion du saut — permet un double saut
        if (Input.GetButtonDown("Jump") && jumpCount < maxJump)
        {
            // Formule physique pour calculer la vélocité initiale nécessaire pour atteindre la hauteur souhaitée
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Incrémente le compteur de saut
            jumpCount++;

            // Déclenche les animations de saut si l'Animator est disponible
            if (animator != null)
            {
                animator.SetTrigger("Jump");
                animator.SetBool("isJumping", true);
            }
        }

        // Applique la gravité à la vélocité verticale
        velocity.y += gravity * Time.deltaTime;
        // Applique le mouvement vertical (saut/chute)
        controller.Move(velocity * Time.deltaTime);
        
        // Met à jour l'état du sol pour la prochaine frame
        wasGroundedLastFrame = isGrounded;
    }
}
