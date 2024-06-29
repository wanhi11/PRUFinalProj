using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class FinishGame : MonoBehaviour
{
    public GameObject finishPanel; // Reference to the finish panel
    public TMP_Text scoreText; // Reference to the score text UI
    public TMP_Text timeText; // Reference to the time text UI
    public TMP_Text topTimeText; // Reference to display the top time from leaderboard
    private Timer timer; // Reference to the Timer script

    private string baseUrl = "http://localhost:5103/api/LeaderBoard"; // Base URL for LeaderBoard API

    void Start()
    {
        finishPanel.SetActive(false); // Initially hide the finish panel
        timer = FindObjectOfType<Timer>(); // Find the Timer script in the scene
    }

    public void FinishValue()
    {
        finishPanel.SetActive(true); // Show the finish panel
        scoreText.text = ItemManager.Instance.GetTotalItems().ToString(); // Display the total score
        double time = timer.GetCurrentTime();
        timeText.text = time.ToString("F2"); // Display the total time

        // Save new record
        string username = PlayerPrefs.GetString("Username", "player");
        StartCoroutine(SaveNewRecord(username, time));

        // Get top leaderboard
        StartCoroutine(GetTopLeaderboard(username));
    }

    private IEnumerator SaveNewRecord(string username, double time)
    {
        var url = $"{baseUrl}/SaveNewRecord";
        var requestBody = new AddNewRecordRequest { UserName = username, Time = time };
        var json = JsonUtility.ToJson(requestBody);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            var response = JsonUtility.FromJson<ApiResult<AddNewRecordResponse>>(request.downloadHandler.text);
            if (response.success)
            {
                Debug.Log("Record saved successfully");
            }
            else
            {
                Debug.LogError("Failed to save the record");
            }
        }
    }

    private IEnumerator GetTopLeaderboard(string username)
    {
        var url = $"{baseUrl}/GetTop/{username}";
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            var response = JsonUtility.FromJson<ApiResult<TopScoreResponse>>(request.downloadHandler.text);
            if (response.success)
            {
                var topScore = response.result.TopLeaderBoard.Time;
                topTimeText.text = topScore.ToString("F2"); // Display the top time
            }
            else
            {
                Debug.LogError("Failed to get the top leaderboard");
            }
        }
    }
}

[Serializable]
public class AddNewRecordRequest
{
    public string UserName;
    public double Time;
}

[Serializable]
public class AddNewRecordResponse
{
    public LeaderBoardModel TopScore;
}

[Serializable]
public class TopScoreResponse
{
    public LeaderBoardModel TopLeaderBoard;
}

[Serializable]
public class LeaderBoardModel
{
    public int? UserId;
    public double Time;
}

[Serializable]
public class ApiResult<T>
{
    public bool success;
    public T result;
}
