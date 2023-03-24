using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------
//ボス  HPBar
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
        //フラグの初期化
        isShaveGauge = false;

        Gauge.enabled = false;
    }
    //---------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        //ゲージを削る処理
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
        //ダメージゲージを削る
        Damage_Gauge.fillAmount -= Time.deltaTime / 5; 

        //ダメージゲージが現在のHp未満の長さになったら
        if (Damage_Gauge.fillAmount < HP_Gauge.fillAmount)
        {
            //削る処理を終了する
            setShaveGauge(false);

            //削り終わったのでゲージを非表示にする
            Damage_Gauge.enabled = false;
        }
    }
    //---------------------------------------------------------
    public void Change_HPGauge(float hp,float max_Hp)
    {
        Damage_Gauge.enabled = true;                        //ダメージゲージを使用するので表示する
        Damage_Gauge.fillAmount = HP_Gauge.fillAmount;      //ダメージゲージのFillを緑と同じ長さにする

        //現在のHpを更新する（緑のゲージをいきなり削るのでここで処理を行う）
        HP_Gauge.fillAmount = hp / max_Hp;  //UIを更新する

        //残りゲージで色を変える
        if (HP_Gauge.fillAmount <= 0.2f)
        {
            //半分なら赤に変更する
            HP_Gauge.color = new Color(1, 0.5f, 0);
        }
        else if (HP_Gauge.fillAmount <= 0.5f)
        {
            //半分なら黄色に変更する
            HP_Gauge.color = new Color(1, 1, 0);
        }

        setShaveGauge(true);                //フグを立てる　→　赤いゲージ（ダメージゲージ）を削る
    }
    //---------------------------------------------------------
    //---------------------------------------------------------
    //---------------------------------------------------------
    //---------------------------------------------------------
    //---------------------------------------------------------
}
