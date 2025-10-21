using UnityEngine;

[RequireComponent(typeof(SceneDataTemplate))]
public class FightDataTemplate : MonoBehaviour
{
    public FightData[] data;

    [Tooltip("(Opsional) Mengirim sinyal event ke pusat ketika dialog selesai")]
    public string eventNameForLevelDirectorMain = "";

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
        //create randomize for combination
    }
}
