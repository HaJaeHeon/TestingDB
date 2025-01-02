using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;
using System;
using TMPro;

public class DeleteValue : MonoBehaviour
{
    private NpgsqlConnection connection;
    private int textUserNo; //유저 번호

    void Start()
    {
        try
        {
            //PgSQL 연결 설정
            string connectionString = $"Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=password;";
            connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }
        catch (NpgsqlException ex)  //Sql관련 예외 처리
        {
            Debug.LogError("PostgreSQL Exception: " + ex.Message);
        }
        catch (Exception ex)    //일반적인 예외 처리
        {
            Debug.LogError("Exception: " + ex.Message);
        }
    }

    public void DeleteUserByNo()
    {
        textUserNo = this.gameObject.GetComponent<UserNoComponent>().UserNo;    //유저 번호 가져오기
        try
        {
            string deleteQuery = "DELETE FROM Users WHERE userNo = @UserNo;";   //UserNo인 행 삭제
            NpgsqlCommand deleteCommand = new NpgsqlCommand(deleteQuery, connection);
            deleteCommand.Parameters.AddWithValue("@UserNo", textUserNo); //UserNo에 textUserNo를 대입함

            int rowsAffected = deleteCommand.ExecuteNonQuery(); //delete 쿼리 실행후 반환된 값은 삭제된 행의 개수
            Debug.Log("Rows Affected: " + rowsAffected);
        }
        catch (NpgsqlException ex)  //Sql관련 예외 처리
        {
            Debug.LogError("PostgreSQL Exception: " + ex.Message);
        }
        catch (Exception ex)    //일반적인 예외 처리
        {
            Debug.LogError("Exception: " + ex.Message);
        }

        Destroy(this.gameObject);
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
