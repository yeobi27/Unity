using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UGS;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Admin
{
    public string id;
    public string pw;

    public Admin() { }

    public Admin(string _id, string _pw)
    {
        id = _id;
        pw = _pw;
    }
}

[System.Serializable]
public class ExhibitionGuide
{
    public int index;
    public string menu;
    public string link;

    public ExhibitionGuide() { }

    public ExhibitionGuide(int _index, string _menu, string _link)
    {
        index = _index;
        menu = _menu;
        link = _link;
    }
}

[System.Serializable]
public class InformationUse
{
    public int index;
    public string menu;
    public string link;

    public InformationUse() { }

    public InformationUse(int _index, string _menu, string _link)
    {
        index = _index;
        menu = _menu;
        link = _link;
    }
}

[System.Serializable]
public class AboutTheMuseum
{
    public int index;
    public string menu;
    public string contents;

    public AboutTheMuseum() { }

    public AboutTheMuseum(int _index, string _menu, string _contents)
    {
        index = _index;
        menu = _menu;
        contents = _contents;
    }
}

[System.Serializable]
public class HistoricDescription_Summary
{
    public int index;
    public string sites;
    public string summary;

    public HistoricDescription_Summary() { }

    public HistoricDescription_Summary(int _index, string _sites, string _summary)
    {
        index = _index;
        sites = _sites;
        summary = _summary;
    }
}

[System.Serializable]
public class HistoricDescription
{
    public int index;
    public string sites;
    public string formationPeriod;
    public string excavationPeriod;
    public string tombFeature;
    public string historicalWorth;
    public string mainCollection;
    public string location;

    public HistoricDescription() { }

    public HistoricDescription(int _index, string _sites, string _formationPeriod, string _excavationPeriod, string _tombFeature, string _historicalWorth, string _mainCollection, string _location)
    {
        index = _index;
        sites = _sites;
        formationPeriod = _formationPeriod;
        excavationPeriod = _excavationPeriod;
        tombFeature = _tombFeature;
        historicalWorth = _historicalWorth;
        mainCollection = _mainCollection;
        location = _location;
    }
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private bool isCompleted = false;

    public TMP_Text loading_text;
    public GameObject loadPanel;

    private float progress;
    private int total_loadCount = 6;
    private int loadAmount = 0;
    private int loadCount = 0;

    // myTween isPlaying
    bool isPlaying;

    public Admin admin;
    public List<ExhibitionGuide> exhibitionGuide = new List<ExhibitionGuide>();
    public List<InformationUse> informationUse = new List<InformationUse>();
    public AboutTheMuseum aboutTheMuseum;
    public List<HistoricDescription_Summary> historicDescription_Summaries = new List<HistoricDescription_Summary>();
    public List<HistoricDescription> historicDescription = new List<HistoricDescription>();

    // Admin Login State
    public bool loginIn;

    private void Awake()
    {
        if (Instance != null) { Destroy(this.gameObject); }
        else { Instance = this; }

        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        loadPanel.SetActive(true);
        loadAmount = 100 / total_loadCount;
        progress = loadAmount * loadCount;

        // Server Addressable Loaded

        StartCoroutine(_LoadData());
        StartCoroutine(_LoadProgressbar());
    }

    IEnumerator _LoadData()
    {
        progress = loadAmount * loadCount;
        // Load ID
        UnityGoogleSheet.LoadFromGoogle<string, BCM_docent_Info.Admin>((list, map) =>
        {
            list.ForEach(x =>
            {
                admin.id = x.ID;
                admin.pw = x.PW;
            });
        }, true);

        yield return new WaitUntil(() => UnityPlayerWebRequest.Instance.reqProcessing == false);
        loadCount++; // 1
        progress = loadAmount * loadCount;

        // Exhibition Guide Link
        UnityGoogleSheet.LoadFromGoogle<int, BCM_docent_Info.Exhibition_Link_Data>((list, map) =>
        {
            for (int i = 0; i < list.Count; i++)
            {
                exhibitionGuide.Add(new ExhibitionGuide(list[i].index, list[i].Menu, list[i].Link));
            }

            //list.ForEach(x =>
            //{
            //    Debug.Log(x.Menu + ", " + x.Link);
                
            //    exhibitionGuide[0].menu = x.Menu;
            //});
        }, true);

        yield return new WaitUntil(() => UnityPlayerWebRequest.Instance.reqProcessing == false);
        loadCount++;    // 2
        progress = loadAmount * loadCount;

        // Information Use Link
        UnityGoogleSheet.LoadFromGoogle<int, BCM_docent_Info.InformationUse_Link_Data>((list, map) =>
        {
            for (int i = 0; i < list.Count; i++)
            {
                informationUse.Add(new InformationUse(list[i].index, list[i].Menu, list[i].Link));
            }
        }, true);

        yield return new WaitUntil(() => UnityPlayerWebRequest.Instance.reqProcessing == false);
        loadCount++;    // 3
        progress = loadAmount * loadCount;

        // about The Museum
        UnityGoogleSheet.LoadFromGoogle<int, BCM_docent_Info.aboutTheMuseum_Text_Data>((list, map) =>
        {
            list.ForEach(x =>
            {
                aboutTheMuseum.index = x.index;
                aboutTheMuseum.menu = x.Menu;
                aboutTheMuseum.contents = x.Contents;
            });
        }, true);

        yield return new WaitUntil(() => UnityPlayerWebRequest.Instance.reqProcessing == false);
        loadCount++;    // 4
        progress = loadAmount * loadCount;
        //historicDescription_Summaries

        // HistoricSites Text
        UnityGoogleSheet.LoadFromGoogle<int, BCM_docent_Info.HistoricSites_Summary>((list, map) =>
        {
            for (int i = 0; i < list.Count; i++)
            {
                historicDescription_Summaries.Add(new HistoricDescription_Summary(list[i].index, list[i].sites, list[i].summary));
            }
        }, true);

        yield return new WaitUntil(() => UnityPlayerWebRequest.Instance.reqProcessing == false);
        loadCount++;    // 5
        progress = loadAmount * loadCount;

        // HistoricSites Text
        UnityGoogleSheet.LoadFromGoogle<int, BCM_docent_Info.HistoricSites_Description>((list, map) =>
        {
            for (int i = 0; i < list.Count; i++)
            {
                historicDescription.Add(new HistoricDescription(
                        list[i].index, list[i].sites, list[i].formationPeriod, list[i].excavationPeriod, 
                        list[i].tombFeature, list[i].historicalWorth, list[i].mainCollection, list[i].location));
            }
        }, true);

        yield return new WaitUntil(() => UnityPlayerWebRequest.Instance.reqProcessing == false);
        loadCount++;    // 6
        progress = loadAmount * loadCount;
    }

    IEnumerator _LoadProgressbar()
    {
        float past_time = 0.1f;
        float percentage = 0;

        while (!isCompleted)
        {
            yield return null;

            //past_time += Time.deltaTime;

            if (percentage >= 90)
            {
                percentage = Mathf.Lerp(percentage, 100, past_time);

                if (percentage == 100)
                {
                    isCompleted = true;
                    PanelFadeOut();
                }
            }
            else
            {
                percentage = Mathf.Lerp(percentage, progress, past_time);
                if (percentage >= 90) past_time = 0.5f;
            }

            loading_text.text = percentage.ToString("0") + "%"; //로딩 퍼센트 표기
        }
    }

    private void PanelFadeOut()
    {
        if (isPlaying) { return; }
        
        isPlaying = true;

        Sequence seq;

        seq = DOTween.Sequence()
            .OnStart(() =>
            {
                loadPanel.GetComponent<CanvasGroup>().alpha = 0f;
            })
            .Append(loadPanel.GetComponent<CanvasGroup>().DOFade(0, 0.4f))
            .OnComplete(() =>
            {
                // Completed
                loadPanel.SetActive(false);                
                isPlaying = false;
            });
    }
}
