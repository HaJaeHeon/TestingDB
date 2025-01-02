using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;
using System;
using TMPro;

public class CheckingDB : MonoBehaviour
{
    private NpgsqlConnection connection;

    public TMP_InputField inputfieldUserName;   //입력된 이름
    public TMP_InputField inputfieldAge;        //입력된 나이

    public TMP_Text checkCommend;   //확인된 값 결과를 알려주는 text

    void Start()
    {
        try
        {
            //PgSql 연결 설정
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

    public void VerifyUserData()
    {
        string selectQuery = "SELECT COUNT(*) FROM Users WHERE name = @UserName AND age = @Age;";   //User테이블에서 name과 age가 일치하는 행의 갯수
        NpgsqlCommand selectCommand = new NpgsqlCommand(selectQuery, connection);
        selectCommand.Parameters.AddWithValue("@UserName", inputfieldUserName.text); //입력된 이름
        Debug.Log(inputfieldUserName.text);
        selectCommand.Parameters.AddWithValue("@Age", Int32.Parse(inputfieldAge.text)); //나이를 정수로 변환
        Debug.Log(Int32.Parse(inputfieldAge.text));

        int count = Convert.ToInt32(selectCommand.ExecuteScalar()); //쿼리 실행

        //결과를 조건문을 통해 입력한 name과 age가 일치하는지 확인
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
