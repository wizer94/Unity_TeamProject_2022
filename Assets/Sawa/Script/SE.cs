using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour
{
    [SerializeField] AudioSource audio;

    //SE�Đ�
    public void Play(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
    }
    //���ʐݒ�
    public void setVolume(float volume)
    {
        audio.volume = volume;
    }
}
