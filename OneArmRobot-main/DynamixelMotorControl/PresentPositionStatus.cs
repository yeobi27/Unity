using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.InteropServices;
using dynamixel_sdk;

public class PresentPositionStatus : MonoBehaviour
{
    // Control table address
    public const int ADDR_PRO_TORQUE_ENABLE = 64;                 // Control table address is different in Dynamixel model
    public const int ADDR_PRO_GOAL_POSITION = 116;
    public const int ADDR_PRO_PRESENT_POSITION = 132;

    // Protocol version
    public const int PROTOCOL_VERSION = 2;                   // See which protocol version is used in the Dynamixel

    // Default setting
    public const int DXL_ID = 15;                   // Dynamixel ID: 11
    public const int BAUDRATE = 1000000;
    public const string DEVICENAME = "COM9";              // Check which port is being used on your controller
                                                          // ex) Windows: "COM1"   Linux: "/dev/ttyUSB0" Mac: "/dev/tty.usbserial-*"

    public const int TORQUE_ENABLE = 1;                   // Value for enabling the torque
    public const int TORQUE_DISABLE = 0;                   // Value for disabling the torque
    public const int DXL_MINIMUM_POSITION_VALUE = 0;             // Dynamixel will rotate between this value
    //public const int DXL_MAXIMUM_POSITION_VALUE = 4095;              // and this value (note that the Dynamixel would not move when the position value is out of movable range. Check e-manual about the range of the Dynamixel you use.)
    public const int DXL_MAXIMUM_POSITION_VALUE = 2048;              // and this value (note that the Dynamixel would not move when the position value is out of movable range. Check e-manual about the range of the Dynamixel you use.)
    public const int DXL_MOVING_STATUS_THRESHOLD = 24;                  // Dynamixel moving status threshold

    public const byte ESC_ASCII_VALUE = 0x1b;

    public const int COMM_SUCCESS = 0;                   // Communication Success result value
    public const int COMM_TX_FAIL = -1001;               // Communication Tx Failed

    int port_num;

    int groupwrite_num = 0;
    int groupread_num = 0;

    int index = 1;
    int dxl_comm_result = COMM_TX_FAIL;                                   // Communication result
    bool dxl_addparam_result = false;                                     // AddParam result
    bool dxl_getdata_result = false;                                      // GetParam result

    int[] dxl_goal_position = new int[2] { DXL_MINIMUM_POSITION_VALUE, DXL_MAXIMUM_POSITION_VALUE };         // Goal position
    byte dxl_error = 0;                                                   // Dynamixel error
    Int32 dxl_present_position = 0;                                       // Present position
    public int Pos = 687;

    // Start is called before the first frame update
    void Start()
    {
        port_num = dynamixel.portHandler(DEVICENAME);
        dynamixel.packetHandler();

        // Initialize GroupBulkWrite Struct
        groupwrite_num = dynamixel.groupBulkWrite(port_num, PROTOCOL_VERSION);

        // Initialize Groupbulkread Structs
        groupread_num = dynamixel.groupBulkRead(port_num, PROTOCOL_VERSION);

        OpenPort();

        EnableTorque();

        Debug.Log("Press any key to continue! (or press ESC to quit!)");
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
        dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_ENABLE);
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

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 멈춤
            // Disable Dynamixel Torque
            dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL_ID, ADDR_PRO_TORQUE_ENABLE, TORQUE_DISABLE);
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
       
        if (Input.GetKey(KeyCode.Space))
        {
            // Read present position
            dxl_present_position = (Int32)dynamixel.read4ByteTxRx(port_num, PROTOCOL_VERSION, DXL_ID, ADDR_PRO_PRESENT_POSITION);
                if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
                {
                    dynamixel.printTxRxResult(PROTOCOL_VERSION, dxl_comm_result);
                }
                else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
                {
                    dynamixel.printRxPacketError(PROTOCOL_VERSION, dxl_error);
                }

                Debug.Log("[ID: " + DXL_ID + " PresPos: " + dxl_present_position);

        }
    }
}
