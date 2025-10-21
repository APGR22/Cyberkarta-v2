using cbkta.UI.Controls;
using Unity.Cinemachine;
using UnityEngine;

public class cbkta_GlobalUI : MonoBehaviour
{
    public GameObject cam;
    public CinemachineCamera cinemachineCamera;
    public UIControls controls;
    public GameObject dialog = null;
    public GameObject pauseMenu = null;
    public GameObject fight = null;
    public GameObject canvas;
    public GameObject secondEnvironments = null;
    public FadeController fadeController = null;
    public SoundManagerLogic soundManagerLogic = null;
    public LevelEntryPortal nextLevelEntryPortal = null;
    public LevelDirectorMain levelDirectorMain = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
