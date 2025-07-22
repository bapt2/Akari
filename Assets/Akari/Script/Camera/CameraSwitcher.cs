using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineCamera camTPS;
    public CinemachineCamera camFPS;

    [Header("Player Components")]
    [SerializeField] private Transform player; 
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;

    [Header("Settings")]
    public float switchBackDelay = 3f;

    public bool isFPS = true;
    private Coroutine switchBackCoroutine;

    void Start()
    {
        // On démarre en FPS par défaut
        camFPS.Priority = 30;
        camTPS.Priority = 10;

        SetTPSVisuals(false);

        // Aligne le joueur dès le départ pour éviter l'inversion
        AlignPlayerWithCamera(camFPS);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchCamera();
        }
    }

    public void SwitchCamera()
    {
        isFPS = !isFPS;

        if (isFPS)
        {
            camFPS.Priority = 30;
            camTPS.Priority = 10;

            SetTPSVisuals(false);

            AlignPlayerWithCamera(camFPS); // Aligne le joueur

            if (switchBackCoroutine != null)
            {
                StopCoroutine(switchBackCoroutine);
                switchBackCoroutine = null;
            }
        }
        else
        {
            camFPS.Priority = 10;
            camTPS.Priority = 30;

            SetTPSVisuals(true);

            if (switchBackCoroutine != null)
                StopCoroutine(switchBackCoroutine);

            switchBackCoroutine = StartCoroutine(ReturnToFPSAfterDelay());
        }
    }

    private void AlignPlayerWithCamera(CinemachineCamera camera)
    {
        if (player == null || camera == null) return;

        // Utilise uniquement l'axe Y de la caméra (pas de pitch/roll)
        Vector3 camForward = camera.transform.forward;
        camForward.y = 0;

        if (camForward.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(camForward, Vector3.up);
            player.rotation = targetRotation;
        }
    }

    private IEnumerator ReturnToFPSAfterDelay()
    {
        yield return new WaitForSeconds(switchBackDelay);

        if (!isFPS)
        {
            SwitchCamera();
        }

        switchBackCoroutine = null;
    }

    private void SetTPSVisuals(bool enable)
    {
        if (playerAnimator != null)
            playerAnimator.enabled = enable;

        if (playerMeshRenderer != null)
            playerMeshRenderer.enabled = enable;
    }

    public bool IsFPS()
    {
        return isFPS;
    }

    public bool IsTPS()
    {
        return !isFPS;
    }
}
