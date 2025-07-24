using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BedInteraction : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    public Transform interactionPoint;
    public Animator animator;
    public GameObject player;

    [Tooltip("Nom de la scène d'outro à charger")]
    public string outroScene = "Outro";

    private bool hasInteracted = false;
    [SerializeField] private float animationDuration = 3.5f; // 
    public void Interact()
    {
        if (hasInteracted) return;
        hasInteracted = true;

        // Placer le joueur au bon endroit
        player.transform.position = interactionPoint.position;
        player.transform.rotation = interactionPoint.rotation;

        // Utiliser le trigger 
        animator.SetTrigger("sitAndLay");

        // Attendre la durée puis charger Outro
        StartCoroutine(WaitThenLoadOutro(animationDuration));
    }

    private IEnumerator WaitThenLoadOutro(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Outro");
    }
}
