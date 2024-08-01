using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] AudioSource SFXplayer;//AudioSource播放器
    [SerializeField] AudioSource musicPlayer;//AudioSource播放器
    [SerializeField] float Min_PITH = 0.9f;
    [SerializeField] float Max_PITH = 1.1f;
    public void PlayerMusicAudio(AudioClip clip)
    {
        musicPlayer.clip = clip;
        musicPlayer.Play();
    }
    public void PlaySFXaudio(AudioData audioData)
    {
        SFXplayer.PlayOneShot(audioData.audioClip, audioData.volume);
    }
    //播放随机音调
    public void PlayRandomSFXaudio(AudioData audioData)
    {
        SFXplayer.pitch=Random.Range(Min_PITH, Max_PITH);
        PlaySFXaudio(audioData);
    }
    //播放随机音乐
    public void PlayRandomSFXaudio(AudioData[] audioData)
    {
        PlayRandomSFXaudio(audioData[Random.Range(0, audioData.Length)]);
    }
    public void MusicPlayerJumpForward(float seconds)
    {
        // 确保音频文件正在播放
        if (musicPlayer.isPlaying)
        {
            // 计算新的时间点
            float newTime = musicPlayer.time + seconds;

            // 确保新时间点在音频时长范围内
            if (newTime < musicPlayer.clip.length)
            {
                musicPlayer.time = newTime;
            }
            else
            {
                musicPlayer.time = musicPlayer.clip.length;
            }
        }
    }
    public void MusicPlayerVolume(float value)
    {
        musicPlayer.volume=value;
    }
    public void SFXPlayerVolume(float value)
    {
        SFXplayer.volume=value;
    }
    public float GetSfxVolume => SFXplayer.volume;
    public float GetMusicVolume => musicPlayer.volume;
}
[System.Serializable]
public class AudioData// [System.Serializable]  序列化到编辑器中
{
    public AudioClip audioClip;
    public float volume;
}
