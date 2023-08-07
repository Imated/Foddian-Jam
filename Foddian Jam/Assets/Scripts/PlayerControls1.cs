using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls1 : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] AudioSource StartTurnSFX;
    [SerializeField] AudioSource StopTurnSFX;
    [SerializeField] GameObject anchorPointPrefab;
    [SerializeField] GameObject failAnchorPrefab;
    [SerializeField] LineRenderer lineRenderer;
    LineRenderer swivelLine;
    GameObject anchorPoint;
    GameObject failAnchor;

    bool allowAnchor;
    bool turning;
    [SerializeField] Vector2 anchorPosition;
    [SerializeField] float angleVariance;
    [SerializeField] float driftSpeed;
    [SerializeField] Vector2 playerToAnchor;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        allowAnchor = true;
        turning = false;
    }

    private void Update()
    {
        if (!PauseMenu.IsPaused) 
        {
            if(rb.velocity.magnitude <= 1f)
                return;
            if (Input.GetMouseButtonDown(0))
            {
                swivelLine = Instantiate(lineRenderer);
                swivelLine.positionCount = 2;
                anchorPosition = GetAnchorPosition();
                swivelLine.SetPosition(1, anchorPosition);
                StartTurnSFX.Play();
                turning = true;
            }

            if (Input.GetMouseButton(0) && swivelLine != null)
            {
                swivelLine.SetPosition(0, gameObject.transform.position);

                // Check if anchor is within tolerance for a full revolve
                // Angle of 0 is same direction, 90 is perpendicular either way, 180 is opposite
                if (AnchorAngle(anchorPosition) >= 90 + angleVariance) // Anchor in back
                {
                    // Drift to match projected circle
                    // Set swivel line to blue
                    var grad = new Gradient();
                    grad.colorKeys = new GradientColorKey[]
                    {
                    new GradientColorKey(Color.blue, 0f),
                    new GradientColorKey(Color.blue, 1f),
                    };
                    swivelLine.colorGradient = grad;

                    // Handle movement to enter a circle
                    if (allowAnchor)
                    {
                        Drift();
                    }

                    // Create visual anchor
                    if (failAnchor == null)
                    {
                        failAnchor = Instantiate(failAnchorPrefab, anchorPosition, Quaternion.identity);
                    }
                    if (anchorPoint != null)
                    {
                        Destroy(anchorPoint);
                    }
                }
                else if (AnchorAngle(anchorPosition) <= 90 - angleVariance) // Anchor in front
                {
                    // Pull toward the pivot
                    // Set swivel line to blue
                    var grad = new Gradient();
                    grad.colorKeys = new GradientColorKey[]
                    {
                    new GradientColorKey(Color.blue, 0f),
                    new GradientColorKey(Color.blue, 1f),
                    };
                    swivelLine.colorGradient = grad;

                    // DO NOTHING I GUESS??????
                    // You CAN use Pull() but it's too easy

                    // Handle movement to pull toward the pivot
                    //if (allowAnchor)
                    //{
                    //    Pull();
                    //}

                    // Create visual anchor
                    if (failAnchor == null)
                    {
                        failAnchor = Instantiate(failAnchorPrefab, anchorPosition, Quaternion.identity);
                    }
                    if (anchorPoint != null)
                    {
                        Destroy(anchorPoint);
                    }
                }
                else // Anchor is within tolerance
                {
                    // Properly revolve
                    // Set swivel line to red
                    var grad = new Gradient();
                    grad.colorKeys = new GradientColorKey[]
                    {
                    new GradientColorKey(Color.red, 0f),
                    new GradientColorKey(Color.red, 1f),
                    };
                    swivelLine.colorGradient = grad;

                    // Handle movement in a circle
                    if (allowAnchor)
                    {
                        Revolve();
                    }

                    // Replace and create visual anchor
                    if (failAnchor != null)
                    {
                        Destroy(failAnchor);
                    }
                    if (anchorPoint == null)
                    {
                        anchorPoint = Instantiate(anchorPointPrefab, anchorPosition, Quaternion.identity);
                    }
                }
            }

            if (!Input.GetMouseButton(0))
            {
                if (turning)
                {
                    StopTurnSFX.Play();
                    turning = false;
                }
                if (anchorPoint != null)
                {
                    Destroy(anchorPoint);
                }
                if (failAnchor != null)
                {
                    Destroy(failAnchor);
                }
                if (swivelLine != null)
                {
                    Destroy(swivelLine);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            allowAnchor = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            allowAnchor = true;
        }
    }

    float AnchorAngle(Vector2 clickPosition)
    {
        playerToAnchor = clickPosition - new Vector2(transform.position.x, transform.position.y);
        float angle = Vector2.Angle(rb.velocity, playerToAnchor);

        // Angle of 0 is same direction, 90 is perpendicular either way, 180 is opposite
        return angle;
    }

    Vector2 GetAnchorPosition()
    {
        Vector2 anchorPosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        return (anchorPosition);
    }

    void Revolve()
    {
        Vector2 perpendicular = Vector2.Perpendicular(playerToAnchor);
        float angleCheck = Vector2.Angle(rb.velocity, perpendicular);
        if (angleCheck > 90) // Greater than 90 means perpendicular is facing the opposite way
        {
            perpendicular = -perpendicular;
        }
        rb.velocity = perpendicular.normalized * rb.velocity.magnitude;
    }

    void Drift()
    {
        Vector2 perpendicular = Vector2.Perpendicular(playerToAnchor);
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

    void Pull()
    {
        float signedAngle = Vector2.SignedAngle(playerToAnchor, rb.velocity);
        // Positive angle means velocity is CCW to perpendicular
        // Negative angle means velocity is CW to perpendicular
        Quaternion rotation;
        if (signedAngle > 0)
        {
            rotation = Quaternion.AngleAxis(-driftSpeed, Vector3.forward);
        }
        else
        {
            rotation = Quaternion.AngleAxis(driftSpeed, Vector3.forward);
        }
        rb.velocity = rotation * rb.velocity;
    }
}
