using UnityEngine;

public class AppearOnlyInTPS : MonoBehaviour
{
    public CameraSwitcher cameraSwitcher;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (meshRenderer == null || cameraSwitcher == null) return;

        // On active le mesh uniquement en TPS
        meshRenderer.enabled = !cameraSwitcher.IsFPS();
    }
}
