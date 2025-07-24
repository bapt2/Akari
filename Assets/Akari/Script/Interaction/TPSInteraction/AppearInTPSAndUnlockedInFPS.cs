using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class AppearInTPSAndUnlockedInFPS : MonoBehaviour
{
    public CameraSwitcher cameraSwitcher;
    public bool unlocked = false;
    public bool changeAppliedFPS;
    public bool changeAppliedTPS;

    public Material onFPSMaterial;
    public Material onTPSMaterial;

    private MeshRenderer[] meshRenderers;
    public Vector3 basePosition;


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

            if (!changeAppliedFPS)
            {
                StartCoroutine(ChangeCoolDown(isFPS));
                Debug.Log("test");
                foreach (var mr in meshRenderers)
                {
                    mr.enabled = unlocked;
                    mr.material = onFPSMaterial;
                }
            }
        }
        else
        {
            if (!changeAppliedTPS)
            {
                StartCoroutine(ChangeCoolDown());
                foreach (var mr in meshRenderers)
                {
                    mr.enabled = true;
                    mr.material = onTPSMaterial;
                }
            }
        }
    }

    public IEnumerator ChangeCoolDown(bool isFPS = false)
    {
        if (isFPS)
        {
            changeAppliedFPS = true;
            yield return new WaitForSecondsRealtime(120f);
            changeAppliedFPS = false;
        }
        else
        {
            changeAppliedTPS = true;
            yield return new WaitForSecondsRealtime(15f);
            changeAppliedTPS = false;
            unlocked = true;
            changeAppliedFPS = false;

            yield break;
        }
    }

    public IEnumerator ReplaceObject()
    {
        yield return new WaitForSecondsRealtime(10f);
        transform.position = basePosition;
    }
}
