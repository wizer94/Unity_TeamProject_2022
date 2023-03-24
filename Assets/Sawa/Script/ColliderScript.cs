using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------------------------------
//コライダー用スクリプト
//-----------------------------------------------------------------

public class ColliderScript : MonoBehaviour
{
    GameObject parent;
    //移動
    [SerializeField] bool MoveFlag = true;
    //追尾
    [SerializeField] bool SearchFlag = false;
    //攻撃
    [SerializeField] bool AttackFlag = false;

    string ObjName;

    void Start()
    {
        ObjName = this.gameObject.name;

        parent = transform.root.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //移動用コライダー
        if(ObjName == "MoveColLeft" || ObjName == "MoveColRight" ||
           ObjName == "MoveColUp" || ObjName == "MoveColDown")
        {
            //壁 or 敵と接触していたら
            if (col.transform.tag == "Wall" || col.transform.tag == "Enemy")
            {
                MoveFlag = false;

                //ボスの場合一定時間でtrueに戻す
                if(parent.name == "BossPrefab")
                {
                    Invoke("ReTrue", 3);
                }
            }
        }

        //探索範囲
        if(ObjName == "SearchArea" && col.gameObject.tag == "player")
        {
            SearchFlag = true;
        }

        //攻撃範囲
        if(ObjName == "AttackArea" && col.gameObject.tag == "player")
        {
            AttackFlag = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        //壁と接触しなくなったら
        if (ObjName == "MoveColLeft" || ObjName == "MoveColRight" ||
           ObjName == "MoveColUp" || ObjName == "MoveColDown")
        {
            //壁 or 敵と接触していたら
            if (col.transform.tag == "Wall" || col.transform.tag == "Enemy")
            {
                MoveFlag = true;
            }
        }
        //探索範囲
        if (ObjName == "SearchArea" && col.gameObject.tag == "player")
        {
            SearchFlag = false;
        }

        //攻撃範囲
        if (ObjName == "AttackArea" && col.gameObject.tag == "player")
        {
            AttackFlag = false;
        }
    }

    public bool getMoveFlag()
    {
        return MoveFlag;
    }
    public bool getSearchFlag()
    {
        return SearchFlag;
    }
    public bool getAttackFlag()
    {
        return AttackFlag;
    }

    //Invoke用
    void ReTrue()
    {
        MoveFlag = true;
    }
}
