using System.Collections;
using System.Collections.Generic;
using Ardunity;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO.Ports;
using UnityEngine.Events;
using System.Runtime.InteropServices;

//public enum RobotType {
//    ArmRobot,
//    PistonRobot,
//    Type3,
//    Type4,
//    Type5,
//    Type6,
//    Type7,
//    Type8,
//    Type9,
//    Type10
//};

//MonoBehaviour
public class ButtonManager : CommSocket
{
    //지정된 창의 표시 상태 설정
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    //활성화된 윈도우-함수를 호출한 쓰레드와 연동 된 녀석의 핸들을 받는다.
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [Header("ArdunityApps")]
    public ArdunityApp ardunityApp;
    [Header("CommSerials")]
    public CommSerial commSerial;
    [Header("Dropdown")]
    public Dropdown dropdown;
    [Header("Buttons")]
    public Text ref_connect_disconnect_text;


    void Start()
    {
        SetDropdownOptions();
    }

    /// <summary>
    /// 프로그램 종료
    /// </summary>
    public void OnWindowCloseButtonClick()
    {
        Application.Quit();
    }

    /// <summary>
    /// 윈도우폼 최소화 함수
    /// </summary>
    public void OnMinimizeButtonClick()
    {
        ShowWindow(GetActiveWindow(), 2);
    }

    public void OnConnected()
    {
        Debug.Log("On Connected");
        ref_connect_disconnect_text.text = "Disconnect";
    }

    void OnConnectionFailed()
    {
        Debug.Log("On Connection Failed");
    }

    public void OnDisconnected()
    {
        Debug.Log("On Disconnected");
        ref_connect_disconnect_text.text = "Connect";
    }

    void OnLostConnection()
    {
        Debug.Log("On Lost Connection");
    }

    public void StartStopButton()
    {
        // 연결
        if (ref_connect_disconnect_text.text.Equals("Connect"))
        {
            ardunityApp.Connect();
        }
        else if (ref_connect_disconnect_text.text.Equals("Disconnect"))
        {
            ardunityApp.Disconnect();
        }
    }

    private void SetDropdownOptions()// Dropdown 목록 생성
    {
        dropdown.options.Clear();
     
        // Port 검색
        string[] ports = SerialPort.GetPortNames();
        Debug.Log("port Length : " + ports.Length);

        for (int j = 0; j < ports.Length; j++)//1부터 10까지
        {
            // Caption 대기 고정
            if (j==0)
            {
                Dropdown.OptionData captionText = new Dropdown.OptionData();
                captionText.text = "Glove Option"+(j+1);
                dropdown.options.Add(captionText);
            }

            Dropdown.OptionData option = new Dropdown.OptionData();
            //option.text = i.ToString() + "갯수";
            Debug.Log("port : " + ports[j]);
            option.text = ports[j];
            dropdown.options.Add(option);
        }
    }

    public void OnValueChange()
    {
        try
        {
            commSerial.device.address = "//./" + dropdown.captionText.text;

            Debug.Log("commSerial.device.address : " + commSerial.device.address);

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void SearchButton()// SelectButton을 누름으로써 값 테스트.
    {
        SetDropdownOptions();
    }
}

