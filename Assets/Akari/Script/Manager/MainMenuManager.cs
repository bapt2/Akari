using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject MainMenuPanel;
    public GameObject ControlPanel;

    public void StartButton(int sceneIndex)
    {
        LevelLoader.instance.LoadLevel(sceneIndex);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ShowControls()
    {
        MainMenuPanel.SetActive(false);
        ControlPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        ControlPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }
}
