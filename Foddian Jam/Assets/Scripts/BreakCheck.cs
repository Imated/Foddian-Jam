using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCheck : MonoBehaviour
{
    public GameObject character;
    public Rigidbody2D rb;

    public float angleThreshold = 45f;
    public float speedThreshold = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = character.GetComponent<Rigidbody2D>();

        // Gets velocity magnitude of exactly 4
        //Vector2 force = Vector2.up * 200;
        //rb.AddForce(force);

        rb.velocity = new Vector2(0, 10);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 relVelocity = collision.relativeVelocity;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            print("pre impact speed: " + relVelocity.magnitude);
            Vector2 averageNormal = Vector2.zero;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                averageNormal += contact.normal;
            }
            averageNormal = averageNormal.normalized;

            // Angles closer to 0 are sharp collisions, 90 is parallel to surface
            float impactAngle = Vector2.Angle(relVelocity, averageNormal);
            print("angle of impact: " + impactAngle);
            if ((impactAngle < angleThreshold) && (relVelocity.magnitude >= speedThreshold))
            {
                print("break");
            }
            print("post impact speed: " + rb.velocity.magnitude);
            print("component toward surface: " + relVelocity.magnitude * Mathf.Cos(impactAngle * Mathf.Deg2Rad));
        }

        else
        {
            print("wrong tag lmao");
        }
    }
}
