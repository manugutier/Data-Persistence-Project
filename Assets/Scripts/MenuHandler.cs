using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

public class MenuHandler : MonoBehaviour
{
    public static MenuHandler instance;
    public GameObject[] startMenuButtons;
    public InputField nameInput;
    public Text highScoreText;
    public string highScoreString;
    public int highScore;
    public int score;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();
        highScoreText.text = highScoreString;
    }

    private void Update()
    {
        if (score > highScore)
        {
            highScore = score;
            highScoreString = $"Best score : {nameInput.text} : {highScore}";
            highScoreText.text = highScoreString;
        }
    }
    public void LoadLevel()
    {
        startMenuButtons[0].SetActive(false);
        startMenuButtons[1].SetActive(false);
        startMenuButtons[2].SetActive(false);

        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        SaveHighScore();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [System.Serializable]
    public class SaveData
    {
        public string highScoreString;
        public int highScore;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.highScoreString = highScoreString;
        data.highScore = highScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScoreString = data.highScoreString;
            highScore = data.highScore;
        }
    }
}
