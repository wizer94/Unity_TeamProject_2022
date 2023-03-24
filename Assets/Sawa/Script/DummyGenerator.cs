using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵生成

public class DummyGenerator : MonoBehaviour
{
    //ダミープレファブ
    [SerializeField] GameObject DummyPrefab;
    //エフェクト
    [SerializeField] ParticleSystem mist;
    //生成位置
    [SerializeField] GameObject[] Dummys;
    List<Vector2> InsPos = new List<Vector2>();
    List<float> GeneTimer = new List<float>();  //再生成タイマー

    [SerializeField] float GSpan = 0;   //再生成時間

    void Start()
    {
        //登録されたダミープレファブの位置を保存
        for (int i = 0;i < Dummys.Length; ++i)
        {
            //情報の追加
            addInfos(i);

            GeneTimer.Add(0);
        }
    }
    //-------------------------------------------------------------
    void Update()
    {
        //Destroyされたオブジェクトの再生成タイマーをカウント
        for(int i = 0;i < Dummys.Length; ++i)
        {
            if(Dummys[i] == null)
            {
                //再生成タイマーをカウント
                GeneTimer[i] += Time.deltaTime;

                if(GeneTimer[i] >= GSpan)
                {
                    //再生成
                    InsDummys(i);
                    GeneTimer[i] = 0;
                }
            }
        }
    }
    //-------------------------------------------------------------
    public void InsDummys(int num)
    {
        //空なら
        if (Dummys[num] == null)
        {
            //あらかじめ保存していた位置に生成する
            GameObject tmp = Instantiate(DummyPrefab, InsPos[num], Quaternion.identity);
            //空の場所に追加する
            Dummys[num] = tmp;

            //エフェクトも生成する
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
