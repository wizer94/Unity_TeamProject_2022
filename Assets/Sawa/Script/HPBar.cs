using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------
//�{�X  HPBar
//---------------------------------------------------------

public class HPBar : MonoBehaviour
{
    [SerializeField] Canvas Gauge;
    [SerializeField] Image HP_Gauge;
    [SerializeField] Image Damage_Gauge;

    bool isShaveGauge;

    //---------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        //�t���O�̏�����
        isShaveGauge = false;

        Gauge.enabled = false;
    }
    //---------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        //�Q�[�W����鏈��
        if (isShaveGauge)
        {
            Shave_HPGauge();
        }
    }
    //---------------------------------------------------------
    //setter
    void setShaveGauge(bool Flag)
    {
        isShaveGauge = Flag;
    }
    public void Enabled_HPGauge(bool Flag)
    {
        Gauge.enabled = Flag;
    }
    //---------------------------------------------------------
    //getter
    public bool getShaveGauge()
    {
        return isShaveGauge;
    }
    //---------------------------------------------------------
    void Shave_HPGauge()
    {
        //�_���[�W�Q�[�W�����
        Damage_Gauge.fillAmount -= Time.deltaTime / 5; 

        //�_���[�W�Q�[�W�����݂�Hp�����̒����ɂȂ�����
        if (Damage_Gauge.fillAmount < HP_Gauge.fillAmount)
        {
            //��鏈�����I������
            setShaveGauge(false);

            //���I������̂ŃQ�[�W���\���ɂ���
            Damage_Gauge.enabled = false;
        }
    }
    //---------------------------------------------------------
    public void Change_HPGauge(float hp,float max_Hp)
    {
        Damage_Gauge.enabled = true;                        //�_���[�W�Q�[�W���g�p����̂ŕ\������
        Damage_Gauge.fillAmount = HP_Gauge.fillAmount;      //�_���[�W�Q�[�W��Fill��΂Ɠ��������ɂ���

        //���݂�Hp���X�V����i�΂̃Q�[�W�������Ȃ���̂ł����ŏ������s���j
        HP_Gauge.fillAmount = hp / max_Hp;  //UI���X�V����

        //�c��Q�[�W�ŐF��ς���
        if (HP_Gauge.fillAmount <= 0.2f)
        {
            //�����Ȃ�ԂɕύX����
            HP_Gauge.color = new Color(1, 0.5f, 0);
        }
        else if (HP_Gauge.fillAmount <= 0.5f)
        {
            //�����Ȃ物�F�ɕύX����
            HP_Gauge.color = new Color(1, 1, 0);
        }

        setShaveGauge(true);                //�t�O�𗧂Ă�@���@�Ԃ��Q�[�W�i�_���[�W�Q�[�W�j�����
    }
    //---------------------------------------------------------
    //---------------------------------------------------------
    //---------------------------------------------------------
    //---------------------------------------------------------
    //---------------------------------------------------------
}
