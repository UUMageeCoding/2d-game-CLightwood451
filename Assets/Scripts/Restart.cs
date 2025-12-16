
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public static Restart instance;
    
    void Awake()
    {
        // Singleton pattern - only one GameManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        // Check for R key press to restart scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartCurrentScene();
        }
    }
    
    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}