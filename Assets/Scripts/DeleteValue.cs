using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;
using System;
using TMPro;

public class DeleteValue : MonoBehaviour
{
    private NpgsqlConnection connection;
    private int textUserNo; //���� ��ȣ

    void Start()
    {
        try
        {
            //PgSQL ���� ����
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

    public void DeleteUserByNo()
    {
        textUserNo = this.gameObject.GetComponent<UserNoComponent>().UserNo;    //���� ��ȣ ��������
        try
        {
            string deleteQuery = "DELETE FROM Users WHERE userNo = @UserNo;";   //UserNo�� �� ����
            NpgsqlCommand deleteCommand = new NpgsqlCommand(deleteQuery, connection);
            deleteCommand.Parameters.AddWithValue("@UserNo", textUserNo); //UserNo�� textUserNo�� ������

            int rowsAffected = deleteCommand.ExecuteNonQuery(); //delete ���� ������ ��ȯ�� ���� ������ ���� ����
            Debug.Log("Rows Affected: " + rowsAffected);
        }
        catch (NpgsqlException ex)  //Sql���� ���� ó��
        {
            Debug.LogError("PostgreSQL Exception: " + ex.Message);
        }
        catch (Exception ex)    //�Ϲ����� ���� ó��
        {
            Debug.LogError("Exception: " + ex.Message);
        }

        Destroy(this.gameObject);
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
