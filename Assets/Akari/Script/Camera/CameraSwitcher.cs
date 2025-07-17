using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineCamera camTPS;
    public CinemachineCamera camFPS;

    private bool isFPS = false;

    void Start()
    {
        camTPS.Priority = 10;
        camFPS.Priority = 30;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchCamera();
        }
    }

    void SwitchCamera()
    {
        isFPS = !isFPS;

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
