using UnityEngine;
using Npgsql;
using System;
using TMPro;
using System.Collections;

public class SqlConnection : MonoBehaviour
{
    private NpgsqlConnection connection;

    public TMP_InputField User_name_txt;
    public TMP_InputField User_age_txt;

    public TMP_Text warning_bothText;
    public TMP_Text warning_String;
    public TMP_Text warning_Int;

    public TMP_Text verifyText;

    //public TMP_Text result_txt;

    public GameObject data_object;
    public Transform parentTransform;

    void Start()
    {
        try
        {
            // PostgreSQL 연결 설정
            string connectionString = $"Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=password;";
            connection = new NpgsqlConnection(connectionString);
            connection.Open();

            // 테이블 초기화
            InitializeTable();
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

            if (string.IsNullOrEmpty(User_name_txt.text) || string.IsNullOrEmpty(User_age_txt.text))
            {
                StartCoroutine("WarningInsertMessage");
                return;
            }

            else if (User_name_txt.text != null && string.IsNullOrEmpty(User_name_txt.text))


            {
                User_name_txt.text = null;
                StartCoroutine("WarningStringMessage");
                return;
            }
            else if (User_age_txt.text != null && !IsInteger(User_age_txt.text))
            {
                User_age_txt.text = null;
                StartCoroutine("WarningIntegerMessage");
                return;

            }

            char[] newName = User_name_txt.text.ToCharArray();
            int newAge = Int32.Parse(User_age_txt.text);

            //AddWithValue 메서드를 사용하여 파라미터를 추가
            insertCommand.Parameters.AddWithValue("@Name", newName);
            insertCommand.Parameters.AddWithValue("@Age", newAge);

            //ExecuteNonQuery 메서드를 호출하여 쿼리 실행
            int rowsAffected = insertCommand.ExecuteNonQuery();
            //Rows Affected : 삽입에 성공한 행의 수가 출력
            Debug.Log("Rows Affected: " + rowsAffected);

            StartCoroutine("SuccessInsert");
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
                int userno = reader.GetInt32(reader.GetOrdinal("userNo"));
                string name = reader.GetString(reader.GetOrdinal("Name"));
                int age = reader.GetInt32(reader.GetOrdinal("Age"));
           
                data_object.GetComponentInChildren<TMP_Text>().text =
                    ("UserNo : " + userno + " , Name : " + name + " , Age : " + age + "\n");
                
                GameObject loadData = Instantiate(data_object, parentTransform);

                loadData.GetComponent<UserNoComponent>().UserNo = userno;
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

    void InitializeTable()
    {
        try
        {
            // 테이블 삭제 쿼리
            string dropQuery = "DROP TABLE IF EXISTS Users;";
            NpgsqlCommand dropCommand = new NpgsqlCommand(dropQuery, connection);
            dropCommand.ExecuteNonQuery();

            // 테이블 생성 쿼리
            string createQuery = "CREATE TABLE Users(userNo SERIAL PRIMARY KEY, name VARCHAR(20), age INT);";
            NpgsqlCommand createCommand = new NpgsqlCommand(createQuery, connection);
            createCommand.ExecuteNonQuery();
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

    IEnumerator WarningInsertMessage()
    {
        warning_bothText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        warning_bothText.gameObject.SetActive(false);
    }

    IEnumerator WarningStringMessage()
    {
        warning_String.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        warning_String.gameObject.SetActive(false);
    }

    IEnumerator WarningIntegerMessage()
    {
        warning_Int.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        warning_Int.gameObject.SetActive(false);
    }

    IEnumerator SuccessInsert()
    {
        verifyText.color = Color.green;
        verifyText.text = "Success Insert Values";
        verifyText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        verifyText.gameObject.SetActive(false);
    }

    bool IsInteger(string text)
    {
        int result;
        return int.TryParse(text, out result);
    }
}