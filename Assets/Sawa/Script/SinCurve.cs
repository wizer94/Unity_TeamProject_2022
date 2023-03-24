using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinCurve : MonoBehaviour
{
    //���V�֘A-----------------------------------------------------------------------------------
    //�A�C�e�����V�Ɋւ���ϐ�
    const float Circumference = 2 * Mathf.PI;              //�~��
    float Around_Time;                              //����ɂ����鎞��
    float Wave;                                     //���g��
    float Circle_size;                              //�~�̑傫��

    Vector3 pos;

    private void Start()
    {
        //�A�C�e�����V�Ɋւ���ϐ��̏�����
        Around_Time = 1.5f;              //����ɂ����鎞��
        Wave = GetWave(Around_Time);     //���g�������߂�
        Circle_size = 0.45f;            //�~�̑傫��

        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveSinCurve();
    }

    //�T�C���J�[�u�ɉ����Ĉړ����郁�\�b�h----------------------------------------------------------------
    void MoveSinCurve()
    {
        //Sin�J�[�u���g�p����
        float sin = GetSinCurve();

        //�ړ�����
        if(Time.timeScale != 0)
        {
            transform.position = pos + new Vector3(0, sin, 0);
        }
    }
    //�T�C���J�[�u�̒l��Ԃ����\�b�h----------------------------------------------------------------
    private float GetSinCurve()
    {
        //�T�C���J�[�u��Ԃ�
        return Mathf.Sin(Circumference * Wave * Time.time) * Circle_size;
    }
    //���g����Ԃ����\�b�h---------------------------------------------------------------------------
    float GetWave(float Circle)  //�����@���@����
    {
        //���g����Ԃ�
        return 1 / Circle;
    }
}
