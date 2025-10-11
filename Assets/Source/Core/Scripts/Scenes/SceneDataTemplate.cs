using UnityEngine;

public enum SceneData
{
    Dialog,
    Fight,
}

public class SceneDataTemplate : MonoBehaviour
{
    public SceneData[] scenes;
    [HideInInspector] public bool hasScene = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
