using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//panel�ٲٱ�
public class PanelCtrl : MonoBehaviour
{
    public enum ActivePanel
    {
        CheckingPage = 0,   //Ȯ�� ������
        RegisterPage = 1    //��� ������
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
