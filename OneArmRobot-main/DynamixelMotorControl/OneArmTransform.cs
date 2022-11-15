using Leap.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class OneArmTransform : MonoBehaviour
{
    public GameObject LoPolyHandRight;
    private bool isSave_init_rate;   // 현재 각도 비율 저장 초기화시

    [Header("ID 11")]
    public Transform ID11;
    public float ID11_goal_rate;     // 현재 각도 비율

    [Header("ID 12")]
    public Transform ID12;
    public float ID12_goal_rate;


    [Header("ID 13")]
    public Transform ID13;
    public float ID13_goal_rate;

    [Header("ID 14")]
    public Transform palm;
    private float ID14_data;
    public float ID14_goal_rate;     // 현재 각도 비율

    [Header("ID 15")]
    public Transform left;    // 엄지
    public Transform right;    // 중지
    private float finger_distance;     // float 값으로 전달
    public float ID15_goal_rate;
    public static OneArmTransform Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Repeat_per_cycle", 0.1f);   // 모터의 주기를 100ms or 200ms 주기위한 코루틴 함수
    }

    IEnumerator Repeat_per_cycle(float sec)
    {
        WaitForSeconds ws = new WaitForSeconds(sec);

        while (true)
        {
            if (LoPolyHandRight.activeSelf)
            {
                #region Dynamixel ID 11
                /// <summary>
                /// 0 ~2048 ~4095
                /// 2048 디폴트
                /// UnityEditor.TransformUtils.GetInspectorRotation(ID11).y)
                /// </summary>
                ID11_goal_rate = (float)Mathf.InverseLerp(-90f, 90f, UnityEditor.TransformUtils.GetInspectorRotation(ID11).y);   // 목표위치 비율 설정

                #endregion

                #region Dynamixel ID 12
                /// <summary>
                /// 700 ~ 1800 ~ 3400
                /// 1800 디폴트
                /// </summary>

                ID12_goal_rate = (float)Mathf.InverseLerp(10f, -180f, UnityEditor.TransformUtils.GetInspectorRotation(ID12).x);   // 목표위치 비율 설정

                #endregion

                #region Dynamixel ID 13
                /// <summary>
                /// 700 ~ 1800 ~ 3400
                /// 1800 디폴트
                /// </summary>
                ID13_goal_rate = (float)Mathf.InverseLerp(100f, 0f, UnityEditor.TransformUtils.GetInspectorRotation(ID13).x);   // 목표위치 비율 설정

                #endregion

                #region Dynamixel ID 14

                /// <summary>
                /// 638 ~ 2676 ~ 3253
                /// 1748 디폴트
                /// </summary>

                ID14_data = Mathf.Clamp(Mathf.DeltaAngle(0, -palm.localEulerAngles.x), -50f, 40f);
                ID14_goal_rate = (float)Mathf.InverseLerp(40, -50f, ID14_data);

                #endregion

                #region Dynamixel ID 15
                /// <summary>
                /// 1460(닫음) ~ 2817(열림)
                /// 2817 디폴트
                /// </summary>

                //Debug.Log("left.position : " + left.position.x);
                //Debug.Log("right.position : " + right.position.x);

                finger_distance = (left.localPosition - right.localPosition).magnitude; // 손가락 사이 거리 계산

                ID15_goal_rate = (float)Mathf.InverseLerp(0.067f, 0.1092f, finger_distance);
                #endregion

                yield return ws;    // 위에서 주어진 new WaitForSeconds(sec) 초 만큼 지연시킴

            }
            yield return null;
        }
    }
}