using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls2 : MonoBehaviour
{
    [SerializeField] float turningForce;

    Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidbody.velocity = Vector2.up * 5;
    }

    void turnLeft()
    {
        Vector2 originalVeclocity = rigidbody.velocity;

        rigidbody.velocity = Quaternion.AngleAxis(.1f, Vector3.forward) * rigidbody.velocity;
    }

    void turnRight()
    {
        Vector2 originalVeclocity = rigidbody.velocity;

        rigidbody.velocity = Quaternion.AngleAxis(-.1f, Vector3.forward) * rigidbody.velocity;
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
