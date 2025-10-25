using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Used by <see cref="Dialogue"/>
/// </summary>
public enum NonPlayerType
{
    None,
    Human,
    Robot
}

public class NonPlayer : MonoBehaviour
{
    public cbkta_GlobalStates cbkta_globalstates;
    public cbkta_GlobalObjects cbkta_globalobjects;

    private bool triggedWithPlayer = false;
    //private SpriteRenderer indicator;

    private bool indicatorChildGameObject = false;

    void Awake()
    {

        //this.indicator = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //this.indicator.enabled = true;

        foreach (Transform child in transform)
        {
            GameObject obj = child.gameObject;
            obj.SetActive(true);
        }

        this.indicatorChildGameObject = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (!this.indicator.enabled) return;
        if (!this.indicatorChildGameObject) return;

        if (
            this.triggedWithPlayer
            &&
            this.cbkta_globalobjects.playerTriggeredWithObject != null
            &&
            this.cbkta_globalstates.isInteractionTrigger
            )
        {
            foreach (Transform child in transform)
            {
                GameObject obj = child.gameObject;
                obj.SetActive(false);
            }

            //this.indicator.enabled = false;
            this.indicatorChildGameObject = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        this.triggedWithPlayer = collision.CompareTag("Player");
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        this.triggedWithPlayer = !collision.CompareTag("Player");
    }
}
