using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//-----------------------------------------------------------------
//敵 通常の敵
//-----------------------------------------------------------------

public class NormalEnemy : EnemyClass
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

    //探索関連の変数
    ColliderScript SearchAreaCol;
    //攻撃関連の変数
    ColliderScript AttackAreaCol;

    NavMeshAgent Nav;

    bool[] NoMoveDir = new bool[4] { true, true, true, true};
    [SerializeField]ColliderScript[] MoveCol = new ColliderScript[4];     //スクリプト格納

    float DamageTimer;

    bool isSearch;  //探索状態になるか

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
    }

    //-----------------------------------------------------------------
    void Update()
    {
        //エフェクト時間
        if (getDamageHitFlag())
        {
            //最初なら
            if(!ImgScript.getDamageEffFlag())
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
            //NavMeshを切る
            Nav.enabled = false;

            //攻撃を止める
            attack.setAttackFlag(false);

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

        setIsSearch(false);
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

                //画像を変更する
                ImgScript.ChangeImage(false);
                break;
            //探索状態-----------------------------
            case State.Search:
                //移動の変数を呼ぶ前に移動方向の確認
                for (int i = 0; i < 4; ++i) 
                {
                    NoMoveDir[i] = MoveCol[i].getMoveFlag();
                }

                vec = move.Search(getMoveSpeed(),NoMoveDir);
                gameObject.transform.Translate(vec);

                break;
            //追尾状態-----------------------------
            case State.AimPlayer:
                //ナビメッシュを起動させる
                if (!Nav.enabled)
                {
                    Nav.enabled = true;
                }

                Nav.SetDestination(player.transform.position);

                break;
            //停止状態-----------------------------
            case State.Stop:
                //停止
                vec = move.Stop();

                break;
            //攻撃状態-----------------------------
            case State.Attack:
                //攻撃　インスタンスした武器を使う

                attack.setAttackFlag(true);
                attack.Fire(false);

                if (!OneceFlag)
                {
                    //画像を変更する
                    ImgScript.ChangeImage(true);
                    OneceFlag = true;
                }

                //一定距離を取る
                if (HitCircle(this.gameObject, player, TakeDist))
                {
                    if(CheckNoMoveDir())
                    {
                        vec = move.TakeDistance(player, this.gameObject, getMoveSpeed());
                        gameObject.transform.Translate(vec);
                    }
                }

                break;
            //逃げ状態-----------------------------
            case State.Escape:
                //プレイヤと反対方向に移動

                //壁を確認する
                if (CheckNoMoveDir())
                {
                    vec = move.Escape(player, this.gameObject, getMoveSpeed());
                    gameObject.transform.Translate(vec);
                }

                //一定時間で消える
                if(MoveTimer >= 10)
                {
                    //αを設定する
                    ImgScript.ChengeAlphaDec();

                    //0.1f以下なら非表示にする
                    if (ImgScript.getAlpha() <= 0.1f)
                    {
                        //消滅する
                        DestroyMe();
                    }
                }

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
                //カメラ内にいるなら
                if (getIsSearch())
                {
                    st = State.Search;
                }
                //攻撃を受けたら
                if (getDamageHitFlag())
                {
                    st = State.AimPlayer;
                }
                break;
            //探索状態-----------------------------
            case State.Search:
                //カメラ外にいるなら
                if (!getIsSearch())
                {
                    st = State.Non;
                }

                //停止
                //一定時間移動したら止まる
                if (MoveTimer >= 2)
                {
                    st = State.Stop;
                }

                //追尾
                //プレイヤが探索範囲に入ったら
                if (SearchAreaCol.getSearchFlag())
                {
                    st = State.AimPlayer;
                }

                //逃げ(一定体力を下回ったら)
                if (getEscHP() >= getHp())
                {
                    st = State.Escape;
                }
                
                break;
            //追尾状態-----------------------------
            case State.AimPlayer:

                //探索
                //追尾範囲からプレイヤが出たら && ドローンに呼ばれていない時
                if (!SearchAreaCol.getSearchFlag() && !CameSc.getCallEnemyFlag())
                {
                    st = State.Non;
                    this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    Nav.enabled = false;
                }

                //停止
                //プレイヤが攻撃範囲に入ったら
                if (AttackAreaCol.getAttackFlag())
                {
                    st = State.Stop;
                    this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    Nav.enabled = false;
                }

                //逃げ(一定体力を下回ったら)
                if (getEscHP() >= getHp())
                {
                    st = State.Escape;
                    this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    Nav.enabled = false;
                }
                break;
            //停止状態-----------------------------
            case State.Stop:
                //索敵　プレイヤが攻撃範囲から出た　or　一定時間経過した
                if (MoveTimer >= 1)
                {
                    st = State.Search;

                    //移動方向を再抽選する
                    move.setMoveDir(true);
                }

                //攻撃　攻撃範囲内にプレイヤがいたら
                if (AttackAreaCol.getAttackFlag())
                {
                    st = State.Attack;
                }

                //逃げ(一定体力を下回ったら)
                if (getEscHP() >= getHp())
                {
                    st = State.Escape;
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
                    attack.setAttackFlag(false);
                }

                //逃げ(一定体力を下回ったら)
                if (getEscHP() >= getHp())
                {
                    st = State.Escape;
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

        //カメラのリストから自分を消す
        GameObject c = GameObject.Find("Main Camera");
        c.GetComponent<CameraScript>().RemoveEnemy(Parent.name);

        kill(Parent);
    }
    //-----------------------------------------------------------------
    public void setState_AimPlayer()
    {
        setState(State.AimPlayer);
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

        //探索用コライダーのスクリプトを取得
        GameObject SearchObj = GetObj.GetChild_Obj(this.gameObject, "SearchArea");
        SearchAreaCol = SearchObj.GetComponent<ColliderScript>();

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
        if(ImgObj.name == "NormalEnemyImg")
        {
            Nav.speed = 4.0f;
        }
        else if(ImgObj.name == "BigEnemyImg")
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
        GameObject[] temp = new GameObject[] {
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
        attack = Weapon.GetComponent<EAttack>();
    }
    //-----------------------------------------------------------------
    void ZeroClear()
    {
        DamageTimer = 0;
    }
    //-----------------------------------------------------------------
    public void setIsSearch(bool Flag)
    {
        isSearch = Flag;
    }
    public bool getIsSearch()
    {
        return isSearch;
    }
    //-----------------------------------------------------------------
    //-----------------------------------------------------------------
}
