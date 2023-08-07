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

        if (col.relativeVelocity.magnitude >= boundaryBreakSpeed)
        {
            timer.SetActive(false);
            endGameUI.SetActive(true);
            endGameUIAnimator.Play("End Game UI");
            endGameText.text = $"CONGRATULATIONS! \n" +
                               $"You have beaten the impossible in {(timer.GetComponent<Timer>().timer):F2} seconds.";
            leaderboard.StartSubmitScoreCoroutine((int) (timer.GetComponent<Timer>().timer * 100)); // need this because this gameobject becomes inactive and will not run a coroutine
            boundaries.SetActive(false);

            mainMusic.gameObject.SetActive(false);
            endMusic.gameObject.SetActive(true);
            
            HasGameEnded = true;
        }
    }
}
