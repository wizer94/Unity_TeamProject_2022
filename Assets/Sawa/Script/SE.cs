using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour
{
    [SerializeField] AudioSource audio;

    //SE再生
    public void Play(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
    }
    //音量設定
    public void setVolume(float volume)
    {
        audio.volume = volume;
    }
}
