using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Attributes;


/// <summary>
/// 질문 리스트 정리
/// 언제 지어졌나요??
/// </summary>

public class Message
{
    public string Text;
    public Text TextObject;
    public DocentMessageType MessageType;
}

public enum DocentMessageType
{
    User, Bot
}

// A collection of Intents
public class BotDialog : Dialog
{
    // Intents : 인사
    [Expression("안녕")]
    [Expression("안냥")]
    [Expression("하위")]
    [Expression("안녕하세요")]
    [Expression("너는 누구야")]
    [Expression("누구세요")]
    public void Hello(Context context, Result result)
    {
        result.SendResponse("안녕하세요 복천이에요!");
    }

    // Intents : 웃다
    [Expression("ㅎㅎ")]
    [Expression("하핳")]
    [Expression("하하")]
    [Expression("헤헤")]
    [Expression("헤헿")]
    [Expression("^^")]
    public void Laugh(Context context, Result result)
    {
        int rand;

        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        rand = UnityEngine.Random.Range(0, 3);

        switch (rand)
        {
            case 0:
                result.SendResponse("^~^ /");
                break;
            case 1:
                result.SendResponse("헤헿");
                break;
            case 2:
                result.SendResponse("기분이 좋네요 저도 ^~^");
                break;
            default:
                break;
        }
    }

    // 선택지
    [Expression("복천박물관")]
    public void BCM_Option(Context context, Result result)
    {
        int rand;

        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        rand = UnityEngine.Random.Range(0, 3);

        // 박물관 정보
        switch (rand)
        {
            case 0:
                result.SendResponse("나들이 가기 좋은 부산 복천박물관!");
                break;
            case 1:
                result.SendResponse("복천동고분군에서 출토된 유물을 중심으로,");
                result.SendResponse("삼한시대부터 삼국시대까지 부산의 역사를 보여주는 복천박물관!");
                break;
            case 2:
                result.SendResponse("복천동고분군에서 출토된 유물을 중심으로,");
                result.SendResponse("삼한시대부터 삼국시대까지 부산의 역사를 보여주는 복천박물관!");
                break;
            default:
                break;
        }
    }
    [Expression("어디")]
    public void Where_AddKeyword(Context context, Result result)
    {
        result.SendResponse("키워드가 필요합니다. ex) 어디에 위치하나요?, 어디에 있나요?");
    }
    // 위치, 주소
    [Expression("복천박물관위치")]
    [Expression("복천박물관 위치")]
    [Expression("복천박물관 주소")]
    [Expression("복천박물관은 어디에 있나요?")]
    [Expression("어디에있나요?")]
    [Expression("어디에 있나요?")]
    public void Where(Context context, Result result)
    {
        result.SendResponse("47800 부산광역시 동래구 복천로 63");
        result.SendResponse("https://museum.busan.go.kr/bokcheon/index");
        result.SendResponse("https://museum.busan.go.kr/bokcheon/bkmap");
    }

    [Expression("복천동고분군발굴완료")]
    [Expression("복천동고분군 발굴완료")]
    [Expression("복천동고분군 발굴이 완료되었나요?")]
    public void ExcavationStateQuestion(Context context, Result result)
    {
        result.SendResponse("아닙니다. 현재까지 존재가 확인되었으나 연구가 축적되고");
        result.SendResponse("기술이 더욱 향상된 미래에 조사하기 위해");
        result.SendResponse("발굴을 진행하지 않은 무덤이 많습니다.");
    }

    [Expression("복천동고분군에서 강아지와 산책해도 되나요?")]
    public void Walk_Question(Context context, Result result)
    {
        result.SendResponse("복천동고분군은 사적 제273호로 지정된 국가문화재로, 반려동물과의 산책을 원칙적으로 금지하고 있습니다.");
    }

    [Expression("보물이있나요?")]
    [Expression("보물이 있나요?")]
    [Expression("복천동고분군에서 출토된 유물 중 보물이 있나요?")]
    public void Treasure_Question(Context context, Result result)
    {
        result.SendResponse("네 물론입니다. 총 5건이 보물로 지정되었습니다.");
        result.SendResponse("1. 7호분 출토 말머리 장식 뿔잔 : 1975년 5월 16일, 보물 제598호로 지정");
        result.SendResponse("2. 11호분 출토 금동관 : 2016년 11월 16일, 보물 제1922호로 지정");
        result.SendResponse("3. 22호분 출토 청동칠두령 : 2019년 3월 6일, 보물 제2019호로 지정");
        result.SendResponse("4. 38호분 출토 철제갑옷 일괄 : 2019년 3월 6일 보물 제2020호로 지정");
        result.SendResponse("5. 11호분 출토 도기 거북장식 원통형기대 및 단경호 : 2020년 2월 27일, 보물 제2059호로 지정");
    }

    [Expression("복천동고분군 숫자2개 숫자1개 지칭하는 무덤 무슨차이죠?")]
    [Expression("복천동고분군을 보니 숫자 2개를 사용하여 지칭하는 무덤이 있고 숫자 1개를 사용하여 지칭하는 무덤이 있는데 무슨 차이죠?")]
    public void ExcavationInfo_Question(Context context, Result result)
    {
        result.SendResponse("네 물론입니다. 총 5건이 보물로 지정되었습니다.");
        result.SendResponse("1. 7호분 출토 말머리 장식 뿔잔 : 1975년 5월 16일, 보물 제598호로 지정");
        result.SendResponse("2. 11호분 출토 금동관 : 2016년 11월 16일, 보물 제1922호로 지정");
        result.SendResponse("3. 22호분 출토 청동칠두령 : 2019년 3월 6일, 보물 제2019호로 지정");
        result.SendResponse("4. 38호분 출토 철제갑옷 일괄 : 2019년 3월 6일 보물 제2020호로 지정");
        result.SendResponse("5. 11호분 출토 도기 거북장식 원통형기대 및 단경호 : 2020년 2월 27일, 보물 제2059호로 지정");
    }

    [Expression("언제")]
    public void When_AddKeyword(Context context, Result result)
    {
        result.SendResponse("키워드가 필요합니다. ex) 언제까지 관람 되나요?, 언제까지 출입 가능한가요?");
    }

    // 관람 시간
    [Expression("언제까지 관람 되나요?")]
    [Expression("언제까지 관람 가능한가요?")]
    [Expression("언제까지 관람이 가능한가요?")]
    [Expression("언제까지 출입 가능한가요?")]
    [Expression("언제까지 출입이 가능한가요?")]
    [Expression("언제까지 입장 가능한가요?")]
    [Expression("언제까지 입장이 가능한가요?")]
    [Expression("언제까지 여나요?")]
    [Expression("언제까지 열려있나요?")]
    [Expression("언제까지 휴관인가요?")]
    [Expression("예약없이 관람 가능한가요?")]
    [Expression("예약없이 관람이 가능한가요?")]
    [Expression("일요일은 휴관인가요?")]
    public void When(Context context, Result result)
    {
        result.SendResponse("관람시간 : 평일 및 일요일 : 09:00 ~ 18:00\n입장시간 : 평일 및 일요일 : 09:00 ~ 17:00\n휴관일 : 1월 1일, 매주 월요일");
        //result.SendResponse("입장시간 : 평일 및 일요일 : 09:00 ~ 17:00");
        //result.SendResponse("휴관일 : 1월 1일, 매주 월요일");
    }

    // 사진 가능 여부
    [Expression("사진 찍어도 되나요?")]
    [Expression("사진 촬영 가능 여부")]
    public void Photo(Context context, Result result)
    {
        result.SendResponse("전시유물에 대한 이해와 학습을 목적으로 한 사진 촬영은 가능합니다.");
        result.SendResponse("사진 촬영 시 아래 항목을 유의해 주시기 바랍니다.");
        result.SendResponse("첫째, 플래시나 삼각대를 이용한 사진 촬영은 유물에 대한 손상과 주변 관람객들의 관람에 불편을 초래하므로 금지합니다.");
        result.SendResponse("둘째, 상업적 용도의 촬영은 허용하지 않습니다.");
        result.SendResponse("셋째, 다른 관람객의 이용에 불편을 끼치지 않는 범위 내에서 사진 촬영을 허용합니다.");
        result.SendResponse("학술적인 목적으로 특정한 유물의 사진 자료가 필요하신 경우는");
        result.SendResponse(" 전화로 구체적인 내용을 문의해 주시면 상세히 안내해 드리겠습니다.");
    }

    // 자료 요구
    [Expression("발표 요지문 자료 요청 방법")]
    [Expression("세미나 자료 요청해도 되나요?")]
    public void RequestDocument(Context context, Result result)
    {
        result.SendResponse("세미나 발표 요지문 등 박물관에서 발간한 자료가 학술적인 목적으로 필요하신 경우,");
        result.SendResponse("전화로 구체적인 내용을 문의해 주시면 상세히 안내해 드리겠습니다.");
        result.SendResponse("문의처 : 복천박물관 조사보존실 ☏ 550-0335");
    }

    // 자료 요구
    [Expression("채용")]
    [Expression("채용관련")]
    [Expression("학예연구사")]
    public void Employment(Context context, Result result)
    {
        result.SendResponse("복천박물관을 비롯하여 부산박물관·정관박물관·부산근대역사관·임시수도기념관·동삼동패총전시관·시민공원역사관은");
        result.SendResponse("부산광역시 산하의 시립박물관이며, 근무하고 있는 학예연구사는 부산시 지방학예연구직 공무원입니다.");
        result.SendResponse("따라서 채용은 부산광역시 공무원 임용시험에 의해 이루어집니다.");
        result.SendResponse("시험과목이나 지원조건 등에 대해서는 부산광역시 홈페이지의 기존 채용공고를 참고하시면 됩니다.", TimeSpan.FromSeconds(0.3f));
        result.SendResponse("자세한 사항은 부산광역시 인터넷 홈페이지(http://www.busan.go.kr)의");
        result.SendResponse("「취업정보> 시험정보> 시험정보안내」또는 부산광역시 인사과 고시 담당으로 문의 바랍니다");
    }

    // 자료 요구
    [Expression("숙제관련")]
    public void Homework(Context context, Result result)
    {
        result.SendResponse("학생들이 숙제와 관련하여 박물관 전시내용과 역사 전반을 질문하는 경우가 있습니다.");
        result.SendResponse("학생 스스로가 박물관 관람이나 홈페이지 조회 또는 관련 도서를 통해 문제를 해결하시기 바랍니다.");
        result.SendResponse("이러한 경험의 축적이 학생의 능력 향상에 소중한 기회가 될 것입니다.");
    }

    // 유물
    [Expression("유물")]
    public void Collection_Option_1(Context context, Result result)
    {
        result.SendResponse("유물 무엇을 말씀하시는 건가요?");

        ///     프리팹 선택하기
        ///     [Expression("소장유물")]
        ///     [Expression("전시유물")]
    }

    // 유물
    [Expression("소장")]
    public void Collection_Option_2(Context context, Result result)
    {
        result.SendResponse("소장하고있는 유물 말씀하시는 건가요?");

        ///     프리팹 선택하기
        ///     [Expression("소장유물")]
        ///     [Expression("전시유물")]
    }
    // 유물 현황
    [Expression("전시")]
    [Expression("전시관")]
    public void Exhibition(Context context, Result result)
    {
        result.SendResponse("제1전시실, 제2전시실, 야외전시관이 있습니다.");
    }
    // 유물 현황
    [Expression("유물현황")]
    [Expression("소장유물")]
    [Expression("전시유물")]
    [Expression("유물발견")]
    public void Collection(Context context, Result result)
    {
        result.SendResponse("소장유물 : 9,178점 (2021년 10월 30일 현재)");
        result.SendResponse("발굴유물 : 9,053점 , 기증유물 125점");
        result.SendResponse("");
        result.SendResponse("전지유물, 자료 : 2,057점 (2021년 10월 30일 현재)");
        result.SendResponse("전시실명    전시유물    전시보조자료    합계");
        result.SendResponse("제1전시실   498         712           1,210");
        result.SendResponse("제2전시실   338         18            356");
        result.SendResponse("야외전시관  0           489           489");
        result.SendResponse("계         836         1,219         2,055");
    }

    // 유물
    [Expression("예약")]
    [Expression("예약할 수 있나요?")]
    [Expression("교육받을 수 있나요?")]
    [Expression("강좌들을 수 있나요?")]
    [Expression("체험할 수 있나요?")]
    [Expression("견학할 수 있나요?")]
    public void Reservation(Context context, Result result)
    {
        result.SendResponse("통합예약시스템에서 확인해 보세요.");
        result.SendResponse("https://reserve.busan.go.kr/exprn/list?srchResveInsttCd=5");
    }

    // 일반 질문들 ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // 의도를 찾지 못했을 때
    [Fallback]
    public void DefaultFallback(Context context, Result result)
    {
        int rand;

        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        rand = UnityEngine.Random.Range(0, 3);

        result.SendResponse("아래와 같은 말들은 잘 이해해요. 참고부탁드려요.");
        result.SendResponse("");

        switch (rand)
        {
            case 0:
                // 프리팹 생성하기 - 질문 리스트 1
                result.SendResponse("복천동고분군에서 출토된 유물 중 보물이 있나요?");
                result.SendResponse("복천동고분군 발굴이 모두 완료되었나요?");
                result.SendResponse("복천동고분군에서 강아지와 산책해도 되나요?");
                break;
            case 1:
                // 프리팹 생성하기 - 질문 리스트 2
                result.SendResponse("언제까지 관람 되나요?");
                result.SendResponse("예약없이 관람이 가능한가요?");
                result.SendResponse("일요일은 휴관인가요?");

                break;
            case 2:
                // 프리팹 생성하기 - 질문 리스트 3
                result.SendResponse("사진 찍어도 되나요?");
                result.SendResponse("교육받을 수 있나요?");
                result.SendResponse("세미나 자료 요청해도 되나요?");
                break;
            default:
                break;
        }
    }
}

public class ChatManager : MonoBehaviour
{
    public GameObject YellowArea, WhiteArea, DateArea;
    public RectTransform ContentRect;
    public Scrollbar scrollBar;
    public TMP_InputField myInputField;
    AreaScript LastArea;

    OscovaBot MainBot;

    private void Start()
    {
        try
        {
            MainBot = new OscovaBot();
            OscovaBot.Logger.LogReceived += (s, o) =>
            {
                Debug.Log($"OscovaBot: {o.Log}");
            };

            MainBot.Dialogs.Add(new BotDialog());
            //Knowledge.json file referenced without extension.
            //Workspace file extensions must be changed from .west to .json
            //var txtAsset = (TextAsset)Resources.Load("knowledge", typeof(TextAsset));
            //var tileFile = txtAsset.text;
            //MainBot.ImportWorkspace("tileFile");
            MainBot.Trainer.StartTraining();

            MainBot.MainUser.ResponseReceived += (sender, evt) =>
            {
                // 챗봇의 채팅
                //AddMessage($"Bot: {evt.Response.Text}", DocentMessageType.Bot);
                Chat(false, evt.Response.Text, "복천이", Resources.Load<Texture2D>("ETC/Chatbot"));
            };
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }
    public void SendMessageToBot()
    {
        var userMessage = myInputField.text;

        Chat(true, userMessage, "나", null);

        if (!string.IsNullOrEmpty(userMessage))
        {        
            var request = MainBot.MainUser.CreateRequest(userMessage);
            var evaluationResult = MainBot.Evaluate(request);
            evaluationResult.Invoke();  // 컨텍스트 점수가 높은것을 부여한다.

            // clear the input field and focus on it
            //myInputField.Select();
            myInputField.text = "";
            myInputField.ActivateInputField();
        }
    }
    /// <summary>
    /// Called every frame to check for return key presses.
    /// A return key press will send the chat
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessageToBot();
        }
    }

    /// <summary>
    /// isSend : 보내는 화자 ( 나 : 노란색 , 유저 : 흰색 )
    /// text : 메세지
    /// user : 보내는 사람 이름
    /// picture : 이미지
    /// </summary>
    /// <param name="isSend"></param>
    /// <param name="text"></param>
    /// <param name="user"></param>
    /// <param name="picture"></param>

    public void Chat(bool isSend, string text, string user, Texture2D picture)
    {
        if (text.Trim() == "") return;

        bool isBottom = scrollBar.value <= 0.00001f;

        // 보내는 사람은 노랑, 받는 사람은 흰색영역을 크게 만들고 텍스트 대입
        AreaScript Area = Instantiate(isSend ? YellowArea : WhiteArea).GetComponent<AreaScript>();
        Area.transform.SetParent(ContentRect.transform, false);
        Area.BoxRect.sizeDelta = new Vector2(800, Area.BoxRect.sizeDelta.y);    // Area : 가로 세로 크기 조정 - 가로는 750 고정, 세로는 유동적        
        Area.TextRect.GetComponent<TMP_Text>().text = text;
        Fit(Area.BoxRect);

        Debug.Log("Area.BoxRect.sizeDelta : " + Area.BoxRect.sizeDelta);

        // 두 줄 이상이면 크기를 줄여가면서, 한 줄이 아래로 내려가면 바로 전 크기를 대입
        float X = Area.TextRect.sizeDelta.x + 42;   // 800 + 42 > 이상이면 두줄!
        float Y = Area.TextRect.sizeDelta.y;        // 텍스트 만큼의 크기
        if (Y > 49)     // 무조건 큼
        {
            for (int i = 0; i < 200; i++)
            {
                Area.BoxRect.sizeDelta = new Vector2(X - i * 2, Area.BoxRect.sizeDelta.y);
                Fit(Area.BoxRect);

                if (Y != Area.TextRect.sizeDelta.y) { Area.BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
            }
        }
        else Area.BoxRect.sizeDelta = new Vector2(X, Y);

        // 현재 것에 분까지 나오는 날짜와 유저이름 대입
        DateTime t = DateTime.Now;
        Area.Time = t.ToString("yyyy-MM-dd-HH-mm");
        Area.User = user;


        // 현재 것은 항상 새로운 시간 대입
        int hour = t.Hour;
        if (t.Hour == 0) hour = 12;
        else if (t.Hour > 12) hour -= 12;
        Area.TimeText.text = (t.Hour > 12 ? "오후 " : "오전 ") + hour + ":" + t.Minute.ToString("D2");

        // 이전 것과 같으면 이전 시간, 꼬리 없애기
        bool isSame = LastArea != null && LastArea.Time == Area.Time && LastArea.User == Area.User;
        if (isSame) LastArea.TimeText.text = "";
        
        //Area.Tail.SetActive(!isSame);
        
        // 타인이 이전 것과 같으면 이미지, 이름 없애기
        if (!isSend)
        {
            //Area.ChatBotImage.gameObject.SetActive(!isSame);
            //Area.ChatBotText.gameObject.SetActive(!isSame);
            //Area.ChatBotText.text = Area.User;
            //if (picture != null) Area.ChatBotImage.sprite = Sprite.Create(picture, new Rect(0, 0, picture.width, picture.height), new Vector2(0.5f, 0.5f));
        }

        // 날짜영역.....
        // 이전 것과 날짜가 다르면 날짜영역 보이기
        if (LastArea != null && LastArea.Time.Substring(0, 10) != Area.Time.Substring(0, 10))
        {
            Transform CurDateArea = Instantiate(DateArea).transform;
            CurDateArea.SetParent(ContentRect.transform, false);
            CurDateArea.SetSiblingIndex(CurDateArea.GetSiblingIndex() - 1);

            string week = "";
            switch (t.DayOfWeek)
            {
                case DayOfWeek.Sunday: week = "일"; break;
                case DayOfWeek.Monday: week = "월"; break;
                case DayOfWeek.Tuesday: week = "화"; break;
                case DayOfWeek.Wednesday: week = "수"; break;
                case DayOfWeek.Thursday: week = "목"; break;
                case DayOfWeek.Friday: week = "금"; break;
                case DayOfWeek.Saturday: week = "토"; break;
            }
            CurDateArea.GetComponent<AreaScript>().DateText.text = t.Year + "년 " + t.Month + "월 " + t.Day + "일 " + week + "요일";
        }


        // 텍스트가 업데이트 되면
        // 바로위에 BoxRect > Area > Content 순서대로 업데이트 되어야 함
        Fit(Area.BoxRect);  
        Fit(Area.AreaRect);
        Fit(ContentRect);
        LastArea = Area;

        // 스크롤바가 위로 올라간 상태에서 메시지를 받으면 맨 아래로 내리지 않음
        if (!isSend && !isBottom) return;
        Invoke("ScrollDelay", 0.03f);
    }

    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);

    void ScrollDelay() => scrollBar.value = 0;
}