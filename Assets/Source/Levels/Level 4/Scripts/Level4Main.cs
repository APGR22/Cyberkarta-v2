using UnityEngine;

public class Level4Main : LevelMain
{
    public GameObject dialogTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.ParentInit(() =>
        {
            this.dialogTrigger.SetActive(true);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
