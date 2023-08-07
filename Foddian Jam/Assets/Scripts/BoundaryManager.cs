using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class BoundaryManager : MonoBehaviour
{
    public static bool HasGameEnded;
    
    [SerializeField] private float boundaryBreakSpeed = 0;
    [SerializeField] private GameObject boundaries;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private Animator endGameUIAnimator;
    [SerializeField] private TMP_Text endGameText;
    [SerializeField] private LeaderboardManager3 leaderboard;
    [SerializeField] private AudioSource mainMusic;
    [SerializeField] private AudioSource endMusic;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(!col.transform.CompareTag("Player"))
            return;
        var rigidBody = col.gameObject.GetComponent<Rigidbody2D>();

        if (rigidBody.velocity.magnitude >= boundaryBreakSpeed)
        {
            timer.SetActive(false);
            endGameUI.SetActive(true);
            endGameUIAnimator.Play("End Game UI");
            endGameText.text = $"CONGRATULATIONS! \n" +
                               $"You have beaten the impossible in {(timer.GetComponent<Timer>().timer):F2} seconds.";
            print("a");
            leaderboard.StartSubmitScoreCoroutine((int) Math.Floor(timer.GetComponent<Timer>().timer)); // need this because this gameobject becomes inactive and will not run a coroutine
            boundaries.SetActive(false);
            print("b");

            mainMusic.gameObject.SetActive(false);
            endMusic.gameObject.SetActive(true);

            HasGameEnded = true;
        }
    }
}
