using UnityEngine;

public static class DeviceRotation
{
    public static bool HasGyroscope
    {
        get
        {
            return SystemInfo.supportsGyroscope;
        }
    }

    public static Quaternion Get()
    {
        return HasGyroscope
                    ? ReadGyroscopeRotation()
                    : Quaternion.identity;
    }

    public static void InitGyro()
    {
        if (HasGyroscope)
        {
            Input.gyro.enabled = true;              // enable the gyroscope
            Input.gyro.updateInterval = 0.0167f;    // set the update interval to it's highest value (60 Hz)
        }
    }

    private static Quaternion ReadGyroscopeRotation()
    {
        return new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
    }
}