using UnityEngine;

public class TPSGrabber : MonoBehaviour
{
    [Header("Grab Settings")]
    [SerializeField] private Transform grabPoint;
    [SerializeField] private float grabDistance = 50f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private CameraSwitcher cameraSwitcher;
    [SerializeField] private float fpsScaleMultiplier = 5f;

    private KeyCode grabKey = KeyCode.H;

    private GameObject heldObject;
    private Rigidbody heldRb;

    // Pour stocker la taille d'origine des objets
    private System.Collections.Generic.Dictionary<GameObject, Vector3> originalScales = new System.Collections.Generic.Dictionary<GameObject, Vector3>();

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
            GameObject target = hit.collider.gameObject;
            bool isFPS = cameraSwitcher != null && cameraSwitcher.IsFPS();

            // Récupérer le script de visibilité
            AppearInTPSAndUnlockedInFPS appearScript = target.GetComponent<AppearInTPSAndUnlockedInFPS>();

            // Si on est en FPS, on ne peut prendre que les objets débloqués (unlocked = true)
            if (isFPS && (appearScript == null || !appearScript.unlocked))
                return;

            heldObject = target;
            heldRb = heldObject.GetComponent<Rigidbody>();

            if (heldRb != null)
            {
                heldObject.transform.SetParent(grabPoint);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;

                heldRb.useGravity = false;
                heldRb.freezeRotation = true;
                heldRb.isKinematic = true;

                if (appearScript != null && !appearScript.unlocked)
                {
                    // Si on est en TPS et que l'objet n'est pas encore débloqué, on le débloque
                    if (!isFPS)
                    {
                        appearScript.unlocked = true;
                    }
                }

                // Gestion de la taille doublée en FPS au grab
                if (isFPS)
                {
                    if (!originalScales.ContainsKey(heldObject))
                        originalScales[heldObject] = heldObject.transform.localScale;

                    heldObject.transform.localScale = originalScales[heldObject] * fpsScaleMultiplier;
                }
                else
                {
                    // En TPS, on remet la taille d'origine (au cas où)
                    if (originalScales.ContainsKey(heldObject))
                        heldObject.transform.localScale = originalScales[heldObject];
                }
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

            // La taille reste doublée si l'objet est débloqué et on est en FPS
            AppearInTPSAndUnlockedInFPS appearScript = heldObject.GetComponent<AppearInTPSAndUnlockedInFPS>();
            bool isFPS = cameraSwitcher != null && cameraSwitcher.IsFPS();

            if (appearScript != null && appearScript.unlocked && isFPS && originalScales.ContainsKey(heldObject))
            {
                heldObject.transform.localScale = originalScales[heldObject] * fpsScaleMultiplier;
            }
            else if (originalScales.ContainsKey(heldObject))
            {
                // Sinon taille normale
                heldObject.transform.localScale = originalScales[heldObject];
            }
        }
        heldObject = null;
        heldRb = null;
    }

    void Throw()
    {
        if (heldRb != null)
        {
            heldRb.isKinematic = false;
            heldRb.useGravity = true;
            heldRb.freezeRotation = false;

            AppearInTPSAndUnlockedInFPS appearScript = heldObject.GetComponent<AppearInTPSAndUnlockedInFPS>();
            bool isFPS = cameraSwitcher != null && cameraSwitcher.IsFPS();

            if (appearScript != null && appearScript.unlocked && isFPS && originalScales.ContainsKey(heldObject))
            {
                heldObject.transform.localScale = originalScales[heldObject] * fpsScaleMultiplier;
            }
            else if (originalScales.ContainsKey(heldObject))
            {
                heldObject.transform.localScale = originalScales[heldObject];
            }

            heldRb.AddForce(transform.forward * 700f);
        }

        heldObject = null;
        heldRb = null;
    }
}
