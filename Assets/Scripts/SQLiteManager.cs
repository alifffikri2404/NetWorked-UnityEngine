/*using UnityEngine;
using System.IO;
using Mono.Data.Sqlite;
using System.Collections.Generic;

public class SQLiteManager : MonoBehaviour
{
    public const string JsonFileName = "ScoreData.json";
    public const string DatabaseFileName = "scoredatadb.db";
    public string jsonFilePath;
    public string databasePath;
    bool isInitialized = false;

    public void SaveDataButtonClicked()
    {
        if (!isInitialized)
        {
            InitializeSQLite();
            isInitialized = true;
        }
    }

    public void InitializeSQLite()
    {
        jsonFilePath = Path.Combine(Application.persistentDataPath, JsonFileName);
        databasePath = Path.Combine(Application.persistentDataPath, DatabaseFileName);

        if (File.Exists(jsonFilePath))
        {
            WriteJsonDataToSQLite();
        }
        else
        {
            Debug.LogError("ScoreData.json not found!");
        }
    }

    public void WriteJsonDataToSQLite()
    {
        *//*        ScoreData[] scoreDataArray = LoadScoreDataFromJson();

                using (var connection = new SqliteConnection($"URI=file:{databasePath}"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    // Create the ScoreData table
                    command.CommandText = "CREATE TABLE IF NOT EXISTS ScoreData (Name TEXT, Diamond INTEGER)";
                    command.ExecuteNonQuery();

                    // Insert JSON data into SQLite
                    foreach (ScoreData data in scoreDataArray)
                    {
                        command.CommandText = $"INSERT INTO ScoreData (Name, Diamond) VALUES ('{data.Name}', {data.Diamond})";
                        command.ExecuteNonQuery();
                    }
                }*//*
        ScoreData[] scoreDataArray = LoadScoreDataFromJson();

        using (var connection = new SqliteConnection($"URI=file:{databasePath}"))
        {
            connection.Open();
            var command = connection.CreateCommand();

            // Create the ScoreData table
            command.CommandText = "CREATE TABLE IF NOT EXISTS ScoreData (ID INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Diamond INTEGER)";
            command.ExecuteNonQuery();

            foreach (ScoreData data in scoreDataArray)
            {
                // Check if the ID exists in the database
                command.CommandText = $"SELECT ID FROM ScoreData WHERE ID = {data.ID}";
                Debug.Log("Select ID from ScoreData where ID = ID");
                var reader = command.ExecuteReader();

                if (reader.Read()) // If the ID exists, update the data
                {
                    reader.Close();
                    command.CommandText = $"UPDATE ScoreData SET Name = '{data.Name}', Diamond = {data.Diamond} WHERE ID = {data.ID}";
                    Debug.Log("Update ScoreData");
                    command.ExecuteNonQuery();
                }
                else // If the ID doesn't exist, insert a new record
                {
                    reader.Close();
                    command.CommandText = $"INSERT INTO ScoreData (Name, Diamond) VALUES ('{data.Name}', {data.Diamond})";
                    Debug.Log("Insert ScoreData");
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public ScoreData[] LoadScoreDataFromJson()
    {
        string json = File.ReadAllText(jsonFilePath);
        ScoreDataList dataList = JsonUtility.FromJson<ScoreDataList>(json);
        return dataList.ScoreData.ToArray();
    }
}

*//*[System.Serializable]
public class ScoreDataList
{
    public List<ScoreData> ScoreData = new List<ScoreData>();
}*/
