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
            // PostgreSQL ���� ����
            string connectionString = $"Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=password;";
            connection = new NpgsqlConnection(connectionString);
            connection.Open();

            // ���̺� �ʱ�ȭ
            InitializeTable();
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

    //������ ����
    public void InsertValueButton()
    {
        try
        {
            string insertQuery = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age);";
            NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection);

            if (string.IsNullOrEmpty(userNameText.text) || string.IsNullOrEmpty(userAgeText.text))  //InputField�� name �̳� age �� ���������
            {
                StartCoroutine("WarningInsertMessage");
                return;
            }

            else if (userNameText.text != null && string.IsNullOrEmpty(userNameText.text))  //InputField�� name�� ä�������� string���� ��ȯ�� ���� ���°��?
            {
                userNameText.text = null;
                StartCoroutine("WarningStringMessage");
                return;
            }
            else if (userAgeText.text != null && !IsInteger(userAgeText.text))  //InputField�� age�� ä�������� age�� �������� �ƴ� ���
            {
                userAgeText.text = null;
                StartCoroutine("WarningIntegerMessage");
                return;

            }

            char[] newName = userNameText.text.ToCharArray();
            int newAge = Int32.Parse(userAgeText.text);

            //�Ķ���� �߰�
            insertCommand.Parameters.AddWithValue("@Name", newName);
            insertCommand.Parameters.AddWithValue("@Age", newAge);
            
            int rowsAffected = insertCommand.ExecuteNonQuery(); //���� ����
            Debug.Log("Rows Affected: " + rowsAffected);    //�߰��� ���� ���� ���

            StartCoroutine("SuccessInsert");
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

    //���̺� ��ȸ
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
        catch (NpgsqlException ex)  //Sql���� ���� ó��
        {
            Debug.LogError("PostgreSQL Exception: " + ex.Message);
        }
        catch (Exception ex)    //�Ϲ����� ���� ó��
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

    void InitializeTable()
    {
        try
        {
            // ���̺� ���� ����
            string dropQuery = "DROP TABLE IF EXISTS Users;";
            NpgsqlCommand dropCommand = new NpgsqlCommand(dropQuery, connection);
            dropCommand.ExecuteNonQuery();

            // ���̺� ���� ����
            string createQuery = "CREATE TABLE Users(userNo SERIAL PRIMARY KEY, name VARCHAR(20), age INT);";
            NpgsqlCommand createCommand = new NpgsqlCommand(createQuery, connection);
            createCommand.ExecuteNonQuery();
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

    //�������� �������� Text�� ǥ��

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

    //parsing�� ���� �������� bool�� ǥ��
    bool IsInteger(string text)
    {
        int result;
        return int.TryParse(text, out result);
    }
}