using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UGS;
using System;

[System.Serializable]
public class Collection
{
    public int num;     // Nar_audioClips 에 들어간 배열번호랑 맟추기
    public string site;
    public string collection;

    public Collection() { }

    public Collection(int _num, string _collection)
    {
        num = _num;
        collection = _collection;
    }
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Background")]
    [SerializeField] Image background;
    [SerializeField] Sprite spt_mainbackground;
    [SerializeField] Sprite spt_exhibition_guide;
    [SerializeField] Sprite spt_information_use;
    [SerializeField] Sprite spt_aboutthemuseum;

    const float duration = 0.25f; // animation duration

    [Header("Object")]
    [SerializeField] GameObject mainBackGround;     // 메인 패널(AR도슨트, 전시안내, 이용안내, 박물관소개)
    [SerializeField] GameObject mainPopup;          // 설정, 언어설정, 로그인 패널 모음 오브젝트
    [SerializeField] GameObject homeButton;         // 홈 버튼
    [SerializeField] GameObject topMenu;            // 챗봇, 언어선택, 설정 
    [SerializeField] GameObject languageSettingButton;     // 언어선택버튼
    [SerializeField] GameObject exhibitionGuide;    // 전시안내 패널
    [SerializeField] GameObject informationUse;     // 이용안내 패널
    [SerializeField] GameObject aboutTheMuseum;     // 박물관 소개 패널
    [SerializeField] GameObject aboutTheMuseum_ScrollView;  // 박물관 소개 패널 안에 스크롤뷰
    [SerializeField] TMP_Text aboutTheMuseum_ScrollViewText;  // 박물관 소개 패널 안에 스크롤뷰안에 콘텐츠
    [SerializeField] GameObject aboutTheMuseum_ModifyInputField;    // 박물관 소개 패널 안에 스크롤뷰 수정을 위한 인풋필드
    [SerializeField] GameObject arDocent;           // 메인의 AR 도슨트 패널
    [SerializeField] GameObject chatBot;            // 챗봇 패널
    [SerializeField] GameObject arBackGround;       // AR 도슨트씬을 위한 AR 도슨트패널

    [Header("Popup")]
    [SerializeField] GameObject preferencesPanel_popup; // Fadeinout 을 위한 설정 팝업 패널
    [SerializeField] GameObject preferences_popup;      // 설정 팝업
    [SerializeField] GameObject languagePanel_popup;    // Fadeinout 을 위한 언어설정 팝업 패널
    [SerializeField] GameObject language_popup;         // 언어설정 팝업
    [SerializeField] GameObject login_popup;            // 로그인 팝업

    // CanvasGroup : Fadeinout 을 위한 캔버스 그룹
    private CanvasGroup exhibitionGuide_canvasGroup;
    private CanvasGroup informationUse_canvasGroup;
    private CanvasGroup aboutTheMuseum_canvasGroup;
    private CanvasGroup languageSetting_canvasGroup;
    private CanvasGroup preferencesSetting_canvasGroup;
    private CanvasGroup chatbot_canvasGroup;
    private CanvasGroup topMenu_canvasGroup;
    private CanvasGroup arDocent_canvasGroup;

    // RectTransform : 패널 이동을 위한 RectTrasform
    private RectTransform exhibitionGuideRT;
    private RectTransform informationUseRT;
    private RectTransform aboutTheMuseumRT;

    // OnComplete CallBack : 현재 패널&팝업 체크를 위한 Enum 변수
    private Panel currentPanel;
    private Popup currentPopup;
    public HistoricSites currentSite;
    public SitesDescription currentsitesDesc;
    
    // myTween isPlaying : 트윈 움직임 중일 때 중복클릭 방지를 위한 bool 값
    bool isPlaying;

    // HistoricSites Dictionary
    [Header("Historic Sites Obj")]
    [SerializeField] GameObject[] mapPin;       // 지도상에 위치를 선택했을 때 핀 하이라이트 작업을 위한 오브젝트
    [SerializeField] GameObject[] sites;        // 지도상 핀을 선택시 올라오는 간단설명 패널
    [SerializeField] GameObject blockPanel;     // 선택 시 레이캐스트를 통해서 내리기위해 만든 패널
    [SerializeField] Sprite spt_clickedmapPin;      // 맵핀 선택시 바뀌는 스프라이트
    [SerializeField] Sprite spt_unclickedmapPin;    // 선택되지 않았을 때 스프라이트

    private float pinPos;

    [Header("Sites Description Obj")]    
    [SerializeField] GameObject[] sitesDescription;     // 오디오 듣기 누르면 올라오는 자세한 고분 설명 패널
    [SerializeField] GameObject _backButton;            // 백버튼
    [SerializeField] GameObject chatbotSoundbar;        // 챗봇&사운드바

    // 현재 선택된 지역의 이름
    private string siteName;

    // 패널 관리를 위한 Enum & GameObject Dictionary
    Dictionary<HistoricSites, GameObject> historicSites;        
    Dictionary<SitesDescription, GameObject> historicSitesDesc;

    private GraphicRaycaster gr;    // 클릭 했을 때 알기 위한 레이캐스트

    [Header("Docent Simulation")]
    [Space(16)]
    public GameObject tutorialPanel;    // AR 도슨트 튜토리얼 패널
    public Button tutorialOffButton;    // 튜토리얼 끄기 버튼
    public GameObject guidebarObj;      // 도슨트 상 가이드바
    public GameObject missMarkerPopup;  // 오브젝트를 놓쳤을 때 띄우는 팝업
    public Button missMarkerButton;     // 오디오 그대로 듣기 위한 버튼
    [SerializeField] GameObject[] guideTextObj;         // 어떤 고분인지 안내해주는 패널
    [SerializeField] GameObject[] collectionTextObj;    // 어떤 유물인지 알려주는 패널    
    public bool isKeepListening = false;                // 계속 듣기 눌렀는지 체크해주는 bool 값

    [Header("Collection")]
    public Collection[] collection;                     // 유물들 저장 변수
    public int selectedCollection = -1;                 // 선택된 유물 초기화
    public GameObject collectionOptionPanel;            // 유물 선택 패널
    public GameObject collectionOption;                 // 유물 프리팹
    public Transform collectionOptionParent;            // 유물 버튼들 넣어두는 부모 오브젝트
    
    [Header("Loading UI")]
    [SerializeField] GameObject loading;                // 로딩씬 패널
    [SerializeField] CanvasGroup fade_img;              // 씬 넘어가는 부분의 Fadeinout 패널
    [SerializeField] TMP_Text loading_text;             // 퍼센트 텍스트
    
    float fadeDuration = 1f;                            //암전되는 시간

    [Header("Login Data")]
    [SerializeField] TMP_Text adminSettingButtonText;
    [SerializeField] TMP_InputField id;                 // 로그인 아이디
    [SerializeField] TMP_InputField pw;                 // 로그인 패스워드
    [SerializeField] GameObject alertPopup;             // 잘못 입력 했을 때 팝업
    [SerializeField] GameObject modifyPanel;            // 링크 수정을 위한 팝업 패널
    [SerializeField] GameObject modifyPopup;            // 링크 수정 팝업
    [SerializeField] TMP_Text popupTitle;               // 현재 수정하는 팝업창 제목
    [SerializeField] TMP_InputField categoryField;      // 수정할 입력필드 : 제목
    [SerializeField] TMP_InputField linkField;          // 수정할 입력필드 : 링크
                                                        // 
    [SerializeField] GameObject siteDescSummary_ToModify;   // // SiteDescSummary_ToModify : 간단한 지역 설명
    // Modify InputText : 자세한 고분설명
    [SerializeField] GameObject siteDescription_to_Modify;  // 수정할 입력필드 : 수정 패널
    [SerializeField] TMP_Text siteNameText;                     // 수정할 입력필드 : 고분 이름
    [SerializeField] TMP_InputField formationInputField;    // 수정할 입력필드 : 조성 시기
    [SerializeField] TMP_InputField excavationInputField;   // 수정할 입력필드 : 발굴 시기
    [SerializeField] TMP_InputField tombfeatureInputField;  // 수정할 입력필드 : 무덤 특징

    private int currentIndex = 0;                       // 수정하려고 선택한 패널에서의 순서 > 구글시트에 저장하기 위해서는 index가 필요함


    public enum Panel
    {
        Main,
        ExhibitionGuide,
        InformationUse,
        AboutTheMuseum,
        ARDocent
    }

    public enum Popup
    {
        Main,
        ChatBot,
        LanguageSetting,
        PreferencesSetting,
        AdminSetting
    }

    public enum HistoricSites
    {
        Main,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Eleven,
        Twelve,
        Thirteen
    }

    public enum SitesDescription
    {
        Main,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Eleven,
        Twelve,
        Thirteen
    }

    // Awake -> OnEnable -> SceneManager.sceneLoaded -> Start

    private void Awake()
    {
        if (Instance != null) { Destroy(this.gameObject); }
        else { Instance = this; }

        gr = GetComponent<GraphicRaycaster>(); 

        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += OnSceneLoaded; // 이벤트에 추가

        missMarkerButton.onClick.AddListener(() => isKeepListening = true);
    }

    // 로드 씬 이벤트 제거
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트에서 제거
    }

    // 씬 로드되면 콜백
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //if (SceneManager.GetActiveScene().name == "Main")
        //{
            //transform.GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        //}
        //else if (SceneManager.GetActiveScene().name == "ARDocent")
        //{
        //    transform.GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("ARCamera").GetComponent<Camera>();
        //}

        Sequence seq;

        seq = DOTween.Sequence()
            .Prepend(fade_img.DOFade(0, fadeDuration))
            .OnComplete(() =>
            {
                //로딩화면 띄우며, 씬 로드 시작
                fade_img.blocksRaycasts = false;
                loading.SetActive(false);
            });
    }

    void Start()
    {
        UIInitialize();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            if (results.Count <= 0) return;
            // 이벤트 처리부분
            //results[0].gameObject.transform.position = ped.position;

            // Graphic ray
            Debug.Log(results[0].gameObject.name);

            if (results[0].gameObject.name.Equals("BlockPanel") && currentSite != HistoricSites.Main && !isPlaying)
            {
                Disable_HistoricSite();
            }
        }
    }

    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);

    private void UIInitialize()
    {
        InitCanvasGroup();
        InitRectTransform();
        InitHistoricSites();
    }

    private void InitCanvasGroup()
    {
        #region 캔버스 그룹 초기화
        // 캔버스 그룹 초기화
        exhibitionGuide_canvasGroup = exhibitionGuide.GetComponent<CanvasGroup>();
        informationUse_canvasGroup = informationUse.GetComponent<CanvasGroup>();
        aboutTheMuseum_canvasGroup = aboutTheMuseum.GetComponent<CanvasGroup>();
       
        languageSetting_canvasGroup = languagePanel_popup.GetComponent<CanvasGroup>();
        preferencesSetting_canvasGroup = preferencesPanel_popup.GetComponent<CanvasGroup>();
        
        chatbot_canvasGroup = chatBot.GetComponent<CanvasGroup>();
        topMenu_canvasGroup = topMenu.GetComponent<CanvasGroup>();

        arDocent_canvasGroup = arDocent.GetComponent<CanvasGroup>();
        #endregion
    }

    private void InitRectTransform()
    {
        #region 메인 RectTransform 초기화
        exhibitionGuideRT = exhibitionGuide.GetComponent<RectTransform>();
        informationUseRT = informationUse.GetComponent<RectTransform>();
        aboutTheMuseumRT = aboutTheMuseum.GetComponent<RectTransform>();
        #endregion
    }

    private void InitHistoricSites()
    {
        #region 고분지역 딕셔너리 초기화
        historicSites = new Dictionary<HistoricSites, GameObject>();
        historicSitesDesc = new Dictionary<SitesDescription, GameObject>();

        historicSites.Add(HistoricSites.One, sites[0]);
        historicSites.Add(HistoricSites.Two, sites[1]);
        historicSites.Add(HistoricSites.Three, sites[2]);
        historicSites.Add(HistoricSites.Four, sites[3]);
        historicSites.Add(HistoricSites.Five, sites[4]);
        historicSites.Add(HistoricSites.Six, sites[5]);
        historicSites.Add(HistoricSites.Seven, sites[6]);
        historicSites.Add(HistoricSites.Eight, sites[7]);
        historicSites.Add(HistoricSites.Nine, sites[8]);
        historicSites.Add(HistoricSites.Ten, sites[9]);
        historicSites.Add(HistoricSites.Eleven, sites[10]);
        historicSites.Add(HistoricSites.Twelve, sites[11]);
        historicSites.Add(HistoricSites.Thirteen, sites[12]);

        historicSitesDesc.Add(SitesDescription.One, sitesDescription[0]);
        historicSitesDesc.Add(SitesDescription.Two, sitesDescription[1]);
        historicSitesDesc.Add(SitesDescription.Three, sitesDescription[2]);
        historicSitesDesc.Add(SitesDescription.Four, sitesDescription[3]);
        historicSitesDesc.Add(SitesDescription.Five, sitesDescription[4]);
        historicSitesDesc.Add(SitesDescription.Six, sitesDescription[5]);
        historicSitesDesc.Add(SitesDescription.Seven, sitesDescription[6]);
        historicSitesDesc.Add(SitesDescription.Eight, sitesDescription[7]);
        historicSitesDesc.Add(SitesDescription.Nine, sitesDescription[8]);
        historicSitesDesc.Add(SitesDescription.Ten, sitesDescription[9]);
        historicSitesDesc.Add(SitesDescription.Eleven, sitesDescription[10]);
        historicSitesDesc.Add(SitesDescription.Twelve, sitesDescription[11]);
        historicSitesDesc.Add(SitesDescription.Thirteen, sitesDescription[12]);
        #endregion
    }

    public void HomeButtonClick()
    {
        Debug.Log(currentPanel);

        switch (currentPanel)
        {
            case Panel.ExhibitionGuide:
                Disable_ExhibitionGuide();
                break;
            case Panel.InformationUse:
                Disable_InformationUse();
                break;
            case Panel.AboutTheMuseum:
                Disable_AboutTheMuseum();
                break;
            case Panel.ARDocent:
                Disable_ARDocent();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 모든 버튼 관리
    /// </summary>
    // 언어설정 켜기
    public void Enable_Admin_Login()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        if (DataManager.Instance.loginIn)
        {
            Logout();
        }

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                login_popup.SetActive(true);
                login_popup.transform.localScale = new Vector3(0.0001f,0.0001f,0.0001f);
            })
            .Append(login_popup.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.Unset))
            .OnComplete(() =>
            {
                // Completed
                currentPopup = Popup.AdminSetting;
                isPlaying = false;
            });
    }

    public void Disable_Admin_Login()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .Append(login_popup.transform.DOScale(new Vector3(0.0001f, 0.0001f, 0.0001f), 0.3f).SetEase(Ease.Unset))
            .OnComplete(() =>
            {
                // Completed
                currentPopup = Popup.Main;
                login_popup.SetActive(false);
                isPlaying = false;
            });
    }

    public void Enable_Popup_LanguageSetting()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                languagePanel_popup.SetActive(true);
                language_popup.transform.localScale = new Vector3(0.0001f,0.0001f, 0.0001f);
                languageSetting_canvasGroup.alpha = 0f;
            })
            .Append(language_popup.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.Unset))
            .Join(languageSetting_canvasGroup.DOFade(1, 0.3f))
            .OnComplete(() =>
            {
                // Completed
                currentPopup = Popup.LanguageSetting;
                isPlaying = false;
            });     
    }

    // 언어설정 끄기
    public void Disable_Popup_LanguageSetting()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .Append(language_popup.transform.DOScale(new Vector3(0.0001f,0.0001f, 0.0001f), 0.3f).SetEase(Ease.Unset))
            .Join(languageSetting_canvasGroup.DOFade(0, 0.3f))
            .OnComplete(() =>
            {
                // Completed
                currentPopup = Popup.Main;
                languagePanel_popup.SetActive(false);
                isPlaying = false;
            });
    }

    // 설정 켜기
    public void Enable_Popup_PreferencesSetting()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                preferencesPanel_popup.SetActive(true);
                preferences_popup.transform.localScale = new Vector3(0.0001f, 0.0001f,0.0001f);
                preferencesSetting_canvasGroup.alpha = 0f;
            })
            .Append(preferences_popup.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.Unset))
            .Join(preferencesSetting_canvasGroup.DOFade(1, 0.3f))
            .OnComplete(() =>
            {
                // Completed
                currentPopup = Popup.PreferencesSetting;
                isPlaying = false;
            });
    }

    // 설정 끄기
    public void Disable_Popup_PreferencesSetting()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .Append(preferences_popup.transform.DOScale(new Vector3(0.0001f,0.0001f, 0.0001f), 0.3f).SetEase(Ease.Unset))
            .Join(preferencesSetting_canvasGroup.DOFade(0, 0.3f))
            .OnComplete(() =>
            {
                // Completed
                currentPopup = Popup.Main;
                preferencesPanel_popup.SetActive(false);
                isPlaying = false;
            });
    }

    /// <summary>
    /// 모든 탭 관리
    /// </summary>
    // 전시 안내 켜기
    public void Enable_ExhibitionGuide()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                exhibitionGuide.SetActive(true);
                homeButton.SetActive(true);
                exhibitionGuide_canvasGroup.alpha = 0f;
                Crossfade(spt_exhibition_guide);
            })
            .Append(exhibitionGuideRT.DOAnchorPosY(0, 1f).SetEase(Ease.Unset))
            .Join(exhibitionGuide_canvasGroup.DOFade(1, 0.3f))
            .OnComplete(() =>
            {
                // Completed
                currentPanel = Panel.ExhibitionGuide;
                isPlaying = false;
            });
    }

    // 전시 안내 끄기
    public void Disable_ExhibitionGuide()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                homeButton.SetActive(false);
                Crossfade(spt_mainbackground);
            })
            .Append(exhibitionGuideRT.DOAnchorPosY(-1677, 1f).SetEase(Ease.Unset))
            .Join(exhibitionGuide_canvasGroup.DOFade(0, 0.5f))
            .OnComplete(() =>
            {
                // Completed
                currentPanel = Panel.Main;
                exhibitionGuide.SetActive(false);
                isPlaying = false;
            });
    }
    // 이용안내 켜기
    public void Enable_InformationUse()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                informationUse.SetActive(true);
                homeButton.SetActive(true);
                informationUse_canvasGroup.alpha = 0f;
                Crossfade(spt_information_use);
            })
            .Append(informationUseRT.DOAnchorPosY(0, 1f).SetEase(Ease.Unset))
            .Join(informationUse_canvasGroup.DOFade(1, 0.3f))
            .OnComplete(() =>
            {
                // Completed
                currentPanel = Panel.InformationUse;
                isPlaying = false;
            });
    }

    // 이용안내 끄기
    public void Disable_InformationUse()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                homeButton.SetActive(false);
                Crossfade(spt_mainbackground);
            })
            .Append(informationUseRT.DOAnchorPosY(-782f, 1f).SetEase(Ease.Unset))
            .Join(informationUse_canvasGroup.DOFade(0, 0.5f))
            .OnComplete(() =>
            {
                // Completed
                currentPanel = Panel.Main;
                informationUse.SetActive(false);
                isPlaying = false;
            });
    }    
    // 박물관 안내 켜기
    public void Enable_AboutTheMuseum()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                aboutTheMuseum.SetActive(true);

                if (DataManager.Instance.loginIn)
                {
                    // 관리자 모드
                    aboutTheMuseum_ScrollView.SetActive(false);
                    aboutTheMuseum_ModifyInputField.SetActive(true);

                    // 데이터 매니저에서 가져옴
                    aboutTheMuseum_ModifyInputField.GetComponent<TMP_InputField>().text = DataManager.Instance.aboutTheMuseum.contents;
                }
                else
                {
                    // 사용자
                    aboutTheMuseum_ScrollView.SetActive(true);
                    aboutTheMuseum_ModifyInputField.SetActive(false);

                    // 데이터 매니저에서 가져옴
                    aboutTheMuseum_ScrollViewText.text = DataManager.Instance.aboutTheMuseum.contents;
                }

                homeButton.SetActive(true);
                aboutTheMuseum_canvasGroup.alpha = 0f;
                Crossfade(spt_aboutthemuseum);
            })
            .Append(aboutTheMuseumRT.DOAnchorPosY(0, 1f).SetEase(Ease.Unset))
            .Join(aboutTheMuseum_canvasGroup.DOFade(1, 0.3f))
            .OnComplete(() =>
            {
                // Completed
                currentPanel = Panel.AboutTheMuseum;
                isPlaying = false;
            });
    }

    // 박물관 안내 끄기
    public void Disable_AboutTheMuseum()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                homeButton.SetActive(false);
                Crossfade(spt_mainbackground);
            })
            .Append(aboutTheMuseumRT.DOAnchorPosY(-1703f, 1f).SetEase(Ease.Unset))
            .Join(aboutTheMuseum_canvasGroup.DOFade(0, 0.5f))
            .OnComplete(() =>
            {
                // Completed
                currentPanel = Panel.Main;

                if (DataManager.Instance.loginIn)
                {
                    // 관리자 모드
                    aboutTheMuseum_ModifyInputField.SetActive(false);
                }
                else
                {
                    // 사용자
                    aboutTheMuseum_ScrollView.SetActive(false);
                }

                aboutTheMuseum.SetActive(false);
                isPlaying = false;
            });
    }
    
    public void Enable_ChatBot()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        // 도슨트 씬이면 , 탑메뉴가 아예 없음
        if (SceneManager.GetActiveScene().name == "ARDocent")
        {
            seq = DOTween.Sequence()
                .OnStart(() =>
                {
                    chatBot.SetActive(true);
                    chatbot_canvasGroup.alpha = 0f;
                })
                .Append(chatbot_canvasGroup.DOFade(1, 0.3f))
                //.Join(topMenu_canvasGroup.DOFade(0, 0.3f))
                .OnComplete(() =>
                {
                    // Completed
                    currentPopup = Popup.ChatBot;
                    //topMenu.SetActive(false);
                    isPlaying = false;
                });
        }
        else
        {
            seq = DOTween.Sequence()
                .OnStart(() =>
                {
                    chatBot.SetActive(true);
                    chatbot_canvasGroup.alpha = 0f;
                })
                .Append(chatbot_canvasGroup.DOFade(1, 0.3f))
                .Join(topMenu_canvasGroup.DOFade(0, 0.3f))
                .OnComplete(() =>
                {
                    // Completed
                    currentPopup = Popup.ChatBot;
                    topMenu.SetActive(false);
                    isPlaying = false;
                });
        }        
    } 

    public void Disable_ChatBot()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        if (currentsitesDesc == SitesDescription.Main)
        {
            // Main Panel 에서의 상태
            seq = DOTween.Sequence()
                .Append(topMenu_canvasGroup.DOFade(1, 0.5f))
                .Join(chatbot_canvasGroup.DOFade(0, 0.5f))
                .OnComplete(() =>
                {
                    // Completed
                    currentPopup = Popup.Main;
                    chatBot.SetActive(false);

                    topMenu.SetActive(true);

                    isPlaying = false;
                });
        }
        else
        {
            // 고분자세히설명란에서 챗봇에 들어간 후 나오면 탑메뉴 안생기게 해야함
            // 자세히 보기 아닌 모든 상태
            seq = DOTween.Sequence()
                .Prepend(chatbot_canvasGroup.DOFade(0, 0.5f))
                .OnComplete(() =>
                {
                    // Completed
                    currentPopup = Popup.Main;
                    chatBot.SetActive(false);

                    isPlaying = false;
                });
        }
    }

    public void Enable_ARDocent()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                arDocent.SetActive(true);
                homeButton.SetActive(true);
                arDocent_canvasGroup.alpha = 0f;
                //Crossfade(spt_exhibition_guide);
            })
            .Append(arDocent_canvasGroup.DOFade(1, 0.3f))
            .OnComplete(() =>
            {
                // Completed
                currentPanel = Panel.ARDocent;
                isPlaying = false;
            });
    }

    public void Disable_ARDocent()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                homeButton.SetActive(false);
                arDocent_canvasGroup.alpha = 0f;
            })
            .Append(arDocent_canvasGroup.DOFade(0, 0.3f))
            .OnComplete(() =>
            {
                // Completed
                currentPanel = Panel.Main;
                arDocent.SetActive(false);
                isPlaying = false;
            });
    }

    Tween loopween;
    // 고분 선택시 간단 설명 올리기
    public void Enable_HistoricSite(int _site)
    {
        if (currentSite == HistoricSites.Main && !blockPanel.activeSelf)
        {
            if (isPlaying) { return; }

            isPlaying = true;

            if (DataManager.Instance.loginIn)
            {
                // 관리자 로그인

                Sequence seq;
                Color color;

                // 맵핀 반복 하이라이트
                pinPos = mapPin[_site - 1].transform.localPosition.y;
                mapPin[_site - 1].GetComponent<Image>().sprite = spt_clickedmapPin;
                loopween = mapPin[_site - 1].transform.DOLocalMoveY(pinPos + 20, 1).SetLoops(-1, LoopType.Yoyo);

                seq = DOTween.Sequence()
                    .OnStart(() =>
                    {
                        blockPanel.SetActive(true);
                        color = blockPanel.GetComponent<Image>().color;
                        color.a = 0f;
                        blockPanel.GetComponent<Image>().color = color;

                        siteDescSummary_ToModify.SetActive(true);

                        // 고분 타이틀 들고오기
                        siteDescSummary_ToModify.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = siteName;
                        // 고분 간단 설명 들고오기
                        siteDescSummary_ToModify.transform.GetChild(3).gameObject.GetComponent<TMP_InputField>().text = DataManager.Instance.historicDescription_Summaries[_site - 1].summary;

                        siteDescSummary_ToModify.GetComponent<CanvasGroup>().alpha = 0f;
                    })
                    .Prepend(blockPanel.GetComponent<Image>().DOFade(0.5f, 0.5f).SetEase(Ease.Unset))
                    .Append(siteDescSummary_ToModify.GetComponent<RectTransform>().DOAnchorPosY(-752f, 0.5f).SetEase(Ease.Unset))
                    .Join(siteDescSummary_ToModify.GetComponent<CanvasGroup>().DOFade(1, 0.4f))
                    .OnComplete(() =>
                    {
                        // Completed
                        currentSite = (HistoricSites)_site;
                        isPlaying = false;
                    });
            }
            else
            {
                Sequence seq;
                Color color;

                // 맵핀 반복 하이라이트
                pinPos = mapPin[_site - 1].transform.localPosition.y;
                mapPin[_site - 1].GetComponent<Image>().sprite = spt_clickedmapPin;
                loopween = mapPin[_site - 1].transform.DOLocalMoveY(pinPos + 20, 1).SetLoops(-1, LoopType.Yoyo);

                seq = DOTween.Sequence()
                    .OnStart(() =>
                    {
                        blockPanel.SetActive(true);
                        color = blockPanel.GetComponent<Image>().color;
                        color.a = 0f;
                        blockPanel.GetComponent<Image>().color = color;

                        historicSites[(HistoricSites)_site].SetActive(true);

                        // 텍스트에 데이터매니저에서 가져온 값 넣기
                        sites[_site - 1].transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = DataManager.Instance.historicDescription_Summaries[_site - 1].summary;

                        historicSites[(HistoricSites)_site].GetComponent<CanvasGroup>().alpha = 0f;
                    })
                    .Prepend(blockPanel.GetComponent<Image>().DOFade(0.5f, 0.5f).SetEase(Ease.Unset))
                    .Append(historicSites[(HistoricSites)_site].GetComponent<RectTransform>().DOAnchorPosY(-752f, 0.5f).SetEase(Ease.Unset))
                    .Join(historicSites[(HistoricSites)_site].GetComponent<CanvasGroup>().DOFade(1, 0.4f))
                    .OnComplete(() =>
                    {
                        // Completed
                        currentSite = (HistoricSites)_site;
                        isPlaying = false;
                    });
            }
        }
    }
    // 고분 선택시 간단 설명 내리기
    public void Disable_HistoricSite()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        if (DataManager.Instance.loginIn)
        {
            // 관리자 모드
            Sequence seq;

            // 맵핀 반복 쥬김
            loopween.Kill();
            mapPin[(int)currentSite - 1].GetComponent<Image>().sprite = spt_unclickedmapPin;

            seq = DOTween.Sequence()
                .Prepend(siteDescSummary_ToModify.GetComponent<RectTransform>().DOAnchorPosY(-1832f, 0.5f).SetEase(Ease.Unset))
                .Join(siteDescSummary_ToModify.GetComponent<CanvasGroup>().DOFade(0, 0.4f))
                .Join(mapPin[(int)currentSite - 1].transform.DOLocalMoveY(pinPos, 1))
                .Append(blockPanel.GetComponent<Image>().DOFade(0f, 0.5f).SetEase(Ease.Unset))
                .OnComplete(() =>
                {
                    blockPanel.SetActive(false);
                    siteDescSummary_ToModify.SetActive(false);
                    currentSite = HistoricSites.Main;

                    isPlaying = false;
                });
        }
        else
        {
            Sequence seq;

            loopween.Kill();
            mapPin[(int)currentSite - 1].GetComponent<Image>().sprite = spt_unclickedmapPin;

            seq = DOTween.Sequence()
                .Prepend(historicSites[currentSite].GetComponent<RectTransform>().DOAnchorPosY(-1832f, 0.5f).SetEase(Ease.Unset))
                .Join(historicSites[currentSite].GetComponent<CanvasGroup>().DOFade(0, 0.4f))
                .Join(mapPin[(int)currentSite - 1].transform.DOLocalMoveY(pinPos, 1))
                .Append(blockPanel.GetComponent<Image>().DOFade(0f, 0.5f).SetEase(Ease.Unset))
                .OnComplete(() =>
                {
                    blockPanel.SetActive(false);
                    historicSites[currentSite].SetActive(false);
                    currentSite = HistoricSites.Main;

                    isPlaying = false;
                });
        }
    }

    public void SitesDescSummary_ModifyButton()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        // 데이터 매니저에 저장
        DataManager.Instance.historicDescription_Summaries[(int)currentSite - 1].summary = siteDescSummary_ToModify.transform.GetChild(3).gameObject.GetComponent<TMP_InputField>().text;

        // 구글 시트에 저장
        var updateData = new BCM_docent_Info.HistoricSites_Summary();
        // 업데이트 시키려면 순서대로 들어가야함
        updateData.index = (int)currentSite - 1;
        updateData.sites = siteName;
        updateData.summary = siteDescSummary_ToModify.transform.GetChild(3).gameObject.GetComponent<TMP_InputField>().text;

        UnityGoogleSheet.Write(updateData);

        // 관리자 모드
        Sequence seq;

        // 맵핀 반복 쥬김
        loopween.Kill();
        mapPin[(int)currentSite - 1].GetComponent<Image>().sprite = spt_unclickedmapPin;

        seq = DOTween.Sequence()
            .Prepend(siteDescSummary_ToModify.GetComponent<RectTransform>().DOAnchorPosY(-1832f, 0.5f).SetEase(Ease.Unset))
            .Join(siteDescSummary_ToModify.GetComponent<CanvasGroup>().DOFade(0, 0.4f))
            .Join(mapPin[(int)currentSite - 1].transform.DOLocalMoveY(pinPos, 1))
            .Append(blockPanel.GetComponent<Image>().DOFade(0f, 0.5f).SetEase(Ease.Unset))
            .OnComplete(() =>
            {
                blockPanel.SetActive(false);
                siteDescSummary_ToModify.SetActive(false);
                currentSite = HistoricSites.Main;

                isPlaying = false;
            });
    }

    // 고분 이름 가져오기
    public void GetSiteName(string _siteName)
    {
        siteName = _siteName;
    }

    // 자세히 보기 올리기
    public void Enable_SiteDescription(int _site)
    {
        if (isPlaying) { return; }

        isPlaying = true;
        currentsitesDesc = (SitesDescription)_site;

        //// Modify InputText : 자세한 고분설명
        //[SerializeField] GameObject siteDescription_to_Modify;  // 수정할 입력필드 : 수정 패널
        //[SerializeField] TMP_Text siteNameText;                 // 수정할 입력필드 : 고분 이름
        //[SerializeField] TMP_InputField formationInputField;    // 수정할 입력필드 : 조성 시기
        //[SerializeField] TMP_InputField excavationInputField;   // 수정할 입력필드 : 발굴 시기
        //[SerializeField] TMP_InputField tombfeatureInputField;  // 수정할 입력필드 : 무덤 특징

        if (DataManager.Instance.loginIn)
        {
            // 관리자 모드일 때
            Sequence seq;
            Color color;

            seq = DOTween.Sequence()
                .OnStart(() =>
                {
                    siteDescription_to_Modify.SetActive(true); // 고분 자세히 오브젝트 활성화

                    // 입력텍스트에 텍스트 기입
                    siteNameText.text = siteName;
                    formationInputField.text = DataManager.Instance.historicDescription[_site - 1].formationPeriod;
                    excavationInputField.text = DataManager.Instance.historicDescription[_site - 1].excavationPeriod;
                    tombfeatureInputField.text = DataManager.Instance.historicDescription[_site - 1].tombFeature;

                    chatbotSoundbar.SetActive(true);

                    _backButton.SetActive(true);
                    color = _backButton.GetComponent<Image>().color;
                    color.a = 0f;
                    _backButton.GetComponent<Image>().color = color;
                })
                .Prepend(siteDescription_to_Modify.GetComponent<RectTransform>().DOAnchorPosY(0f, 1f).SetEase(Ease.Unset))
                .Join(siteDescription_to_Modify.GetComponent<CanvasGroup>().DOFade(1, 0.6f))
                .Join(chatbotSoundbar.GetComponent<RectTransform>().DOAnchorPosY(-2, 1f).SetEase(Ease.Unset))
                .Join(topMenu_canvasGroup.DOFade(0, 0.6f))
                .Append(_backButton.GetComponent<Image>().DOFade(1, 0.6f))
                .OnComplete(() =>
                {
                    topMenu.SetActive(false);
                    isPlaying = false;
                });
        }
        else
        {
            Sequence seq;
            Color color;

            seq = DOTween.Sequence()
                .OnStart(() =>
                {
                    historicSitesDesc[(SitesDescription)_site].SetActive(true); // 고분 자세히 오브젝트 활성화

                    GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Contents");   // Contents Tag 가 있는 게임오브젝트들을 순서대로 가져온다.
                    
                    // 0:조성시기, 1:발굴시기, 2:무덤특징
                    gameObjects[0].GetComponent<TMP_Text>().text = DataManager.Instance.historicDescription[_site - 1].formationPeriod;
                    gameObjects[1].GetComponent<TMP_Text>().text = DataManager.Instance.historicDescription[_site - 1].excavationPeriod;
                    gameObjects[2].GetComponent<TMP_Text>().text = DataManager.Instance.historicDescription[_site - 1].tombFeature;
                    gameObjects[3].GetComponent<TMP_Text>().text = DataManager.Instance.historicDescription[_site - 1].historicalWorth;
                    gameObjects[4].GetComponent<TMP_Text>().text = DataManager.Instance.historicDescription[_site - 1].mainCollection;
                    gameObjects[5].GetComponent<TMP_Text>().text = DataManager.Instance.historicDescription[_site - 1].location;

                    // Fit Alignment
                    Fit(gameObjects[0].GetComponent<RectTransform>());  // FormationPeriod
                    Fit(gameObjects[1].GetComponent<RectTransform>());  // ExcavationPeriod
                    Fit(gameObjects[2].GetComponent<RectTransform>());  // TombFeature
                    Fit(gameObjects[3].GetComponent<RectTransform>());  // HistoricalWorth
                    Fit(gameObjects[4].GetComponent<RectTransform>());  // MainCollection
                    Fit(gameObjects[5].GetComponent<RectTransform>());  // Location
                    Fit(gameObjects[6].GetComponent<RectTransform>());  // Content Rect

                    chatbotSoundbar.SetActive(true);

                    _backButton.SetActive(true);
                    color = _backButton.GetComponent<Image>().color;
                    color.a = 0f;
                    _backButton.GetComponent<Image>().color = color;
                })
                .Prepend(historicSitesDesc[(SitesDescription)_site].GetComponent<RectTransform>().DOAnchorPosY(0f, 1f).SetEase(Ease.Unset))
                .Join(historicSitesDesc[(SitesDescription)_site].GetComponent<CanvasGroup>().DOFade(1, 0.6f))
                .Join(chatbotSoundbar.GetComponent<RectTransform>().DOAnchorPosY(-2, 1f).SetEase(Ease.Unset))
                .Join(topMenu_canvasGroup.DOFade(0, 0.6f))
                .Append(_backButton.GetComponent<Image>().DOFade(1, 0.6f))
                .OnComplete(() =>
                {
                    topMenu.SetActive(false);
                    isPlaying = false;
                });
        }
    }

    // 자세히 보기 내리기 >> 나레이션 초기화
    public void Disable_SiteDescription()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        SoundManager.Instance.StopNAR();

        if (DataManager.Instance.loginIn)
        {
            // 관리자 모드일 때 처리
            siteName = "Main";

            Sequence seq;

            seq = DOTween.Sequence()
                .Prepend(siteDescription_to_Modify.GetComponent<RectTransform>().DOAnchorPosY(-903f, 1f).SetEase(Ease.Unset))
                .Join(siteDescription_to_Modify.GetComponent<CanvasGroup>().DOFade(0, 0.6f))
                .Join(chatbotSoundbar.GetComponent<RectTransform>().DOAnchorPosY(-903f, 1f).SetEase(Ease.Unset))
                .Join(topMenu_canvasGroup.DOFade(1, 0.6f))
                .Join(_backButton.GetComponent<Image>().DOFade(0, 0.6f))
                .OnComplete(() =>
                {
                    // 고분 간단 설명란
                    siteDescription_to_Modify.SetActive(false);
                    chatbotSoundbar.SetActive(false);
                    topMenu.SetActive(true);
                    _backButton.SetActive(false);
                    currentsitesDesc = SitesDescription.Main;
                    isPlaying = false;
                });
        }
        else
        {
            Sequence seq;

            seq = DOTween.Sequence()
                .Prepend(historicSitesDesc[currentsitesDesc].GetComponent<RectTransform>().DOAnchorPosY(-903f, 1f).SetEase(Ease.Unset))
                .Join(historicSitesDesc[currentsitesDesc].GetComponent<CanvasGroup>().DOFade(0, 0.6f))
                .Join(chatbotSoundbar.GetComponent<RectTransform>().DOAnchorPosY(-903f, 1f).SetEase(Ease.Unset))
                .Join(topMenu_canvasGroup.DOFade(1, 0.6f))
                .Join(_backButton.GetComponent<Image>().DOFade(0, 0.6f))
                .OnComplete(() =>
                {
                    // 고분 간단 설명란
                    historicSitesDesc[currentsitesDesc].SetActive(false);
                    chatbotSoundbar.SetActive(false);
                    topMenu.SetActive(true);
                    _backButton.SetActive(false);
                    currentsitesDesc = SitesDescription.Main;
                    isPlaying = false;
                });
        }
    }

    // Fade in / out background
    public void Crossfade(Sprite _to)
    {
        background.DOKill(); // It's a good idea to kill off previous animations
        background.DOCrossfadeImage(_to, duration, () =>
        {
            //A callback for when complete. Just a hacky way of getting a callback.
            // NOTE: Will not be called if killed.
            //Debug.Log("Crossfade animation completed successfully");
        });
    }

    public void SelectedCollection(int _collectionNum)
    {
        Debug.Log("_collectionNum : " + _collectionNum);

        selectedCollection = _collectionNum;

        ChangeARDocentScene();
    }

    // 고분에 맞는 유물 선택
    public void Enable_CollectionSelectOption(string _site)
    {
        Debug.Log("isPlaying: " + isPlaying);
        Debug.Log("_site: " + _site);

        if (isPlaying) { return; }

        isPlaying = true;

        // 유물 선택 패널 활성화
        collectionOptionPanel.SetActive(true);

        // 선택한 고분에 맞는 유물 갯수대로 프리팹 생성
        for (int i = 0; i < collection.Length; i++)
        {
            if (collection[i].site == _site)    
            {
                int seletedNum = i; // 0 ~ 15

                GameObject go = Instantiate(collectionOption, collectionOptionParent);

                if (collection[i].collection == "도기 거북장식 원통형 기대 및 단경호")
                {
                    go.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().fontSize = 65;
                    go.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = collection[i].collection;
                }
                else
                {
                    go.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = collection[i].collection;
                }
                

                go.GetComponent<Button>().onClick.AddListener(() => SelectedCollection(seletedNum));
            }
        }

        Sequence seq;

        seq = DOTween.Sequence()
            .Prepend(collectionOptionPanel.GetComponent<CanvasGroup>().DOFade(1, 0.3f))
            .Join(topMenu_canvasGroup.DOFade(0, 0.3f))
            .OnComplete(() =>
            {
                isPlaying = false;
            });
    }

    // 유물선택에서 뒤로가기
    public void Disable_CollectionSelectOption()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        // 버튼들 없애기
        Transform[] childList = collectionOptionParent.GetComponentsInChildren<Transform>(true);
        if (childList != null)
        {
            for (int i = 2; i < childList.Length; i++)
            {
                if (childList[i] != transform)
                    Destroy(childList[i].gameObject);
            }
        }

        // 탑메뉴, 메인팝업 살리기

        Sequence seq;

        seq = DOTween.Sequence()
            .Prepend(collectionOptionPanel.GetComponent<CanvasGroup>().DOFade(0, 0.3f))
            .Join(topMenu_canvasGroup.DOFade(1, 0.3f))
            .OnComplete(() =>
            {
                collectionOptionPanel.SetActive(false);
                isPlaying = false;
            });
    }

    // 씬 바뀌기전에 유물이름 들고감
    public void ChangeARDocentScene()
    {
        ////////////// 수정사항 ////////////////////
        //// sceneName 을 받는게 아니라 유물 클릭 했을 때의
        //// 번호 받아오기 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 .. 유물번호는 나레이션에 배열연결에 사용
        //// 유물의 이름을 받아와서 번호 부여 > 0 ~ 10 까지는 메인 나레이션 ( 고분 11개 )
        //// 배열 [11] 부터 유물 번호 받아오기

        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {        
                loading.SetActive(true);
                fade_img.blocksRaycasts = true; //아래 레이캐스트 막기
            })
            .Prepend(fade_img.DOFade(1, fadeDuration))
            .OnComplete(() =>
            {
                // 이전 씬에 있는 오브젝트들 끄고 넘어가기
                mainBackGround.SetActive(false);
                mainPopup.SetActive(false);
                arDocent.SetActive(false);
                topMenu.SetActive(false);

                // ARDocent 씬에 오브젝트 띄우기
                arBackGround.SetActive(true);
                guidebarObj.SetActive(true);
                guideTextObj[selectedCollection].SetActive(true);

                isPlaying = false;

                isKeepListening = false;
                //로딩화면 띄우며, 씬 로드 시작
                StartCoroutine("LoadScene", "ARDocent");
            });
    }
    // 메인으로 씬 로드
    public void ChangeMainScene()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        // 메인 씬 갈 때 정지
        SoundManager.Instance.StopNAR();

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                loading.SetActive(true);
                fade_img.blocksRaycasts = true; //아래 레이캐스트 막기
            })
            .Prepend(fade_img.DOFade(1, fadeDuration))
            .OnComplete(() =>
            {
                // 이전 씬에 있는 오브젝트들 끄고 넘어가기
                guidebarObj.SetActive(true);
                guideTextObj[selectedCollection].SetActive(false);
                collectionTextObj[selectedCollection].SetActive(false);
                arBackGround.SetActive(false);

                selectedCollection = -1;    // 유물 선택되지 않은 상태

                // Main 씬에 오브젝트 띄우기
                mainBackGround.SetActive(true);
                mainPopup.SetActive(true);
                arDocent.SetActive(true);                

                isPlaying = false;

                //로딩화면 띄우며, 씬 로드 시작
                StartCoroutine("LoadScene", "Main");
            });
    }

    // 씬 로드하기
    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; //퍼센트 딜레이용

        float past_time = 0;
        float percentage = 0;

        while (!(async.isDone))
        {
            yield return null;

            past_time += Time.deltaTime;

            if (percentage >= 90)
            {
                percentage = Mathf.Lerp(percentage, 100, past_time);

                if (percentage == 100)
                {
                    async.allowSceneActivation = true; //씬 전환 준비 완료
                }
            }
            else
            {
                percentage = Mathf.Lerp(percentage, async.progress * 100f, past_time);
                if (percentage >= 90) past_time = 0;
            }
            loading_text.text = percentage.ToString("0") + "%"; //로딩 퍼센트 표기
        }
    }

    // 현재 선택된 고분의 유물 설명 오브젝트
    public void Enable_CollectionDescription()
    {
        collectionTextObj[selectedCollection].SetActive(true);
    }
    //public void Disable_CollectionDescription()
    //{
    //    collectionTextObj[selectedCollection].SetActive(false);
    //}

    // 로그인 버튼 클릭 시
    public void Login()
    {
        Debug.Log(DataManager.Instance.admin.id);
        Debug.Log(id.text);
        Debug.Log(DataManager.Instance.admin.pw);
        Debug.Log(pw.text);
        
        if (DataManager.Instance.admin.id == id.text)// && DataManager.Instance.admin.pw == pw.text)
        {
            DataManager.Instance.loginIn = true;    // 관리자 로그인 확인
            // 로그인 성공 시 
            adminSettingButtonText.text = "로그아웃";

            // 팝업창 다끔
            languageSettingButton.SetActive(false);
            login_popup.transform.localScale = new Vector3(0.0001f,0.0001f, 0.0001f);            
            login_popup.SetActive(false);

            //Disable_Popup_PreferencesSetting();
            preferences_popup.transform.localScale = new Vector3(0.0001f, 0.0001f,0.0001f);
            preferencesSetting_canvasGroup.alpha = 0f;
            preferencesPanel_popup.SetActive(false);

            currentPopup = Popup.Main;
        }
        else
        {
            // 아이디 또는 비밀번호를 잘못 입력하셨습니다
            AlertPopup();
        }
    }

    public void Logout()
    {        
        DataManager.Instance.loginIn = false;    // 관리자 로그아웃 확인

        // 관리자 로그인 텍스트 바뀜
        adminSettingButtonText.text = "관리자 설정";

        // 팝업창 다끔
        languageSettingButton.SetActive(false);             // 탑메뉴에 언어설정 버튼 끄기
        login_popup.transform.localScale = new Vector3(0.0001f, 0.0001f,0.0001f);
        login_popup.SetActive(false);

        preferences_popup.transform.localScale = new Vector3(0.0001f,0.0001f, 0.0001f);
        preferencesSetting_canvasGroup.alpha = 0f;
        preferencesPanel_popup.SetActive(false);

        currentPopup = Popup.Main;  
    }

    // 로그인 정보 다를 때
    private void AlertPopup()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                alertPopup.SetActive(true);
                alertPopup.GetComponent<CanvasGroup>().alpha = 0f;
            })
            .Prepend(alertPopup.GetComponent<CanvasGroup>().DOFade(1, 0.4f))
            .Append(alertPopup.GetComponent<CanvasGroup>().DOFade(0, 1f).SetDelay(1))
            .OnComplete(() =>
            {
                // Completed
                alertPopup.SetActive(false);
                isPlaying = false;
            });
    }

    ///////// 관리자 모드 ///////// public bool loginIn = true;
    // 링크수정 팝업
    public void ModifyPopup(TMP_Text _categoryName)
    {
        if (DataManager.Instance.loginIn)
        {
            // 관리자 모드일 때
            if (isPlaying) { return; }

            isPlaying = true;

            // 팝업창에 텍스트 가져오기
            popupTitle.text = _categoryName.text;
            categoryField.text = _categoryName.text;

            linkField.text = FindEqualsLink(_categoryName.text); // 링크 가져오기와 인덱스 가져오기

            Sequence seq;

            seq = DOTween.Sequence()
                .OnStart(() =>
                {
                    modifyPanel.SetActive(true);
                    modifyPanel.GetComponent<CanvasGroup>().alpha = 0f;
                    modifyPopup.transform.localScale = new Vector3(0.0001f,0.0001f, 0.0001f);
                })
                .Prepend(modifyPanel.GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetEase(Ease.Unset))
                .Append(modifyPopup.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.Unset))
                .OnComplete(() =>
                {
                    // Completed
                    currentPopup = Popup.AdminSetting;
                    isPlaying = false;
                });
        }
        else
        {
            // URL 열기 : 데이터매니저에서 가져와서 링크 넣어둠
            Application.OpenURL(FindEqualsLink(_categoryName.text));
        }
    }

    // 링크수정 패널 닫기
    public void ModifyDisappear()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .Prepend(modifyPopup.transform.DOScale(new Vector3(0.0001f, 0.0001f,0.0001f), 0.3f).SetEase(Ease.Unset))
            .Append(modifyPanel.GetComponent<CanvasGroup>().DOFade(0, 0.3f).SetEase(Ease.Unset))
            .OnComplete(() =>
            {
                // Completed
                currentPopup = Popup.Main;
                modifyPanel.SetActive(false);
                isPlaying = false;
            });
    }
    
    // 링크수정 패널 수정하기 버튼
    public void Link_ModifyButton()
    {
        // 현재 패널
        if (currentPanel == Panel.ExhibitionGuide)
        {
            StartCoroutine(Modify_exhibitionGuide());
        }
        else
        {
            StartCoroutine(Modify_informationUse());
        }
    }

    IEnumerator Modify_exhibitionGuide()
    {
        // 전시관
        for (int i = 0; i < DataManager.Instance.exhibitionGuide.Count; i++)
        {
            if (DataManager.Instance.exhibitionGuide[i].index == currentIndex)
            {
                // 메뉴, 링크 수정하기
                var updateData = new BCM_docent_Info.Exhibition_Link_Data();
                updateData.index = currentIndex;
                updateData.Menu = categoryField.text;
                updateData.Link = linkField.text;

                // 구글시트에 적용
                UnityGoogleSheet.Write<BCM_docent_Info.Exhibition_Link_Data>(updateData);

                // 데이터매니저에 적용
                DataManager.Instance.exhibitionGuide[i].menu = categoryField.text;
                DataManager.Instance.exhibitionGuide[i].link = linkField.text;
            }
        }

        yield return null;
    }
    
    IEnumerator Modify_informationUse()
    {
        // 이용안내
        for (int i = 0; i < DataManager.Instance.informationUse.Count; i++)
        {
            if (DataManager.Instance.informationUse[i].index == currentIndex)
            {
                // 메뉴, 링크 수정하기
                var updateData = new BCM_docent_Info.InformationUse_Link_Data();
                updateData.index = currentIndex;
                updateData.Menu = categoryField.text;
                updateData.Link = linkField.text;

                // 구글시트에 적용
                UnityGoogleSheet.Write<BCM_docent_Info.InformationUse_Link_Data>(updateData);

                // 데이터매니저에 적용
                DataManager.Instance.informationUse[i].menu = categoryField.text;
                DataManager.Instance.informationUse[i].link = linkField.text;
            }
        }

        yield return null;
    }

    // 선택한 카테고리의 링크
    private string FindEqualsLink(string _categoryName)
    {
        if (currentPanel == Panel.ExhibitionGuide)
        {
            // 전시관
            for (int i = 0; i < DataManager.Instance.exhibitionGuide.Count; i++)
            {
                if (DataManager.Instance.exhibitionGuide[i].menu == _categoryName)
                {
                    currentIndex = DataManager.Instance.exhibitionGuide[i].index;
                    return DataManager.Instance.exhibitionGuide[i].link;
                }
            }
        }
        else
        {
            // 이용안내
            for (int i = 0; i < DataManager.Instance.informationUse.Count; i++)
            {
                if (DataManager.Instance.informationUse[i].menu == _categoryName)
                {
                    currentIndex = DataManager.Instance.informationUse[i].index;
                    return DataManager.Instance.informationUse[i].link;
                }
            }
        }

        return "NOT_FOUND_LINK";
    }

    // 인사말 수정하기 버튼
    public void AboutTheMuseum_ModifyButton()
    {
        if (isPlaying) { return; }

        isPlaying = true;

        // 데이터 매니저에 저장
        DataManager.Instance.aboutTheMuseum.contents = aboutTheMuseum_ModifyInputField.GetComponent<TMP_InputField>().text;

        // 구글 시트에 저장
        var updateData = new BCM_docent_Info.aboutTheMuseum_Text_Data();
        // 업데이트 시키려면 순서대로 들어가야함
        updateData.index = 0;
        updateData.Menu = "인사말";
        updateData.Contents = aboutTheMuseum_ModifyInputField.GetComponent<TMP_InputField>().text;

        Debug.Log(aboutTheMuseum_ModifyInputField.GetComponent<TMP_InputField>().text);

        UnityGoogleSheet.Write(updateData);

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                homeButton.SetActive(false);
                Crossfade(spt_mainbackground);
            })
            .Append(aboutTheMuseumRT.DOAnchorPosY(-1703f, 1f).SetEase(Ease.Unset))
            .Join(aboutTheMuseum_canvasGroup.DOFade(0, 0.5f))
            .OnComplete(() =>
            {
                // Completed
                currentPanel = Panel.Main;

                // 관리자 모드
                aboutTheMuseum_ModifyInputField.SetActive(false);

                aboutTheMuseum.SetActive(false);
                isPlaying = false;
            });
    }

    // 자세한 설명의 고분패널 수정하기 버튼
    public void SitesDescription_ModifyButton()
    {
        if (isPlaying) { return; }

        isPlaying = true;
        
        //// Modify InputText : 자세한 고분설명
        //[SerializeField] GameObject siteDescription_to_Modify;  // 수정할 입력필드 : 수정 패널
        //[SerializeField] TMP_Text siteNameText;                     // 수정할 입력필드 : 고분 이름
        //[SerializeField] TMP_InputField formationInputField;    // 수정할 입력필드 : 조성 시기
        //[SerializeField] TMP_InputField excavationInputField;   // 수정할 입력필드 : 발굴 시기
        //[SerializeField] TMP_InputField tombfeatureInputField;  // 수정할 입력필드 : 무덤 특징

        // 데이터 매니저에 저장 > 나중에 불러올 때 바로 업데이트 되서 텍스트에 저장됨
        DataManager.Instance.historicDescription[(int)currentsitesDesc].sites = siteName;
        DataManager.Instance.historicDescription[(int)currentsitesDesc].formationPeriod = formationInputField.text;
        DataManager.Instance.historicDescription[(int)currentsitesDesc].excavationPeriod = excavationInputField.text;
        DataManager.Instance.historicDescription[(int)currentsitesDesc].tombFeature = tombfeatureInputField.text;

        // 구글 시트에 저장
        var updateData = new BCM_docent_Info.HistoricSites_Description();
        // 업데이트 시키려면 순서대로 들어가야함
        updateData.index = (int)currentsitesDesc;
        updateData.sites = DataManager.Instance.historicDescription[(int)currentsitesDesc].sites;
        updateData.formationPeriod = DataManager.Instance.historicDescription[(int)currentsitesDesc].formationPeriod;
        updateData.excavationPeriod = DataManager.Instance.historicDescription[(int)currentsitesDesc].excavationPeriod;
        updateData.tombFeature = DataManager.Instance.historicDescription[(int)currentsitesDesc].tombFeature;
        
        UnityGoogleSheet.Write(updateData);

        Sequence seq;

        seq = DOTween.Sequence()
            .Prepend(siteDescription_to_Modify.GetComponent<RectTransform>().DOAnchorPosY(-903f, 1f).SetEase(Ease.Unset))
            .Join(siteDescription_to_Modify.GetComponent<CanvasGroup>().DOFade(0, 0.6f))
            .Join(chatbotSoundbar.GetComponent<RectTransform>().DOAnchorPosY(-903f, 1f).SetEase(Ease.Unset))
            .Join(topMenu_canvasGroup.DOFade(1, 0.6f))
            .Join(_backButton.GetComponent<Image>().DOFade(0, 0.6f))
            .OnComplete(() =>
            {
                // 고분 간단 설명란
                siteDescription_to_Modify.SetActive(false);
                chatbotSoundbar.SetActive(false);
                topMenu.SetActive(true);
                _backButton.SetActive(false);
                currentsitesDesc = SitesDescription.Main;
                isPlaying = false;
            });
    }
}
