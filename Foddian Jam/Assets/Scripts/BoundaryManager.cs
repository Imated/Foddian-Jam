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
    [SerializeField] LeaderboardManager2 leaderboard;

    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rigidBody.velocity.magnitude >= boundaryBreakSpeed)
        {
            boundaries.SetActive(false);
            victoryText.SetActive(true);
            timer.GetComponent<Timer>().timerActive = false;
            StartCoroutine(leaderboard.SubmitScoreRoutine((int)Math.Floor(timer.GetComponent<Timer>().timer)));
        }
    }
}
