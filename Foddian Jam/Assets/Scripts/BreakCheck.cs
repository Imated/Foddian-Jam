using System;
using UnityEngine;
using Cinemachine;
using Random = UnityEngine.Random;

public class BreakCheck : MonoBehaviour
{
    public Rigidbody2D rb;
    public Collider2D ballCollider;
    
    [SerializeField] float angleThreshold;
    [SerializeField] float speedThreshold;
    [SerializeField] bool useLinearGain;
    [SerializeField] float linearSpeedGain;
    [SerializeField] AnimationCurve speedGainCurve;
    [SerializeField] GameObject speedFragmentPerfab;
    [SerializeField] Vector2 speedFragmentsRange;
    [SerializeField] float spedFragmentsRandomOffset = 2f;
    [SerializeField] float deflectionMod;
    [SerializeField] float magnetMod;

    [SerializeField] bool curBreak;
    [SerializeField] float _timer;

    [SerializeField] AudioClip breakAudioClip;
    [SerializeField] AudioSource glideAudio;
    [SerializeField] AudioSource audioSource;
    [SerializeField] CinemachineImpulseSource cinemachineImpulseSource;

    private Vector2 _direction;
    private Vector2 _position;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballCollider = GetComponent<Collider2D>();

        curBreak = false;
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
            if (impactAngle <= angleThreshold) {
                if (relVelocity.magnitude >= speedThreshold)
                {
                    CameraShakeManager.instance.cameraShake(cinemachineImpulseSource);
                    audioSource.PlayOneShot(breakAudioClip);
                    // At a 45 degree impact, go perpendicular to surface (Unity physics)
                    // At a 0 degree impact, go straight forward

                    // Set flags
                    print("break");
                    print("impact angle: " + impactAngle);

                    // Set break logic and allow phasing
                    //curBreak = true;
                    //ballCollider.isTrigger = true;

                    // Calculate deflection velocity vector
                    Vector2 deflectVelocity = relVelocity.magnitude * deflectionMod * averageNormal;

                    // Apply deflection against velocity before collision
                    //rb.velocity = -(relVelocity) + deflectVelocity;

                    // ALTERNATE: Apply deflection as new velocity (wack because angle change)
                    // rb.velocity = deflectVelocity;

                    // ALTERNATE: Reflect velocity across normal to bounce
                    Vector2 reflect = Vector2.Reflect(-relVelocity, averageNormal);
                    rb.velocity = reflect * deflectionMod;
                }
                else // Not a fast enough collision
                {
                    glideAudio.gameObject.SetActive(true);

                    // Vector2 magnetism = relVelocity.magnitude * magnetMod * -(averageNormal);
                    // rb.velocity += magnetism;
                }
            }
            else // Not a sharp collision
            {
                glideAudio.gameObject.SetActive(true);

                Vector2 magnetism = relVelocity.magnitude * magnetMod * -(averageNormal);
                rb.velocity += magnetism;
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
            if (useLinearGain) // Linear speed scaling
            {
                // Gain a flat amount of speed per tick
                rb.velocity = (rb.velocity.magnitude + linearSpeedGain) * rb.velocity.normalized;
            }
            else // Multiplicative scaling
            {
                // Gain speed according to Speed Gain Curve based on time in contact
                rb.velocity *= 1 + speedGainCurve.Evaluate(_timer);
                _timer += Time.deltaTime;
            }
            
            var rotationAmount = collision.relativeVelocity.magnitude * 0.5f;
            var normal = collision.contacts[0].normal;
            float direction = Vector3.Cross(collision.relativeVelocity, normal).z > 0 ? 1 : -1;  // Determine if clockwise or counter-clockwise
            transform.Rotate(0, 0, rotationAmount * direction);

            
            // Find normal of curve surface
            Vector2 averageNormal = Vector2.zero;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                averageNormal += contact.normal;
            }
            averageNormal = averageNormal.normalized;

            // Apply magnet velocity toward curve
            Vector2 magnetism = rb.velocity.magnitude * magnetMod * -(averageNormal);
            rb.velocity += magnetism;
        }
        else
        {
            print("wrong tag lmao");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        glideAudio.gameObject.SetActive(false);
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