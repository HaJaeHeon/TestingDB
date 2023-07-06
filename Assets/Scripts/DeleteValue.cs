using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;
using System;
using TMPro;

public class DeleteValue : MonoBehaviour
{
    private NpgsqlConnection connection;
    private int textUserno;

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

    public void DeleteUserByNo()
    {
        textUserno = this.gameObject.GetComponent<UserNoComponent>().UserNo;
        try
        {
            string deleteQuery = "DELETE FROM Users WHERE userNo = @UserNo;";
            NpgsqlCommand deleteCommand = new NpgsqlCommand(deleteQuery, connection);
            deleteCommand.Parameters.AddWithValue("@UserNo", textUserno);

            int rowsAffected = deleteCommand.ExecuteNonQuery();
            Debug.Log("Rows Affected: " + rowsAffected);
        }
        catch (NpgsqlException ex)
        {
            Debug.LogError("PostgreSQL Exception: " + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception: " + ex.Message);
        }
        Destroy(this.gameObject);
    }
}
