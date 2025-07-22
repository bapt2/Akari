using System;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineCamera camTPS;
    public CinemachineCamera camFPS;

    private CinemachinePanTilt fpsPanTilt;

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
        fpsPanTilt = camFPS.GetComponent<CinemachinePanTilt>();
        InitializeFPSCamera();
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
            InitializeFPSCamera();

            SetTPSVisuals(false);


            if (switchBackCoroutine != null)
            {
                StopCoroutine(switchBackCoroutine);
                switchBackCoroutine = null;
            }
        }
        else
        {
            fpsPanTilt.enabled = false;
            camFPS.Priority = 10;
            camTPS.Priority = 30;

            SetTPSVisuals(true);

            if (switchBackCoroutine != null)
                StopCoroutine(switchBackCoroutine);

            switchBackCoroutine = StartCoroutine(ReturnToFPSAfterDelay());
        }
    }

    public void InitializeFPSCamera()
    {
        camFPS.Priority = 30;
        camTPS.Priority = 10;
        // Get direction from camera to target
        Vector3 direction = player.forward;

        // Calculate pan (Y rotation) - horizontal angle
        float pan = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Calculate tilt (X rotation) - vertical angle
        float horizontalDistance = new Vector2(direction.x, direction.z).magnitude;
        float tilt = -Mathf.Atan2(direction.y, horizontalDistance) * Mathf.Rad2Deg;

        fpsPanTilt.TiltAxis.Value = tilt;
        fpsPanTilt.PanAxis.Value = pan;
        fpsPanTilt.enabled = true;
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
