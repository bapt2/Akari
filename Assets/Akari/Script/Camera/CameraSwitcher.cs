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
        camFPS.Priority = 30;
        camTPS.Priority = 10;
        SetTPSVisuals(false); // Démarrage en FPS
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

            // Réaligne le joueur correctement
            AlignPlayerWithCamera(camFPS);

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

    Vector3 camForward = camera.transform.forward;
    camForward.y = 0;

    Debug.DrawRay(player.position, player.forward * 2, Color.blue, 2f); // Player forward
    Debug.DrawRay(player.position, camForward * 2, Color.red, 2f); // Camera forward (sans y)

    if (Vector3.Dot(player.forward, camForward) < 0f)
    {
        camForward = -camForward;
    }

    if (camForward.sqrMagnitude > 0.001f)
    {
        player.rotation = Quaternion.LookRotation(camForward);
        Debug.Log($"Player rotation alignée sur caméra: {camForward}");
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

    public bool IsFPS() => isFPS;
    public bool IsTPS() => !isFPS;
}
