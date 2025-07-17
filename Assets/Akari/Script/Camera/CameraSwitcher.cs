using UnityEngine;
using Unity.Cinemachine; 

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineCamera camTPS;
    public CinemachineCamera camFPS;

    private bool isFPS = true; // On commence en FPS

    void Start()
    {
        // Caméra FPS active au début
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
        isFPS = !isFPS; //  On inverse bien l’état

        if (isFPS)
        {
            camFPS.Priority = 30;
            camTPS.Priority = 10;
        }
        else
        {
            camFPS.Priority = 10;
            camTPS.Priority = 30;
        }
    }
}
