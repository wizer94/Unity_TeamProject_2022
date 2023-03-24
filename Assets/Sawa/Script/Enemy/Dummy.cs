using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : EnemyClass
{
    //絵のスクリプト
    [SerializeField] EnemyImg ImgScript;

    float DamageTimer;

    //-----------------------------------------------------------------    
    void Start()
    {
        //出現させる
        setAppearFlag(true);

        //初期化
        Initialize();

        //オブジェクト取得
        GetObjects();

        //スクリプト取得
        GetScripts();
    }

    //-----------------------------------------------------------------
    void Update()
    {
        //エフェクト時間
        if (getDamageHitFlag())
        {
            //最初なら
            if (!ImgScript.getDamageEffFlag())
            {
                //フラグを変える
                ImgScript.setDamageEffFlag(true);
            }

            //エフェクト時間の判定
            if (DamageTimer >= 0.2f)
            {
                DamageTimer = 0;
                setDamageHitFlag(false);

                //エフェクトを終了する
                ImgScript.setDamageEffFlag(false);
            }
            DamageTimer += Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        //生存・無効化の判定
        if (getAppearFlag())
        {
            Think();
            Move();
        }
        else
        {
            //コライダーを切る
            ImgScript.setActiveCollider(false);

            //αを設定する
            ImgScript.ChengeAlphaDec();
        }

        //画像の向きの変更
        ImgTurn();
    }
    //-----------------------------------------------------------------
    //各種情報の設定
    void Initialize()
    {
        //ゼロクリア
        ZeroClear();

        //移動速度
        setMoveSpeed(getMoveSpeed());
        //状態の設定
        setState(State.Non);
        //HP
        setHP(getMaxHp());
    }
    //-----------------------------------------------------------------
    public override void Move()
    {
        //自身の状態を取得して分岐する
        switch (getState())
        {
            //無効状態-----------------------------
            case State.Non:
                break;
            //探索状態-----------------------------
            case State.Search:
                break;
            //追尾状態-----------------------------
            case State.AimPlayer:
                break;
            //停止状態-----------------------------
            case State.Stop:
                break;
            //攻撃状態-----------------------------
            case State.Attack:
                break;
            //逃げ状態-----------------------------
            case State.Escape:
                break;
            //回避状態-----------------------------
            case State.Avoidance:
                //処理なし
                break;
            //デフォルト-----------------------------
            default:
                //基本ここに来ることはない
                Debug.Log("Move-デフォルト");
                break;
        }
    }

    //-----------------------------------------------------------------
    public override void Think()
    {
        //状態を一時保存する変数
        State st = getState();

        //自身の状態を取得して分岐する
        switch (getState())
        {
            //無効状態-----------------------------
            case State.Non:

                //無条件で停止状態にする
                st = State.Stop;
                break;
            //探索状態-----------------------------
            case State.Search:
                break;
            //追尾状態-----------------------------
            case State.AimPlayer:
                break;
            //停止状態-----------------------------
            case State.Stop:
                break;
            //攻撃状態-----------------------------
            case State.Attack:
                break;
            //逃げ状態-----------------------------
            case State.Escape:
                break;
            //回避状態-----------------------------
            case State.Avoidance:
                break;
            //デフォルト-----------------------------
            default:
                //基本ここに来ることはない
                Debug.Log("Think-デフォルト");
                break;
        }

        //タイマーをカウント
        MoveTimer += Time.deltaTime;

        //状態を変更する関数を呼ぶ
        ChangeState(st);
    }
    //-----------------------------------------------------------------
    void setEnemyImg(GameObject g)
    {
        enemyImg = g;
    }
    public GameObject getEnemyImg()
    {
        return enemyImg;
    }
    //-----------------------------------------------------------------
    public void HitDamage(float damage)
    {
        //回避状態でない && 生きている
        if (getAppearFlag())
        {
            float tempHP = getHp() - damage;

            //HPを変更する
            setHP(tempHP);

            //エフェクトを表示
            par.Play(HitDamageEff, this.gameObject.transform.position);
            //SE再生
            se.Play(SE_hit);

            //残りHPの判定　&&　生存していたら
            if (getHp() <= 0 && getAppearFlag())
            {
                //アイテムを落とす
                int DropCnt = Random.Range(1, getMaxDropCnt() + 1);
                DropItem(this.gameObject.transform.position, DropCnt);
                //フラグをfalseにする
                setAppearFlag(false);

                //消滅する
                DestroyMe();

                //エフェクトを表示
                par.PlayAround(destEff, this.gameObject.transform.position);
                //SEの再生
                se.Play(SE_Explos);
            }

            //被ダメフラグを立てる
            setDamageHitFlag(true);
        }
    }
    //-----------------------------------------------------------------
    void DestroyMe()
    {
        GameObject Parent = transform.parent.gameObject;
       
        //消滅する（ダミーはキル数に含まないのでそのままDestroyする）
        Destroy(Parent, 2);
    }
    //-----------------------------------------------------------------
    void ImgTurn()
    {
        Vector2 playerVector = player.transform.position - transform.position;
        bool isFlip = playerVector.x < 0;

       　//向きを変更する関数を呼ぶ
       　ImgScript.ImgTurn(isFlip);
    }
    //-----------------------------------------------------------------
    //Start時にオブジェクトを取得する
    void GetObjects()
    {
        //プレイヤの取得
        player = GameObject.FindGameObjectWithTag("player");

        //絵とオブジェクトを同期させる
        GameObject ImgObj = transform.parent.transform.GetChild(0).gameObject;
        setEnemyImg(ImgObj);
    }
    //-----------------------------------------------------------------
    void GetScripts()
    {
        //絵のスクリプトを取得
        ImgScript = enemyImg.GetComponent<EnemyImg>();
    }
    //-----------------------------------------------------------------
    void ZeroClear()
    {
        DamageTimer = 0;
    }
    //-----------------------------------------------------------------
    //-----------------------------------------------------------------
}
