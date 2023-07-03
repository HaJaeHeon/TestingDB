using UnityEngine;
using Npgsql;
using System;

public class SqlConnection : MonoBehaviour
{
    private NpgsqlConnection connection;

    [SerializeField] private char User_name;
    [SerializeField] private int User_age;

    void Start()
    {
        try
        {
            // PostgreSQL ���� ����
            string connectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=password;";
            connection = new NpgsqlConnection(connectionString);
            connection.Open();

            // ���̺� �ʱ�ȭ
            InitializeTable();

            // ������ ����
            string deleteQuery = "DELETE FROM Users;";
            NpgsqlCommand deleteCommand = new NpgsqlCommand(deleteQuery, connection);
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
    }


    public void InsertValueButton()
    {
        try
        {
            // ������ ���� ����
            string insertQuery = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age);";
            NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection);

            string newName = "John";
            int newAge = 30;

            //AddWithValue �޼��带 ����Ͽ� �Ķ���͸� �߰�
            insertCommand.Parameters.AddWithValue("@Name", newName);
            insertCommand.Parameters.AddWithValue("@Age", newAge);

            //ExecuteNonQuery �޼��带 ȣ���Ͽ� ���� ����
            int rowsAffected = insertCommand.ExecuteNonQuery();
            //Rows Affected : ���Կ� ������ ���� ���� ���
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
    }

    public void ShowAll()
    {
        try
        {
            // ���̺� ��ü ��ȸ
            string query = "SELECT * FROM Users;";
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            NpgsqlDataReader reader = command.ExecuteReader();

            // ��� �б�
            while (reader.Read())
            {
                // ������ ó��
                string name = reader.GetString(reader.GetOrdinal("Name"));
                int age = reader.GetInt32(reader.GetOrdinal("Age"));

                Debug.Log("Name : " + name + " , Age : " + age);
            }

            reader.Close();
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

    void OnDestroy()
    {
        // ���� ����
        if (connection != null && connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
        }
    }

    private void InitializeTable()
    {
        string createTableQuery = "CREATE TABLE IF NOT EXISTS Users (Name TEXT, Age INT);";
        NpgsqlCommand createTableCommand = new NpgsqlCommand(createTableQuery, connection);
        createTableCommand.ExecuteNonQuery();
    }
}