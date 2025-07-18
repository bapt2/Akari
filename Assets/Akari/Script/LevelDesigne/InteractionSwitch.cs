using UnityEngine;

public class InteractionSwitch : MonoBehaviour, IInteractable
{
    public Vector3 newPosition;
    public GameObject player;

    public void Interact()
    {
        player.transform.position = newPosition;
    }
}
