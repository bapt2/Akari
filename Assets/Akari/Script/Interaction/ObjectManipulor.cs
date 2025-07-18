using UnityEngine;

public class ObjectManipulor : MonoBehaviour
{
    [Header("Grabbing Settings")]
    public Transform grabPoint; // Point devant la caméra
    public float grabDistance = 3f;
    public LayerMask interactableLayer; 

    private GameObject heldObject;
    private Rigidbody heldRb;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
                TryGrab();
            else
                Drop();
        }

        if (heldObject != null)
        {
            heldRb.MovePosition(grabPoint.position);
        }

        if (Input.GetMouseButtonDown(0) && heldObject != null)
        {
            Throw();
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, interactableLayer))
        {
            heldObject = hit.collider.gameObject;
            heldRb = heldObject.GetComponent<Rigidbody>();
            if (heldRb != null)
            {
                heldRb.useGravity = false;
                heldRb.freezeRotation = true;
            }
        }
    }

    void Drop()
    {
        if (heldRb != null)
        {
            heldRb.useGravity = true;
            heldRb.freezeRotation = false;
        }
        heldObject = null;
        heldRb = null;
    }

    void Throw()
    {
        heldRb.useGravity = true;
        heldRb.freezeRotation = false;
        heldRb.AddForce(transform.forward * 500f);
        heldObject = null;
        heldRb = null;
    }
}
