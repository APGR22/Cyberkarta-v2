using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public Sprite icon;
    public string text;
    public NonPlayerType nonPlayerType;
}

[System.Serializable]
public class DialogueData2D
{
    public DialogueData[] data;

    //data type
}