using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    [SerializeField] float boundaryBreakSpeed = 0;
    [SerializeField] GameObject boundaries;
    [SerializeField] GameObject timer;
    [SerializeField] GameObject victoryText;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(!col.transform.CompareTag("Player"))
            return;
        var rigidBody = col.gameObject.GetComponent<Rigidbody2D>();

        if(rigidBody.velocity.magnitude >= boundaryBreakSpeed)
        {
            boundaries.SetActive(false);
            victoryText.SetActive(true);
            timer.GetComponent<Timer>().timerActive = false;   
        }
    }
}
