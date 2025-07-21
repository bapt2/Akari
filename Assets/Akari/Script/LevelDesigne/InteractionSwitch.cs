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

    public Vector3 newPosition;
    public GameObject door;

    [SerializeField]
    BoxCollider objectToDesactivate;

    GameObject player;

    public void Interact()
    {
        Debug.Log("Test de passage");
        if (!trueLever)
        {
            player.transform.position = newPosition;
        }
        else
        {
            OpenDoor();
        }
    }

    private void Update()
    {
        // pour tester la fonctionnalité plus facilement
        if (Input.GetKeyDown(KeyCode.E) && door != null)
        {
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        if (objectToDesactivate != null)
        {
            objectToDesactivate.enabled = false;
        }

        while (lerpTime < 1f) 
        {
            // faire monter la grille de la porte de manière fluide
            lerpTime += Time.deltaTime * speed;
            lerpTime = Mathf.Clamp01(lerpTime);
            lerpValue = Mathf.Lerp(startYDoor, 5.5f, lerpTime);
            door.transform.localPosition = new Vector3(-1.5f, lerpValue, 0f);
            yield return null;
        }
    }
}
