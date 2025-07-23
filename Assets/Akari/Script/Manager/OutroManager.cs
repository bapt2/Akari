using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OutroManager: MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button restartButton;
    public Button quitButton;

    void Start()
    {
        // Masquer les boutons au d�but
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        // Lancer la vid�o
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }

        // Lier les boutons � leurs fonctions
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Afficher les boutons une fois la vid�o termin�e
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    void RestartGame()
    {
        SceneManager.LoadScene("MainMenuScene"); // 
    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitter le jeu (ne fonctionne pas dans l'�diteur)");
    }
}
