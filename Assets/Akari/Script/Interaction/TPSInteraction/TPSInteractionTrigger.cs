using UnityEngine;

public class TPSInteractionTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private CameraSwitcher cameraSwitcher;
    [SerializeField] private Animator animator;

    public void Interact()
    {
        if (cameraSwitcher != null)
        {
            cameraSwitcher.SwitchCamera();
            Debug.Log("Caméra changée via interaction !");
        }

        if (animator!= null)
        {
            animator.SetTrigger("Interact");
        }
    }

}
