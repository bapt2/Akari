using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{

    public GameObject LoadingScreen;

    public static LevelLoader instance;

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

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    // permet de charger la scene de manière asynchrone
    IEnumerator LoadAsync(int sceneIndex)
    {
        LoadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        yield return new WaitForSeconds(2f);
    }

}
