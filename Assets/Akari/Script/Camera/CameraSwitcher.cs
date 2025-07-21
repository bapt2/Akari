using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineCamera camTPS;
    public CinemachineCamera camFPS;

    [Header("Player Components")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SkinnedMeshRenderer playerMeshRenderer;

    [Header("Settings")]
    public float switchBackDelay = 3f;

    private bool isFPS = true;
    private Coroutine switchBackCoroutine;

    void Start()
    {
        camFPS.Priority = 30;
        camTPS.Priority = 10;

        SetTPSVisuals(false); // FPS par défaut
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
