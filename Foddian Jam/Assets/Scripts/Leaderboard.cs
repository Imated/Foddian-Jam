using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class Leaderboard : MonoBehaviour
{
    int leaderboardID = 16665;

    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlaeyerID");
        
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID.ToString(), (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully uploaded score");
                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done = false);
    }
}
