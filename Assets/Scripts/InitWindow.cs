using UnityEngine;
using UnityEngine.UI;

public class InitWindow : MonoBehaviour
{
    public bool isFullScreen = false;
    public Toggle fullScreenToggle;

    void Start()
    {
        // �ʱ� â ��� ����
        if (isFullScreen)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
    }

    void Update()
    {
        // Ű �Է��� ���� â ��� ����
        if (fullScreenToggle.isOn)
        {
            isFullScreen = true;
            ToggleFullScreen();
        }
        else
        {
            isFullScreen = false;
            ToggleFullScreen();
        }
    }

    void ToggleFullScreen()
    {
        //isFullScreen = !isFullScreen;

        if (isFullScreen)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
    }
}