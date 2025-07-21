using UnityEngine;

public class ActivateXRay : MonoBehaviour
{
    public bool hasChildren;

    public GameObject objectToChangeLayer;

    public LayerMask defaultLayer;
    public LayerMask layerToChange;


    private void OnTriggerEnter(Collider other)
    {
        SwitchLayer.instance.ChangeLayer(objectToChangeLayer, layerToChange, hasChildren);
    }

    //Désactive le XRay
    private void OnTriggerExit(Collider other)
    {
        SwitchLayer.instance.ChangeLayer(objectToChangeLayer, defaultLayer, hasChildren);
    }

}
