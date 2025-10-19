using System.Collections;
using UnityEngine;

public class cbkta_GlobalLogicHelper : MonoBehaviour
{
    public System.Random GenerateRandomSystem()
    {
        return new System.Random(System.Guid.NewGuid().GetHashCode());
    }

    public void CanvasDisableFollowCamera(Canvas canvas)
    {
        canvas.worldCamera = null;
    }

    public void CanvasEnableFollowCamera(Canvas canvas, Camera camera)
    {
        canvas.worldCamera = camera;
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
