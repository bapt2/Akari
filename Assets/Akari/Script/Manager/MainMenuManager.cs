using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void StartButton(int sceneIndex)
    {
        LevelLoader.instance.LoadLevel(sceneIndex);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
