using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VIewResult : MonoBehaviour
{
    public GameObject resultPanel;
    public Transform parentObject;

    private void Start()
    {
        foreach (Transform child in parentObject)
        {
            Destroy(child.gameObject);
            Debug.Log(child.name);
        }
    }

    public void OnOffResult()
    {
        if (resultPanel.activeSelf == true)
            resultPanel.SetActive(false);
        else
            resultPanel.SetActive(true);
    }

    public void CloseResult()
    {
        resultPanel.SetActive(false);

        foreach (Transform child in parentObject)
        {
            Destroy(child.gameObject);
        }
    }
}
