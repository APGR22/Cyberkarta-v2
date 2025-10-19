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

[System.Serializable]
public class FightData
{
    public int minArrowCounts = 3;
    public int maxArrowCounts = 5;

    [Header("Determinator")]
    public bool shakeCamera = false;
    public bool shakePanel = true;
}
