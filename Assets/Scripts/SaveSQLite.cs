using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;

public class SaveSQLite : MonoBehaviour
{
    public List<ScoreData> scoreDataList = new List<ScoreData>();
    public int currentID = 0;
    public string databasePath;
    public string filePath;
    private SqliteConnection connection;

    void Start()
    {
        LoadJson();
        InitializeDatabase();
    }

    void OnDestroy()
    {
        if (connection != null)
        {
            connection.Close();
        }
    }

    void InitializeDatabase()
    {
        /*        databasePath = Path.Combine(Application.persistentDataPath, "scoredatadb.db");
                connection = new SqliteConnection($"URI=file:{databasePath}");
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE IF NOT EXISTS ScoreData (ID INTEGER PRIMARY KEY, Name TEXT, Diamond INTEGER)";
                command.ExecuteNonQuery();*/
        string filename = "scoredatadb.db";
        string filepath = Path.Combine(Application.persistentDataPath, filename);
        databasePath = "URI=file:" + filepath;

        try
        {
            connection = new SqliteConnection(databasePath);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "CREATE TABLE IF NOT EXISTS ScoreData (ID INTEGER PRIMARY KEY, Name TEXT, Diamond INTEGER)";
            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError("Database initialization error: " + e.Message);
        }
    }

    public void SaveToJson_SQLite()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");
        int diamondCount = PlayerPrefs.GetInt("DiamondCount", 0) * 10;

        ScoreData data = new ScoreData();
        data.ID = currentID++;
        data.Name = playerName;
        data.Diamond = diamondCount;

        scoreDataList.Add(data);
        SaveJson();
        SaveToSQLite(data);
    }

    public void SaveJson()
    {
        string jsonData = JsonUtility.ToJson(new ScoreDataWrapper(scoreDataList.ToArray()), true);
        filePath = Path.Combine(Application.persistentDataPath, "ScoreData.json");
        File.WriteAllText(filePath, jsonData);
    }

    public void SaveToSQLite(ScoreData data)
    {
        var command = connection.CreateCommand();
        command.CommandText = $"INSERT INTO ScoreData (ID, Name, Diamond) VALUES ({data.ID}, '{data.Name}', {data.Diamond})";
        command.ExecuteNonQuery();
    }

    public void LoadJson()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "ScoreData.json");
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            ScoreDataWrapper wrapper = JsonUtility.FromJson<ScoreDataWrapper>(jsonData);

            if (wrapper != null && wrapper.ScoreData != null)
            {
                scoreDataList = new List<ScoreData>(wrapper.ScoreData);
                foreach (ScoreData scoreData in scoreDataList)
                {
                    if (scoreData.ID >= currentID)
                    {
                        currentID = scoreData.ID + 1;
                    }
                }
            }
            else
            {
                Debug.LogError("ScoreDataWrapper or ScoreData array is null in the JSON file.");
            }
        }
        else
        {
            Debug.LogError("ScoreData.json not found!");
        }
    }

    [Serializable]
    public class ScoreDataWrapper
    {
        public ScoreData[] ScoreData;

        public ScoreDataWrapper(ScoreData[] array)
        {
            ScoreData = array;
        }
    }
}

[System.Serializable]
public class ScoreData
{
    public int ID;
    public string Name;
    public int Diamond;
}

[System.Serializable]
public class ScoreDataList
{
    public List<ScoreData> ScoreData = new List<ScoreData>();
}