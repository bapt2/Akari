using UnityEngine;

public class ObjectManipulor : MonoBehaviour
{
    [Header("References")]
    public CameraSwitcher cameraSwitcher;   // Pour savoir si on est en FPS
    public Transform grabPoint;             // Position où l'objet est tenu
    public float grabDistance = 3f;
    public LayerMask interactableLayer;

    [Header("Scale Settings")]
    public float scaleMultiplier = 1f;      // Taille à appliquer quand on grab en FPS

    private GameObject heldObject;
    private Rigidbody heldRb;

    private Vector3 originalScale;          // Pour stocker l'échelle de base
    private Vector3 grabbedScale;           // Échelle une fois doublée
    private bool hasBeenScaled = false;     // Empêche de redoubler la taille

    void Update()
    {
        // Clic droit pour grab/drop
        if (Input.GetMouseButtonDown(1))
        {
            if (heldObject == null)
                TryGrab();
            else
                Drop();
        }

        // Déplace l'objet avec le point de grab
        if (heldObject != null)
        {
            heldRb.MovePosition(grabPoint.position);
        }

        // Clic gauche pour lancer
        if (Input.GetMouseButtonDown(0) && heldObject != null)
        {
            Throw();
        }
    }

    void TryGrab()
    {
        Camera activeCamera = Camera.main;
        Ray ray = activeCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, interactableLayer))
        {
            heldObject = hit.collider.gameObject;
            heldRb = heldObject.GetComponent<Rigidbody>();

            if (heldRb != null)
            {
                heldRb.useGravity = false;
                heldRb.freezeRotation = true;

                // Applique la scale uniquement si on est en FPS et pas encore agrandi
                if (cameraSwitcher != null && cameraSwitcher.IsFPS() && !hasBeenScaled)
                {
                    originalScale = heldObject.transform.localScale;
                    grabbedScale = originalScale * scaleMultiplier;
                    heldObject.transform.localScale = grabbedScale;
                    hasBeenScaled = true;
                }
            }
        }
    }

    void Drop()
    {
        if (heldRb != null)
        {
            heldObject.transform.SetParent(null);

            // On garde la taille doublée
            heldObject.transform.localScale = hasBeenScaled ? grabbedScale : originalScale;

            heldRb.useGravity = true;
            heldRb.freezeRotation = false;
        }

        heldObject = null;
        heldRb = null;
    }

    void Throw()
    {
        Camera activeCamera = Camera.main;
        Vector3 throwDirection = activeCamera.transform.forward;

        // Toujours garder la scale après le lancer
        if (heldObject != null)
            heldObject.transform.localScale = hasBeenScaled ? grabbedScale : originalScale;

        heldRb.useGravity = true;
        heldRb.freezeRotation = false;
        heldRb.AddForce(throwDirection * 500f);

        heldObject.transform.SetParent(null);
        heldObject = null;
        heldRb = null;
    }
}
