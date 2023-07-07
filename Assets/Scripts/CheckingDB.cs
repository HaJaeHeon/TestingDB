using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;
using System;
using TMPro;

public class CheckingDB : MonoBehaviour
{
    private NpgsqlConnection connection;

    public TMP_InputField inputfieldUserName;
    public TMP_InputField inputfieldAge;

    public TMP_Text checkCommend;

    void Start()
    {
        try
        {
            // PostgreSQL 연결 설정
            string connectionString = $"Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=password;";
            connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }
        catch (NpgsqlException ex)
        {
            Debug.LogError("PostgreSQL Exception: " + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception: " + ex.Message);
        }
    }

    public void VerifyUserData()
    {
        string selectQuery = "SELECT COUNT(*) FROM Users WHERE name = @UserName AND age = @Age;";
        NpgsqlCommand selectCommand = new NpgsqlCommand(selectQuery, connection);
        selectCommand.Parameters.AddWithValue("@UserName", inputfieldUserName.text);
        Debug.Log(inputfieldUserName.text);
        Debug.Log(Int32.Parse(inputfieldAge.text));
        selectCommand.Parameters.AddWithValue("@Age", Int32.Parse(inputfieldAge.text));

        int count = Convert.ToInt32(selectCommand.ExecuteScalar());

        if (count > 0)
        {
            checkCommend.text = "User data matches the database.";
            checkCommend.color = Color.green;
            Debug.Log("User data matches the database.");
        }
        else
        {
            checkCommend.text = "User data does not match the database.";
            checkCommend.color = Color.red;
            Debug.Log("User data does not match the database.");
        }
    }


    void OnDestroy()
    {
        // 연결 종료
        if (connection != null && connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
        }
    }
}
