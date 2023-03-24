using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//-----------------------------------------------------------------
//オブジェクト　取得クラス
//-----------------------------------------------------------------

public class ObjectGetClass : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------
    //オブジェクトを取得するメソッド
    public GameObject GetGameObject(string ObjName)
    {
        //オブジェクトを取得する
        GameObject Obj = GameObject.Find(ObjName);

        //見つからなかった場合ゲームを終了する
        if (Obj == null)
        {
            Debug.Log("Object is null");
        }

        return Obj;
    }
    //---------------------------------------------------------------------------------------------------------
    //オブジェクトをタグで取得するメソッド
    public GameObject GetGameObject_Tag(string ObjName)
    {
        //オブジェクトを取得する
        GameObject Obj = GameObject.FindWithTag(ObjName);

        //見つからなかった場合ゲームを終了する
        if (Obj == null)
        {
            Debug.Log("ObjectTag is null");
        }

        return Obj;
    }
    //---------------------------------------------------------------------------------------------------------
    //子オブジェクトを取得するメソッド
    public GameObject GetChild_Obj(GameObject Obj, string ChildName)
    {
        //子オブジェクトを取得する
        GameObject child = Obj.transform.Find(ChildName).gameObject;

        //見つからなかった場合ゲームを終了する
        if (child == null)
        {
            Debug.Log("Children is null");
        }

        //引数の名前の子オブジェクトを返す
        return child;
    }
    //---------------------------------------------------------------------------------------------------------
    // 親オブジェクトを取得するメソッド
    public GameObject GetParent()
    {
        GameObject par = transform.parent.gameObject;

        if(par == null)
        {
            Debug.Log("親オブジェクトを取得できませんでした");
        }

        return par;
    }
    //---------------------------------------------------------------------------------------------------------
    //SpriteRendererを取得メソッド
    public SpriteRenderer Get_SRen(GameObject Obj)
    {
        //引数のオブジェクトのSpriteRendererを取得する
        SpriteRenderer sr = Obj.GetComponent<SpriteRenderer>();

        //見つからなかった場合ゲームを終了する
        if (sr == null)
        {
            Debug.Log("SpriteRenderer is null");
        }

        //SpriteRendererを返す
        return sr;
    }

    //---------------------------------------------------------------------------------------------------------
    //Rigidbodyを取得メソッド
    public Rigidbody2D GetRigid2D(GameObject obj)
    {
        Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();

        //見つからなかった場合ゲームを終了する
        if (rigid == null)
        {
            Debug.Log("Rigidbody is null");
        }

        return rigid;
    }

    //---------------------------------------------------------------------------------------------------------
    //Colliderを取得メソッド
    public Collider2D GetCollider2D(GameObject obj)
    {
        Collider2D Col = obj.GetComponent<Collider2D>();

        //見つからなかった場合ゲームを終了する
        if (Col == null)
        {
            Debug.Log("Collider2D is null");
        }

        return Col;
    }
    //---------------------------------------------------------------------------------------------------------
    //CapsuleColliderを取得メソッド
    public CapsuleCollider2D GetCapsuleCollider2D(GameObject obj)
    {
        CapsuleCollider2D col = obj.GetComponent<CapsuleCollider2D>();

        if (col == null)
        {
            Debug.Log("CapsuleCollider2D is null");
        }

        return col;
    }
    //---------------------------------------------------------------------------------------------------------
    //BoxColliderを取得メソッド
    public BoxCollider2D GetBoxCollider2D(GameObject obj)
    {
        BoxCollider2D Col = obj.GetComponent<BoxCollider2D>();

        //見つからなかった場合ゲームを終了する
        if (Col == null)
        {
            Debug.Log("BoxCollider2D is null");
        }

        return Col;
    }
    //---------------------------------------------------------------------------------------------------------
    //アニメーターを取得するメソッド
    public Animator GetAnim(GameObject obj)
    {
        return obj.GetComponent<Animator>();
    }
    //---------------------------------------------------------------------------------------------------------
    //Imageを取得するメソッド
    public Image GetImage(GameObject obj)
    {
        return obj.GetComponent<Image>();
    }
    //---------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
}
