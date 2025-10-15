using UnityEngine;

public class mainMenuLogic : MonoBehaviour
{
    public GameObject objectTracking;

    void Move()
    {
        this.objectTracking.transform.Translate(new Vector2(5f * Time.deltaTime, 0));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.Move();
    }

    // Update is called once per frame
    void Update()
    {
        this.Move();
    }
}
