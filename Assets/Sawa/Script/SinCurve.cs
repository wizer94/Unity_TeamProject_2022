using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinCurve : MonoBehaviour
{
    //浮遊関連-----------------------------------------------------------------------------------
    //アイテム浮遊に関する変数
    const float Circumference = 2 * Mathf.PI;              //円周
    float Around_Time;                              //一周にかかる時間
    float Wave;                                     //周波数
    float Circle_size;                              //円の大きさ

    Vector3 pos;

    private void Start()
    {
        //アイテム浮遊に関する変数の初期化
        Around_Time = 1.5f;              //一周にかかる時間
        Wave = GetWave(Around_Time);     //周波数を求める
        Circle_size = 0.45f;            //円の大きさ

        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveSinCurve();
    }

    //サインカーブに応じて移動するメソッド----------------------------------------------------------------
    void MoveSinCurve()
    {
        //Sinカーブを使用する
        float sin = GetSinCurve();

        //移動する
        if(Time.timeScale != 0)
        {
            transform.position = pos + new Vector3(0, sin, 0);
        }
    }
    //サインカーブの値を返すメソッド----------------------------------------------------------------
    private float GetSinCurve()
    {
        //サインカーブを返す
        return Mathf.Sin(Circumference * Wave * Time.time) * Circle_size;
    }
    //周波数を返すメソッド---------------------------------------------------------------------------
    float GetWave(float Circle)  //引数　→　周期
    {
        //周波数を返す
        return 1 / Circle;
    }
}
