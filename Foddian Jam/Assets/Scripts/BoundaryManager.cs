using System;
using UnityEngine;
using TMPro;

public class BoundaryManager : MonoBehaviour
{
    [SerializeField] float boundaryBreakSpeed = 0;
    [SerializeField] GameObject boundaries;
    [SerializeField] GameObject timer;
    [SerializeField] GameObject endGameUI;
    [SerializeField] Animation endGameAnim;
    [SerializeField] TMP_Text endGameText;
    [SerializeField] LeaderboardManager3 leaderboard;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(!col.transform.CompareTag("Player"))
            return;
        var rigidBody = col.gameObject.GetComponent<Rigidbody2D>();

        if (rigidBody.velocity.magnitude >= boundaryBreakSpeed)
        {
            endGameUI.SetActive(true);
            endGameAnim.Play();
            endGameText.text = $"CONGRATULATIONS! \n" +
                               $"You have beaten the impossible in {(timer.GetComponent<Timer>().timer):F2} seconds.";
            timer.GetComponent<Timer>().timerActive = false;
            timer.SetActive(false);
            leaderboard.StartSubmitScoreCoroutine((int) Math.Floor(timer.GetComponent<Timer>().timer)); // need this because this gameobject becomes inactive and will not run a coroutine
            boundaries.SetActive(false);
        }
    }
}
