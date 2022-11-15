using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; private set; }

    public float _battery_Voltage, _battery_Percent, _battery_Current;
    public float _solar_Voltage, _solar_Current;
    public bool _isconnected;

    public GameObject _batteryState_Charging;
    public GameObject _batteryState_Offline;

    [SerializeField] private TMP_Text _solarInfo_A, _solarInfo_V, _solarInfo_W;
    [SerializeField] private TMP_Text _usageInfo_A, _usageInfo_V, _usageInfo_W;
    [SerializeField] private TMP_Text _batteryInfo_minV, _batteryInfo_maxV, _batteryInfo_Temp;
    [SerializeField] private TMP_Text _charging_Time;
    [SerializeField] private TMP_Text _percent;
    [SerializeField] private Slider remainingBattery;
    [SerializeField] private Button connectButton, disconnectButton;
    [SerializeField] private TMP_Text stateText;

    private float bP, sP;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _battery_Voltage = 0f;
        _battery_Percent = 0f;
        _battery_Current = 0f;
        _solar_Voltage = 0f;
        _solar_Current = 0f;
        _isconnected = false;
        _batteryState_Offline.SetActive(true);
        _batteryState_Charging.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isconnected)
        {
            // connected
            connectButton.gameObject.SetActive(false);
            disconnectButton.gameObject.SetActive(true);
            // Battery//////////////////////////////////////////////////////////////////////////////////
            // Watt Calc
            bP = _battery_Voltage * _battery_Current;
            bP = (float)(Math.Truncate(bP * 10) / 10);

            // Usage Info
            setUsageInfo(_battery_Voltage.ToString(), _battery_Current.ToString(), (bP).ToString());

            // Charging Status Info
            setChargingInfo(_battery_Percent.ToString());

            remainingBattery.value = -_battery_Percent;
            ///////////////////////////////////////////////////////////////////////////////////////////

            // Solar/////////////////////////////////////////////////////////////////////////////////
            // Watt Calc
            sP = _solar_Voltage * _solar_Current;
            sP = (float)(Math.Truncate(sP * 10) / 10);

            // Solar Info
            setSolarInfo(_solar_Voltage.ToString(), _solar_Current.ToString(), (sP).ToString());
            /////////////////////////////////////////////////////////////////////////////////////////

            // Battery State/////////////////////////////////////////////////////////////////////////
            if (bP != 0f && sP != 0f)
            {
                _batteryState_Offline.SetActive(false);
                _batteryState_Charging.SetActive(true);
            }
            else
            {
                _batteryState_Offline.SetActive(true);
                _batteryState_Charging.SetActive(false);
            }
            /////////////////////////////////////////////////////////////////////////////////////////
        }
        else
        {
            // disconnected
            connectButton.gameObject.SetActive(true);
            disconnectButton.gameObject.SetActive(false);
            _battery_Voltage = 0f;
            _battery_Percent = 0f;
            _battery_Current = 0f;
            _solar_Voltage = 0f;
            _solar_Current = 0f;

            setUsageInfo("0", "0", "0");
            setSolarInfo("0", "0", "0");
            setChargingInfo("0");

            _batteryState_Offline.SetActive(true);
            _batteryState_Charging.SetActive(false);
        }
    }
    
    void setSolarInfo(string volt, string curr, string watt)
    {
        // _solarInfo_A, _solarInfo_V, _solarInfo_W;
        if (_solarInfo_A == null || _solarInfo_V == null || _solarInfo_W == null)
        {
            return;
        }
        else
        {
            _solarInfo_A.text = curr + "A";
            _solarInfo_V.text = volt + "V";
            _solarInfo_W.text = watt + "W";
        }
    }

    void setChargingInfo(string percent)
    {
        if (_percent == null)
        {
            return;
        }
        else
        {
            _percent.text = percent + "%";
        }
    }

    void setUsageInfo(string volt, string curr, string watt)
    {
        // _solarInfo_A, _solarInfo_V, _solarInfo_W;
        if (_usageInfo_A == null || _usageInfo_V == null || _usageInfo_W == null)
        {
            return;
        }
        else
        {
            _usageInfo_A.text = curr + "A";
            _usageInfo_V.text = volt + "V";
            _usageInfo_W.text = watt + "W";
        }
    }

}
