using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.cam = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new(cam.transform.position.x, cam.transform.position.y, transform.position.z);
    }
}
