using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls2 : MonoBehaviour
{
    [SerializeField] float turningForce;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = Vector2.up * 5;
    }

    void turnLeft()
    {
        Vector2 originalVeclocity = rb.velocity;

        rb.velocity = Quaternion.AngleAxis(.1f, Vector3.forward) * rb.velocity;
    }

    void turnRight()
    {
        Vector2 originalVeclocity = rb.velocity;

        rb.velocity = Quaternion.AngleAxis(-.1f, Vector3.forward) * rb.velocity;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            turnLeft();
        }

        if (Input.GetMouseButton(1))
        {
            turnRight();
        }
    }
}
