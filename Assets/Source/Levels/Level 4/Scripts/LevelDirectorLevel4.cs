using System.Collections.Generic;
using UnityEngine;

public class LevelDirectorLevel4 : LevelDirectorMain
{
    private List<LevelDirectorData> eventsRegistered = new List<LevelDirectorData>
    {
        new LevelDirectorData(),
    };

    protected override List<LevelDirectorData> GetEventsRegistered()
    {
        return this.eventsRegistered;
    }

    private int FightDoneCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
