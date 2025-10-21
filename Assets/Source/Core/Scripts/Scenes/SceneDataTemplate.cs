using UnityEngine;

public enum SceneData
{
    Dialog,
    Fight,
    EnterSecondEnvironment,
    ExitSecondEnvironment,
}

public class SceneDataTemplate : MonoBehaviour
{
    public SceneData[] scenes;
    [HideInInspector] public bool hasScene = false; //apakah sudah scene

    [Tooltip("(Opsional) Mengirim sinyal event ke pusat ketika semua scene interaksi selesai")]
    public string eventNameForLevelDirectorMain = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
