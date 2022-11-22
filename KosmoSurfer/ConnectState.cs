using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectState : MonoBehaviour
{
    [SerializeField]
    GameObject PopupPanel;
    [SerializeField]
    GameObject fitTagPopupPanel;
    enum SensorState
    {
        Connecting,
        Connected,
        DisConnected
    }

    [SerializeField]
    Sprite[] connectSprite;    //연결 이미지 0: 끊김(빨강) , 1: 연결(초록)
    Image connectImg;

    private void Start()
    {
        Debug.Log("SensorStateReciver ConnectState : Start");
        SensorManager.instance.SensorStateReciver += GetConnectState;
        connectImg = GetComponent<Image>();

        // 이미 연결된 상태면 - 인게임일때
        if (SensorManager.instance._connected)
        {
            connectImg.sprite = connectSprite[1];
        }
    }

    private void OnDisable()
    {
        Debug.Log("SensorStateReciver ConnectState : Disable");
        SensorManager.instance.SensorStateReciver -= GetConnectState;
    }
    // 현재 연결상태 센서매니저에서 받음
    private void GetConnectState(int _state)
    {
        // 연결상태에 따라 이미지 바꾸기
        switch (_state)
        {
            case (int)SensorState.Connecting:
                Debug.Log("SensorStateReciver State : Connecting");
                connectImg.sprite = connectSprite[0];
                FitTagConnectStatePopup((int)SensorState.Connecting);
                break;
            case (int)SensorState.Connected:
                Debug.Log("SensorStateReciver State : Connected");
                connectImg.sprite = connectSprite[1];
                FitTagConnectStatePopup((int)SensorState.Connected);
                break;
            case (int)SensorState.DisConnected:
                Debug.Log("SensorStateReciver State : DisConnected");
                connectImg.sprite = connectSprite[0];
                FitTagConnectStatePopup((int)SensorState.DisConnected);
                break;
            default:
                break;
        }
    }    
    // 연결상태 버튼 클릭
    public void FitTagConnectStatePopupAction()
    {
        if (SensorManager.instance._connected)
        {
            FitTagConnectStatePopup(1);
        }
        else if (SensorManager.instance.connecting)
        {
            FitTagConnectStatePopup(0);
        }
        else
        {
            FitTagConnectStatePopup(2);
        }
    }
    // 연결상태에 따른 팝업
    private void FitTagConnectStatePopup(int _sel)
    {
        if (_sel == 0)
        {
            // 연결상태에 따른 팝업 달라짐
            PopupPanel.SetActive(true);
            fitTagPopupPanel.SetActive(true);
            fitTagPopupPanel.GetComponentInChildren<TMP_Text>().text = "FIT-TAG 연결중 입니다.\n잠시만 기다려 주세요.";
            fitTagPopupPanel.transform.GetChild(1).gameObject.SetActive(false);  // 연결됨
            fitTagPopupPanel.transform.GetChild(2).gameObject.SetActive(false); // 연결끊김
            fitTagPopupPanel.transform.GetChild(3).gameObject.SetActive(true); // 연결중
        }
        else if (_sel == 1)
        {
            // 연결상태에 따른 팝업 달라짐
            PopupPanel.SetActive(true);
            fitTagPopupPanel.SetActive(true);
            fitTagPopupPanel.GetComponentInChildren<TMP_Text>().text = "FIT-TAG 연결이 되었습니다.";
            fitTagPopupPanel.transform.GetChild(1).gameObject.SetActive(true);
            fitTagPopupPanel.transform.GetChild(2).gameObject.SetActive(false);
            fitTagPopupPanel.transform.GetChild(3).gameObject.SetActive(false); // 연결중
        }
        else
        {
            PopupPanel.SetActive(true);
            fitTagPopupPanel.SetActive(true);
            fitTagPopupPanel.GetComponentInChildren<TMP_Text>().text = "FIT-TAG 연결이 끊겼습니다.\n기기의 전원을 켜서 다시 연결해주세요.";
            fitTagPopupPanel.transform.GetChild(1).gameObject.SetActive(false);
            fitTagPopupPanel.transform.GetChild(2).gameObject.SetActive(true);
            fitTagPopupPanel.transform.GetChild(3).gameObject.SetActive(false); // 연결중
        }
    }
    // 센서 연결하기 버튼
    public void ConnectFitTag()
    {
        SensorManager.instance.StartProcess();
    }

    public void DisConnectFitTag()
    {
        SensorManager.instance.StopProcess();
    }
}
