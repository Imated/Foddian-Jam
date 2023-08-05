using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigidbody;
    bool rotationStarted = false;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.velocity = Vector2.right;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && readyToRotate(getClickPosition()))
        {
            print("rotation started");
            rotationStarted = true;
        }

        if(Input.GetMouseButton(0) && rotationStarted)
        {
            float radius = Vector2.Distance(getClickPosition(), new Vector2(transform.position.x, transform.position.y));

            rigidbody.AddForce(calculateGravitationalForce(radius, getClickPosition()));
        }

        if (!Input.GetMouseButton(0))
        {
            rotationStarted = false;
        }
    }

    bool readyToRotate(Vector2 clickPosition)
    {
        Vector2 vectorBetweenClickPointAndPlayer = clickPosition - new Vector2(transform.position.x, transform.position.y);
        float angle = Vector2.Angle(rigidbody.velocity, vectorBetweenClickPointAndPlayer);

        if ((angle > 89 & angle < 91) || (angle > 269 & angle < 271))
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    Vector2 getClickPosition()
    {
        Vector2 mousPosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        return (mousPosition);
    }

    Vector2 calculateGravitationalForce(float radius, Vector2 clickPosition)
    {
        float velocity = Mathf.Sqrt(Mathf.Pow(rigidbody.velocity.x, 2) + Mathf.Pow(rigidbody.velocity.y, 2));
        float force = Mathf.Pow(velocity, 2) / radius;
        Vector2 vector = clickPosition - new Vector2(transform.position.x, transform.position.y);

        return (vector.normalized * force);
    }
}
