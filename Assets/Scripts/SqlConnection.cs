using UnityEngine;
using Npgsql;
using System;
using TMPro;
using System.Collections;

public class SqlConnection : MonoBehaviour
{
    private NpgsqlConnection connection;

    public TMP_InputField userNameText;
    public TMP_InputField userAgeText;

    public TMP_Text warningNotStringAndIntText;
    public TMP_Text warningNotString;
    public TMP_Text warningNotInt;

    public TMP_Text verifyText;

    //public TMP_Text result_txt;

    public GameObject dataObject;
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
        catch (NpgsqlException ex)  //Sql관련 예외 처리
        {
            Debug.LogError("PostgreSQL Exception: " + ex.Message);
        }
        catch (Exception ex)    //일반적인 예외 처리
        {
            Debug.LogError("Exception: " + ex.Message);
        }
    }

    //데이터 삽입
    public void InsertValueButton()
    {
        try
        {
            string insertQuery = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age);";
            NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection);

            if (string.IsNullOrEmpty(userNameText.text) || string.IsNullOrEmpty(userAgeText.text))  //InputField의 name 이나 age 가 비어있으면
            {
                StartCoroutine("WarningInsertMessage");
                return;
            }

            else if (userNameText.text != null && string.IsNullOrEmpty(userNameText.text))  //InputField의 name은 채워졌지만 string으로 변환시 값이 없는경우?
            {
                userNameText.text = null;
                StartCoroutine("WarningStringMessage");
                return;
            }
            else if (userAgeText.text != null && !IsInteger(userAgeText.text))  //InputField의 age는 채워졌지만 age가 정수형이 아닌 경우
            {
                userAgeText.text = null;
                StartCoroutine("WarningIntegerMessage");
                return;

            }

            char[] newName = userNameText.text.ToCharArray();
            int newAge = Int32.Parse(userAgeText.text);

            //파라미터 추가
            insertCommand.Parameters.AddWithValue("@Name", newName);
            insertCommand.Parameters.AddWithValue("@Age", newAge);
            
            int rowsAffected = insertCommand.ExecuteNonQuery(); //쿼리 실행
            Debug.Log("Rows Affected: " + rowsAffected);    //추가된 행의 갯수 찍기

            StartCoroutine("SuccessInsert");
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

    //테이블 조회
    public void ShowAll()
    {
        try
        {
            string query = "SELECT * FROM Users;";
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int userno = reader.GetInt32(reader.GetOrdinal("userNo"));
                string name = reader.GetString(reader.GetOrdinal("Name"));
                int age = reader.GetInt32(reader.GetOrdinal("Age"));
           
                dataObject.GetComponentInChildren<TMP_Text>().text =
                    ("UserNo : " + userno + " , Name : " + name + " , Age : " + age + "\n");
                
                GameObject loadData = Instantiate(dataObject, parentTransform);

                loadData.GetComponent<UserNoComponent>().UserNo = userno;
            }
            reader.Close();
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
        catch (NpgsqlException ex)  //Sql관련 예외 처리
        {
            Debug.LogError("PostgreSQL Exception: " + ex.Message);
        }
        catch (Exception ex)    //일반적인 예외 처리
        {
            Debug.LogError("Exception: " + ex.Message);
        }
    }

    //성공인지 실패인지 Text로 표현

    IEnumerator WarningInsertMessage()
    {
        warningNotStringAndIntText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningNotStringAndIntText.gameObject.SetActive(false);
    }

    IEnumerator WarningStringMessage()
    {
        warningNotString.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningNotString.gameObject.SetActive(false);
    }

    IEnumerator WarningIntegerMessage()
    {
        warningNotInt.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningNotInt.gameObject.SetActive(false);
    }

    IEnumerator SuccessInsert()
    {
        verifyText.color = Color.green;
        verifyText.text = "Success Insert Values";
        verifyText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        verifyText.gameObject.SetActive(false);
    }

    //parsing한 값이 정수인지 bool로 표현
    bool IsInteger(string text)
    {
        int result;
        return int.TryParse(text, out result);
    }
}