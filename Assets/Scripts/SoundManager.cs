using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] AudioClip[] bgms = null;
    [SerializeField] AudioClip[] effectSounds = null;
    private AudioSource bgmAudio;
    private AudioSource effectAudio;
    private AudioSource tutoEffectAudio;
    private void Awake()
    {
        SoundManager[] smanagers = FindObjectsOfType<SoundManager>();
        if (smanagers.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        bgmAudio = GetComponent<AudioSource>();
        effectAudio = transform.GetChild(0).GetComponent<AudioSource>();
        tutoEffectAudio = transform.GetChild(1).GetComponent<AudioSource>();
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "StartScene")
        {
            StartScene_VolumeSetting();
        }
    }

    public void StartScene_VolumeSetting()
    {
        string SAVE_PATH = Application.persistentDataPath + "/Save";
        string SAVE_FILENAME = "/SaveFile.txt";
        string json = "";
        if (File.Exists(SAVE_PATH + SAVE_FILENAME))
        {
            json = File.ReadAllText(SAVE_PATH + SAVE_FILENAME);
            User user = JsonUtility.FromJson<User>(json);

            bgmAudio.volume = user.bgmVolume;
            effectAudio.volume = user.effectVolume;
            tutoEffectAudio.volume = user.effectVolume;
            bgmAudio.mute = user.bgmMute;
            effectAudio.mute = user.effectMute;
            tutoEffectAudio.mute = user.effectMute;
        }
        else
        {
            return;
        }
    }

    public void VolumeSetting()
    {
        bgmAudio.volume = GameManager.Inst.CurrentUser.bgmVolume;
        effectAudio.volume = GameManager.Inst.CurrentUser.effectVolume;
        tutoEffectAudio.volume = GameManager.Inst.CurrentUser.effectVolume;
        bgmAudio.mute = GameManager.Inst.CurrentUser.bgmMute;
        effectAudio.mute = GameManager.Inst.CurrentUser.effectMute;
        tutoEffectAudio.mute = GameManager.Inst.CurrentUser.effectMute;
    }

    public void SetSpeedBGM(float speed)
    {
        bgmAudio.pitch = speed;
    }
    public void BGMVolume(float value)
    {
        if (bgmAudio == null) return;
        bgmAudio.volume = value;
        GameManager.Inst.CurrentUser.bgmVolume = value;
    }

    public void BGMMute(bool isMute)
    {
        bgmAudio.mute = isMute;
    }
    public void EffectMute(bool isMute)
    {
        effectAudio.mute = isMute;
        tutoEffectAudio.mute = isMute;
    }

    public void EffectVolume(float value)
    {
        if (effectAudio == null) return;
        effectAudio.volume = value;
        tutoEffectAudio.volume = value;
        GameManager.Inst.CurrentUser.effectVolume = value;
    }
    public void SetBGM(int bgmNum)
    {
        bgmAudio.Stop();
        bgmAudio.clip = bgms[bgmNum];
        bgmAudio.Play();
    }
    public void SetEffectSound(int effectNum)
    {
        effectAudio.Stop();

        effectAudio.clip = effectSounds[effectNum];
        effectAudio.Play();
    }

    public void SetTutoEffectAudio(int effectNum)
    {
        tutoEffectAudio.clip = effectSounds[effectNum];
        tutoEffectAudio.Play();
    }
    public void StopTutoEffect()
    {
        tutoEffectAudio.Stop();
    }

    public void StopBGM()
    {
        bgmAudio.Stop();
    }

}
