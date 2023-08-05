using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCheck : MonoBehaviour
{
    public GameObject character;
    public Rigidbody2D rb;

    [SerializeField] private float angleThreshold = 45f;
    [SerializeField] private float speedThreshold = 10f;
    [SerializeField] private AnimationCurve speedGainCurve;

    private float _timer;

    private void Start()
    {
        rb = character.GetComponent<Rigidbody2D>();

        // Gets velocity magnitude of exactly 4
        //Vector2 force = Vector2.up * 200;
        //rb.AddForce(force);

        rb.velocity = new Vector2(0, 4);
    }

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
            if ((impactAngle < angleThreshold) && (relVelocity.magnitude >= speedThreshold))
            {
                // Change whatever values need to be changed for a curve break here
                print("break");
            }
        }
        else
        {
            print("wrong tag lmao");
        }
    }

    // private void Update()
    // {
    //     if (_timer > 1f)
    //         _timer = 0f;
    // }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            rb.velocity *= 1 + speedGainCurve.Evaluate(_timer);
            _timer += Time.deltaTime;
        }
        else
        {
            print("wrong tag lmao");
        }
    }
}
