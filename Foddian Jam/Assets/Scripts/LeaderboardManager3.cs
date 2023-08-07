using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class LeaderboardManager3 : MonoBehaviour
{
    int leaderboardID = 16693;

    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");

        LootLockerSDKManager.GetMemberRank(16693.ToString(), playerID, (response) =>
        {
            if (response.statusCode == 200) 
            {
                Debug.Log("Successful");
                if (scoreToUpload < response.score || response.score == 0)
                {
                    print($"Score: {scoreToUpload}, PlayerID: {playerID}");
                    LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID.ToString(), (response) =>
                    {
                        if (response.success)
                        {
                            print($"Score: {response.score}, PlayerID: {response.member_id}");
                            Debug.Log("Successfully uploaded score");
                            done = true;
                        }
                        else
                        {
                            Debug.Log("Failed" + response.Error);
                            done = true;
                        }
                    });
                }
            } 
            else 
            {
                Debug.Log("failed: " + response.Error);
            }
        });
        yield return new WaitWhile(() => done = false);
    }

    public void StartSubmitScoreCoroutine(int score)
    {
        StartCoroutine(SubmitScoreRoutine(score));
    }
}
