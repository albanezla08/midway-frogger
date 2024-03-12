using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Sprite[] spriteOptions;
    // private Transform tr;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        // tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;

        // choose random color for car
        sr.sprite = spriteOptions[Random.Range(0,spriteOptions.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        rb.position += Vector2.left * speed * Time.deltaTime;
        if (rb.position.x + (sr.sprite.bounds.size.x/2) < -cam.orthographicSize * cam.aspect)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerMovement>().Respawn();
        }
    }

    public float getSpeed()
    {
        return speed;
    }
    public void setSpeed(float val)
    {
        speed = val;
    }
}
