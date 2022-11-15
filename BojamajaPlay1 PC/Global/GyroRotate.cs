using UnityEngine;

public class GyroRotate : MonoBehaviour
{
    private Vector3 startRot;
    private float rot;
    private int axisDirection;
    private Quaternion referenceRotation;
    private Quaternion deviceRotation;
    private Quaternion eliminationOfXY;
    private Quaternion rotationZ;
    private float deltaRot;

    public enum RotateAxis
    {
        x, y, z
    }
    public enum Type
    {
        lerp,
        delta,
        deltaClamp,
        literal,
        literalClamp,
    }
    public RotateAxis rotateAxis;
    public Type type = Type.lerp;
    public float limit;
    public float speed = 3f;
    public bool switchDirection;

    void Awake()
    {
        DeviceRotation.InitGyro();
    }
    void Start()
    {
        if (Screen.sleepTimeout != SleepTimeout.NeverSleep)
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    void OnEnable()
    {
        startRot = transform.localEulerAngles;
    }
    void Update()
    {
        rotationZ = GetGyroRotation();
        switch (type)
        {
            case Type.lerp:
                {
                    // if (InDeadzone())
                    // {
                    //     if (lerpStabilize == true)
                    //     {
                    //         switch (rotateAxis)
                    //         {
                    //             case RotateAxis.x:
                    //                 transform.localEulerAngles = new Vector3(Mathf.LerpAngle(transform.localEulerAngles.x, startRot.x, speed * Time.deltaTime), 0f, 0f);
                    //                 break;
                    //             case RotateAxis.y:
                    //                 transform.localEulerAngles = new Vector3(0f, Mathf.LerpAngle(transform.localEulerAngles.y, startRot.y, speed * Time.deltaTime), 0f);
                    //                 break;
                    //             case RotateAxis.z:
                    //                 transform.localEulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(transform.localEulerAngles.z, startRot.z, speed * Time.deltaTime));
                    //                 break;
                    //         }
                    //     }
                    // }
                    // else
                    // {
                    //     if (rotationZ.eulerAngles.z < 180f)
                    //     {
                    //         switch (rotateAxis)
                    //         {
                    //             case RotateAxis.x:
                    //                 transform.localEulerAngles = new Vector3(Mathf.LerpAngle(transform.localEulerAngles.x, startRot.x - limit, speed * Time.deltaTime), 0f, 0f);
                    //                 break;
                    //             case RotateAxis.y:
                    //                 transform.localEulerAngles = new Vector3(0f, Mathf.LerpAngle(transform.localEulerAngles.y, startRot.y - limit, speed * Time.deltaTime), 0f);
                    //                 break;
                    //             case RotateAxis.z:
                    //                 transform.localEulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(transform.localEulerAngles.z, startRot.z - limit, speed * Time.deltaTime));
                    //                 break;
                    //         }
                    //     }
                    //     else
                    //     {
                    //         switch (rotateAxis)
                    //         {
                    //             case RotateAxis.x:
                    //                 transform.localEulerAngles = new Vector3(Mathf.LerpAngle(transform.localEulerAngles.x, startRot.x + limit, speed * Time.deltaTime), 0f, 0f);
                    //                 break;
                    //             case RotateAxis.y:
                    //                 transform.localEulerAngles = new Vector3(0f, Mathf.LerpAngle(transform.localEulerAngles.y, startRot.y + limit, speed * Time.deltaTime), 0f);
                    //                 break;
                    //             case RotateAxis.z:
                    //                 transform.localEulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(transform.localEulerAngles.z, startRot.z + limit, speed * Time.deltaTime));
                    //                 break;
                    //         }
                    //     }
                    // }
                    // transform.localEulerAngles *= switchDirection ? -1 : 1;
                }
                break;
            case Type.delta:
                {
                    if (rotationZ.eulerAngles.z < 180f)
                        deltaRot = rotationZ.eulerAngles.z / 180f;
                    else
                        deltaRot = ((360f - rotationZ.eulerAngles.z) / 180f) * -1f;

                    deltaRot *= speed * (switchDirection ? -1 : 1);
                    switch (rotateAxis)
                    {
                        case RotateAxis.x:
                            transform.localEulerAngles += new Vector3(deltaRot, 0f, 0f);
                            break;
                        case RotateAxis.y:
                            transform.localEulerAngles += new Vector3(0f, deltaRot, 0f);
                            break;
                        case RotateAxis.z:
                            transform.localEulerAngles += new Vector3(0f, 0f, deltaRot);
                            break;
                    }
                }
                break;
            case Type.deltaClamp:
                {
                    float angle = rotationZ.eulerAngles.z;
                    if (angle < 180f)
                    {
                        angle = ClampLHS();
                        deltaRot = angle / limit;
                    }
                    else if (angle > 180f)
                    {
                        angle = ClampRHS();
                        deltaRot = ((360f - angle) / limit) * -1f;
                    }

                    deltaRot *= speed * (switchDirection ? -1 : 1);
                    switch (rotateAxis)
                    {
                        case RotateAxis.x:
                            transform.localEulerAngles += new Vector3(deltaRot, 0f, 0f);
                            break;
                        case RotateAxis.y:
                            transform.localEulerAngles += new Vector3(0f, deltaRot, 0f);
                            break;
                        case RotateAxis.z:
                            transform.localEulerAngles += new Vector3(0f, 0f, deltaRot);
                            break;
                    }
                }
                break;
            case Type.literal:
                {
                    rot = rotationZ.eulerAngles.z * speed;
                    switch (rotateAxis)
                    {
                        case RotateAxis.x:
                            transform.localEulerAngles = new Vector3(rotationZ.eulerAngles.z, 0f, 0f);
                            break;
                        case RotateAxis.y:
                            transform.localEulerAngles = new Vector3(0f, rotationZ.eulerAngles.z, 0f);
                            break;
                        case RotateAxis.z:
                            transform.localEulerAngles = new Vector3(0f, 0f, rotationZ.eulerAngles.z); ;
                            break;
                    }
                    transform.localEulerAngles *= switchDirection ? -1 : 1;
                }
                break;
            case Type.literalClamp:
                {
                    float angle = rotationZ.eulerAngles.z;
                    if (angle < 180f)
                        angle = ClampLHS();
                    else if (angle > 180f)
                        angle = ClampRHS();

                    switch (rotateAxis)
                    {
                        case RotateAxis.x:
                            transform.localEulerAngles = new Vector3(angle, 0f, 0f);
                            break;
                        case RotateAxis.y:
                            transform.localEulerAngles = new Vector3(0f, angle, 0f);
                            break;
                        case RotateAxis.z:
                            transform.localEulerAngles = new Vector3(0f, 0f, angle);
                            break;
                    }
                    transform.localEulerAngles *= switchDirection ? -1 : 1;
                }
                break;
        }

    }

    private float ClampLHS()
    {
        return Mathf.Clamp(rotationZ.eulerAngles.z, 0, limit);
    }
    private float ClampRHS()
    {
        return Mathf.Clamp(rotationZ.eulerAngles.z, 360f - limit, 360f);
    }

    private Quaternion GetGyroRotation()
    {
        referenceRotation = Quaternion.identity;
        deviceRotation = DeviceRotation.Get();
        eliminationOfXY = Quaternion.Inverse(
            Quaternion.FromToRotation(referenceRotation * Vector3.forward,
                                      deviceRotation * Vector3.forward)
        );
        return eliminationOfXY * deviceRotation;
    }

    // private bool InDeadzone()
    // {
    //     if (lerpDeadzone > 0f)
    //     {
    //         if (rotationZ.eulerAngles.z < lerpDeadzone || 360f - lerpDeadzone < rotationZ.eulerAngles.z && rotationZ.eulerAngles.z > 180f)
    //         {
    //             return true;
    //         }
    //         else return false;
    //     }
    //     else return false;
    // }
}