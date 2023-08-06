using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class BoundaryManager : MonoBehaviour
{
    [SerializeField] float boundaryBreakSpeed = 0;
    [SerializeField] GameObject boundaries;
    [SerializeField] GameObject timer;
    [SerializeField] GameObject victoryText;
    [SerializeField] Leaderboard leaderboard;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(!col.transform.CompareTag("Player"))
            return;
        var rigidBody = col.gameObject.GetComponent<Rigidbody2D>();

        if (rigidBody.velocity.magnitude >= boundaryBreakSpeed)
        {
            boundaries.SetActive(false);
            victoryText.SetActive(true);
            timer.GetComponent<Timer>().timerActive = false;
            StartCoroutine(leaderboard.SubmitScoreRoutine((int)Math.Floor(timer.GetComponent<Timer>().timer)));
        }
    }
}
