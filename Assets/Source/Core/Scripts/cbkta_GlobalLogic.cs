using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cbkta_GlobalLogic : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;
    public cbkta_GlobalStates cbkta_globalstates;
    public cbkta_GlobalTags cbkta_globaltags;

    private bool UIControlsEnabled = true;
    private GameObject globalsharinginfo;

    //also used for "Continue" button
    public void TogglePauseMenu()
    {
        if (this.cbkta_globalui.pauseMenu == null) return;

        this.cbkta_globalstates.isPause = !this.cbkta_globalstates.isPause;

        this.cbkta_globalui.pauseMenu.SetActive(this.cbkta_globalstates.isPause);

        Time.timeScale = this.cbkta_globalstates.isPause ? 0 : 1;
    }

    [ContextMenu("Toggle Input System")]
    public void ToggleInputSystem()
    {
        if (this.UIControlsEnabled)
        {
            this.cbkta_globalui.controls.Disable();
        }
        else
        {
            this.cbkta_globalui.controls.Enable();
        }

        this.UIControlsEnabled = !this.UIControlsEnabled;
    }

    public void NextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        index++;
        SceneManager.LoadScene(index);
    }

    void Awake()
    {
        this.cbkta_globalui.controls = new();

        this.globalsharinginfo = GameObject.FindWithTag(this.cbkta_globaltags.globalSharingInformation);
        //if (this.globalsharinginfo != null ? this.globalsharinginfo.scene.name != "DontDestroyOnLoad" : false) DontDestroyOnLoad(this.globalsharinginfo);

        /**setup**/

        this.cbkta_globalui.controls.UI.Pause.performed += ctx =>
        {
            this.TogglePauseMenu();
        };

        this.cbkta_globalui.controls.UI.Interact.performed += ctx =>
        {
            this.cbkta_globalstates.isInteractionTrigger = true;
        };

        this.cbkta_globalui.controls.UI.Interact.canceled += ctx =>
        {
            this.cbkta_globalstates.isInteractionTrigger = false;
        };
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnEnable()
    {
        this.cbkta_globalui.controls.Enable();
    }

    void OnDisable()
    {
        this.cbkta_globalui.controls.Disable();
    }
}
