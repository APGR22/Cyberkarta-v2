using cbkta.UI.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cbkta_GlobalLogic : MonoBehaviour
{
    public cbkta_GlobalUI cbkta_globalui;
    public cbkta_GlobalStates cbkta_globalstates;
    public cbkta_GlobalObjects cbkta_globalobjects;

    private bool UIControlsEnabled = true;

    //cache
    private Vector3 playerPositionStayedOn = Vector3.zero;

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
        this.cbkta_globalstates.sceneIndex++;

        SceneManager.LoadScene(this.cbkta_globalstates.sceneIndex);
    }

    public void PreviousScene()
    {
        this.cbkta_globalstates.sceneIndex--;
        if (this.cbkta_globalstates.sceneIndex < 0) this.cbkta_globalstates.sceneIndex = 0;

        SceneManager.LoadScene(this.cbkta_globalstates.sceneIndex);
    }

    public void FreezePlayer()
    {
        //if (this.playerPositionStayedOn == Vector3.zero)
        //{
        //    this.playerPositionStayedOn = this.cbkta_globalobjects.player.transform.position;
        //}

        //this.cbkta_globalobjects.player.transform.position = this.playerPositionStayedOn;

        //setup
        UIControls controls = this.cbkta_globalui.controls;

        //settings
        controls.Player.Disable();
    }

    public void UnfreezePlayer()
    {
        //setup
        UIControls controls = this.cbkta_globalui.controls;

        //settings
        controls.Player.Enable();
    }

    //Events

    /// <summary>
    /// Execute function on next frame.
    /// </summary>
    /// <remarks>
    /// If <paramref name="nextFrame"/> is 0, it means current frame.
    /// </remarks>
    /// <param name="func"></param>
    /// <param name="nextFrame">0 means current frame</param>
    public void ExecuteFuncOnNextFrame(Action func, int nextFrame = 1)
    {
        if (nextFrame < 0) nextFrame = 0;
        if (func == null) return;

        StartCoroutine(this.CoroutineExecuteFuncOnNextFrame(func, nextFrame));
    }

    IEnumerator CoroutineExecuteFuncOnNextFrame(Action func, int nextFrame)
    {
        int frameCount = 0;

        while (frameCount < nextFrame)
        {
            frameCount++;
            yield return null;
        }

        func();
    }

    void Awake()
    {
        this.cbkta_globalui.controls = new();

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
        this.cbkta_globalstates.sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        //if (this.playerPositionStayedOn != Vector3.zero) this.FreezePlayer();
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
