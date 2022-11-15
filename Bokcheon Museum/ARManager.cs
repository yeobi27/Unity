using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARManager : MonoBehaviour
{
    //public GameObject AR_Cam;
    public GameObject[] collection;
    //private bool isFirstEntry = true;
    private float targetTime = 2f;
    private float currentTime = 0f;
    private bool isPopup = false;
    private bool isFirst = true;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ARManager Start");
    }

    private void OnEnable()
    {
        // 튜토리얼 패널 띄우기
        UIManager.Instance.tutorialPanel.SetActive(true);
        //UIManager.Instance.tutorialOffButton.onClick.AddListener(() => SetARCAM());
    }

    private void OnDisable()
    {
        //UIManager.Instance.tutorialOffButton.onClick.RemoveListener(() => SetARCAM());
        //AR_Cam.SetActive(false);
        // Completed
        UIManager.Instance.missMarkerPopup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFirst)
        {
            if (!UIManager.Instance.isKeepListening)
            {
                // 2초이상일때
                if (currentTime > targetTime && !isPopup)
                {
                    // 시간 초기화
                    currentTime = 0f;
                    // 팝업 띄우기
                    missMarkerPopupDelay();
                    // 팝업 상태 변경 체크
                    isPopup = true;
                }
                else if (!isPopup)
                {
                    currentTime += Time.deltaTime;
                }
            }
        }
    }

    // 이미지 찾았을 때
    public void OnFoundObject()
    {
        missMarkerDisappear();

        isFirst = false;

        // 상태변경
        isPopup = true;
        currentTime = 0f;
        
        Debug.Log("Found Target");
        Debug.Log("Select Target : " + UIManager.Instance.collection[UIManager.Instance.selectedCollection].collection);
        
        for (int i = 0; i < collection.Length; i++)
        {
            if (collection[i].name == UIManager.Instance.collection[UIManager.Instance.selectedCollection].collection)
            {
                Debug.Log("collection Name : "+ collection[i].name);

                UIManager.Instance.missMarkerPopup.SetActive(false);
                UIManager.Instance.guidebarObj.SetActive(false);
                collection[i].SetActive(true);
            }
        }

        UIManager.Instance.Enable_CollectionDescription();
    }

    // 이미지 잃어버렸을 때
    public void OnLostObject()
    {
        Debug.Log("Lost Target");
        isPopup = false;
    }

    private void missMarkerDisappear()
    {
        if (isFirst) { return; }

        Sequence seq;

        seq = DOTween.Sequence()
            .Append(UIManager.Instance.missMarkerPopup.transform.DOScale(new Vector3(0.0001f, 0.0001f, 0.0001f), 0.2f).SetEase(Ease.Unset))
            .OnComplete(() =>
            {
                // Completed
                UIManager.Instance.missMarkerPopup.SetActive(false);
            });
    }

    private void missMarkerPopupDelay()
    {
        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                UIManager.Instance.missMarkerPopup.SetActive(true);
                UIManager.Instance.missMarkerPopup.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
            })
            .Append(UIManager.Instance.missMarkerPopup.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Unset))
            .OnComplete(() =>
            {
                    // Completed
                    UIManager.Instance.missMarkerPopup.SetActive(true);
            });
    }
}
