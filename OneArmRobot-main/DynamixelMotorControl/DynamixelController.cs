using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.InteropServices;
using dynamixel_sdk;

public class DynamixelController : MonoBehaviour
{
    // Control table address
    public const int ADDR_PRO_TORQUE_ENABLE = 64;                 // Control table address is different in Dynamixel model
    //public const int ADDR_PRO_LED_RED = 65;
    public const int ADDR_PRO_GOAL_POSITION = 116;
    public const int ADDR_PRO_PRESENT_POSITION = 132;

    // Data Byte Length
    //public const int LEN_PRO_LED_RED = 1;
    public const int LEN_PRO_GOAL_POSITION = 4;
    public const int LEN_PRO_PRESENT_POSITION = 4;

    // Protocol version
    public const int PROTOCOL_VERSION = 2;                   // See which protocol version is used in the Dynamixel

    // Default setting
    public const int DXL11_ID = 11;                   // Dynamixel ID: 11
    public const int DXL12_ID = 12;                   // Dynamixel ID: 12
    public const int DXL13_ID = 13;                   // Dynamixel ID: 13
    public const int DXL14_ID = 14;                   // Dynamixel ID: 14
    public const int DXL15_ID = 15;                   // Dynamixel ID: 15
    public const int BAUDRATE = 1000000;
    public const string DEVICENAME = "COM9";              // Check which port is being used on your controller
                                                          // ex) Windows: "COM1"   Linux: "/dev/ttyUSB0" Mac: "/dev/tty.usbserial-*"

    public const int TORQUE_ENABLE = 1;                   // Value for enabling the torque
    public const int TORQUE_DISABLE = 0;                   // Value for disabling the torque
    public const int DXL_MINIMUM_POSITION_VALUE = 0;             // Dynamixel will rotate between this value
    public const int DXL_MAXIMUM_POSITION_VALUE = 4095;              // and this value (note that the Dynamixel would not move when the position value is out of movable range. Check e-manual about the range of the Dynamixel you use.)    
    public const int DXL_MOVING_STATUS_THRESHOLD = 20;                  // Dynamixel moving status threshold

    public const byte ESC_ASCII_VALUE = 0x1b;

    public const int COMM_SUCCESS = 0;                   // Communication Success result value
    public const int COMM_TX_FAIL = -1001;               // Communication Tx Failed

    public const int PROFILE_ACCELERATION = 108;    //Address of Profile Acceleration in Contorl Table 
    public const int PROFILE_VELOCITY = 112;        //Address of Profile Velocity in Contorl Table 

    public const int P_POSITION_D_GAIN = 80;    //Address of Position D Gain in Contorl Table 
    public const int P_POSITION_I_GAIN = 82;    //Address of Position I Gain in Contorl Table 
    public const int P_POSITION_P_GAIN = 84;    //Address of Position P Gain in Contorl Table 

    int port_num;

    int groupwrite_num = 0;
    int groupread_num = 0;

    int index = 0;
    int dxl_comm_result = COMM_TX_FAIL;                                   // Communication result
    bool dxl_addparam_result = false;                                     // AddParam result
    bool dxl_getdata_result = false;                                      // GetParam result

    int[] dxl_goal_position = new int[2] { DXL_MINIMUM_POSITION_VALUE, DXL_MAXIMUM_POSITION_VALUE };         // Goal position
    byte dxl_error = 0;                                                   // Dynamixel error
    ushort dxl_p_gain = 5000;                                             // P Gain
    ushort dxl_i_gain = 500;                                              // I Gain
    ushort dxl_d_gain = 600;                                              // D Gain
    Int32 dxl11_present_position = 0;                                       // Present position
    Int32 dxl12_present_position = 0;                                       // Present position
    Int32 dxl13_present_position = 0;                                       // Present position
    Int32 dxl14_present_position = 0;                                       // Present position
    Int32 dxl15_present_position = 0;                                       // Present position

    public GameObject LoPolyHandRight;
    
    private bool isSave_init_rate;  //

    private float ID11_prev_rate;
    private float ID12_prev_rate;
    private float ID13_prev_rate;
    private float ID14_prev_rate;
    private float ID15_prev_rate;

    private float ID11_Angle;
    private float ID12_Angle;
    private float ID13_Angle;
    private float ID14_Angle;
    private float ID15_Angle;

    // Start is called before the first frame update
    void Start()
    {
        isSave_init_rate = true;    // Normalize 초기값을 한번 가져오기 위한 bool

        port_num = dynamixel.portHandler(DEVICENAME);   // port number
        dynamixel.packetHandler();                      // Handler 선언

        // Initialize GroupBulkWrite Struct
        groupwrite_num = dynamixel.groupBulkWrite(port_num, PROTOCOL_VERSION);      // Bulk 쓰기

        // Initialize Groupbulkread Structs
        groupread_num = dynamixel.groupBulkRead(port_num, PROTOCOL_VERSION);        // Bulk 읽기

        OpenPort();     // 포트 열기

        EnableTorque(); // 토크 주기

        ProfileSetting();   // Profile Setting : 속도 제한

        GroupBulkWriteAddParam();   // 디폴트 각도

        GroupBulkReadAddParam();    // 데이터 수신 : 현재각도

        
    }

    public void OpenPort()
    {
        // Open port
        if (dynamixel.openPort(port_num))
        {
            Debug.Log("Succeeded to open the port!");
        }
        else
        {
            Debug.Log("Failed to open the port!");
            Debug.Log("Press any key to terminate...");
            //Console.ReadKey();
            //return;
        }

        // Set port baudrate
        if (dynamixel.setBaudRate(port_num, BAUDRATE))
        {
            Debug.Log("Succeeded to change the baudrate!");
        }
        else
        {
            Debug.Log("Failed to change the baudrate!");
            Debug.Log("Press any key to terminate...");
            //Console.ReadKey();
            //return;
        }
    }

    public void EnableTorque()
    {
        // Enable Dynamixel#11 Torque
        dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL11_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_ENABLE);
        if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
        }
        else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
        }
        else
        {
            Debug.Log("Dynamixel 11 has been successfully connected");
        }

        // Enable Dynamixel#12 Torque
        dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL12_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_ENABLE);
        if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
        }
        else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
        }
        else
        {
            Debug.Log("Dynamixel 12 has been successfully connected");
        }

        // Enable Dynamixel#13 Torque
        dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL13_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_ENABLE);
        if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
        }
        else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
        }
        else
        {
            Debug.Log("Dynamixel 13 has been successfully connected");
        }

        // Enable Dynamixel#14 Torque
        dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL14_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_ENABLE);
        if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
        }
        else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
        }
        else
        {
            Debug.Log("Dynamixel 14 has been successfully connected");
        }

        // Enable Dynamixel#15 Torque
        dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL15_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_ENABLE);
        if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
        }
        else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
        }
        else
        {
            Debug.Log("Dynamixel 15 has been successfully connected");
        }
    }

    // 속도, 가속도 세팅하는 함수
    public void ProfileSetting()
    {
        // Profile Setting
        dynamixel.write4ByteTxRx(port_num, PROTOCOL_VERSION, DXL11_ID, PROFILE_VELOCITY, (UInt32)200);
        dynamixel.write4ByteTxRx(port_num, PROTOCOL_VERSION, DXL11_ID, PROFILE_ACCELERATION, (UInt32)20);

        dynamixel.write4ByteTxRx(port_num, PROTOCOL_VERSION, DXL12_ID, PROFILE_VELOCITY, (UInt32)200);
        dynamixel.write4ByteTxRx(port_num, PROTOCOL_VERSION, DXL12_ID, PROFILE_ACCELERATION, (UInt32)20);

        dynamixel.write4ByteTxRx(port_num, PROTOCOL_VERSION, DXL13_ID, PROFILE_VELOCITY, (UInt32)200);
        dynamixel.write4ByteTxRx(port_num, PROTOCOL_VERSION, DXL13_ID, PROFILE_ACCELERATION, (UInt32)20);

        dynamixel.write4ByteTxRx(port_num, PROTOCOL_VERSION, DXL14_ID, PROFILE_VELOCITY, (UInt32)200);
        dynamixel.write4ByteTxRx(port_num, PROTOCOL_VERSION, DXL14_ID, PROFILE_ACCELERATION, (UInt32)20);

        dynamixel.write4ByteTxRx(port_num, PROTOCOL_VERSION, DXL15_ID, PROFILE_VELOCITY, (UInt32)200);
        dynamixel.write4ByteTxRx(port_num, PROTOCOL_VERSION, DXL15_ID, PROFILE_ACCELERATION, (UInt32)20);
    }

    public void DisableTorque()
    {
        // 멈춤
        // Disable Dynamixel Torque
        dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL11_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_DISABLE);
        if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
        }
        else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
        }

        // 멈춤
        // Disable Dynamixel Torque
        dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL12_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_DISABLE);
        if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
        }
        else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
        }

        // 멈춤
        // Disable Dynamixel Torque
        dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL13_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_DISABLE);
        if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
        }
        else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
        }

        // 멈춤
        // Disable Dynamixel Torque
        dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL14_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_DISABLE);
        if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
        }
        else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
        }

        // 멈춤
        // Disable Dynamixel Torque
        dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL15_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_DISABLE);
        if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
        }
        else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
        {
            Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
        }

        // Close port
        dynamixel.closePort(port_num);
    }

    public void GroupBulkWriteAddParam()
    {
        // Add parameter storage for Dynamixel#11 goal position
        dxl_addparam_result = dynamixel.groupBulkWriteAddParam(groupwrite_num, DXL11_ID, ADDR_PRO_GOAL_POSITION, LEN_PRO_GOAL_POSITION, (UInt32)2048, LEN_PRO_GOAL_POSITION);
        if (dxl_addparam_result != true)
        {
            Debug.Log("[ID: 11] groupBulkWrite addparam failed : " + DXL11_ID);
            return;
        }

        //// Add parameter storage for Dynamixel#12 goal position
        dxl_addparam_result = dynamixel.groupBulkWriteAddParam(groupwrite_num, DXL12_ID, ADDR_PRO_GOAL_POSITION, LEN_PRO_GOAL_POSITION, (UInt32)2040, LEN_PRO_GOAL_POSITION);
        if (dxl_addparam_result != true)
        {
            Debug.Log("[ID: 12] groupBulkWrite addparam failed : " + DXL12_ID);
            return;
        }

        ////Add parameter storage for Dynamixel#13 goal position
        dxl_addparam_result = dynamixel.groupBulkWriteAddParam(groupwrite_num, DXL13_ID, ADDR_PRO_GOAL_POSITION, LEN_PRO_GOAL_POSITION, (UInt32)2152, LEN_PRO_GOAL_POSITION);
        if (dxl_addparam_result != true)
        {
            Debug.Log("[ID: 13] groupBulkWrite addparam failed : " + DXL13_ID);
            return;
        }

        //// Add parameter storage for Dynamixel#14 goal position
        dxl_addparam_result = dynamixel.groupBulkWriteAddParam(groupwrite_num, DXL14_ID, ADDR_PRO_GOAL_POSITION, LEN_PRO_GOAL_POSITION, (UInt32)2047, LEN_PRO_GOAL_POSITION);
        if (dxl_addparam_result != true)
        {
            Debug.Log("[ID: 14] groupBulkWrite addparam failed : " + DXL14_ID);
            return;
        }

        //// Add parameter storage for Dynamixel#15 goal position
        dxl_addparam_result = dynamixel.groupBulkWriteAddParam(groupwrite_num, DXL15_ID, ADDR_PRO_GOAL_POSITION, LEN_PRO_GOAL_POSITION, (UInt32)2800, LEN_PRO_GOAL_POSITION);
        if (dxl_addparam_result != true)
        {
            Debug.Log("[ID: 15] groupBulkWrite addparam failed : " + DXL15_ID);
            return;
        }

        // Bulkwrite goal position and LED value : 쓰기
        dynamixel.groupBulkWriteTxPacket(groupwrite_num);
        if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
            dynamixel.printTxRxResult(PROTOCOL_VERSION, dxl_comm_result);

        // Clear bulkwrite parameter storage
        dynamixel.groupBulkWriteClearParam(groupwrite_num);
    }

    public void GroupBulkReadAddParam()
    {

        // Bulkread present position and moving status
        dynamixel.groupBulkReadTxRxPacket(groupread_num);
        if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
            dynamixel.printTxRxResult(PROTOCOL_VERSION, dxl_comm_result);

        // Add parameter storage for Dynamixel#1 present position value
        dxl_addparam_result = dynamixel.groupBulkReadAddParam(groupread_num, DXL11_ID, ADDR_PRO_PRESENT_POSITION, LEN_PRO_PRESENT_POSITION);
        if (dxl_addparam_result != true)
        {
            Debug.Log("[ID: 11] groupBulkRead addparam failed : " + DXL11_ID);
            return;
        }

        //// Add parameter storage for Dynamixel#2 present moving value
        dxl_addparam_result = dynamixel.groupBulkReadAddParam(groupread_num, DXL12_ID, ADDR_PRO_PRESENT_POSITION, LEN_PRO_PRESENT_POSITION);
        if (dxl_addparam_result != true)
        {
            Debug.Log("[ID: 12] groupBulkRead addparam failed " + DXL12_ID);
            return;
        }

        // Add parameter storage for Dynamixel#1 present position value
        dxl_addparam_result = dynamixel.groupBulkReadAddParam(groupread_num, DXL13_ID, ADDR_PRO_PRESENT_POSITION, LEN_PRO_PRESENT_POSITION);
        if (dxl_addparam_result != true)
        {
            Debug.Log("[ID: 13] groupBulkRead addparam failed : " + DXL13_ID);
            return;
        }

        // Add parameter storage for Dynamixel#2 present moving value
        dxl_addparam_result = dynamixel.groupBulkReadAddParam(groupread_num, DXL14_ID, ADDR_PRO_PRESENT_POSITION, LEN_PRO_PRESENT_POSITION);
        if (dxl_addparam_result != true)
        {
            Debug.Log("[ID: 14] groupBulkRead addparam failed " + DXL14_ID);
            return;
        }

        // Add parameter storage for Dynamixel#2 present moving value
        dxl_addparam_result = dynamixel.groupBulkReadAddParam(groupread_num, DXL15_ID, ADDR_PRO_PRESENT_POSITION, LEN_PRO_PRESENT_POSITION);
        if (dxl_addparam_result != true)
        {
            Debug.Log("[ID: 15] groupBulkRead addparam failed " + DXL15_ID);
            return;
        }

        // Get Dynamixel present position value
        dxl11_present_position = (Int32)dynamixel.groupBulkReadGetData(groupread_num, DXL11_ID, ADDR_PRO_PRESENT_POSITION, LEN_PRO_PRESENT_POSITION);
        dxl12_present_position = (Int32)dynamixel.groupBulkReadGetData(groupread_num, DXL12_ID, ADDR_PRO_PRESENT_POSITION, LEN_PRO_PRESENT_POSITION);
        dxl13_present_position = (Int32)dynamixel.groupBulkReadGetData(groupread_num, DXL13_ID, ADDR_PRO_PRESENT_POSITION, LEN_PRO_PRESENT_POSITION);
        dxl14_present_position = (Int32)dynamixel.groupBulkReadGetData(groupread_num, DXL14_ID, ADDR_PRO_PRESENT_POSITION, LEN_PRO_PRESENT_POSITION);
        dxl15_present_position = (Int32)dynamixel.groupBulkReadGetData(groupread_num, DXL15_ID, ADDR_PRO_PRESENT_POSITION, LEN_PRO_PRESENT_POSITION);

        //Debug.Log("[ID: 11] : " + DXL11_ID + " ,Present Position : " + dxl11_present_position);
        //Debug.Log("[ID: 12] : " + DXL12_ID + " ,Present Position : " + dxl12_present_position);
        //Debug.Log("[ID: 13] : " + DXL13_ID + " ,Present Position : " + dxl13_present_position);
        //Debug.Log("[ID: 14] : " + DXL14_ID + " ,Present Position : " + dxl14_present_position);
        //Debug.Log("[ID: 15] : " + DXL15_ID + " ,Present Position : " + dxl15_present_position);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 멈춤
            // Disable Dynamixel Torque
            dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL11_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_DISABLE);
            if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
            {
                Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
            }
            else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
            {
                Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
            }

            // 멈춤
            // Disable Dynamixel Torque
            dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL12_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_DISABLE);
            if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
            {
                Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
            }
            else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
            {
                Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
            }

            // 멈춤
            // Disable Dynamixel Torque
            dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL13_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_DISABLE);
            if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
            {
                Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
            }
            else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
            {
                Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
            }

            // 멈춤
            // Disable Dynamixel Torque
            dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL14_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_DISABLE);
            if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
            {
                Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
            }
            else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
            {
                Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
            }

            // 멈춤
            // Disable Dynamixel Torque
            dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL15_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_DISABLE);
            if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
            {
                Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getTxRxResult(PROTOCOL_VERSION, dxl_comm_result)));
            }
            else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
            {
                Debug.Log(Marshal.PtrToStringAnsi(dynamixel.getRxPacketError(PROTOCOL_VERSION, dxl_error)));
            }

            // Close port
            dynamixel.closePort(port_num);

            return;
        }

        if (LoPolyHandRight.activeSelf)
        {
            if (isSave_init_rate)
            {
                ID11_prev_rate = OneArmTransform.Instance.ID11_goal_rate;    // 현재 위치 비율저장 : 한번만 쓰임
            }

            // 0 ~ 1 사이
            if (Mathf.Abs(Mathf.Abs(OneArmTransform.Instance.ID11_goal_rate) - Mathf.Abs(ID11_prev_rate)) > 0.015f || isSave_init_rate)
            {
                ID11_Angle = Mathf.Lerp(4095f, 0f, OneArmTransform.Instance.ID11_goal_rate);   // 모터각도 목표위치 저장 
                ID11_prev_rate = OneArmTransform.Instance.ID11_goal_rate;    // 현재 위치 저장           
                //Debug.Log("ID11 Angle : " + ID11_Angle);   // 모터각도 목표위치 저장 );
            }

            //// Add parameter storage for Dynamixel#11 goal position
            dxl_addparam_result = dynamixel.groupBulkWriteAddParam(groupwrite_num, DXL11_ID, ADDR_PRO_GOAL_POSITION, LEN_PRO_GOAL_POSITION, (UInt32)ID11_Angle, LEN_PRO_GOAL_POSITION);
            if (dxl_addparam_result != true)
            {
                Debug.Log("[ID: 11] groupBulkWrite addparam failed : " + DXL11_ID);
                return;
            }

            if (isSave_init_rate)
            {
                ID12_prev_rate = OneArmTransform.Instance.ID12_goal_rate;    // 현재 위치 비율저장 : 한번만 쓰임
            }

            if (Mathf.Abs(Mathf.Abs(OneArmTransform.Instance.ID12_goal_rate) - Mathf.Abs(ID12_prev_rate)) > 0.015f || isSave_init_rate)
            {
                ID12_Angle = Mathf.Lerp(700f, 3400f, OneArmTransform.Instance.ID12_goal_rate);   // 모터각도 목표위치 저장 
                ID12_prev_rate = OneArmTransform.Instance.ID12_goal_rate;    // 현재 위치 저장
                //Debug.Log("ID12 Angle : " + ID12_Angle);   // 모터각도 목표위치 저장 );
            }

            //// Add parameter storage for Dynamixel#12 goal position
            dxl_addparam_result = dynamixel.groupBulkWriteAddParam(groupwrite_num, DXL12_ID, ADDR_PRO_GOAL_POSITION, LEN_PRO_GOAL_POSITION, (UInt32)ID12_Angle, LEN_PRO_GOAL_POSITION);
            if (dxl_addparam_result != true)
            {
                Debug.Log("[ID: 12] groupBulkWrite addparam failed : " + DXL12_ID);
                return;
            }

            if (isSave_init_rate)
            {
                ID13_prev_rate = OneArmTransform.Instance.ID13_goal_rate;    // 현재 위치 비율저장 : 한번만 쓰임
            }

            if (Mathf.Abs(Mathf.Abs(OneArmTransform.Instance.ID13_goal_rate) - Mathf.Abs(ID13_prev_rate)) > 0.015f || isSave_init_rate)
            {
                ID13_Angle = Mathf.Lerp(2120f, 3400f, OneArmTransform.Instance.ID13_goal_rate);   // 모터각도 목표위치 저장 
                ID13_prev_rate = OneArmTransform.Instance.ID13_goal_rate;    // 현재 위치 저장
                //Debug.Log("ID13 Angle : " + ID13_Angle);   // 모터각도 목표위치 저장 );
            }

            //Add parameter storage for Dynamixel#13 goal position
            //dxl_addparam_result = dynamixel.groupBulkWriteAddParam(groupwrite_num, DXL13_ID, ADDR_PRO_GOAL_POSITION, LEN_PRO_GOAL_POSITION, (UInt32)Present_R_Hand_Transform.Instance._z_R_forearm_pos, LEN_PRO_GOAL_POSITION);
            dxl_addparam_result = dynamixel.groupBulkWriteAddParam(groupwrite_num, DXL13_ID, ADDR_PRO_GOAL_POSITION, LEN_PRO_GOAL_POSITION, (UInt32)ID13_Angle, LEN_PRO_GOAL_POSITION);
            if (dxl_addparam_result != true)
            {
                Debug.Log("[ID: 13] groupBulkWrite addparam failed : " + DXL13_ID);
                return;
            }

            if (isSave_init_rate)
            {
                ID14_prev_rate = OneArmTransform.Instance.ID14_goal_rate;    // 현재 위치 비율저장 : 한번만 쓰임
            }

            if (Mathf.Abs(Mathf.Abs(OneArmTransform.Instance.ID14_goal_rate) - Mathf.Abs(ID14_prev_rate)) > 0.015f || isSave_init_rate)
            {
                ID14_Angle = Mathf.Lerp(638f, 3253f, OneArmTransform.Instance.ID14_goal_rate);   // 모터각도 목표위치 저장 
                ID14_prev_rate = OneArmTransform.Instance.ID14_goal_rate;    // 현재 위치 저장
                //Debug.Log("ID14 Angle : " + ID14_Angle);   // 모터각도 목표위치 저장 );
            }

            //Add parameter storage for Dynamixel#14 goal position
            dxl_addparam_result = dynamixel.groupBulkWriteAddParam(groupwrite_num, DXL14_ID, ADDR_PRO_GOAL_POSITION, LEN_PRO_GOAL_POSITION, (UInt32)ID14_Angle, LEN_PRO_GOAL_POSITION);
            if (dxl_addparam_result != true)
            {
                Debug.Log("[ID: 14] groupBulkWrite addparam failed : " + DXL14_ID);
                return;
            }

            if (isSave_init_rate)
            {
                ID15_prev_rate = OneArmTransform.Instance.ID15_goal_rate;    // 현재 위치 비율저장 : 한번만 쓰임
            }

            if (Mathf.Abs(Mathf.Abs(OneArmTransform.Instance.ID15_goal_rate) - Mathf.Abs(ID15_prev_rate)) > 0.015f || isSave_init_rate)
            {
                ID15_Angle = Mathf.Lerp(1460f, 2817f, OneArmTransform.Instance.ID15_goal_rate);   // 모터각도 목표위치 저장 
                ID15_prev_rate = OneArmTransform.Instance.ID15_goal_rate;    // 현재 위치 저장
                //Debug.Log("ID14 Angle : " + ID14_Angle);   // 모터각도 목표위치 저장 );
            }

            //Add parameter storage for Dynamixel#15 goal position
            dxl_addparam_result = dynamixel.groupBulkWriteAddParam(groupwrite_num, DXL15_ID, ADDR_PRO_GOAL_POSITION, LEN_PRO_GOAL_POSITION, (UInt32)ID15_Angle, LEN_PRO_GOAL_POSITION);
            if (dxl_addparam_result != true)
            {
                Debug.Log("[ID: 15] groupBulkWrite addparam failed : " + DXL15_ID);
                return;
            }

            // Bulkwrite goal position and LED value : 쓰기
            dynamixel.groupBulkWriteTxPacket(groupwrite_num);
            if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
                dynamixel.printTxRxResult(PROTOCOL_VERSION, dxl_comm_result);

            // Clear bulkwrite parameter storage
            dynamixel.groupBulkWriteClearParam(groupwrite_num);

            GroupBulkReadAddParam();
         
            isSave_init_rate = false;
        }
        else
        {
            isSave_init_rate = true;
            GroupBulkWriteAddParam();
        }
    }

    void OnApplicationQuit()
    {
        DisableTorque();
    }

}
