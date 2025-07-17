using UnityEngine;

public class TPSInteractionTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private CameraSwitcher cameraSwitcher;

    public void Interact()
    {
        if (cameraSwitcher != null)
        {
            cameraSwitcher.SwitchCamera();
            Debug.Log("Caméra changée via interaction !");
        }
    }
}
