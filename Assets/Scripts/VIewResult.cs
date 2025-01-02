using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VIewResult : MonoBehaviour
{
    public GameObject resultPanel;  //결과창
    public Transform parentObject;

    //결과창 Clear
    private void Start()
    {
        foreach (Transform child in parentObject)
        {
            Destroy(child.gameObject);
            Debug.Log(child.name);
        }
    }

    //결과창 On Off
    public void OnOffResult()
    {
        if (resultPanel.activeSelf == true)
            resultPanel.SetActive(false);
        else
            resultPanel.SetActive(true);
    }

    //결과창 닫을 시 Clear
    public void CloseResult()
    {
        resultPanel.SetActive(false);

        foreach (Transform child in parentObject)
        {
            Destroy(child.gameObject);
        }
    }
}
