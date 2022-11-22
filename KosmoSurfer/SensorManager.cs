using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;

public class SensorManager : MonoBehaviour
{
    public static SensorManager instance { get; private set; }

    private string DeviceName = "ESP32";// "ESP32 FITNESS LEFT";
    private string ServiceUUID = "c123";
    private string Characteristic = "c124";

    float qx, qy, qz, qw;

    enum States
    {
        None,
        Scan,
        Connect,
        Subscribe,
        Unsubscribe,
        Disconnect,
        Communication,
    }

    public bool _connected = false;
    public bool connecting;    //연결 상태
    private float _timeout = 0f;
    private States _state = States.None;
    private string _deviceAddress;

    //센서 연결 팝업 
    public Sprite[] connectSprite;    //연결 이미지 0: 끊김(빨강) , 1: 연결(초록)
    
    public Action<int> SensorStateReciver;

    enum SensorState
    {
        Connecting,
        Connected,
        DisConnected
    }

    public float GetTurnAndTilt {
        get { return qx; }
    }
    
    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    void Reset()
    {
        _timeout = 0f;
        _state = States.None;
        _deviceAddress = null;
    }

    void SetState(States newState, float timeout)
    {
        _state = newState;
        _timeout = timeout;
    }

    public void StartProcess()
    {
        Reset();
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            SetState(States.Scan, 0.2f);
        }, (error) =>
        {
            // 에러가 나면
            Popup_SenSor_State_Show();
            BluetoothLEHardwareInterface.Log("Error: " + error);
        });
    }

    public void StopProcess()
    {
        SetState(States.Unsubscribe, 0.3f);
    }

    public void SetSensorAddr()
    {
        StopProcess();
        PlayerPrefs.SetString("SensorAddr", "");
    }


    // 연결상태에 따라서 팝업 띄움
    public void Popup_SenSor_State_Show()
    {
        MethodBase m = MethodBase.GetCurrentMethod();
        Debug.Log($"Executing {m.ReflectedType.Name} / {m.Name}");

    }

    // Use this for initialization
    void Start()
    {
        StartProcess();
    }
    // Update is called once per frame
    void Update()
    {
        if (_timeout > 0f)
        {
            _timeout -= Time.deltaTime;
            if (_timeout <= 0f)
            {
                _timeout = 0f;
                switch (_state)
                {
                    case States.None:
                        break;

                    case States.Scan:

                        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
                        {
                            //setStateText("OKAddress : " + address + "/ Name : " + name);
                            // we only want to look at devices that have the name we are looking for
                            // this is the best way to filter out devices
                            if (name.Contains(DeviceName))
                            {
                                connecting = true;  //연결 성공

                                // it is always a good idea to stop scanning while you connect to a device
                                // and get things set up
                                BluetoothLEHardwareInterface.StopScan();

                                // add it to the list and set to connect to it
                                _deviceAddress = address;

                                if (PlayerPrefs.GetString("SensorAddr").Equals(""))
                                    PlayerPrefs.SetString("SensorAddr", address);

                                SetState(States.Connect, 0.5f);

                                SensorStateReciver?.Invoke((int)SensorState.Connecting);    // 0
                            }
                            else
                            {
                                connecting = false;  //연결 실패

                                Popup_SenSor_State_Show();
                            }

                        }, null, false, false);
                        break;

                    case States.Connect:

                        // note that the first parameter is the address, not the name. I have not fixed this because
                        // of backwards compatiblity.
                        // also note that I am note using the first 2 callbacks. If you are not looking for specific characteristics you can use one of
                        // the first 2, but keep in mind that the device will enumerate everything and so you will want to have a timeout
                        // large enough that it will be finished enumerating before you try to subscribe or do any other operations.
                        BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceAddress, null, null,
                            (address, serviceUUID, characteristicUUID) =>
                            {
                                if (PlayerPrefs.GetString("SensorAddr").Equals(address))
                                {
                                    if (IsEqual(serviceUUID, FullUUID(ServiceUUID)))
                                    {
                                        // if we have found the characteristic that we are waiting for
                                        // set the state. make sure there is enough timeout that if the
                                        // device is still enumerating other characteristics it finishes
                                        // before we try to subscribe
                                        if (IsEqual(characteristicUUID, FullUUID(Characteristic)))
                                        {
                                            _connected = true;
                                            SetState(States.Subscribe, 2f);

                                            connecting = true;  //연결 중
                                        }
                                    }
                                }
                                else
                                {

                                    connecting = false;
                                    Popup_SenSor_State_Show();
                                }


                            }, (disconnectedAddress) =>
                            {
                                BluetoothLEHardwareInterface.Log("Device disconnected: " + disconnectedAddress);

                                _connected = false;
                                Debug.Log("SensorStateReciver?.Invoke : DisConnected");
                                SensorStateReciver?.Invoke((int)SensorState.DisConnected);
                                Popup_SenSor_State_Show();
                            });
                        break;

                    case States.Subscribe:
                        //setStateText("Subscribing to ESP32");
                        _connected = true;
                        Debug.Log("SensorStateReciver?.Invoke : Connected");
                        SensorStateReciver?.Invoke((int)SensorState.Connected);

                        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_deviceAddress,
                        FullUUID(ServiceUUID),
                        FullUUID(Characteristic), null,
                        (address, characteristicUUID, bytes) =>
                        {
                            if (bytes.Length == 16)
                            {
                                qx = BitConverter.ToSingle(bytes, 0);
                                qy = BitConverter.ToSingle(bytes, 4);
                                qz = BitConverter.ToSingle(bytes, 8);
                                qw = BitConverter.ToSingle(bytes, 12);
                            }
                        });

                        // set to the none state and the user can start sending and receiving data
                        _state = States.None;

                        break;

                    case States.Unsubscribe:
                        BluetoothLEHardwareInterface.UnSubscribeCharacteristic(_deviceAddress, FullUUID(ServiceUUID),
                            FullUUID(Characteristic),
                            null);
                        SetState(States.Disconnect, 2f);
                        break;

                    case States.Disconnect:
                        if (_connected)
                        {
                            BluetoothLEHardwareInterface.DisconnectPeripheral(_deviceAddress, (address) =>
                            {
                                BluetoothLEHardwareInterface.DeInitialize(() =>
                                {
                                    _connected = false;
                                    _state = States.None;                                  

                                    SensorStateReciver?.Invoke((int)SensorState.DisConnected);
                                    Popup_SenSor_State_Show();
                                });
                            });
                        }
                        else
                        {                            
                            BluetoothLEHardwareInterface.DeInitialize(() => { _state = States.None; });
                        }
                        break;
                }
            }
        }
    }

    string FullUUID(string uuid)
    {
        return "1234" + uuid + "-1234-1234-1234-1234123412341234";
    }

    bool IsEqual(string uuid1, string uuid2)
    {
        if (uuid1.Length == 4)
            uuid1 = FullUUID(uuid1);
        if (uuid2.Length == 4)
            uuid2 = FullUUID(uuid2);

        return (uuid1.ToUpper().Equals(uuid2.ToUpper()));
    }

}
