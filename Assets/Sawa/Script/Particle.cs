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

        //追尾設定
        if (isTracking)
        {
            this.gameObject.transform.position = TrackingObj.transform.position;
        }
    }

    //追尾設定
    public void setTracking(GameObject TrackObj)
    {
        isTracking = true;
        TrackingObj = TrackObj;
    }
    //アニメーション時間の設定
    public void setDuartion(float Duration)
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        particle.Stop();    //止めないと設定できない 

        var m = particle.main;
        m.duration = Duration;

        particle.Play();
    }
}
