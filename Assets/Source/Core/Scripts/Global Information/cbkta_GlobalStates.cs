using UnityEngine;

public class cbkta_GlobalStates : MonoBehaviour
{
    public bool isPause = false;
    public bool isDialogDone = false;
    public bool isFightDone = false;

    public bool isEnterDialog = false;

    public bool isInteractionTrigger = false;

    public int sceneIndex = 0;
    /// <summary>
    /// If true, then the scene will be paused
    /// </summary>
    /// <remarks>
    /// This flag used by the <see cref="SceneDirector"/> to determine whether to pause the current scene.
    /// </remarks>
    public bool isSceneInterrupted = false;

    public int playerStatsIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
