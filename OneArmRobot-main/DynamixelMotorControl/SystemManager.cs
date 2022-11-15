using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    //지정된 창의 표시 상태 설정
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    //활성화된 윈도우-함수를 호출한 쓰레드와 연동되 년석의 핸들을 받는다.
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();



    /// <summary>
    /// 시스템 종료
    /// </summary>
    public void OnWindowCloseButtonClick()
    {
        Application.Quit();
    }

    /// <summary>
    /// 윈도우폼 최소화 함수
    /// </summary>
    public void OnMiniMizeButtonClick()
    {
        ShowWindow(GetActiveWindow(), 2);
    }

    public void OnMaxMizeButtonClick()
    {
        ShowWindow(GetActiveWindow(), 1);
    }
}
