using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls2 : MonoBehaviour
{
    [SerializeField] float turningDegreesPerFrame = 0;
    [SerializeField] float launchMagnitude = 0;
    [SerializeField] float launchThreshold = 0;

    Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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

        if (Input.GetKeyDown(KeyCode.Space) & rigidbody.velocity.magnitude <= launchThreshold)
        {
            Vector2 mousPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 moveVector = mousPosition - new Vector2(transform.position.x, transform.position.y);
            
            launch(moveVector);
        }
    }

    void turnLeft()
    {
        Vector2 originalVeclocity = rigidbody.velocity;

        rigidbody.velocity = Quaternion.AngleAxis(turningDegreesPerFrame, Vector3.forward) * rigidbody.velocity;
    }

    void turnRight()
    {
        Vector2 originalVeclocity = rigidbody.velocity;

        rigidbody.velocity = Quaternion.AngleAxis(-turningDegreesPerFrame, Vector3.forward) * rigidbody.velocity;
    }

    void launch(Vector2 direction)
    {
        rigidbody.velocity = direction.normalized * launchMagnitude;
    }
}
