using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackEff : MonoBehaviour
{
    [SerializeField] GameObject TrackObj;
    [SerializeField] Vector2 OffsetPos;

    ParticleSystem particle;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //�ǔ���̑��݂��m���߂�
        if(TrackObj != null)
        {
            particle.transform.position = (Vector2)TrackObj.transform.position + OffsetPos;
        }
        else
        {
            //�ǔ��悪�Ȃ��Ȃ����
            Destroy(this.gameObject);
        }
    }

    public void TurnEff(bool isFlip, float ScaleX)
    {
        //�G�t�F�N�g�𔽓]
        particle.transform.localScale = new Vector3(ScaleX, 1, 1);

        //Offset�l�̕�����ς���
        OffsetPos.x *= -1;
    }

    //�G�t�F�N�g�̕\���ύX
    public void setActive(bool Flag)
    {
        if (!Flag)
        {
            particle.Stop();
        }
        else
        {
            particle.Play();
        }
    }

    public void setTackObj(GameObject obj)
    {
        TrackObj = obj;
    } 
}
