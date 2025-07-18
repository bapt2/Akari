using UnityEngine;

public class TPSGrabber : MonoBehaviour
{
    [Header("Grab Settings")]
    [SerializeField] private Transform grabPoint;
    [SerializeField] private float grabDistance = 50f;
    [SerializeField] private LayerMask interactableLayer;
    private KeyCode grabKey = KeyCode.H; 

    private GameObject heldObject;
    private Rigidbody heldRb;

    void Update()
    {
        if (Input.GetKeyDown(grabKey))
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

        if (Input.GetMouseButtonDown(1) && heldObject != null)
        {
            Throw();
        }

        Debug.DrawRay(transform.position + Vector3.up * 1f, transform.forward * grabDistance, Color.green);
    }

    void TryGrab()
{
    Ray ray = new Ray(transform.position + Vector3.up * 1f, transform.forward);
    if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, interactableLayer))
    {
        heldObject = hit.collider.gameObject;
        heldRb = heldObject.GetComponent<Rigidbody>();
        if (heldRb != null)
        {
            heldObject.transform.SetParent(grabPoint);
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localRotation = Quaternion.identity;

            heldRb.useGravity = false;
            heldRb.freezeRotation = true;
            heldRb.isKinematic = true;
        }
    }
}

void Drop()
{
    if (heldRb != null)
    {
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
        heldRb.useGravity = true;
        heldRb.freezeRotation = false;
        heldRb.AddForce(transform.forward * 700f);
        heldObject = null;
        heldRb = null;
    }
}
