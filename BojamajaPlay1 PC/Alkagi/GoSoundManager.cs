using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class goSound
{
    public string soundName;
    public AudioClip clip;
}

public class GoSoundManager : MonoBehaviour
{
    public static GoSoundManager Instance { get; private set; }

    [Header("사운드 등록")]
    [SerializeField] goSound[] bgmSounds;
    [SerializeField] goSound[] sfxSounds;

    [Header("브금 플레이어")]
    [SerializeField] AudioSource bgmPlayer;

    [Header("효과음 플레이어")]
    [SerializeField] AudioSource[] sfxPlayer;

    public AudioSource levelUp;
    public AudioSource iconChange;

    [Header("효과음")]
    public AudioClip levelup_sound;
    public AudioClip iconChange_sound;



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
                //Debug.Log("All sound effects are in use!");
                return;
            }
        }
        //Debug.Log("There are no registered sound effects!");
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

    public void LevelUpSound()
    {
        levelUp.PlayOneShot(levelup_sound);

    }

    public void IconImageChange()
    {
        iconChange.PlayOneShot(iconChange_sound);
    }

    public void AllSoundPause()
    {
        bgmPlayer.Pause();
        for(int i=0; i< sfxPlayer.Length; i++)
            sfxPlayer[i].Pause();

        levelUp.Pause();
        iconChange.Pause();
    }

    public void AllSoundPlay()
    {
        bgmPlayer.UnPause();
        for (int i = 0; i < sfxPlayer.Length; i++)
            sfxPlayer[i].UnPause();

        levelUp.UnPause();
        iconChange.UnPause();
    }
}
