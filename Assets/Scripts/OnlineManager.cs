using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using System.IO;
using UnityEngine.Networking;
using System.Text;

public class OnlineManager : MonoBehaviour {
    public static class Word_Server {
        public static string Words { get; set; }
    }

    public class UserData {
        public static string EmailID { get; set; }
        public static string Password { get; set; }
        public static bool Success { get; set; }
        public static string GamerID { get; set; }
        public static string Name { get; set; }
    }

    //185.202.172.73
    public static readonly string URL = "https://cb-wordserver.herokuapp.com";
    public static readonly string UsersURL = URL + "/users";
    public static readonly string AuthenticateURL = URL + "/authenticate";
    public static readonly string SignUpURL = URL + "/add";
    public static readonly string GetDetailsURL = URL + "/gamer";
    public static readonly string UpdateURL = URL + "/update";
    public static readonly string LeaderBoardsURL = URL + "/leaderboard";
    public static readonly string LeaderBoardsUploadURL = URL + "/uploadscore";

    int count = 0;

    private UIManager uiManager;

    private void Awake() {
        uiManager = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIManager>();
        DontDestroyOnLoad(this);
        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }
        // StartCoroutine(wakeUp());
    }

    private IEnumerator wakeUp() {
        Debug.Log("Attempting WakeUp");

        WWWForm form = new WWWForm();
        form.AddField("dummy", "dummy");

        using (UnityWebRequest www = UnityWebRequest.Post(URL + "/dummy/", form)) {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, string> dict in www.GetResponseHeaders()) {
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }

                if (www.GetResponseHeader("code") != null) {
                    if (count > 10) {
                        ErrorPanel.ShowError("Fatal Error. Server Not Responding");
                        count = 0;
                    } else {
                        count++;
                        Debug.Log("Continuing");
                        StartCoroutine(wakeUp());
                    }
                } else {
                    Debug.Log("Success Awake!");
                    count = 0;
                }
            }
        }
    }

    public IEnumerator Login() {
        Debug.Log("Attempting Login");
        WWWForm form = new WWWForm();
        form.AddField("email", UserData.EmailID);
        form.AddField("password", UserData.Password);

        using (UnityWebRequest www = UnityWebRequest.Post(AuthenticateURL, form)) {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError || www.error != null) {
                Debug.Log(www.error);
                ErrorPanel.ShowError("ERROR: " + www.error);
            } else {
                var response = JSON.Parse(www.downloadHandler.text);
                Debug.Log("Sign In Successfull");
                UserData.GamerID = response["gamerID"];
                GameController.MyScore.CurrentScore = 0;
                GameController.MyScore.HighScore = 0;
                StartCoroutine(fetchGameData());
                Debug.Log(response["message"]);
            }
        }
        ButtonHelper.ResetClick();
    }

    public IEnumerator Register() {
        Debug.Log("Attempting to Register");
        WWWForm form = new WWWForm();
        form.AddField("email", UserData.EmailID);
        form.AddField("password", UserData.Password);
        form.AddField("gamerID", UserData.GamerID);
        form.AddField("name", UserData.Name);
        using (UnityWebRequest www = UnityWebRequest.Post(SignUpURL, form)) {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError || www.error != null) {
                Debug.Log(www.error);
                ErrorPanel.ShowError("ERROR: " + www.error);
            } else {
                var response = JSON.Parse(www.downloadHandler.text);
                if (response["code"] == 201) {
                    Debug.Log("Register Successfull");
                    UserData.GamerID = response["gamerID"];
                    UserData.EmailID = UserData.EmailID;
                    UserData.Password = UserData.Password;
                    GameController.MyScore.CurrentScore = 0;
                    GameController.MyScore.HighScore = 0;
                    StartCoroutine(fetchGameData());
                } else {
                    ErrorPanel.ShowError(response["message"]);
                }
            }
        }
        ButtonHelper.ResetClick();
    }

    public IEnumerator fetchGameData() {
        Debug.Log("Attempting to Fetch Game Data");
        WWWForm form = new WWWForm();
        form.AddField("gamerID", UserData.GamerID ?? "__no__");
        form.AddField("gamePlatform", GameController.GameData.gamePlatform + "_" + GameController.GameData.gameBuildType + "_" + GameController.GameData.gameVersion);
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError || www.error != null) {
                Debug.Log(www.error);
                ErrorPanel.ShowError("ERROR: " + www.error);
            } else {
                var response = JSON.Parse(www.downloadHandler.text);
                if (response["code"].AsInt == 200) {
                    Debug.Log("Fetch Successfull");
                    Word_Server.Words = response["words"];
                    WordManager.Initialize();
                    GameController.GameData.enabled = response["enabled"];
                    uiManager.SwitchPage(UIManager.Page.MainMenu);
                } else if (response["code"].AsInt == 100) {
                    ErrorPanel.ShowError("Error: " + response["message"]);
                    //StartCoroutine(downloadFile());
                } else {
                    ErrorPanel.ShowError("Fetch Unsuccessfull. Reason: " + response["message"]);
                }
            }
        }
        ButtonHelper.ResetClick();
    }

    IEnumerator DownloadFile(string URL) {
        Debug.Log("Downloading....");
        var www = new WWW("URL");
        yield return www;
        while (!www.isDone) { }
        if (www.error == null) {
            File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + "Words", www.bytes);
            Debug.Log("Download Complete" + www.bytes.ToString());
            ErrorPanel.ShowError("Download Complete.\nLaunch Latest Version From Desktop.");
            Application.Quit();
        } else {
            Debug.Log("Download Failed.");
            ErrorPanel.ShowError("Download Failed: " + www.error.ToString());
        }
    }

    public IEnumerator uploadScoreData() {
        Debug.Log("Attempting to Upload Score Data");
        WWWForm form = new WWWForm();
        form.AddField("score", GameController.MyScore.CurrentScore);
        form.AddField("gamerID", UserData.GamerID);
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError || www.error != null) {
                Debug.Log(www.error);
                ErrorPanel.ShowError("ERROR: " + www.error);
            } else {
                var response = JSON.Parse(www.downloadHandler.text);
                if (response["code"].AsInt == 200) {
                    Debug.Log("Upload Successfull");
                } else {
                    ErrorPanel.ShowError("Upload Unsuccessfull. Reason: " + response["message"]);
                }
            }
            ButtonHelper.ResetClick();
        }
    }

    public IEnumerator downloadScoreData() {
        Debug.Log("Attempting to download LeaderBoard Data");
        WWWForm form = new WWWForm();
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError || www.error != null) {
                Debug.Log(www.error);
                ErrorPanel.ShowError("ERROR: " + www.error);
            } else {
                var response = JSON.Parse(www.downloadHandler.text);
                if (response["code"].AsInt == 200) {
                    Debug.Log("Download Successfull");
                    var data = response["leaderBoardData"].AsArray;
                    List<string> gamerIDs = new List<string>();
                    List<string> scores = new List<string>();
                    foreach (JSONObject obj in data) {
                        var key = obj.GetKeys()[0] as string;
                        gamerIDs.Add(key);
                        scores.Add(obj[key]);
                    }
                    uiManager.showLeaderboardData(gamerIDs, scores);
                    uiManager.SwitchPage(UIManager.Page.HighScore);

                } else {
                    ErrorPanel.ShowError("Fetch Unsuccessfull. Reason: " + response["description"]);
                }
            }
        }
        ButtonHelper.ResetClick();
    }

}
