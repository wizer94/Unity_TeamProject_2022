using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] float Limit;
    float Timer;

    GameObject TrackingObj;
    bool isTracking = false;

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if(Timer >= Limit)
        {
            Destroy(this.gameObject);
        }

        //�ǔ��ݒ�
        if (isTracking)
        {
            this.gameObject.transform.position = TrackingObj.transform.position;
        }
    }

    //�ǔ��ݒ�
    public void setTracking(GameObject TrackObj)
    {
        isTracking = true;
        TrackingObj = TrackObj;
    }
    //�A�j���[�V�������Ԃ̐ݒ�
    public void setDuartion(float Duration)
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        particle.Stop();    //�~�߂Ȃ��Ɛݒ�ł��Ȃ� 

        var m = particle.main;
        m.duration = Duration;

        particle.Play();
    }
}
