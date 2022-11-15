using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public float fadeSpeed = 80f;
    public AudioMixer audioMixer;
    public AudioSource sfx;
    public AudioSource countdown;
    public AudioSource timer;
    public AudioSource fail;
    public AudioSource success;
    public AudioClip[] audioClips;
    public Dictionary<string, AudioClip> lookupTable;

    public static SoundManager Instance { get; private set; }


    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else Instance = this;
    }
    void Start()
    {
        lookupTable = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in audioClips)
        {
            lookupTable.Add(clip.name, clip);
        }
        PlayBGM();

        Timer.RoundEnd += OnRoundEnd;
    }
    void OnDisable()
    {
        Timer.RoundEnd -= OnRoundEnd;
    }

    public void PlaySFX(string clipName, float volume = 1f)
    {
        sfx.PlayOneShot(lookupTable[clipName], volume);
    }
    private void PlayBGM()
    {
        StopCoroutine("FadeIn");
        StopCoroutine("FadeOut");
        StartCoroutine("FadeIn");
    }
    private void StopBGM()
    {
        StopCoroutine("FadeOut");
        StopCoroutine("FadeIn");
        StartCoroutine("FadeOut");
    }

    private void OnRoundEnd()
    {
        StopBGM();

        if (timer.isPlaying)
            timer.Stop();
    }

    IEnumerator FadeIn()
    {
        float vol = -80f;
        audioMixer.SetFloat("volumeBGM", vol);

        while (vol < 0f)
        {
            vol += Time.deltaTime * fadeSpeed;
            audioMixer.SetFloat("volumeBGM", vol);
            yield return new WaitForEndOfFrame();
        }
        audioMixer.SetFloat("volumeBGM", 0f);
    }
    IEnumerator FadeOut()
    {
        float vol = 0f;
        audioMixer.SetFloat("volumeBGM", vol);

        while (vol > -80f)
        {
            vol -= Time.deltaTime * fadeSpeed;
            audioMixer.SetFloat("volumeBGM", vol);
            yield return new WaitForEndOfFrame();
        }
        audioMixer.SetFloat("volumeBGM", -80f);
    }
}
