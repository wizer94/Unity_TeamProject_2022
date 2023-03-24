using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//-----------------------------------------------------------------
//敵 ボス
//-----------------------------------------------------------------

public class Boss : EnemyClass
{
    //移動クラスをインスタンス化
    EMove move = new EMove();

    //武器
    [SerializeField] GameObject Weapon;
    [SerializeField] GameObject Weapon2;
    //攻撃クラス
    EAttack attack_Main;
    EAttack attack_Sub;

    //絵のスクリプト
    [SerializeField] EnemyImg ImgScript;

    [SerializeField] float TakeDist;    //攻撃時　プレイヤとの間の距離

    //攻撃関連の変数
    ColliderScript AttackAreaCol;

    NavMeshAgent Nav;

    bool[] NoMoveDir = new bool[4] { true, true, true, true };
    [SerializeField] ColliderScript[] MoveCol = new ColliderScript[4];     //スクリプト格納

    float DamageTimer;

    bool isRush = false;
    Vector3 RushPoint;               //突進目標地点  
    [SerializeField] ParticleSystem SuctionEff;
    bool isInsRushEff = false;

    //-----------------------------------------------------------------    
    //変更点
    public GameObject result;    
    //-----------------------------------------------------------------    
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

        //NavMeshの無効化
        Nav.enabled = false;

        //スナイパーのレイを消す
        attack_Main.setLayActive(false);

        //変更点
        result.SetActive(false);
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

                //HPゲージを変更する
                this.gameObject.GetComponent<HPBar>().Change_HPGauge(getHp(), getMaxHp());

            }

            //エフェクト時間の判定
            if (DamageTimer >= 0.1f)
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
            //NavMeshを切る
            Nav.enabled = false;

            //攻撃を止める
            attack_Main.setAttackFlag(false);
            attack_Sub.setAttackFlag(false);
            //レイを消す
            attack_Main.setLayActive(false);
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
        InitializeHP();
        //逃げるHP
        setEscHP(0.1f * getMaxHp());
    }
    //-----------------------------------------------------------------
    public override void Move()
    {
        //移動量
        Vector2 vec;

        //自身の状態を取得して分岐する
        switch (getState())
        {
            //無効状態-----------------------------
            case State.Non:
                //索敵の方向をリセットする
                move.setMoveDir(true);

                //HPゲージを消す
                this.gameObject.GetComponent<HPBar>().Enabled_HPGauge(false);

                //画像を変更する
                ImgScript.ChangeImage(false);
                break;
            //探索状態-----------------------------
            case State.Search:
                break;
            //追尾状態-----------------------------
            case State.AimPlayer:
                //ナビメッシュを起動させる
                if (!Nav.enabled)
                {
                    Nav.enabled = true;

                    attack_Main.setAttackFlag(true);
                    attack_Sub.setAttackFlag(true);
                    //レイを出す
                    attack_Main.setLayActive(true);
                }

                //目標を指定
                Nav.SetDestination(player.transform.position);

                //追尾中も攻撃を行う
                attack_Main.Fire(true);
                attack_Sub.Fire(true);

                break;
            //停止状態-----------------------------
            case State.Stop:
                //停止
                vec = move.Stop();

                //突進の準備を行う
                if (isRush)
                {
                    if (!OneceFlag)
                    {
                        //画像を変更する
                        ImgScript.ChangeImage(true);
                        //銃による攻撃は止める
                        attack_Main.setAttackFlag(false);
                        attack_Sub.setAttackFlag(false);
                        attack_Main.setLayActive(false);
                        OneceFlag = true;
                    }
                }
                break;
            //攻撃状態-----------------------------
            case State.Attack:
                //突進でない場合
                //銃による攻撃
                if (!OneceFlag)
                {
                    //画像を変更する
                    ImgScript.ChangeImage(true);
                    //攻撃開始
                    attack_Main.setAttackFlag(true);
                    attack_Sub.setAttackFlag(true);
                    attack_Main.setLayActive(true);
                    OneceFlag = true;
                }
                //攻撃　インスタンスした武器を使う
                attack_Main.Fire(true);
                attack_Sub.Fire(true);

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
            //突進状態-----------------------------
            case State.Rush:
                //目標地点を定める
                if (!OneceFlag)
                {
                    RushPoint = player.transform.position;
                    OneceFlag = true;
                }
                //突進を行う
                if (CheckNoMoveDir())
                {
                    vec = move.Rush(RushPoint, this.gameObject, getMoveSpeed() + 0.2f);
                    gameObject.transform.Translate(vec);
                }
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
                //無条件で停止にする
                st = State.Stop;

                break;
            //探索状態-----------------------------
            case State.Search:               
                break;
            //追尾状態-----------------------------
            case State.AimPlayer:

                //停止
                //プレイヤが攻撃範囲に入ったら
                if (AttackAreaCol.getAttackFlag())
                {
                    st = State.Stop;
                    this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    Nav.enabled = false;
                }
                //プレイヤと10以上離れていたら
                if(getPlayerDist() >= 10)
                {
                    this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    Nav.enabled = false;

                    //状態をStopに変更する
                    st = State.Stop;
                    //突進フラグを立てる
                    isRush = true;

                    //エフェクトを出す
                    if(!isInsRushEff)
                    {
                        ParticleSystem par = Instantiate(SuctionEff, gameObject.transform.position, Quaternion.identity);
                        par.GetComponent<TrackEff>().setTackObj(gameObject);
                        isInsRushEff = true;
                    }
                }
                break;
            //停止状態-----------------------------
            case State.Stop:
                //攻撃を受けたら
                if (getDamageHitFlag())
                {
                    st = State.AimPlayer;

                    //HPゲージを出す
                    this.gameObject.GetComponent<HPBar>().Enabled_HPGauge(true);
                }
                //攻撃　攻撃範囲内にプレイヤがいる
                if (getPlayerDist() <= 10)
                {
                    st = State.Attack;

                    //HPゲージを出す
                    this.gameObject.GetComponent<HPBar>().Enabled_HPGauge(true);
                }
                //突進
                if(MoveTimer >= 2 && isRush)
                {
                    st = State.Rush;
                }
                break;
            //攻撃状態-----------------------------
            case State.Attack:
                //追尾
                //プレイヤが攻撃範囲から出たら
                if (!AttackAreaCol.getAttackFlag())
                {
                    st = State.AimPlayer;

                    //攻撃を止める
                    attack_Main.setAttackFlag(false);
                }

                break;
            //逃げ状態-----------------------------
            case State.Escape:
                //状態遷移なし
                //　→　逃げ続ける

                break;
            //回避状態-----------------------------
            case State.Avoidance:
                //処理なし
                break;
            case State.Rush:
                //4秒間突進する
                if(MoveTimer >= 4)
                {
                    st = State.Stop;
                    isInsRushEff = false;
                }
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
                DropItem(this.gameObject.transform.position, getMaxDropCnt() + 1);
                //フラグをfalseにする
                setAppearFlag(false);

                //HPゲージを消す
                this.gameObject.GetComponent<HPBar>().Enabled_HPGauge(false);

                //エフェクトを表示
                par.PlayAround(destEff, this.gameObject.transform.position);
                //SEの再生
                se.Play(SE_Explos);

                //消滅する
                DestroyMe();
            }

            //被ダメフラグを立てる
            setDamageHitFlag(true);
        }
    }
    //-----------------------------------------------------------------
    //-----------------------------------------------------------------
    void DestroyMe()
    {
        GameObject Parent = transform.parent.gameObject;

        //消滅する
        kill(Parent);
    }
    //-----------------------------------------------------------------
    void ImgTurn()
    {
        Vector2 playerVector = player.transform.position - transform.position;
        bool isFlip = playerVector.x > 0;

       　//向きを変更する関数を呼ぶ
       　ImgScript.ImgTurn(isFlip);
    }
    //-----------------------------------------------------------------
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
    //-----------------------------------------------------------------
    //Start時にオブジェクトを取得する
    void GetObjects()
    {
        //オブジェクト取得クラスをインスタンス化
        ObjectGetClass GetObj = new ObjectGetClass();


        //攻撃範囲コライダーのスクリプトを取得
        GameObject AttackAreaObj = GetObj.GetChild_Obj(this.gameObject, "AttackArea");
        AttackAreaCol = AttackAreaObj.GetComponent<ColliderScript>();

        //プレイヤの取得
        player = GetObj.GetGameObject("player");
        //NavMeshの取得
        Nav = this.gameObject.GetComponent<NavMeshAgent>();

        //絵とオブジェクトを同期させる
        GameObject ImgObj = transform.parent.transform.GetChild(0).gameObject;
        setEnemyImg(ImgObj);

        //敵によってNavMeshの最高速度を決める
        if (ImgObj.name == "NormalEnemyImg")
        {
            Nav.speed = 4.0f;
        }
        else if (ImgObj.name == "BigEnemyImg")
        {
            Nav.speed = 2.0f;
        }
    }
    //-----------------------------------------------------------------
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

        //カメラスクリプト取得
        CameSc = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        //攻撃スクリプト
        attack_Main = Weapon.GetComponent<EAttack>();
        attack_Sub = Weapon2.GetComponent<EAttack>();
    }
    //-----------------------------------------------------------------
    void ZeroClear()
    {
        DamageTimer = 0;
        RushPoint = Vector2.zero;
    }
    //-----------------------------------------------------------------
    float getPlayerDist()
    {
        //プレイヤとの距離を計算
        Vector2 dt = transform.position - player.transform.position;
        float dir = Mathf.Sqrt(dt.x * dt.x + dt.y * dt.y);

        return Mathf.Abs(dir);
    }
    //-----------------------------------------------------------------
    public void OnDestroy()
    {
        result.SetActive(true);
    }
    //-----------------------------------------------------------------

}
