using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : EnemyClass
{   
    //移動クラスをインスタンス化
    EMove move = new EMove();

    //武器
    [SerializeField] GameObject Weapon;
    //攻撃クラス
    [SerializeField] EAttack attack;

    //絵のスクリプト
    [SerializeField] EnemyImg ImgScript;

    [SerializeField] float TakeDist;    //攻撃時　プレイヤとの間の距離

    bool[] NoMoveDir = new bool[4] { true, true, true, true };
    [SerializeField] ColliderScript[] MoveCol = new ColliderScript[4];     //スクリプト格納

    float DamageTimer;

    //---------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        //出現させる
        setAppearFlag(true);

        //初期化
        Initialize();
        move.EMoveInitialize();

        //オブジェクト取得
        GetObjects();

        //スクリプト取得
        GetScripts();

        //スナイパーだけSEの音量を上げる
        se.setVolume(0.2f);
    }
    //---------------------------------------------------------------------
    // Update is called once per frame
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
    //---------------------------------------------------------------------
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
            //攻撃を止める
            attack.setAttackFlag(false);
            attack.setLayActive(false);

            //コライダーを切る
            ImgScript.setActiveCollider(false);

            //αを設定する
            ImgScript.ChengeAlphaDec();
        }

        //画像の向きの変更
        ImgTurn();
    }
    //---------------------------------------------------------------------
    void Initialize()
    {
        //ゼロクリア
        ZeroClear();

        //移動速度
        setMoveSpeed(getMoveSpeed());
        //状態の設定
        setState(State.Non);
        //HP
        InitializeHP();
        //逃げるHP
        setEscHP(0.1f * getMaxHp());
    }
    //---------------------------------------------------------------------
    public override void Move()
    {
        //移動量
        Vector2 vec;

        //自身の状態を取得して分岐する
        switch (getState())
        {
            //無効状態-----------------------------
            case State.Non:
                //処理なし
                break;
            //探索状態-----------------------------
            case State.Search:
                //処理なし
                break;
            //追尾状態-----------------------------
            case State.AimPlayer:
                //処理なし
                break;
            //停止状態-----------------------------
            case State.Stop:

                if (!OneceFlag)
                {
                    attack.setAttackFlag(false);

                    //レイを消す
                    attack.setLayActive(false);

                    //画像を変更する
                    ImgScript.ChangeImage(false);

                    OneceFlag = true;
                }

                break;
            //攻撃状態-----------------------------
            case State.Attack:
                if (!OneceFlag)
                {
                    attack.setAttackFlag(true);
                    //レイを出す
                    attack.setLayActive(true);

                    //画像を変更する
                    ImgScript.ChangeImage(true);

                    OneceFlag = true;
                }

                attack.Fire(false);

                //一定距離を取る
                if (HitCircle(this.gameObject, player, TakeDist))
                {
                    if (CheckNoMoveDir())
                    {
                        vec = move.TakeDistance(player, this.gameObject, getMoveSpeed());
                        gameObject.transform.Translate(vec);
                    }
                }

                break;
            //逃げ状態-----------------------------
            case State.Escape:
                //処理なし
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
    //---------------------------------------------------------------------
    public override void Think()
    {
        //状態を一時保存する変数
        State st = getState();

        //自身の状態を取得して分岐する
        switch (getState())
        {
            //無効状態-----------------------------
            case State.Non:

                //無条件
                st = State.Stop;

                break;
            //探索状態-----------------------------
            case State.Search:
                //処理なし
                break;
            //追尾状態-----------------------------
            case State.AimPlayer:
                //処理なし
                break;
            //停止状態-----------------------------
            case State.Stop:

                //プレイヤが一定範囲内にいる
                if (HitCircle(this.gameObject, player, 25))
                {
                    st = State.Attack;
                }
                break;
            //攻撃状態-----------------------------
            case State.Attack:
                //プレイヤが一定範囲内にいない
                if (!HitCircle(this.gameObject, player, 25))
                {
                    st = State.Stop;
                }

                break;
            //逃げ状態-----------------------------
            case State.Escape:
                //処理なし
                break;
            //回避状態-----------------------------
            case State.Avoidance:
                //処理なし
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
    //---------------------------------------------------------------------
    void setEnemyImg(GameObject g)
    {
        enemyImg = g;
    }
    public GameObject getEnemyImg()
    {
        return enemyImg;
    }
    //---------------------------------------------------------------------
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

                //エフェクトを表示
                par.PlayAround(destEff, this.gameObject.transform.position);
                //SEの再生
                se.setVolume(0.2f);
                se.Play(SE_Explos);

                //消滅する
                DestroyMe();
            }

            //被ダメフラグを立てる
            setDamageHitFlag(true);
        }
    }
    //---------------------------------------------------------------------
    void DestroyMe()
    {
        GameObject Parent = transform.parent.gameObject;

        //消滅する
        kill(Parent);
    }
    //---------------------------------------------------------------------
    void ImgTurn()
    {
        Vector2 playerVector = player.transform.position - transform.position;
        bool isFlip = playerVector.x < 0;

       　//向きを変更する関数を呼ぶ
       　ImgScript.ImgTurn(isFlip);
    }
    //---------------------------------------------------------------------
    //Start時にオブジェクトを取得する
    void GetObjects()
    {
        //オブジェクト取得クラスをインスタンス化
        ObjectGetClass GetObj = new ObjectGetClass();

        //プレイヤの取得
        player = GetObj.GetGameObject("player");

        //絵とオブジェクトを同期させる
        GameObject ImgObj = transform.parent.transform.GetChild(0).gameObject;
        setEnemyImg(ImgObj);
    }
    //---------------------------------------------------------------------
    void GetScripts()
    {
        //絵のスクリプトを取得
        ImgScript = enemyImg.GetComponent<EnemyImg>();


        //子オブジェクト
        GameObject[] temp = new GameObject[4] {
            transform.Find("MoveColLeft").gameObject,
            transform.Find("MoveColRight").gameObject,
            transform.Find("MoveColUp").gameObject,
            transform.Find("MoveColDown").gameObject
        };
        //  スクリプト取得
        for (int i = 0; i < 4; i++)
        {
            MoveCol[i] = temp[i].GetComponent<ColliderScript>();
        }
        //攻撃スクリプト
        attack = Weapon.GetComponent<EAttack>();
    }
    //---------------------------------------------------------------------
    void ZeroClear()
    {
        DamageTimer = 0;
    }
    //---------------------------------------------------------------------
    bool CheckNoMoveDir()
    {
        //四方向を確認
        for (int i = 0; i < 4; ++i)
        {
            NoMoveDir[i] = MoveCol[i].getMoveFlag();
        }

        //四方向確認する
        return NoMoveDir[0] && NoMoveDir[1] && NoMoveDir[2] && NoMoveDir[3];
    }
    //---------------------------------------------------------------------
}
