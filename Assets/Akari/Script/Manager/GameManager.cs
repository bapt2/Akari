using UnityEngine;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
