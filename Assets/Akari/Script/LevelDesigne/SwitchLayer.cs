using UnityEngine;

public class SwitchLayer : MonoBehaviour
{
    public GameObject objectToChangeLayer;

    public LayerMask defaultLayer;
    public LayerMask xRayLayer;

    [Header("Debug")]
    public bool xRayActive;

    //Change le layerMask actif sur l'objet
    //Active le XRay
    private void OnTriggerEnter(Collider other)
    {
        xRayActive = !xRayActive;
        int layerNum = (int)Mathf.Log(xRayLayer.value, 2);
        objectToChangeLayer.layer = layerNum;

        if (objectToChangeLayer.transform.childCount > 0)
        {
            SetLayerAllChildren(objectToChangeLayer.transform, layerNum);
        }

    }

     //Désactive le XRay
    private void OnTriggerExit(Collider other)
    {
        xRayActive = !xRayActive;
        int layerNum = (int)Mathf.Log(defaultLayer.value, 2);
        objectToChangeLayer.layer = layerNum;

        if (objectToChangeLayer.transform.childCount > 0)
        {
            SetLayerAllChildren(objectToChangeLayer.transform, layerNum);
        }
    }

    void SetLayerAllChildren(Transform root, int layer)
    {
        Transform[] children = root.GetComponentsInChildren<Transform>(includeInactive: true);

        foreach (Transform child in children)
        {
            child.gameObject.layer = layer;
        }
    }
}
