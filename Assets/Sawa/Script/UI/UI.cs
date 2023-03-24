using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤのUIを変更する窓口

public class UI : MonoBehaviour
{
    [SerializeField] public PlayerUI PlUI;
    [SerializeField] avoidUI avoidUI;
    [SerializeField] GameObject GameOverEff;

    //緑ゲージを削る
    public void DecGauge(float HP,float maxHp)
    {
        PlUI.DecHPGauge(HP, maxHp);
    }

    //最大HPが変更されたときのUI変更
    public void HealHPGauge(float HP,float maxHp)
    {
        PlUI.ChHeal_HPGauge(HP, maxHp);
    }

    //ダメージを受けた時のUI変更
    public void ChangeHPGauge(float HP,float maxHp)
    {
        PlUI.ChDamage_HPGauge(HP, maxHp);
    }

    public void ChangeW(string name)
    {
        PlUI.getWeaponObjs();
        PlUI.WeaponUI(name);
    }
    //回避クルータイム
    public void setAvoidUI()
    {
        avoidUI.setIsFill(true);
    }
    //ゲームオーバーエフェクト
    public void setGameOverEff()
    {
        GameObject tmp = Instantiate(GameOverEff);
        tmp.GetComponent<GameOverEff>().setIsPlay(true);
    }
}
