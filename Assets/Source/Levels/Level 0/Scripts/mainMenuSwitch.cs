using UnityEngine;

public class mainMenuSwitch : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settings;
    public GameObject credits;
    public GameObject backgroundGrayed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Application.targetFrameRate = 60; //lock 60 FPS
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    enum MenuList
    {
        mainMenu = 0,
        settings = 1,
        credits = 2,
        exit = 3,
    }

    public void SwitchCanvasMenu(int menuInt)
    {
        MenuList menu = (MenuList) menuInt;

        mainMenu.SetActive(menu == MenuList.mainMenu);
        settings.SetActive(menu == MenuList.settings);
        credits.SetActive(menu == MenuList.credits);
        if (menu == MenuList.exit) {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        backgroundGrayed.SetActive(menu != MenuList.mainMenu);
    }
}
