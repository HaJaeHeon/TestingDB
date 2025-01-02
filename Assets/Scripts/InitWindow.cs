using UnityEngine;
using UnityEngine.UI;

public class InitWindow : MonoBehaviour
{
    public bool isFullScreen = false;
    public Toggle fullScreenToggle;

    void Start()
    {
        // 초기 창 모드 설정
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
        // 키 입력을 통한 창 모드 변경
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