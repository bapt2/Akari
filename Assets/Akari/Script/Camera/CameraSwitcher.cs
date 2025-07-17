using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineCamera camTPS;
    public CinemachineCamera camFPS;

    private bool isFPS = true;

    private Coroutine switchBackCoroutine;

    public float switchBackDelay = 3f; // Délai avant retour automatique en FPS

    void Start()
    {
        camFPS.Priority = 30;
        camTPS.Priority = 10;
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

    // Méthode à ajouter pour  appeler appear in tps 
    public bool IsFPS()
    {
        return isFPS;
    }
}
