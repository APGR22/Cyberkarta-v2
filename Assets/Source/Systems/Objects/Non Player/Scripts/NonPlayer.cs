using UnityEngine;

public class NonPlayer : MonoBehaviour
{
    public cbkta_GlobalStates cbkta_globalstates;
    public cbkta_GlobalObjects cbkta_globalobjects;

    private bool triggedWithPlayer = false;
    private SpriteRenderer indicator;

    void Awake()
    {
        this.indicator = transform.GetChild(0).GetComponent<SpriteRenderer>();
        this.indicator.enabled = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!this.indicator.enabled) return;

        if (
            this.triggedWithPlayer
            &&
            this.cbkta_globalobjects.playerTriggeredWithObject != null
            &&
            this.cbkta_globalstates.isInteractionTrigger
            )
        {
            this.indicator.enabled = false;
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
