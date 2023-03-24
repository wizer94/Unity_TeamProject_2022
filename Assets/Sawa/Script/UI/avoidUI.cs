using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class avoidUI : MonoBehaviour
{
    //�}�[�N
    [SerializeField] Image Mark;

    PlayerController PC;

    //�Q�[�W�����t���O
    bool isFill;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        PC = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerController>();
        isFill = false;
        timer = 0;

        setActiveMark(false);
    }

    // Update is called once per frame
    void Update()
    {
        //����N�[���_�E�����Ȃ�
        if(getIsFill())
        {
            //�Q�[�W�𑝉�����
            FillGauge();
        }
    }

    //-------------------------------------------------------------
    //Setter
    public void setIsFill(bool Flag)
    {
        isFill = Flag;
    }
    void setActiveMark(bool Flag)
    {
        Mark.enabled = Flag;
    }
    //-------------------------------------------------------------
    //Getter
    public bool getIsFill()
    {
        return isFill;
    }
    //-------------------------------------------------------------
    void FillGauge()
    {
        //�v���C�����������Ԃ��擾����
        float maxTime = PC.dodge_enalbetime;    //�N���[�^�C��
        float dodgeTime = PC.next_dodgetime;    //�J�E���g

        //0.5�b����Fill���X�^�[�g����
        if(timer >= 0.3f)
        {
            setActiveMark(true);
            Mark.fillAmount = dodgeTime / maxTime;
        }
        else
        {
            setActiveMark(false);
        }

        //�N���[�^�C�����I�������(�ŏ�0.5�b�͔�\���ɂ��Ȃ�)
        if(dodgeTime >= maxTime && timer >= 0.5f)
        {
            setIsFill(false);        
            //�}�[�N�̐F��߂�
            timer = 0;
            setActiveMark(false);
        }

        timer += Time.deltaTime;
    }
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------
    //-------------------------------------------------------------

}
