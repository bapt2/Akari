using UnityEngine;

public class ObjectManipulor : MonoBehaviour
{
    public CameraSwitcher cameraSwitcher;
    public Transform grabPoint;
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
        Camera activeCamera = Camera.main; // suit la Cinemachine active
        Ray ray = activeCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

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
        Camera activeCamera = Camera.main;
        Vector3 throwDirection = activeCamera.transform.forward;

        heldRb.useGravity = true;
        heldRb.freezeRotation = false;
        heldRb.AddForce(throwDirection * 500f);
        heldObject = null;
        heldRb = null;
    }
}
