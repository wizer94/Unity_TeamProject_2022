using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�G����

public class DummyGenerator : MonoBehaviour
{
    //�_�~�[�v���t�@�u
    [SerializeField] GameObject DummyPrefab;
    //�G�t�F�N�g
    [SerializeField] ParticleSystem mist;
    //�����ʒu
    [SerializeField] GameObject[] Dummys;
    List<Vector2> InsPos = new List<Vector2>();
    List<float> GeneTimer = new List<float>();  //�Đ����^�C�}�[

    [SerializeField] float GSpan = 0;   //�Đ�������

    void Start()
    {
        //�o�^���ꂽ�_�~�[�v���t�@�u�̈ʒu��ۑ�
        for (int i = 0;i < Dummys.Length; ++i)
        {
            //���̒ǉ�
            addInfos(i);

            GeneTimer.Add(0);
        }
    }
    //-------------------------------------------------------------
    void Update()
    {
        //Destroy���ꂽ�I�u�W�F�N�g�̍Đ����^�C�}�[���J�E���g
        for(int i = 0;i < Dummys.Length; ++i)
        {
            if(Dummys[i] == null)
            {
                //�Đ����^�C�}�[���J�E���g
                GeneTimer[i] += Time.deltaTime;

                if(GeneTimer[i] >= GSpan)
                {
                    //�Đ���
                    InsDummys(i);
                    GeneTimer[i] = 0;
                }
            }
        }
    }
    //-------------------------------------------------------------
    public void InsDummys(int num)
    {
        //��Ȃ�
        if (Dummys[num] == null)
        {
            //���炩���ߕۑ����Ă����ʒu�ɐ�������
            GameObject tmp = Instantiate(DummyPrefab, InsPos[num], Quaternion.identity);
            //��̏ꏊ�ɒǉ�����
            Dummys[num] = tmp;

            //�G�t�F�N�g����������
            Vector2 EffPos = InsPos[num] + new Vector2(0, -0.5f);
            Instantiate(mist, EffPos, Quaternion.identity);
        }
    }
    //-------------------------------------------------------------
    void addInfos(int num)
    {
        InsPos.Add(Dummys[num].transform.position);
    }
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
}
