using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameManager gmInstance;
    // Start is called before the first frame update
    void Start()
    {
        gmInstance = GameManager.Instance;   
    }

    // Update is called once per frame

    public void Resume()
    {
        gmInstance.Resume();
    }

    public void QuitGame()
    {
        
    }
}
