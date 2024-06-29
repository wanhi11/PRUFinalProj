using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class RoadToHeavenlessController : MonoBehaviour
{
    public GameObject loginPanel, signupPanel, profilePanel, menuPanel, notificationPanel;

    public TMP_InputField loginUsername, loginPassword, signupUsername, signupPassword, signupCPassword;

    public TMP_Text notif_Title_Text, notif_Message_Text, profileUsername_Text;

    private void Start()
    {
        switch (CurrentScreen.CurrentScreenPanel)
        {
            case 0:
                OpenLoginPanel();
                break;
            case 1:
                OpenSignUpPanel();
                break;
            case 2:
                OpenProfilePanel();
                break;
            case 3:
                OpenMenuPanel();
                break;
        }
    }

    public void ResetPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        menuPanel.SetActive(false);
    }

    public void OpenLoginPanel()
    {
        CurrentScreen.CurrentScreenPanel = 0;
        ResetPanel();
        loginPanel.SetActive(true);
    }

    public void OpenSignUpPanel()
    {
        CurrentScreen.CurrentScreenPanel = 1;
        ResetPanel();
        signupPanel.SetActive(true);
    }

    public void OpenProfilePanel()
    {
        CurrentScreen.CurrentScreenPanel = 2;
        ResetPanel();
        profilePanel.SetActive(true);
        profileUsername_Text.text = PlayerPrefs.GetString("Username", ""); // Display the username in the profile panel
    }

    public void OpenMenuPanel()
    {
        CurrentScreen.CurrentScreenPanel = 3;
        ResetPanel();
        menuPanel.SetActive(true);
    }

    public void LoginUser()
    {
        if (string.IsNullOrEmpty(loginUsername.text) || string.IsNullOrEmpty(loginPassword.text))
        {
            ShowNotificationMessage("Error", "Fields Empty! Please Input Details In All Fields");
            return;
        }

        StartCoroutine(LoginRoutine());
    }

    public void SignUpUser()
    {
        if (string.IsNullOrEmpty(signupUsername.text) || string.IsNullOrEmpty(signupPassword.text) || string.IsNullOrEmpty(signupCPassword.text))
        {
            ShowNotificationMessage("Error", "Fields Empty! Please Input Details In All Fields");
            return;
        }

        if (signupPassword.text != signupCPassword.text)
        {
            ShowNotificationMessage("Error", "Passwords do not match");
            return;
        }

        StartCoroutine(RegisterRoutine());
    }

    private IEnumerator LoginRoutine()
    {
        var url = "http://localhost:5103/api/User/Login";
        var requestBody = new AccountRelatedRequest { Username = loginUsername.text, Password = loginPassword.text };
        var json = JsonUtility.ToJson(requestBody);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            ShowNotificationMessage("Error", request.error);
        }
        else
        {
            var response = JsonUtility.FromJson<ApiResult<AccountRelatedResponse>>(request.downloadHandler.text);
            if (response.success)
            {
                // Lưu username vào PlayerPrefs
                PlayerPrefs.SetString("Username", loginUsername.text);
                PlayerPrefs.Save();

                ShowNotificationMessage("Success", "Login successful!");
                OpenProfilePanel();
            }
            else
            {
                ShowNotificationMessage("Error", "Invalid username or password");
            }
        }
    }

    private IEnumerator RegisterRoutine()
    {
        var url = "http://localhost:5103/api/User/Register";
        var requestBody = new AccountRelatedRequest { Username = signupUsername.text, Password = signupPassword.text };
        var json = JsonUtility.ToJson(requestBody);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            ShowNotificationMessage("Error", request.error);
        }
        else
        {
            var response = JsonUtility.FromJson<ApiResult<AccountRelatedResponse>>(request.downloadHandler.text);
            if (response.success)
            {
                ShowNotificationMessage("Success", "Sign up successful! Please log in.");
                OpenLoginPanel();
            }
            else
            {
                ShowNotificationMessage("Error", "This username has been used");
            }
        }
    }

    private void ShowNotificationMessage(string title, string message)
    {
        notif_Title_Text.text = title;
        notif_Message_Text.text = message;

        notificationPanel.SetActive(true);
    }

    public void CloseNotifPanel()
    {
        notif_Title_Text.text = "";
        notif_Message_Text.text = "";

        notificationPanel.SetActive(false);
    }

    public void LogOut()
    {
        profilePanel.SetActive(false);
        profileUsername_Text.text = "";
        PlayerPrefs.DeleteKey("Username");
        OpenLoginPanel();
    }

    public void Menu()
    {
        OpenMenuPanel();
    }
}

[Serializable]
public class AccountRelatedRequest
{
    public string Username;
    public string Password;
}

[Serializable]
public class AccountRelatedResponse
{
    public UserModel UserModel;
}

[Serializable]
public class UserModel
{
    public string Username;
    public string Password;
}