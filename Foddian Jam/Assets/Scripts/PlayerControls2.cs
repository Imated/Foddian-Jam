using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls2 : MonoBehaviour
{
    [SerializeField] float turningDegreesPerFrame = 0;

    private Rigidbody2D _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
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
    }

    void turnLeft()
    {
        Vector2 originalVeclocity = _rigidbody.velocity;

        _rigidbody.velocity = Quaternion.AngleAxis(turningDegreesPerFrame, Vector3.forward) * _rigidbody.velocity;
    }

    void turnRight()
    {
        Vector2 originalVeclocity = _rigidbody.velocity;

        _rigidbody.velocity = Quaternion.AngleAxis(-turningDegreesPerFrame, Vector3.forward) * _rigidbody.velocity;
    }
}
