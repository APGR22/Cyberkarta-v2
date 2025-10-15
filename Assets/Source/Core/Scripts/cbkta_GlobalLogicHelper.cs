using System.Collections;
using UnityEngine;

public class cbkta_GlobalLogicHelper : MonoBehaviour
{
    public System.Random GenerateRandomSystem()
    {
        return new System.Random(System.Guid.NewGuid().GetHashCode());
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
