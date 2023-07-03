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
            // PostgreSQL 연결 설정
            string connectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=password;";
            connection = new NpgsqlConnection(connectionString);
            connection.Open();

            // 테이블 초기화
            InitializeTable();

            // 데이터 삭제
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
            // 데이터 삽입 예시
            string insertQuery = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age);";
            NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection);

            string newName = "John";
            int newAge = 30;

            //AddWithValue 메서드를 사용하여 파라미터를 추가
            insertCommand.Parameters.AddWithValue("@Name", newName);
            insertCommand.Parameters.AddWithValue("@Age", newAge);

            //ExecuteNonQuery 메서드를 호출하여 쿼리 실행
            int rowsAffected = insertCommand.ExecuteNonQuery();
            //Rows Affected : 삽입에 성공한 행의 수가 출력
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
            // 테이블 전체 조회
            string query = "SELECT * FROM Users;";
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            NpgsqlDataReader reader = command.ExecuteReader();

            // 결과 읽기
            while (reader.Read())
            {
                // 데이터 처리
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
        // 연결 종료
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