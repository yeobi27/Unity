using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IceTigerSound
{
    public string soundName;
    public AudioClip clip;
}

public class IceTiger_SoundManager : MonoBehaviour
{
    public static IceTiger_SoundManager Instance { get; private set; }

    [Header("사운드 등록")]
    [SerializeField] IceTigerSound[] bgmSounds;
    [SerializeField] IceTigerSound[] sfxSounds;

    [Header("브금 플레이어")]
    [SerializeField] AudioSource bgmPlayer;

    [Header("효과음 플레이어")]
    [SerializeField] AudioSource[] sfxPlayer;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayRandomBGM();
    }

    public void PlaySE(string _soundName)
    {
        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (_soundName == sfxSounds[i].soundName)
            {
                for (int x = 0; x < sfxPlayer.Length; x++)
                {
                    if (!sfxPlayer[x].isPlaying)
                    {
                        sfxPlayer[x].clip = sfxSounds[i].clip;
                        sfxPlayer[x].Play();
                        return;
                    }
                }
                return;
            }
        }
    }

    public void StopSfx()
    {
        for (int x = 0; x < sfxPlayer.Length; x++)
        {
            if (sfxPlayer[x].isPlaying)
            {
                sfxPlayer[x].Stop();
            }
        }
    }

    public void StopSelectedSfx(string sfx_name)
    {
        for (int x = 0; x < sfxPlayer.Length; x++)
        {
            //Debug.Log("sfxPlayer[x].clip.name : " + sfxPlayer[x].clip.name);
            if (sfxPlayer[x].isPlaying)
            {
                if (sfxPlayer[x].clip.name == sfx_name)
                {
                    sfxPlayer[x].Stop();
                }
            }
        }
    }

    public void PlayRandomBGM()
    {
        //int random = Random.Range(0,2);
        //bgmPlayer.clip = bgmSounds[random].clip;
        bgmPlayer.clip = bgmSounds[0].clip;
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void bgmPlayerVolumeControl(float _volume)
    {
        bgmPlayer.volume = _volume;
    }

    public void bgmPlayerPitchControl(float Pitch)
    {
        switch (Pitch)
        {
            case 1f:
                bgmPlayer.pitch = 1f;
                break;
            case 1.05f:
                bgmPlayer.pitch = 1.05f;
                break;
            case 1.1f:
                bgmPlayer.pitch = 1.1f;
                break;
        }
    }

    public void bgmAfterGameEnd(string endState)
    {
        if (endState.Equals("Success Screen"))
        {
            PlaySE("ClearSFX");
        }
        else if (endState.Equals("Fail Screen"))
        {
            PlaySE("FailSFX");
        }
    }

    public void sfxLimitFiveSec()
    {
        PlaySE("Limit5sec");
    }
}
