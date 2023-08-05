using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls2 : MonoBehaviour
{
    [SerializeField] float turningDegreesPerFrame = 0;
    [SerializeField] float launchMagnitude = 0;
    [SerializeField] float launchThreshold = 0;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private float launchForce;
    [SerializeField, Range(0, 30)] private int numberOfPoints = 15;
    [SerializeField, Range(0, 30)] private int trajectoryLineLength = 15;

    private GameObject[] _points;
    private Rigidbody2D _rigidbody;
    private Camera _camera;
    private Vector2 _direction;

    private void Awake()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
        
        _points = new GameObject[numberOfPoints];

        for (var i = 0; i < numberOfPoints; i++)
        {
            _points[i] = Instantiate(pointPrefab, transform.position, Quaternion.identity, transform);
        }
    }

    private Vector2 PointPosition(float t)
    {
        var pos = (Vector2) transform.position + (_direction.normalized * trajectoryLineLength * t) + 0.5f * Physics2D.gravity * (t * t);
        return pos;
    }


    private void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = mousePosition - transform.position;
        _direction = direction;

        float totalTime = (2 * launchForce) / Mathf.Abs(Physics2D.gravity.y);

        for (var i = 0; i < numberOfPoints; i++)
        {
            float t = totalTime * (i / (float)numberOfPoints);
            _points[i].transform.position = PointPosition(t);
        }

        
        if (Input.GetMouseButton(0))
        {
            turnLeft();
        }

        if (Input.GetMouseButton(1))
        {
            turnRight();
        }

        if (_rigidbody.velocity.magnitude <= launchThreshold)
        {
            for (var i = 0; i < numberOfPoints; i++)
                _points[i].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector2 mousPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 moveVector = mousPosition - new Vector2(transform.position.x, transform.position.y);
            
                launch(moveVector);
            
                for (var i = 0; i < numberOfPoints; i++)
                    _points[i].SetActive(false);
            }
        }
        else
        {
            for (var i = 0; i < numberOfPoints; i++)
                _points[i].SetActive(false);
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

    void launch(Vector2 direction)
    {
        _rigidbody.velocity = direction.normalized * launchMagnitude;
    }
}
