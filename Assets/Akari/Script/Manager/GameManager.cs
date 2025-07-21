using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public Animator animator;

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"Plus d'une instance de {this} dans la scene");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public IEnumerator OpenDoor(GameObject door, float lerpTime, float lerpValue, float speed, float startYDoor, BoxCollider colliderToDesactivate = null)
    {
        if (colliderToDesactivate != null)
        {
            colliderToDesactivate.enabled = false;
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
