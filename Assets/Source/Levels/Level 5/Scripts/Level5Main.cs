using UnityEngine;

public class Level5Main : LevelMain
{
    public BoxCollider2D dialogTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.ParentInit(() =>
        {
            this.dialogTrigger.enabled = true;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
