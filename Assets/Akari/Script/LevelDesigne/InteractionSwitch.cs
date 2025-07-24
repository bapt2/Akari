using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class InteractionSwitch : MonoBehaviour, IInteractable
{
    public bool trueLever;
    public float speed;
    public float startYDoor;
    float lerpTime;
    float lerpValue;
    [SerializeField] private AudioClip leverClip;


    public Vector3 newPosition;
    public GameObject door;

    [SerializeField] BoxCollider objectToDesactivate;
    [SerializeField] GameObject player;

    public void Interact()
    {
        AudioManager.instance.PlayClipAt(leverClip, transform.position);

        if (!trueLever)
        {
            CharacterController playerControler = player.gameObject.GetComponentInChildren<CharacterController>();
            playerControler.enabled = false;
            player.transform.position = newPosition;
            playerControler.enabled = true;

        }
        else
        {
            StartCoroutine(GameManager.instance.OpenDoor(door, lerpTime, lerpValue, speed, startYDoor, objectToDesactivate));
        }
    }

    private void Update()
    {
        // pour tester la fonctionnalit� plus facilement
        //if (Input.GetKeyDown(KeyCode.E) && door != null)
        //{
        //    StartCoroutine(OpenDoor());
        //}
    }


}
