using UnityEngine;

public class ObjectManipulor : MonoBehaviour
{
    public CameraSwitcher cameraSwitcher;
    public Transform grabPoint;
    public float grabDistance = 3f;
    public LayerMask interactableLayer;

    private GameObject heldObject;
    private Rigidbody heldRb;
    private Vector3 initialScale; // pour mémoriser la taille d'origine

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (heldObject == null)
                TryGrab();
            else
                Drop();
        }

        if (heldObject != null)
        {
            // Positionne l'objet sur le grabPoint en bougeant son Rigidbody
            heldRb.MovePosition(grabPoint.position);
        }

        if (Input.GetMouseButtonDown(0) && heldObject != null)
        {
            Throw();
        }
    }

    void TryGrab()
    {
        Camera activeCamera = Camera.main; // utilise la caméra active (Cinemachine ou autre)
        Ray ray = activeCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, interactableLayer))
        {
            heldObject = hit.collider.gameObject;
            heldRb = heldObject.GetComponent<Rigidbody>();

            if (heldRb != null)
            {
                heldRb.useGravity = false;
                heldRb.freezeRotation = true;
                heldRb.isKinematic = true;

                // Parent l'objet au grabPoint pour qu'il suive la main/caméra
                heldObject.transform.SetParent(grabPoint);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;

                // Mémorise la taille d'origine
                initialScale = heldObject.transform.localScale;

                // Double la taille si on est en FPS
                if (cameraSwitcher != null && cameraSwitcher.IsFPS())
                {
                    heldObject.transform.localScale = initialScale * 2f;
                }
                else
                {
                    // Sinon garde la taille normale
                    heldObject.transform.localScale = initialScale;
                }
            }
        }
    }

    void Drop()
    {
        if (heldRb != null)
        {
            // Remet la taille d'origine
            heldObject.transform.localScale = initialScale;

            // Détache l'objet
            heldObject.transform.SetParent(null);

            heldRb.isKinematic = false;
            heldRb.useGravity = true;
            heldRb.freezeRotation = false;
        }

        heldObject = null;
        heldRb = null;
    }

    void Throw()
    {
        if (heldRb != null)
        {
            // Remet la taille d'origine
            heldObject.transform.localScale = initialScale;

            heldObject.transform.SetParent(null);

            heldRb.isKinematic = false;
            heldRb.useGravity = true;
            heldRb.freezeRotation = false;

            Camera activeCamera = Camera.main;
            Vector3 throwDirection = activeCamera.transform.forward;

            heldRb.AddForce(throwDirection * 500f);
        }

        heldObject = null;
        heldRb = null;
    }
}
