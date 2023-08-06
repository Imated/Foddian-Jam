using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls1 : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] GameObject anchorPointPrefab;
    GameObject anchorPoint;

    [SerializeField] Vector2 mousePosition;
    [SerializeField] bool revolving = false;
    [SerializeField] float angleVariance;
    [SerializeField] Vector2 playerToMouse;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = GetMousePosition();
        }

        if (Input.GetMouseButton(0) && ReadyToRotate(mousePosition))
        {
            revolving = true;
            if (anchorPoint == null)
            {
                anchorPoint = Instantiate(anchorPointPrefab, mousePosition, Quaternion.identity);
            }
        }

        if (!Input.GetMouseButton(0))
        {
            revolving = false;
            if (anchorPoint != null)
            {
                Destroy(anchorPoint);
            }
        }

        if (revolving)
        {
            Vector2 perpendicular = Vector2.Perpendicular(playerToMouse);
            float angleCheck = Vector2.Angle(rb.velocity, perpendicular);
            if (angleCheck > 90) // Greater than 90 means perpendicular is facing the opoosite way
            {
                perpendicular = -perpendicular;
            }
            rb.velocity = perpendicular.normalized * rb.velocity.magnitude;
        }

        // Find angle between player velocity and mouse
        //if (Input.GetMouseButton(0))
        //{
        //    Vector2 playerToMouse = GetMousePosition() - new Vector2(transform.position.x, transform.position.y);
        //    float angle = Vector2.Angle(rb.velocity, playerToMouse);
        //    print(angle);
        //}
    }

    bool ReadyToRotate(Vector2 clickPosition)
    {
        playerToMouse = clickPosition - new Vector2(transform.position.x, transform.position.y);
        float angle = Vector2.Angle(rb.velocity, playerToMouse);
        // Angle of 0 is same direction, 90 is perpendicular either way, 180 is opposite

        if (angle >= (90 - angleVariance) && angle <= (90 + angleVariance))
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

    Vector2 GetMousePosition()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        return (mousePosition);
    }
}
