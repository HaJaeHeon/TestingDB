using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCtrl : MonoBehaviour
{
    public enum ActivePanel
    {
        CheckingPage = 0,
        RegisterPage = 1
    }

    public GameObject[] panels;

    public void CheckingPage()
    {
        ChangePanel(ActivePanel.CheckingPage);
    }

    public void RegisterPage()
    {
        ChangePanel(ActivePanel.RegisterPage);
    }

    private void ChangePanel(ActivePanel panel)
    {
        foreach (GameObject _panels in panels)
        {
            _panels.SetActive(false);
        }
        panels[(int)panel].SetActive(true);
    }
}
