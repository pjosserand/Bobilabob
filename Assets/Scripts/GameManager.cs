using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool _isPaused;
    public GameObject PauseMenu;
    public GameObject GameOverMenu;
    void Awake(){
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void GameOver()
    {
        GameOverMenu.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void Win()
    {
        Debug.Log("You win !!!");
    }

    public void Pause()
    {
        _isPaused = !_isPaused;
        Debug.Log(_isPaused);
        PauseMenu.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0.0f : 1.0f;
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
