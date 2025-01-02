using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//panel바꾸기
public class PanelCtrl : MonoBehaviour
{
    public enum ActivePanel
    {
        CheckingPage = 0,   //확인 페이지
        RegisterPage = 1    //등록 페이지
    }

    public GameObject[] panelArray;

    public void CheckingPage()
    {
        ChangePanel(ActivePanel.CheckingPage);
    }

    public void RegisterPage()
    {
        ChangePanel(ActivePanel.RegisterPage);
    }

    private void ChangePanel(ActivePanel activePanel)
    {
        foreach (GameObject panel in panelArray)
        {
            panel.SetActive(false);
        }
        panelArray[(int)activePanel].SetActive(true);
    }
}
