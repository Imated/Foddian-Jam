using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCheck : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D ballCollider;

    public float angleThreshold = 45f;
    public float speedThreshold = 10f;
    public float breakSpeedMod = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballCollider = GetComponent<Collider2D>();

        // Gets velocity magnitude of exactly 4
        //Vector2 force = Vector2.up * 200;
        //rb.AddForce(force);

        rb.velocity = new Vector2(0, 10);
    }

    // Update is called once per frame
    void Update()
    {
    }

    //public bool BreakCurve()
    //{

    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 relVelocity = collision.relativeVelocity;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Vector2 averageNormal = Vector2.zero;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                averageNormal += contact.normal;
            }
            averageNormal = averageNormal.normalized;

            // Angles closer to 0 are sharp collisions, 90 is parallel to surface
            float impactAngle = Vector2.Angle(relVelocity, averageNormal);
            if ((impactAngle <= angleThreshold) && (relVelocity.magnitude >= speedThreshold))
            {
                // At a 45 degree impact, go perpendicular to surface (Unity physics)
                // At a 0 degree impact, go straight forward
                print("break");
                ballCollider.isTrigger = true;
                float breakVelocity = relVelocity.magnitude * breakSpeedMod;
                rb.velocity = rb.velocity.normalized * breakVelocity;
            }
        }

        else
        {
            print("wrong tag lmao");
        }
    }
}
