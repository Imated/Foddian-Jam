using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Launcher : MonoBehaviour
{
    [SerializeField] float launchMagnitude = 5;
    [SerializeField] float launchThreshold = 1;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private Timer timer;
    [SerializeField] private float launchForce = 1;
    [SerializeField, Range(0, 30)] private int numberOfPoints = 5;
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
        if (!PauseMenu.IsPaused)
        {
            var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            var direction = mousePosition - transform.position;
            _direction = direction;

            float totalTime = 2 * launchForce / Mathf.Abs(Physics2D.gravity.y);

            for (var i = 0; i < numberOfPoints; i++)
            {
                var t = totalTime * (i / (float)numberOfPoints);
                _points[i].transform.position = PointPosition(t);
            }

            if (_rigidbody.velocity.magnitude <= launchThreshold)
            {
                for (var i = 0; i < numberOfPoints; i++)
                    _points[i].SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    var moveVector = mousePosition - new Vector3(transform.position.x, transform.position.y);

                    Launch(moveVector);

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
    }

    private void Launch(Vector2 direction)
    {
        _rigidbody.velocity = direction.normalized * launchMagnitude;
        timer.timerActive = true;
    }
}
