using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class LootLockerMenuManager : MonoBehaviour
{
    public Leaderboard leaderboard;


    void Start()
    {
        StartCoroutine(setupRoutine());
    }

    IEnumerator setupRoutine()
    {
        yield return loginRoutine();
        yield return leaderboard.fetchHighscoresRoutine();
    }

    IEnumerator loginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player was logged in successfully");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}
