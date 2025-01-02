using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VIewResult : MonoBehaviour
{
    public GameObject resultPanel;  //���â
    public Transform parentObject;

    //���â Clear
    private void Start()
    {
        foreach (Transform child in parentObject)
        {
            Destroy(child.gameObject);
            Debug.Log(child.name);
        }
    }

    //���â On Off
    public void OnOffResult()
    {
        if (resultPanel.activeSelf == true)
            resultPanel.SetActive(false);
        else
            resultPanel.SetActive(true);
    }

    //���â ���� �� Clear
    public void CloseResult()
    {
        resultPanel.SetActive(false);

        foreach (Transform child in parentObject)
        {
            Destroy(child.gameObject);
        }
    }
}
