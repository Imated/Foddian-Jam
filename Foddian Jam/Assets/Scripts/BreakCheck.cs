using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BreakCheck : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D ballCollider;
    
    [SerializeField] private float angleThreshold;
    [SerializeField] private float speedThreshold;
    [SerializeField] private AnimationCurve speedGainCurve;
    [SerializeField] private GameObject speedFragmentPerfab;
    [SerializeField] private Vector2 speedFragmentsRange;
    [SerializeField] private float spedFragmentsRandomOffset = 2f;
    public float deflectionMod;

    [SerializeField] bool curBreak;
    [SerializeField] float _timer;

    private Vector2 _direction;
    private Vector2 _position;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballCollider = GetComponent<Collider2D>();

        curBreak = false;
        rb.velocity = new Vector2(0, 10);
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
            _direction = -averageNormal;

            // Angles closer to 0 are sharp collisions, 90 is parallel to surface
            float impactAngle = Vector2.Angle(relVelocity, averageNormal);
            if ((impactAngle <= angleThreshold) && (relVelocity.magnitude >= speedThreshold))
            {
                // At a 45 degree impact, go perpendicular to surface (Unity physics)
                // At a 0 degree impact, go straight forward
                
                // Set flags
                print("break");
                print("impact angle: " + impactAngle);
                curBreak = true;
                ballCollider.isTrigger = true;

                // Calculate deflection velocity vector
                Vector2 deflectVelocity = relVelocity.magnitude * deflectionMod * averageNormal;

                // Apply deflection against velocity before collision
                rb.velocity = -(relVelocity) + deflectVelocity;
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
        if (collision.gameObject.CompareTag("Obstacle") && !curBreak)
        {
            rb.velocity *= 1 + speedGainCurve.Evaluate(_timer);
            _timer += Time.deltaTime;
        }
        else
        {
            print("wrong tag lmao");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _timer = 0f;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Obstacle") && curBreak)
        {
            ballCollider.isTrigger = false;
            _position = transform.position;
            SpawnSpeedFragments();
            curBreak = false;
        }
        _timer = 0f;
    }

    private void SpawnSpeedFragments()
    {
        var amount = Random.Range(speedFragmentsRange.x, speedFragmentsRange.y);
        for (int i = 0; i < amount; i++)
        {
            var dir = _direction;
            dir.y = 1f;
            dir.x += Random.Range(-0.25f, 0.25f);
            var pos = _position;
            pos.x += Random.Range(-spedFragmentsRandomOffset, spedFragmentsRandomOffset);
            pos.y += Random.Range(0f, spedFragmentsRandomOffset);
            Instantiate(speedFragmentPerfab, pos, Quaternion.identity).GetComponent<SpedFragment>().Go(dir, rb.velocity.magnitude);
        }
    }
}