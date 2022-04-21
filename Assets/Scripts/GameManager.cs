using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int _win;
    private bool _isPaused;
    public GameObject PauseMenu;

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
        _win = 0;
    }

    public void GameOver()
    {
        _win = -1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void Win()
    {
        _win = 1;
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
