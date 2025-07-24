using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour, IInteractable
{
    public float animationTime;
    public bool isInRange;

    Animator playerAnimator;

    public CameraSwitcher cameraSwitcher;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            playerAnimator = other.gameObject.GetComponent<Animator>();
        }
    }



    public void Interact()
    {
        if (isInRange)
        {
            StartCoroutine(StartEndGame(animationTime));
        }

    }

    IEnumerator StartEndGame(float _animationTime)
    {
        cameraSwitcher.SwitchCamera();
        //déclancher ici l'animation du joueur et une fois fini lancer la fin du jeu
        yield return new WaitForSecondsRealtime(_animationTime);
        GameManager.instance.animator.SetTrigger("FadeIn");
        yield return new WaitForSecondsRealtime(2f);
        LevelLoader.instance.LoadLevel(2);
    }
}
