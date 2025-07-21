using UnityEngine;

public class AppearInTPSAndUnlockedInFPS : MonoBehaviour
{
    public CameraSwitcher cameraSwitcher;
    public bool unlocked = false;

    private MeshRenderer[] meshRenderers;

    void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    void Update()
    {
        if (cameraSwitcher == null || meshRenderers == null)
            return;

        bool isFPS = cameraSwitcher.IsFPS();

        if (isFPS)
        {
            foreach (var mr in meshRenderers)
                mr.enabled = unlocked;
        }
        else
        {
            foreach (var mr in meshRenderers)
                mr.enabled = true;
        }
    }
}
