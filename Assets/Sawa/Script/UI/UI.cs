using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C����UI��ύX���鑋��

public class UI : MonoBehaviour
{
    [SerializeField] public PlayerUI PlUI;
    [SerializeField] avoidUI avoidUI;
    [SerializeField] GameObject GameOverEff;

    //�΃Q�[�W�����
    public void DecGauge(float HP,float maxHp)
    {
        PlUI.DecHPGauge(HP, maxHp);
    }

    //�ő�HP���ύX���ꂽ�Ƃ���UI�ύX
    public void HealHPGauge(float HP,float maxHp)
    {
        PlUI.ChHeal_HPGauge(HP, maxHp);
    }

    //�_���[�W���󂯂�����UI�ύX
    public void ChangeHPGauge(float HP,float maxHp)
    {
        PlUI.ChDamage_HPGauge(HP, maxHp);
    }

    public void ChangeW(string name)
    {
        PlUI.getWeaponObjs();
        PlUI.WeaponUI(name);
    }
    //����N���[�^�C��
    public void setAvoidUI()
    {
        avoidUI.setIsFill(true);
    }
    //�Q�[���I�[�o�[�G�t�F�N�g
    public void setGameOverEff()
    {
        GameObject tmp = Instantiate(GameOverEff);
        tmp.GetComponent<GameOverEff>().setIsPlay(true);
    }
}
