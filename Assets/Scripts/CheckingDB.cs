using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;
using System;
using TMPro;

public class CheckingDB : MonoBehaviour
{
    private NpgsqlConnection connection;

    public TMP_InputField inputfieldUserName;   //�Էµ� �̸�
    public TMP_InputField inputfieldAge;        //�Էµ� ����

    public TMP_Text checkCommend;   //Ȯ�ε� �� ����� �˷��ִ� text

    void Start()
    {
        try
        {
            //PgSql ���� ����
            string connectionString = $"Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=password;";
            connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }
        catch (NpgsqlException ex)  //Sql���� ���� ó��
        {
            Debug.LogError("PostgreSQL Exception: " + ex.Message);
        }
        catch (Exception ex)    //�Ϲ����� ���� ó��
        {
            Debug.LogError("Exception: " + ex.Message);
        }
    }

    public void VerifyUserData()
    {
        string selectQuery = "SELECT COUNT(*) FROM Users WHERE name = @UserName AND age = @Age;";   //User���̺��� name�� age�� ��ġ�ϴ� ���� ����
        NpgsqlCommand selectCommand = new NpgsqlCommand(selectQuery, connection);
        selectCommand.Parameters.AddWithValue("@UserName", inputfieldUserName.text); //�Էµ� �̸�
        Debug.Log(inputfieldUserName.text);
        selectCommand.Parameters.AddWithValue("@Age", Int32.Parse(inputfieldAge.text)); //���̸� ������ ��ȯ
        Debug.Log(Int32.Parse(inputfieldAge.text));

        int count = Convert.ToInt32(selectCommand.ExecuteScalar()); //���� ����

        //����� ���ǹ��� ���� �Է��� name�� age�� ��ġ�ϴ��� Ȯ��
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
        // ���� ����
        if (connection != null && connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
        }
    }
}
