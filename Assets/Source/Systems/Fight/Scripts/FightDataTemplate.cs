using UnityEngine;

public enum FightDataType
{
    arrowUp,
    arrowDown,
    arrowLeft,
    arrowRight,
    //A,
    //B,
    //C,
    //D,
}

[RequireComponent(typeof(SceneDataTemplate))]
public class FightDataTemplate : MonoBehaviour
{
    public int minArrowCounts = 3;
    public int maxArrowCounts = 5;

    [Header("Determinator")]
    public bool shakeCamera = false;
    public bool shakePanel = true;

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
