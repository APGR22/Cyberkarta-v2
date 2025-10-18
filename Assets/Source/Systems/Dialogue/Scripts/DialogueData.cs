using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public Sprite icon;
    public string text;
}

[System.Serializable]
public class DialogueData2D
{
    public DialogueData[] data;
}