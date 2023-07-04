using UnityEngine;
using Npgsql;
using System;
using TMPro;

public class SqlConnection : MonoBehaviour
{
    private NpgsqlConnection connection;

    public TMP_InputField User_name_txt;
    public TMP_InputField User_age_txt;

    //public TMP_Text result_txt;

    public GameObject data_object;
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

            char[] newName = User_name_txt.text.ToCharArray();
            int newAge = Int32.Parse(User_age_txt.text);

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
                int userno = reader.GetInt32(reader.GetOrdinal("userNo"));
                string name = reader.GetString(reader.GetOrdinal("Name"));
                int age = reader.GetInt32(reader.GetOrdinal("Age"));

                //Debug.Log("UserNo : " + userno + " , Name : " + name + " , Age : " + age);                
                data_object.GetComponentInChildren<TMP_Text>().text =
                    ("UserNo : " + userno + " , Name : " + name + " , Age : " + age + "\n");
                GameObject loadData = Instantiate(data_object, parentTransform);
                


                /*RectTransform rectTransform = data_object.GetComponent<RectTransform>();
                
                if (rectTransform != null)
                {
                    rectTransform.anchoredPosition = Vector2.zero;
                    rectTransform.localScale = Vector3.one;
                }
                else
                {
                    Debug.LogWarning("Loaded prefab does not have RectTransform component!");
                }
                */
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

    public void DeleteNo()
    {
        try
        {
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
        catch (NpgsqlException ex)
        {
            Debug.LogError("PostgreSQL Exception: " + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception: " + ex.Message);
        }
    }
}