using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ardunity;
using System;
using System.ComponentModel;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;
using System.IO;
using UnityEngine.Analytics;

[System.Serializable]
public class Ohm
{
    public AnalogInput[] inputList = new AnalogInput[10];
    public List<float> inputValues = new List<float>();

    public Ohm(ref AnalogInput[] _inputList)
    {
        inputList = (AnalogInput[])_inputList.Clone();

        inputValues.Add(_inputList[0].Value);
        inputValues.Add(_inputList[1].Value);
        inputValues.Add(_inputList[2].Value);
        inputValues.Add(_inputList[3].Value);
        inputValues.Add(_inputList[4].Value);
        inputValues.Add(_inputList[5].Value);
        inputValues.Add(_inputList[6].Value);
        inputValues.Add(_inputList[7].Value);
        inputValues.Add(_inputList[8].Value);
        inputValues.Add(_inputList[9].Value);
    }
}

public class GetValueManager : MonoBehaviour
{
    [Header("Analog Input")]
    public AnalogInput[] analoginputList;

    [Header("Input Sliders")]
    public Slider[] sliders;

    [Header("Input Value Text")]
    public TextMeshProUGUI[] textMeshProUGUI;

    [Header("Input Check List")]
    public Toggle[] toggles;

    [Header("Input Button List")]
    public Text[] inputBtnText;

    [Header("Time Left")]
    public float timeLeft = 10f;

    [Header("List Pool")]
    public List<Ohm> valuePool = new List<Ohm>();

    [Header("Stop Watch Text")]
    public Text stopWatch_text;

    [Header("Connect/DisConnect Button Text")]
    public Text ref_connect_disconnect_text;

    [Header("Input Field")]
    public TMP_InputField Limit_inputField;
    public TMP_InputField Period_inputField;

    //[Header("Set Button")]
    //public Button[] setButton;
    //public Text[] setButtonText;

    //// 세팅 값 저장
    //private float[] analoginputSetValues = new float[10];

    private float setTime = 0f;
    private float currentTime = 0f;
    private float periodTime = 0f;
    private float sum = 0f;
    private float avg = 0f;
    private string currentTimeString;

    private float Vin = 5f;
    private float Vout = 0f;
    private float ref_Resister = 10000f;
    private float measure_Resister;
    private float[] measure_Resisters = new float[10];
    private float length;
    private float[] pre_length = new float[10];

    private float temp_measure_Resister;
    private float temp_length;

    private string saveToExcel_path = "C:/CC_Checker/SaveToExcel/";
    private string stopWatchSaveToExcel_path = "C:/CC_Checker/StopWatchSaveToExcel/";
    //start is called before the first frame update
    void Start()
    {
        // 폴더 초기화
        if (!Directory.Exists(saveToExcel_path))
        {
            Directory.CreateDirectory(saveToExcel_path);
        }

        if (!Directory.Exists(stopWatchSaveToExcel_path))
        {
            Directory.CreateDirectory(stopWatchSaveToExcel_path);
        }
    }

    /// <summary>
    /// 힘이 가하지 않은 Relax 상태에서 350옴/인치, 혹은 138옴/cm의 저항값을 갖습니다. 
    /// 이 저항값은 길이에 대해서 선형적으로 증가합니다.고무줄을 당기면, 단위면적이 작아지고 이로인한 저항값이 증가하는 원리입니다.
    /// 예를 들어, 5인치의 고무줄을 7인치까지 당겼을 경우:
    /// 5인치는 약 1750옴입니다.
    /// 여기서 7인치로 증가시켰을 경우의 저항값은 대략 7/5*1750 = 2450옴이 됩니다. 
    /// 
    /// 얇은 고무줄은 1cm 에 2k옴 정도 한다.
    /// </summary>
    /// 
    // Update is called once per frame
    void Update()
    {
        if (ref_connect_disconnect_text.text.Equals("Disconnect"))
        {
            for (int i = 0; i < 10; i++) 
            {
                Vout = (Mathf.Lerp(1024f, 0f, analoginputList[i].Value) * Vin) / 1024;

                measure_Resister = (ref_Resister * ((Vin/Vout)-1)); 

                measure_Resisters[i] = measure_Resister;

                length = measure_Resister / 1800f; 

                if (inputBtnText[i].text.Equals("Input "+(i+1)+" Unset"))
                {
                    sliders[i].value = ((length / pre_length[i]) - 1) * 100;
                    textMeshProUGUI[i].text = (Math.Truncate((((length / pre_length[i]) - 1) * 100) * 10) / 10).ToString() + "%";
                }

                if (sliders[i].value == 0f || inputBtnText[i].text.Equals("Input " + (i + 1) + " Set"))
                {
                    toggles[i].isOn = false;
                }
                else
                {
                    toggles[i].isOn = true;
                }
            }
        }

    }

    public void SettingInputButtons(int num)
    {
        if (inputBtnText[num].text.Equals("Input " + (num + 1) + " Set"))
        {
            inputBtnText[num].text = "Input " + (num + 1) + " Unset";

            Vout = (Mathf.Lerp(1024f, 0f, analoginputList[num].Value) * Vin) / 1024;
            
            measure_Resister = (ref_Resister * ((Vin / Vout) - 1));

            measure_Resisters[num] = measure_Resister;

            length = measure_Resister / 1800f;

            pre_length[num] = length;
        }
        else
        {
            inputBtnText[num].text = "Input " + (num + 1) + " Set";
        }
    }

    public void SaveToExcel()
    {
        if (ref_connect_disconnect_text.text.Equals("Disconnect"))
        {
            Debug.Log("연결된 상태에서 엑셀뽑기");
            // 저항값
            float[] ohm_inputList = new float[analoginputList.Length];

            // 길이
            float[] length_inputList = new float[analoginputList.Length];

            // 변화율
            float[] increment = new float[analoginputList.Length];

            // 저항 입력
            for (int i = 0; i < analoginputList.Length; i++)
            {
                ohm_inputList[i] = ((float)(Math.Truncate(measure_Resisters[i] * 10f) / 10f));
            }

            // 길이 입력
            for (int i = 0; i < ohm_inputList.Length; i++)
            {
                length_inputList[i] = ((float)(Math.Truncate((measure_Resisters[i] / 1800f) * 10) / 10f));
            }

            // 변화율 입력
            for (int i = 0; i < analoginputList.Length; i++)
            {
                increment[i] = ((float)Math.Truncate((((length_inputList[i] / pre_length[i]) - 1) * 100) * 10) / 10);
            }

            //using (var writer = new CsvFileWriter("C:/Users/user/Desktop/CC_Checker_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv"))
            using (var writer = new CsvFileWriter("C:/CC_Checker/SaveToExcel/CC_Checker_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv"))
            {
                //List<string> columns = new List<string>() { "Ohm", "Thumb", "Index", "Middle", "Pinky", "Ring", "", "Length", "Thumb", "Index", "Middle", "Pinky", "Ring" };// making Index Row
                List<string> columns = new List<string>();

                // making Index Row : Ohm
                columns.Add("Ohm");
                for (int i = 0; i < ohm_inputList.Length; i++)
                {
                    if (ohm_inputList[i] != 0)
                    {
                        columns.Add("input" + (i + 1));
                    }
                }
                columns.Add("");

                // making Index Row : Length
                columns.Add("Increment");
                for (int i = 0; i < length_inputList.Length; i++)
                {
                    if (length_inputList[i] != 0)
                    {
                        columns.Add("I_input" + (i + 1));
                    }
                }

                writer.WriteRow(columns);
                columns.Clear();

                // 저항값 : Ohm
                columns.Add(""); // first column null
                for (int i = 0; i < ohm_inputList.Length; i++)
                {
                    if (ohm_inputList[i] != 0)
                    {
                        columns.Add(ohm_inputList[i].ToString());
                    }
                }
                columns.Add(""); // middle column null
                columns.Add(""); // middle column null

                // 길이값 : 변화율
                for (int i = 0; i < length_inputList.Length; i++)
                {
                    if (length_inputList[i] != 0)
                    {
                        columns.Add(increment[i].ToString() + "%");
                    }
                }

                writer.WriteRow(columns);
                columns.Clear();

                columns.Add("");
                writer.WriteRow(columns);
                columns.Clear();
                columns.Add("");
                writer.WriteRow(columns);
                columns.Clear();
                columns.Add("");
                writer.WriteRow(columns);
                columns.Clear();

                columns.Add("");
                columns.Add("Description");
                writer.WriteRow(columns);
                columns.Clear();

                columns.Add("");
                columns.Add("input1~10 : Record Resister Value / L_input1~10 : Record Length Value");
                writer.WriteRow(columns);
                columns.Clear();
            }
        }
    }

    public void StopWatch()
    {
        if (ref_connect_disconnect_text.text.Equals("Disconnect") && !Limit_inputField.text.Equals("") && !Period_inputField.text.Equals(""))   // 연결되어있는 상태 + 시간설정이 되어있음
        {
            if (!Limit_inputField.text.Equals("0") && !Period_inputField.text.Equals("0") && int.Parse(Limit_inputField.text) > int.Parse(Period_inputField.text))
            {
                Debug.Log("연결된 상태에서 엑셀뽑기 평균");
                if (stopWatch_text.text.Equals("Stop Watch"))
                {
                    // 비활성화
                    Limit_inputField.interactable = false;
                    Period_inputField.interactable = false;

                    StopAllCoroutines();
                    StartCoroutine(_StopWatch());
                }
                else
                {
                    StopAllCoroutines();

                    // 활성화
                    Limit_inputField.interactable = true;
                    Period_inputField.interactable = true;

                    currentTimeString = stopWatch_text.text;

                    // Text Change
                    stopWatch_text.text = "Stop Watch";
                    // setTime 처리
                    setTime = 0f;
                    currentTime = 0f;
                    // Pooling 처리
                    StopWatchSaveToExcel();
                }
            }
        }
    }

    IEnumerator _StopWatch()
    {
        timeLeft = int.Parse(Limit_inputField.text);
        periodTime = int.Parse(Period_inputField.text);
        currentTime = 0f;
        setTime = periodTime;   // 초기 주기적인 시간값
        
        WaitForSeconds ws = new WaitForSeconds(1f); // 주기적인 시간 : Period Time

        while (timeLeft > currentTime)  // 총 시간 : timeLeft
        {
            if (currentTime == setTime || currentTime == 0f)
            {
                valuePool.Add(new Ohm(ref analoginputList));   // 클래스내 AnalogInput Value 넣음
            }

            Debug.Log("currentTime : " + currentTime);

            stopWatch_text.text = currentTime.ToString() + "s";

            if (currentTime == setTime)
            {
                setTime += periodTime;  // 주기적인 시간 : Period Time
            }

            currentTime += 1f;
            yield return ws;
        }

        currentTimeString = stopWatch_text.text;
        stopWatch_text.text = "Stop Watch";
        setTime = 0f;
        currentTime = 0f;

        // 활성화
        Limit_inputField.interactable = true;
        Period_inputField.interactable = true;

        StopWatchSaveToExcel();

        yield return null;
    }

    public void StopWatchSaveToExcel()
    {
        //using (var writer = new CsvFileWriter("C:/Users/user/Desktop/CC_Checker_StopWatch_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv"))
        using (var writer = new CsvFileWriter("C:/CC_Checker/StopWatchSaveToExcel/CC_Checker_StopWatch_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv"))
        {
            //List<string> columns = new List<string>() { "Ohm", "Input1", "Input2", "Input3", "Input4", "Input5", "", "Length", "Input1", "Input2", "Input3", "Input4", "Input5" };// making Index Row
            List<string> columns = new List<string>();

            // making Index Row : Ohm
            columns.Add("Ohm");
            for (int i = 0; i < analoginputList.Length; i++)
            {
                columns.Add("input" + (i + 1));
            }
            columns.Add("");

            // making Index Row : Length
            columns.Add("Increment");
            for (int i = 0; i < analoginputList.Length; i++)
            {
                columns.Add("I_input" + (i + 1));
            }

            writer.WriteRow(columns);
            columns.Clear();

            // ListPool
            for (int i = 0; i < valuePool.Count; i++)
            {
                columns.Add((int.Parse(Period_inputField.text) * (i)) + "s"); // 시간대

                for (int j = 0; j < analoginputList.Length; j++)
                {
                    Vout = (Mathf.Lerp(1024f, 0f, valuePool[i].inputValues[j]) * Vin) / 1024;
                    //Debug.Log("Vout : " + Vout);
                    temp_measure_Resister = (ref_Resister * ((Vin / Vout) - 1));        // 측정저항값 
                    
                    columns.Add((Math.Truncate(temp_measure_Resister * 10f) / 10f).ToString());  // 저항
                    //Debug.LogError("i::::"+i+"j :::" +j+ ":::" + (valuePool[i].inputList[j].Value * 1000f).ToString());
                }

                columns.Add(""); // 띄우기

                columns.Add((int.Parse(Period_inputField.text) * (i)) + "s"); // 시간대

                for (int z = 0; z < analoginputList.Length; z++)
                {
                    Vout = (Mathf.Lerp(1024f, 0f, valuePool[i].inputValues[z]) * Vin) / 1024;

                    temp_measure_Resister = (ref_Resister * ((Vin / Vout) - 1));        // 측정저항값 

                    temp_length = temp_measure_Resister / 1800f;

                    Debug.Log("pre_length[" + z +"] : " + pre_length[z]);

                    columns.Add((Math.Truncate((((temp_length / pre_length[z]) - 1) * 100) * 10) / 10).ToString());    // 변화율
                }

                writer.WriteRow(columns);
                columns.Clear();
            }

            //timeLeft = int.Parse(Limit_inputField.text);
            //periodTime = int.Parse(Period_inputField.text);

            Debug.Log("analoginputList.Length : " + analoginputList.Length);
            Debug.Log("valuePool.Count : " + valuePool.Count);

            //columns.Add(currentTimeString + " / " + Period_inputField.text + "s"); // 시간대 및 주기
            columns.Add("Ohm Average"); // 시간대 및 주기

            //////////////////////////////////////////////////////////////////////////////////////////////
            // inputList Ohm Avg
            for (int i = 0; i < analoginputList.Length; i++)
            {
                for (int j = 0; j < valuePool.Count; j++)
                {
                    Vout = (Mathf.Lerp(1024f, 0f, valuePool[j].inputValues[i]) * Vin) / 1024;
                    //Debug.Log("Vout : " + Vout);
                    temp_measure_Resister = (ref_Resister * ((Vin / Vout) - 1));        // 측정저항값 

                    sum += temp_measure_Resister;
                }
                avg = sum / valuePool.Count;

                //Debug.LogError("sum :::" + sum);
                //Debug.LogError("avg :::" + avg);

                columns.Add((Math.Truncate(avg * 10f) / 10f).ToString());

                sum = 0f;
                avg = 0f;
            }
            
            columns.Add("");
            //columns.Add(currentTimeString + " / " + Period_inputField.text + "s"); // 시간대 및 주기
            columns.Add("Increment Average"); // 시간대 및 주기

            ///////////////////////////////////////////////////////////////////////////////////////////////////////
            // inputList Length Avg
            for (int i = 0; i < analoginputList.Length; i++)
            {
                for (int j = 0; j < valuePool.Count; j++)
                {
                    Vout = (Mathf.Lerp(1024f, 0f, valuePool[j].inputValues[i]) * Vin) / 1024;
                    //Debug.Log("Vout : " + Vout);
                    temp_measure_Resister = (ref_Resister * ((Vin / Vout) - 1));        // 측정저항값 

                    temp_length = temp_measure_Resister / 1800f;

                    sum += ((float)Math.Truncate((((temp_length / pre_length[i]) - 1) * 100) * 10) / 10);
                }
                avg = sum / valuePool.Count;
                columns.Add((Math.Truncate(avg * 10f) / 10f).ToString() + "%");

                sum = 0f;
                avg = 0f;
            }
            
            writer.WriteRow(columns);
            columns.Clear();

            columns.Add("");
            writer.WriteRow(columns);
            columns.Clear();
            columns.Add("");
            writer.WriteRow(columns);
            columns.Clear();
            columns.Add("");
            writer.WriteRow(columns);
            columns.Clear();

            columns.Add("");
            columns.Add("Description");
            writer.WriteRow(columns);
            columns.Clear();

            columns.Add("");
            columns.Add("input1~10 : Record Resister Value. / I_input1~10 : Record Increment Value.");
            writer.WriteRow(columns);
            columns.Clear();

            columns.Add("");
            columns.Add("Ohm/Increment Average : It is Ohm/Increment Average according to the Period.");
            writer.WriteRow(columns);
            columns.Clear();
        }

        valuePool.Clear();

    }

}
