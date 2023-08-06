using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls1 : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] GameObject anchorPointPrefab;
    [SerializeField] GameObject failAnchorPrefab;
    GameObject anchorPoint;
    GameObject failAnchor;

    [SerializeField] Vector2 mousePosition;
    [SerializeField] float angleVariance;
    [SerializeField] float driftSpeed;
    [SerializeField] Vector2 playerToMouse;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-10, 0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = GetMousePosition();
        }

        if (Input.GetMouseButton(0))
        {
            if (ReadyToRotate(mousePosition)) {
                Revolve();
                if (failAnchor != null)
                {
                    Destroy(failAnchor);
                }
                if (anchorPoint == null)
                {
                    anchorPoint = Instantiate(anchorPointPrefab, mousePosition, Quaternion.identity);
                }
            }
            else
            {
                Drift();
                if (failAnchor == null)
                {
                    failAnchor = Instantiate(failAnchorPrefab, mousePosition, Quaternion.identity);
                }
            }
        }

        if (!Input.GetMouseButton(0))
        {
            if (anchorPoint != null)
            {
                Destroy(anchorPoint);
            }
            if (failAnchor != null)
            {
                Destroy(failAnchor);
            }
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

    void Revolve()
    {
        Vector2 perpendicular = Vector2.Perpendicular(playerToMouse);
        float angleCheck = Vector2.Angle(rb.velocity, perpendicular);
        if (angleCheck > 90) // Greater than 90 means perpendicular is facing the opposite way
        {
            perpendicular = -perpendicular;
        }
        rb.velocity = perpendicular.normalized * rb.velocity.magnitude;
    }

    void Drift()
    {
        Vector2 perpendicular = Vector2.Perpendicular(playerToMouse);
        float angleCheck = Vector2.Angle(rb.velocity, perpendicular);
        if (angleCheck > 90) // Greater than 90 means perpendicular is facing the opposite way
        {
            perpendicular = -perpendicular;
        }
        float signedAngle = Vector2.SignedAngle(perpendicular, rb.velocity);
        // Positive angle means velocity is CCW to perpendicular
        // Negative angle means velocity is CW to perpendicular
        Quaternion rotation;
        if (signedAngle > 0)
        {
            rotation = Quaternion.AngleAxis(-driftSpeed, Vector3.forward);
        } else
        {
            rotation = Quaternion.AngleAxis(driftSpeed, Vector3.forward);
        }
        rb.velocity = rotation * rb.velocity;
        
    }
}
