using System;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.Audio;


public class SoundManager : Singleton<SoundManager>
{
    // Resources/Sounds 폴더 안의 사운드 소스들을 저장하는 딕셔너리
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    
    private AudioMixer _audioMixer;

    private AudioSource _bgmAudioSource;
    private List<AudioSource> _effectAudioSources;

    public event Func<string, AudioClip> GetAudio;

    //private SettingManager _settingManager;

    private void Awake()
    {
        SetAudioMixer();
        LoadData();
        AddCallbacks();
    }

    private void SetAudioMixer()
    {
        _audioMixer = Resources.Load<AudioMixer>("AudioMixer/BasicAudioMixer");
        
        _effectAudioSources = new List<AudioSource>();
        
        _bgmAudioSource = gameObject.AddComponent<AudioSource>();
        _bgmAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("BGM")[0];
        _bgmAudioSource.dopplerLevel = 0;
    }
    
    private void AddCallbacks()
    {
        // if (_settingManager == null) _settingManager = SettingManager.Instance;
        //
        // _settingManager.OnDataChange += LoadData;
    }

    private void LoadData()
    {
        // if (_settingManager == null) _settingManager = SettingManager.Instance;
        // Debug.Assert(_settingManager != null, "Null Exception : SettingManager");
        //
        // SetVolume(_settingManager.GetBGMVolume(), Enums.AudioType.BGM);
        // SetVolume(_settingManager.GetSFXVolume(), Enums.AudioType.SFX);
    }

    private AudioClip LoadAudioClip(string name)
    {
        if (!audioClips.ContainsKey(name))
        {
            // 요청 시점에 사운드 클립이 없다면 Load
            AudioClip audioClip = GetAudio?.Invoke(name);
            if (audioClip == null)
            {
                Debug.LogError($"AudioClip 로드 실패 : {name}");
                return null;
            }
            audioClips.Add(name, audioClip);
        }
        return audioClips[name];
    }

    // 배경 음악 또는 효과음 재생
    // 배경 음악 재생 : SoundManager.Instance.Play("오디오 클립 이름", AudioType.BGM);
    // 효과음 재생 : SoundManager.Instance.Play("오디오 클립 이름");
    public void Play(string audioClipName, Enums.AudioType audioType = Enums.AudioType.SFX, bool isLooping = false)
    {
        AudioClip audioClip = LoadAudioClip(audioClipName);

        switch (audioType)
        {
            case Enums.AudioType.BGM:
                if (_bgmAudioSource.isPlaying) _bgmAudioSource.Stop();
                _bgmAudioSource.clip = audioClip;
                _bgmAudioSource.loop = true;
                _bgmAudioSource.Play();
                break;
            case Enums.AudioType.SFX:
                AudioSource effectAudioSource = GetSFXAudioSource();
                effectAudioSource.clip = audioClip;
                effectAudioSource.loop = isLooping;
                effectAudioSource.Play();
                break;
        }
    }

    private AudioSource GetSFXAudioSource()
    {
        foreach (var audioSource in _effectAudioSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("SFX")[0];
                audioSource.dopplerLevel = 0;
                return audioSource;
            }
        }

        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        _effectAudioSources.Add(newAudioSource);

        newAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("SFX")[0];

        return newAudioSource;
    }

    // 재생 중인 배경 음악 멈추기
    public void StopBGM()
    {
        if (_bgmAudioSource.isPlaying) _bgmAudioSource.Stop();
    }

    public void StopSFX(string audioClipName)
    {
        foreach (var audioSource in _effectAudioSources)
        {
            if (audioSource.isPlaying && audioSource.clip == audioClips[audioClipName])
            {
                audioSource.Stop(); 
            }
        }
    }

    public void StopSFX()
    {
        foreach (var audioSource in _effectAudioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop(); 
            }
        }
    }

    public void SetVolume(float volume, Enums.AudioType audioType)
    {
        float max = 0;
        
        switch (audioType)
        {
            case Enums.AudioType.BGM:
                max = 0;
                break;

            case Enums.AudioType.SFX:
                max = 0;
                break;
        }
        
        float normalizedVolume = Mathf.Clamp(volume / 100.0f, 0.0001f, 1.0f);
        float dB = Mathf.Log10(normalizedVolume) * 20;
    
        _audioMixer.SetFloat(audioType.ToString(), Mathf.Clamp(dB, -80, max));
    }

    public void FadeBGM(bool isFadeIn)  // 페이드 아웃 적용 시 BGM이 멈춤 -> 배경음악 필요한 시점에 다시 Play 필요
    {
        if (_bgmAudioSource == null || !_bgmAudioSource.isPlaying) return;
        float originalVolume = _bgmAudioSource.volume;

        if (isFadeIn)
        {
            _bgmAudioSource.volume = originalVolume;
            // _bgmAudioSource.DOFade(originalVolume, 1f).SetEase(Ease.OutCirc).SetUpdate(true).OnComplete(() =>
            // {
            //     _bgmAudioSource.volume = originalVolume;
            // });
        }
        else
        {
            // _bgmAudioSource.DOFade(0, 1f).SetEase(Ease.OutCirc).SetUpdate(true).OnComplete(() =>
            // {
            //     _bgmAudioSource.volume = originalVolume;
            //     StopBGM();
            // });
            _bgmAudioSource.volume = originalVolume;
            StopBGM();
        }
    }

    public bool IsPlaying(string audioClipName, Enums.AudioType audioType = Enums.AudioType.SFX)
    {
        switch (audioType)
        {
            case Enums.AudioType.BGM:
                return _bgmAudioSource.isPlaying && _bgmAudioSource.clip == audioClips[audioClipName];
            case Enums.AudioType.SFX:
                foreach (var audioSource in _effectAudioSources)
                {
                    if (audioSource.isPlaying && audioSource.clip == audioClips[audioClipName])
                    {
                        return true;
                    }
                }
                return false;
            default:
                return false;
        }
    }
}
