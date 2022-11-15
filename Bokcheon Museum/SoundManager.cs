using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource bgm;
    public AudioSource sfx;
    public AudioSource nar;

    public float fadeSpeed = 80f;

    public AudioClip[] Bgm_audioClips;
    public AudioClip[] Sfx_audioClips;
    public AudioClip[] Nar_audioClips;  // currentsitesDesc = (SitesDescription)_site; 에서 -1 해준 값

    public Dictionary<string, AudioClip> bgm_lookupTable = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> sfx_lookupTable = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> nar_lookupTable = new Dictionary<string, AudioClip>();

    [Header("Main Audio Description")]
    [SerializeField] GameObject mainAudioSliderObj;
    [SerializeField] Slider mainAudioSlider;
    [SerializeField] GameObject mainPlayButton;
    [SerializeField] GameObject mainPauseButton;

    [Header("Docent Audio Description")]
    [SerializeField] GameObject docentAudioSliderObj;
    [SerializeField] Slider docentAudioSlider;
    [SerializeField] GameObject docentPlayButton;
    [SerializeField] GameObject docentPauseButton;
    // Flag to know if we are draging the Timeline handle
    private bool TimeLineOnDrag = false;
    

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else Instance = this;

        // 나레이션 클립 배열 하나로 사용함
        mainPlayButton.GetComponent<Button>().onClick.AddListener(() => MainPlayNAR(Nar_audioClips[(int)UIManager.Instance.currentsitesDesc - 1].name));    // 배열상 SitesDescription 의 번호 0 ~ 12번 까지 나레이션 (13개)
        docentPlayButton.GetComponent<Button>().onClick.AddListener(() => DocentPlayNAR(Nar_audioClips[UIManager.Instance.selectedCollection].name));   // 배열상 번호 13 부터 나레이션 

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //BGM
        foreach (AudioClip clip in Bgm_audioClips)
        {
            //Debug.Log("clip.name : " + clip.name);
            bgm_lookupTable.Add(clip.name, clip);
        }
        //SFX
        foreach (AudioClip clip in Sfx_audioClips)
        {
            sfx_lookupTable.Add(clip.name, clip);
        }
        //NAR
        foreach (AudioClip clip in Nar_audioClips)
        {
            nar_lookupTable.Add(clip.name, clip);
        }
    }

    float samples;
    private void Update()
    {
        if (TimeLineOnDrag)
        {
            if (mainAudioSliderObj.activeSelf)
            {
                nar.timeSamples = (int)(nar.clip.samples * mainAudioSlider.value);
            }
            else if (docentAudioSliderObj.activeSelf)
            {
                nar.timeSamples = (int)(nar.clip.samples * docentAudioSlider.value);
            }
        }
        else if (nar.isPlaying)
        {
            //Debug.Log("nar.Time : " + nar.time);
            //Debug.Log("nar.clip.length : " + nar.clip.length);
            //Debug.Log("clip.samples : " + (int)(nar.clip.samples)); // 고정값
            //Debug.Log("nar.timeSamples : " + nar.timeSamples);      // 바뀌는 값

            if (mainAudioSliderObj.activeSelf)
            {
                mainAudioSlider.value = (float)nar.timeSamples / (float)nar.clip.samples;
            }
            else if (docentAudioSliderObj.activeSelf)
            {
                docentAudioSlider.value = (float)nar.timeSamples / (float)nar.clip.samples;
            }
        }
    }

    public void PlayBGM(string soundName)
    {
        //현재 재생 중인 사운드와 일치
        if (bgm.isPlaying && bgm.clip.name.Equals(soundName))
            return;
        else
        {
            bgm.clip = Bgm_GetClipByName(soundName);
            bgm.Play();
            StopCoroutine(_Bmg_FadeIn());
            StopCoroutine(_Bmg_FadeOut());
            StartCoroutine(_Bmg_FadeIn());
        }
    }
    public void StopBGM()
    {
        StopCoroutine(_Bmg_FadeOut());
        StopCoroutine(_Bmg_FadeIn());
        StartCoroutine(_Bmg_FadeOut());
    }

    public void MainPlayNAR(string soundName)
    {
        //현재 재생 중인 사운드와 일치
        if (nar.isPlaying && nar.clip.name.Equals(soundName))
            return;
        else
        {
            nar.clip = Nar_GetClipByName(soundName);
            nar.Play();

            // 슬라이더 조절
            // currentTime / nar.clip.length
            //audioSlider[(int)UIManager.Instance.currentSite].value = currentTime / nar.clip.length;

            StopCoroutine(_Nar_FadeIn());
            StopCoroutine(_Nar_FadeOut());
            StartCoroutine(_Nar_FadeIn());
        }
    }

    public void DocentPlayNAR(string soundName)
    {
        //현재 재생 중인 사운드와 일치
        if (nar.isPlaying && nar.clip.name.Equals(soundName))
            return;
        else
        {
            nar.clip = Nar_GetClipByName(soundName);
            nar.Play();

            // 슬라이더 조절
            // currentTime / nar.clip.length
            //audioSlider[(int)UIManager.Instance.currentSite].value = currentTime / nar.clip.length;

            StopCoroutine(_Nar_FadeIn());
            StopCoroutine(_Nar_FadeOut());
            StartCoroutine(_Nar_FadeIn());
        }
    }

    public void PauseNAR()
    {
        nar.Pause();
    }

    public void StopNAR()
    {
        // 나레이션 초기화 부분에서 쓰임

        mainPlayButton.SetActive(true);
        mainPauseButton.SetActive(false);
        
        StopCoroutine(_Nar_FadeOut());
        StopCoroutine(_Nar_FadeIn());
        StartCoroutine(_Nar_FadeOut());
    }

    IEnumerator _Bmg_FadeIn()
    {
        float vol = 0f;
        audioMixer.SetFloat("BGM", vol);

        while (vol < 0f)
        {
            vol += Time.deltaTime * fadeSpeed;
            audioMixer.SetFloat("BGM", vol);
            yield return new WaitForEndOfFrame();
        }
        audioMixer.SetFloat("BGM", 0f);
    }

    IEnumerator _Bmg_FadeOut()
    {
        float vol = 0f;

        audioMixer.SetFloat("BGM", vol);

        while (vol > -80f)
        {
            vol -= Time.deltaTime * fadeSpeed;
            audioMixer.SetFloat("BGM", vol);
            yield return new WaitForEndOfFrame();
        }
        audioMixer.SetFloat("BGM", -80f);
        bgm.Stop();
    }

    IEnumerator _Nar_FadeIn()
    {
        float vol = 0f;
        audioMixer.SetFloat("NAR", vol);

        while (vol < 0f)
        {
            vol += Time.deltaTime * fadeSpeed;
            audioMixer.SetFloat("NAR", vol);
            yield return new WaitForEndOfFrame();
        }
        audioMixer.SetFloat("NAR", 0f);
    }

    IEnumerator _Nar_FadeOut()
    {
        Debug.Log("에러안뜨나??");

        float vol = 0f;

        audioMixer.SetFloat("NAR", vol);

        while (vol > -80f)
        {
            vol -= Time.deltaTime * fadeSpeed;
            audioMixer.SetFloat("NAR", vol);
            yield return new WaitForEndOfFrame();
        }
        audioMixer.SetFloat("NAR", -80f);
        nar.Stop();

        nar.timeSamples = 0;
        mainAudioSlider.value = 0f;
        docentAudioSlider.value = 0f;
    }

    public AudioClip Bgm_GetClipByName(string clipName)
    {
        return bgm_lookupTable[clipName];
    }

    public void PlayBGM(string clipName, float volume = 1f)
    {
        bgm.PlayOneShot(Bgm_GetClipByName(clipName), volume);
    }
    public AudioClip Sfx_GetClipByName(string clipName)
    {
        return sfx_lookupTable[clipName];
    }
    public void PlaySFX(string clipName, float volume = 1f)
    {
        //Debug.Log("PlaySFX Sound name : " + clipName);
        sfx.PlayOneShot(Sfx_GetClipByName(clipName), volume);
    }
    public AudioClip Nar_GetClipByName(string clipName)
    {
        return nar_lookupTable[clipName];
    }

    // Called by the event trigger when the drag begin
    public void TimeLineOnBeginDrag()
    {
        TimeLineOnDrag = true;

        nar.Pause();

        mainPlayButton.SetActive(true);
        mainPauseButton.SetActive(false);
    }


    // Called at the end of the drag of the TimeLine
    public void TimeLineOnEndDrag()
    {
        if (nar.time < nar.clip.length)
        {
            nar.Play();
        }

        TimeLineOnDrag = false;

        mainPlayButton.SetActive(false);
        mainPauseButton.SetActive(true);
    }
}