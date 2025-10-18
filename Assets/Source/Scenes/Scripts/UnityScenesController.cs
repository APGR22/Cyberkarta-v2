using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnityScenesController : MonoBehaviour
{
    public cbkta_GlobalLogicHelper cbkta_globallogichelper;
    public cbkta_GlobalTags cbkta_globaltags;

    public void AddMinorScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void RemoveMinorScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public void ShowMinorSceneVisibility(Scene scene, Action funcEnd = null)
    {
        foreach (GameObject gameObject in scene.GetRootGameObjects())
        {
            if (gameObject.name != "Fade") continue;

            gameObject.SetActive(true);
            FadeController fadeController = gameObject.GetComponent<FadeController>();
            fadeController.FadeOut(() =>
            {
                if (funcEnd != null) funcEnd();
                gameObject.SetActive(false);
            }
            );

            break;
        }
    }

    public void HideMinorSceneVisibility(Scene scene, Action funcEnd = null)
    {
        foreach (GameObject gameObject in scene.GetRootGameObjects())
        {
            if (gameObject.name != "Fade") continue;

            gameObject.SetActive(true);
            FadeController fadeController = gameObject.GetComponent<FadeController>();
            fadeController.FadeIn(() =>
            {
                if (funcEnd != null) funcEnd();
            }
            );

            break;
        }
    }

    public void EnterAnotherScene(Scene scene)
    {
        GameObject obj = this.cbkta_globallogichelper.FindGameObjectWithTagInScene(this.cbkta_globaltags.unityScenesEntering, scene);
        UnityScenesEntering objUSE = obj.GetComponent<UnityScenesEntering>();
        objUSE.Init();
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
