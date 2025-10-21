using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class LevelEntryPortal : MonoBehaviour
{
    public cbkta_GlobalLogic cbkta_globallogic;
    public cbkta_GlobalTags cbkta_globaltags;
    public cbkta_GlobalUI cbkta_globalui;

    [Tooltip("Set sceneIndex to -1 to go to the next scene in build order, -2 to go to the previous scene, or any other non-negative integer to go to that specific scene index.")]
    public int sceneIndex = -1;

    void Init()
    {
        if (this.sceneIndex < -2)
        {
            this.sceneIndex = -1;
        }
    }

    void EnterAnotherScene()
    {
        //setup
        Action funcEnd = null;

        //settings

        this.cbkta_globallogic.FreezePlayer();

        if (this.sceneIndex >= 0)
        {
            funcEnd = () => { SceneManager.LoadScene(this.sceneIndex); };
        }
        else
        {
            if (this.sceneIndex == -1)
            {
                funcEnd = () => { this.cbkta_globallogic.NextScene(); };
            }
            else if (this.sceneIndex == -2)
            {
                funcEnd = () => { this.cbkta_globallogic.PreviousScene(); };
            }
            else
            {
                //tidak ada yang tepat
                return;
            }
        }

        this.cbkta_globalui.fadeController.FadeIn(funcEnd);
    }

    void Awake()
    {
        this.Init();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(this.cbkta_globaltags.player))
        {
            this.EnterAnotherScene();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(this.cbkta_globaltags.player))
        {
            this.EnterAnotherScene();
        }
    }
}