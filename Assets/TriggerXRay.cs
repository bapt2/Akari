using UnityEngine;

public class TriggerXRay : MonoBehaviour
{
    public GameObject doorFrame;
    public LayerMask xRayLayer;
    public LayerMask defaultLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SwitchLayer.instance.ChangeLayer(doorFrame, xRayLayer, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            SwitchLayer.instance.ChangeLayer(doorFrame, defaultLayer, true);
    }
}
