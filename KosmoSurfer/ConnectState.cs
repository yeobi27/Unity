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
    Sprite[] connectSprite;    //���� �̹��� 0: ����(����) , 1: ����(�ʷ�)
    Image connectImg;

    private void Start()
    {
        Debug.Log("SensorStateReciver ConnectState : Start");
        SensorManager.instance.SensorStateReciver += GetConnectState;
        connectImg = GetComponent<Image>();

        // �̹� ����� ���¸� - �ΰ����϶�
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
    // ���� ������� �����Ŵ������� ����
    private void GetConnectState(int _state)
    {
        // ������¿� ���� �̹��� �ٲٱ�
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
    // ������� ��ư Ŭ��
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
    // ������¿� ���� �˾�
    private void FitTagConnectStatePopup(int _sel)
    {
        if (_sel == 0)
        {
            // ������¿� ���� �˾� �޶���
            PopupPanel.SetActive(true);
            fitTagPopupPanel.SetActive(true);
            fitTagPopupPanel.GetComponentInChildren<TMP_Text>().text = "FIT-TAG ������ �Դϴ�.\n��ø� ��ٷ� �ּ���.";
            fitTagPopupPanel.transform.GetChild(1).gameObject.SetActive(false);  // �����
            fitTagPopupPanel.transform.GetChild(2).gameObject.SetActive(false); // �������
            fitTagPopupPanel.transform.GetChild(3).gameObject.SetActive(true); // ������
        }
        else if (_sel == 1)
        {
            // ������¿� ���� �˾� �޶���
            PopupPanel.SetActive(true);
            fitTagPopupPanel.SetActive(true);
            fitTagPopupPanel.GetComponentInChildren<TMP_Text>().text = "FIT-TAG ������ �Ǿ����ϴ�.";
            fitTagPopupPanel.transform.GetChild(1).gameObject.SetActive(true);
            fitTagPopupPanel.transform.GetChild(2).gameObject.SetActive(false);
            fitTagPopupPanel.transform.GetChild(3).gameObject.SetActive(false); // ������
        }
        else
        {
            PopupPanel.SetActive(true);
            fitTagPopupPanel.SetActive(true);
            fitTagPopupPanel.GetComponentInChildren<TMP_Text>().text = "FIT-TAG ������ ������ϴ�.\n����� ������ �Ѽ� �ٽ� �������ּ���.";
            fitTagPopupPanel.transform.GetChild(1).gameObject.SetActive(false);
            fitTagPopupPanel.transform.GetChild(2).gameObject.SetActive(true);
            fitTagPopupPanel.transform.GetChild(3).gameObject.SetActive(false); // ������
        }
    }
    // ���� �����ϱ� ��ư
    public void ConnectFitTag()
    {
        SensorManager.instance.StartProcess();
    }

    public void DisConnectFitTag()
    {
        SensorManager.instance.StopProcess();
    }
}
