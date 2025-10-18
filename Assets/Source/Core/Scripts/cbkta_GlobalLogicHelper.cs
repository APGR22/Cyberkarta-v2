using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cbkta_GlobalLogicHelper : MonoBehaviour
{
    public System.Random GenerateRandomSystem()
    {
        return new System.Random(System.Guid.NewGuid().GetHashCode());
    }

    public GameObject FindGameObjectWithTagInScene(string tag, Scene scene)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objs)
        {
            if (obj.scene == scene) return obj;
        }

        return null;
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
