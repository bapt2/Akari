using UnityEngine;

public class SwitchLayer : MonoBehaviour
{
    public static SwitchLayer instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"Plus d'une instance de {this} dans la scene");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }


    public void ChangeLayer(GameObject gameObject, LayerMask layerMask, bool hasChildren = false)
    {
        int layerNum = (int)Mathf.Log(layerMask.value, 2);
        gameObject.layer = layerNum;

        if (hasChildren)
        {
            SetLayerAllChildren(gameObject.transform, layerNum);
        }
    }

    public void ChangeLayers(GameObject[] gameObjects, LayerMask layerMask)
    {
        int layerNum = (int)Mathf.Log(layerMask.value, 2);
        foreach (GameObject obj in gameObjects)
        {
            obj.layer = layerNum;
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
