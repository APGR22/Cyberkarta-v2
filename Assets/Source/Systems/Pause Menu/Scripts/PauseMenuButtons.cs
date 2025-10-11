using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    public cbkta_GlobalStates cbkta_globalstates;
    public cbkta_GlobalLogic cbkta_globallogic;

    public void Continue()
    {
        this.cbkta_globallogic.TogglePauseMenu();
    }

    public void Settings()
    {}

    public void MainMenu()
    {
        SceneManager.LoadScene(0);

        Time.timeScale = 1;
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
