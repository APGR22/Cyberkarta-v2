using UnityEngine;

public class UnityScenesEntering : MonoBehaviour
{
    protected GameObject gameObjectToActivateWhenStart; //can only accessed by child of this class

    public void Init()
    {
        if (this.gameObjectToActivateWhenStart != null) this.gameObjectToActivateWhenStart.SetActive(true);
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
