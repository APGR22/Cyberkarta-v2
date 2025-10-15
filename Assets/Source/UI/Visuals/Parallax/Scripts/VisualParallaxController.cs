using System.Collections.Generic;
using UnityEngine;

public class VisualParallaxController : MonoBehaviour
{
    public GameObject objectTracking;

    private List<VisualParallaxContent> contents = new();
    private float speedOneDirection = 0f;
    private Vector2 previousObjectTrackingPosition = Vector2.zero;

    void Init()
    {
        foreach (Transform child in transform)
        {
            this.contents.Add(child.GetComponent<VisualParallaxContent>());
        }
    }

    Vector2 GetObjectMovement()
    {
        return this.objectTracking.transform.position - (Vector3) this.previousObjectTrackingPosition;
    }

    void UpdateObjectPosition()
    {
        this.previousObjectTrackingPosition = this.objectTracking.transform.position;
    }

    void MoveContent(VisualParallaxContent parallaxContent, Vector2 movement)
    {
        //identitas = menggunakan atau mematikan nilainya

        float xIdentity = (parallaxContent.direction == VisualParallaxDirection.Left || parallaxContent.direction == VisualParallaxDirection.Right) ? 1 : 0;
        float yIdentity = (parallaxContent.direction == VisualParallaxDirection.Up || parallaxContent.direction == VisualParallaxDirection.Down) ? 1 : 0;

        float objectTrackingXIdentity = movement.x != 0 ? (movement.x < 0 && false ? -1 : 1) : 0;
        float objectTrackingYIdentity = movement.y != 0 ? (movement.y < 0 && false ? -1 : 1) : 0;

        Vector2 identity = new(xIdentity * objectTrackingXIdentity, yIdentity * objectTrackingYIdentity);

        //menghitung kecepatan
        //kecepatan penuh - (kecepatan efek parallax. Semakin besar nilai Z, semakin kecil hasilnya)
        float f = 1 - (1 / (1 + parallaxContent.zDistance));

        //pergerakan objek yang diikuti, dikali dengan nilai kecepatan 0-1
        Vector2 speed = new(movement.x * f, movement.y * f);

        //hasil yang menentukan translasi (pergerakan)
        Vector2 translate = speed * identity;

        parallaxContent.transform.Translate(translate);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.Init();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = this.GetObjectMovement();

        foreach (VisualParallaxContent content in this.contents)
        {
            this.MoveContent(content, movement);
        }

        this.UpdateObjectPosition();
    }
}
