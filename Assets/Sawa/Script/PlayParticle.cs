using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�G�t�F�N�g�Đ��p

public class PlayParticle : MonoBehaviour
{
    ParticleSystem particle;
    Vector2 position;

    int cnt = 0;
    float Duartion = 0;
    bool isSetDuartion = false;

    public void Play(ParticleSystem particle, Vector2 pos)
    {
        //�G�t�F�N�g�𐶐�
        ParticleSystem tmp = Instantiate(particle, pos, Quaternion.identity);
        Particle par = tmp.GetComponent<Particle>();

        //���Ԃ�0�ȊO(�Đݒ�)�Ȃ�
        if (isSetDuartion)
        {
            par.setDuartion(Duartion);
        }
    }
    //���͂ɐ���
    public void PlayAround(ParticleSystem particle,Vector2 pos)
    {
        this.particle = particle;
        position = pos;

        //�G�t�F�N�g����
        for(int i = 0; i < 5; ++i)
        {
            //���W�̕ύX
            Invoke("PlayInvoke", 0.1f * i);
        }
    }
    //�����@+�@�ǔ�
    public void Play(ParticleSystem particle, Vector2 pos,GameObject TackObj)
    {
        //�G�t�F�N�g�𐶐�
        ParticleSystem tmp = Instantiate(particle, pos, Quaternion.identity);
        Particle par = tmp.GetComponent<Particle>();
        par.setTracking(TackObj);

        //���Ԃ�0�ȊO(�Đݒ�)�Ȃ�
        if (isSetDuartion)
        {
            par.setDuartion(Duartion);
        }
    }


    //���Ԃ̐ݒ�
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
        //�G�t�F�N�g�𐶐�
        Play(particle, tmp);

        cnt++;
    }
}
