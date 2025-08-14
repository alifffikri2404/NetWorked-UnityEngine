using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;
using System;
using Mono.Data.Sqlite;

public class Scoreboard : MonoBehaviour
{
    public Transform scoreParent;
    public GameObject scorePrefab;
    private const int MaxScoreEntries = 5;

    private List<Score> scores = new List<Score>();

    public void Start()
    {
        LoadScores();
        scores = scores.OrderByDescending(x => x.Diamond).Take(MaxScoreEntries).ToList();
        DisplayScores();
    }

    public void LoadScores()
    {
        // Load from different JSON file (ScoreData_XXXXX.json)
        /*        string directoryPath = Application.persistentDataPath;

                if (!Directory.Exists(directoryPath))
                {
                    Debug.LogWarning("Directory not found: " + directoryPath);
                    return;
                }

                string[] scoreFiles = Directory.GetFiles(directoryPath, "*.json");

                foreach (string filePath in scoreFiles)
                {
                    string json = File.ReadAllText(filePath);
                    ScoreData data = JsonUtility.FromJson<ScoreData>(json);
                    scores.Add(new Score(data.Name, data.Diamond));
                }*/

        // Load from a single JSON file (ScoreData.json)
        /*        string filePath = Path.Combine(Application.persistentDataPath, "ScoreData.json");

                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    ScoreDataWrapper scoreDataWrapper = JsonUtility.FromJson<ScoreDataWrapper>(jsonData);
                    scores = scoreDataWrapper.ScoreData
                        .Select(data => new Score(data.Name, data.Diamond))
                        .ToList();
                }
                else
                {
                    Debug.LogWarning("ScoreData.json not found");
                }*/
        // Load from scoredatadb.db database
        string databasePath = Path.Combine(Application.persistentDataPath, "scoredatadb.db");

        if (File.Exists(databasePath))
        {
            using (var connection = new SqliteConnection($"URI=file:{databasePath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = "SELECT Name, Diamond FROM ScoreData ORDER BY Diamond DESC";
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    int diamond = reader.GetInt32(1);
                    scores.Add(new Score(name, diamond));
                }

                reader.Close();
            }
        }
        else
        {
            Debug.LogWarning("Database file not found: scoredatadb.db");
        }
    }

    public void DisplayScores()
    {
        float templateHeight = 40f;

        for (int i = 0; i < scores.Count; i++)
        {
            GameObject scoreObject = Instantiate(scorePrefab, scoreParent);
            RectTransform entryRectTransform = scoreObject.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);

            TextMeshProUGUI rankText = scoreObject.transform.Find("Rank").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI nameText = scoreObject.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI diamondText = scoreObject.transform.Find("Score").GetComponent<TextMeshProUGUI>();

            rankText.text = (i + 1).ToString();
            nameText.text = scores[i].Name;
            diamondText.text = scores[i].Diamond.ToString();
        }
    }
}

[System.Serializable]
public class Score
{
    public string Name;
    public int Diamond;

    public Score(string name, int diamond)
    {
        Name = name;
        Diamond = diamond;
    }
}

[System.Serializable]
public class ScoreDataWrapper
{
    public ScoreData[] ScoreData;
}