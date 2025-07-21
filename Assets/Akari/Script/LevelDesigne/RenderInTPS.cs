using UnityEngine;

public class RenderInTPS : MonoBehaviour
{
    public bool hasChange;

    public GameObject[] gameObjectsToChangeLayer;

    public LayerMask visibleInTPS;
    public LayerMask defaultLayer;

    public CameraSwitcher cameraSwitcher;

    private void Update()
    {
        if (cameraSwitcher.isFPS && !hasChange)
        {
            hasChange = true;
            SwitchLayer.instance.ChangeLayers(gameObjectsToChangeLayer, visibleInTPS);
        } 
        else if (!cameraSwitcher.isFPS && hasChange) 
        {

            hasChange = false;
            SwitchLayer.instance.ChangeLayers(gameObjectsToChangeLayer, defaultLayer);
        }
    }

}
