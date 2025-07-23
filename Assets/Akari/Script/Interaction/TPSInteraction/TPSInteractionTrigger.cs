using UnityEngine;
using System.Collections;

public class TPSInteractionTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private CameraSwitcher cameraSwitcher;
    [SerializeField] private Animator animator;
    [SerializeField] private float delayBeforeSwitch = 1.5f; // Durée animation

    public void Interact()
    {
        if (animator != null)
        {
            animator.SetTrigger("interact");
            StartCoroutine(SwitchCameraAfterAnimation());
        }
        else
        {
            Debug.LogWarning("Animator non assigné !");
        }
    }

    private IEnumerator SwitchCameraAfterAnimation()
    {
        yield return new WaitForSeconds(delayBeforeSwitch);

        if (cameraSwitcher != null)
        {
            cameraSwitcher.SwitchCamera();
            Debug.Log("Caméra changée après l'animation !");
        }
    }
}
