using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class HoleTeleport : MonoBehaviour
{
    public Vector3 newPosition;
    Transform player;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject.transform;
            StartCoroutine(TeleportPlayer());
        }
    }

    public IEnumerator TeleportPlayer()
    {

        CharacterController playerControler = player.gameObject.GetComponent<CharacterController>();
        GameManager.instance.animator.SetTrigger("FadeIn");

        yield return new WaitForSecondsRealtime(1.5f);
        playerControler.enabled = false;
        player.position = newPosition;
        playerControler.enabled = true;

        GameManager.instance.animator.SetTrigger("FadeOut");


    }
}
