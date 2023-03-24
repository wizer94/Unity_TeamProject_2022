using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//エフェクト再生用

public class PlayParticle : MonoBehaviour
{
    ParticleSystem particle;
    Vector2 position;

    int cnt = 0;
    float Duartion = 0;
    bool isSetDuartion = false;

    public void Play(ParticleSystem particle, Vector2 pos)
    {
        //エフェクトを生成
        ParticleSystem tmp = Instantiate(particle, pos, Quaternion.identity);
        Particle par = tmp.GetComponent<Particle>();

        //時間が0以外(再設定)なら
        if (isSetDuartion)
        {
            par.setDuartion(Duartion);
        }
    }
    //周囲に生成
    public void PlayAround(ParticleSystem particle,Vector2 pos)
    {
        this.particle = particle;
        position = pos;

        //エフェクト生成
        for(int i = 0; i < 5; ++i)
        {
            //座標の変更
            Invoke("PlayInvoke", 0.1f * i);
        }
    }
    //生成　+　追尾
    public void Play(ParticleSystem particle, Vector2 pos,GameObject TackObj)
    {
        //エフェクトを生成
        ParticleSystem tmp = Instantiate(particle, pos, Quaternion.identity);
        Particle par = tmp.GetComponent<Particle>();
        par.setTracking(TackObj);

        //時間が0以外(再設定)なら
        if (isSetDuartion)
        {
            par.setDuartion(Duartion);
        }
    }


    //時間の設定
    public void setDuartion(float time)
    {
        isSetDuartion = true;
        Duartion = time;
    }
    //Invoke
    void PlayInvoke()
    {
        Vector2[] InsPos = new Vector2[] {new Vector2(-1,-0.6f), new Vector2(0.5f,0.9f), new Vector2(1,-0.4f),
                                          new Vector2(-0.3f,0.8f),new Vector2(0.8f,-0.6f)};

        Vector2 tmp = position += InsPos[cnt];
        //エフェクトを生成
        Play(particle, tmp);

        cnt++;
    }
}
