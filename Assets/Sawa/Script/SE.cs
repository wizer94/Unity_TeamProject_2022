using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour
{
    [SerializeField] AudioSource audio;

    //SEçƒê∂
    public void Play(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
    }
    //âπó ê›íË
    public void setVolume(float volume)
    {
        audio.volume = volume;
    }
}
