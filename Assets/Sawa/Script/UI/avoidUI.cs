using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class avoidUI : MonoBehaviour
{
    //マーク
    [SerializeField] Image Mark;

    PlayerController PC;

    //ゲージ増加フラグ
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
        //回避クールダウン中なら
        if(getIsFill())
        {
            //ゲージを増加する
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
        //プレイヤから回避時間を取得する
        float maxTime = PC.dodge_enalbetime;    //クルータイム
        float dodgeTime = PC.next_dodgetime;    //カウント

        //0.5秒からFillをスタートする
        if(timer >= 0.3f)
        {
            setActiveMark(true);
            Mark.fillAmount = dodgeTime / maxTime;
        }
        else
        {
            setActiveMark(false);
        }

        //クルータイムが終わったら(最初0.5秒は非表示にしない)
        if(dodgeTime >= maxTime && timer >= 0.5f)
        {
            setIsFill(false);        
            //マークの色を戻す
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
