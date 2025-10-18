using UnityEngine;

public class SceneFightEntry : UnityScenesEntering
{
    public GameObject startLogic;

    //On UnityScenesEntering
    //public void Init();

    void Awake()
    {
        this.gameObjectToActivateWhenStart = this.startLogic;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
