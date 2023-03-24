using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//-------------------------------------------------------------------------------------------
//敵　ドローン
//-------------------------------------------------------------------------------------------

public class Drone : EnemyClass
{
    //移動クラスをインスタンス化
    EMove move = new EMove();

    //絵のスクリプト
    [SerializeField] EnemyImg ImgScript;

    //周回ルート配列(可変長配列)
    [SerializeField] List<string> Root = new List<string>();
    [SerializeField]List<int> RTime = new List<int>();
    int dirCnt;     //移動方向カウンタ
    float dirTimer; //移動時間カウンタ

    //探索関連の変数
    ColliderScript SearchAreaCol;
    //攻撃関連の変数
    ColliderScript AttackAreaCol;

    //追尾用
    NavMeshAgent Nav;
    //回避関連の変数
    //BoxCollider2D col;

    //元の位置に戻るための変数
    Vector2 targetPos;
    bool RetTarget;

    Rigidbody2D rigid;

    float DamageTimer;
    float preDamage;

    //-------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        //出現させる
        setAppearFlag(true);

        //初期化
        Initialize();

        //周回ルート読み込み
        LoadRoot();

        //絵と本体を同期させる
        GameObject Parent = transform.parent.gameObject;
        setEnemyImg(Parent.transform.Find("DroneImg").gameObject);

        //オブジェクト取得
        GetObjects();

        //スクリプト取得
        GetScripts();

        //フラグの初期化
        Nav.enabled = false;
        //回避フラグ設定（初弾は確定回避）
        setAvoidFlag(true);
        RetTarget = false;
    }
    //-------------------------------------------------------------------------------------------
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
            Nav.enabled = false;

            //コライダーを切る
            ImgScript.setActiveCollider(false);

            //αを設定する
            ImgScript.ChengeAlphaDec();
        }
    }
    private void Update()
    {
        //無敵時間
        if (getDamageHitFlag())
        {
            //回避ならこの処理から抜ける
            if (getState() == State.Avoidance)
            {
                DamageTimer = 1;
            }

            //最初なら
            if (!ImgScript.getDamageEffFlag())
            {
                //フラグを変える
                ImgScript.setDamageEffFlag(true);
            }

            //無敵時間の判定
            if (DamageTimer >= 0.2f)
            {
                DamageTimer = 0;
                setDamageHitFlag(false);

                //エフェクトを終了する
                ImgScript.setDamageEffFlag(false);
            }
            DamageTimer += Time.deltaTime;
        }

        //画像の向きの変更
        ImgTurn();
    }
    //-------------------------------------------------------------------------------------------
    void Initialize()   //初期化
    {
        //各種情報の設定
        setMoveSpeed(0.05f);   //移動速度
        //状態の設定
        setState(State.Non);
        //HP
        InitializeHP();
        //逃げるHP
        setEscHP(0.1f * getMaxHp());

        //ゼロクリア
        ZeroClear();
    }
    //-------------------------------------------------------------------------------------------
    public override void Move()
    {
        //移動量
        Vector2 vec = Vector2.zero;

        //自身の状態を取得して分岐する
        switch (getState())
        {
            //無効状態-----------------------------
            case State.Non:

                break;
            //探索状態-----------------------------
            case State.Search:

                //周回ルートから外れていない時
                if (!RetTarget)
                {
                    //一定ルート周回
                    //移動量の取得
                    vec = move.SearchDrone(Root[dirCnt], getMoveSpeed());
                    //移動
                    gameObject.transform.Translate(vec);

                    //移動方向を変更するカウンタ
                    if (dirTimer >= RTime[dirCnt])
                    {
                        dirTimer = 0;
                        dirCnt++;

                        //もし配列の最後まで来たら[dirCnt]を初期化する
                        if (dirCnt >= RTime.Count)
                        {
                            dirCnt = 0;
                        }
                    }
                    dirTimer += Time.deltaTime;
                }
                //周回ルートに戻る
                else
                {
                    vec = move.ReturnDrone(this.gameObject.transform.position, targetPos, getMoveSpeed());
                    //移動
                    gameObject.transform.Translate(vec);

                    //元の位置に戻ったか調べる　四方一定距離に入ったか
                    if( this.gameObject.transform.position.x >= targetPos.x - 0.5f &&   //左側
                        this.gameObject.transform.position.x <= targetPos.x + 0.5f &&   //右側
                        this.gameObject.transform.position.y <= targetPos.y + 0.5f &&   //上側
                        this.gameObject.transform.position.y >= targetPos.y - 0.5f)     //下側
                    {
                        //フラグを戻す
                        RetTarget = false;
                    }
                }
                break;
            //追尾状態-----------------------------
            case State.AimPlayer:
                //ナビメッシュを起動させる
                if (!Nav.enabled)
                {
                    Nav.enabled = true;

                    //戻っている途中でない and 前の状態が攻撃でない
                    if (!RetTarget && getPreState() != State.Attack)
                    {
                        //戻る座標を保存する
                        targetPos = this.gameObject.transform.position;
                    }
                }
                //ナビメッシュが起動しているなら
                this.Nav.SetDestination(player.transform.position);

                break;
            //停止状態-----------------------------
            case State.Stop:
                //ナビメッシュを無効化する
                Nav.enabled = false;

                //停止
                vec = move.Stop();
                break;
            //攻撃状態-----------------------------
            case State.Attack:
                if (!OneceFlag)
                {
                    CameSc.setCallEnemyFlag(true);
                    OneceFlag = true;
                }

                //距離を取る処理
                //一定距離を取る
                if (HitCircle(this.gameObject, player, 4))
                {
                    vec = move.TakeDistance(player, this.gameObject, getMoveSpeed());
                    gameObject.transform.Translate(vec);
                }

                break;
            //逃げ状態-----------------------------
            case State.Escape:
                //プレイヤと反対方向に移動
                vec = move.Escape(player, this.gameObject, getMoveSpeed());
                gameObject.transform.Translate(vec);

                //一定時間で消える
                if (MoveTimer >= 10)
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
                //一度だけ力を加える
                if (!OneceFlag)
                {
                    vec = move.Avoidance();
                    rigid.AddForce(vec);

                    OneceFlag = true;
                }

                break;
            //デフォルト-----------------------------
            default:
                //基本ここに来ることはない
                Debug.Log("Move-デフォルト");
                break;
        }
    }
    //-------------------------------------------------------------------------------------------
    public override void Think()
    {
        //状態を一時保存する変数
        State st = getState();

        //自身の状態を取得して分岐する
        switch (getState())
        {
            //無効状態-----------------------------
            case State.Non:
                //無条件で探索へ
                st = State.Search;

                break;
            //探索状態-----------------------------
            case State.Search:
                //追尾
                //プレイヤが探索範囲に入ったら
                if (SearchAreaCol.getSearchFlag())
                {
                    st = State.AimPlayer;
                }
                //回避
                //回避可能　&&　攻撃を受けた
                if (getAvoidFlag() && getDamageHitFlag())
                {
                    st = State.Avoidance;
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
                //追尾範囲からプレイヤが出たら
                if (!SearchAreaCol.getSearchFlag())
                {
                    st = State.Non;
                    this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    Nav.enabled = false;

                    //周回ルートに戻るフラグを立てる
                    RetTarget = true;
                }

                //停止
                //プレイヤが攻撃範囲に入ったら
                if (AttackAreaCol.getAttackFlag())
                {
                    st = State.Stop;
                    this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    Nav.enabled = false;
                }

                //回避
                //回避可能　&&　攻撃を受けた
                if (getAvoidFlag() && getDamageHitFlag())
                {
                    st = State.Avoidance;
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
                if (!AttackAreaCol.getAttackFlag())
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

                //回避
                //回避可能　&&　攻撃を受けた
                if (getAvoidFlag() && getDamageHitFlag())
                {
                    st = State.Avoidance;
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

                    CameSc.setCallEnemyFlag(false);
                }

                //回避
                //回避可能　&&　攻撃を受けた
                if (getAvoidFlag() && getDamageHitFlag())
                {
                    st = State.Avoidance;
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
                //一定時間経過後　→　停止状態になる
                if (MoveTimer >= 0.2f)
                {
                    st = State.Stop;
                    rigid.velocity = Vector3.zero;

                    //受けたダメージを補正する
                    setHP(getHp() + preDamage);

                    //回避フラグを変更する
                    AvoidRateRandom();
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
    //-------------------------------------------------------------------------------------------
    //有効化・無効化の関数
    //有効化はカメラのスクリプトで行う
    public void ActiveFalse()
    {
        //本体の無効化
        this.gameObject.SetActive(false);
    }
    //-------------------------------------------------------------------------------------------
    void setEnemyImg(GameObject g)
    {
        enemyImg = g;
    }
    public GameObject getEnemyImg()
    {
        return enemyImg;
    }
    //-------------------------------------------------------------------------------------------
    public void HitDamageDrone(float damage)
    {
        //自身の状態が回避でない時 && 回避できない時 && 攻撃を受けていない時 に処理を行う
        if (!getAvoidFlag() && !getDamageHitFlag())
        {
            float tempHP = getHp() - damage;
            preDamage = damage;     //ダメージを保存

            //エフェクトを表示
            par.Play(HitDamageEff, this.gameObject.transform.position);
            //SE再生
            se.Play(SE_hit);

            //残りHPの判定
            if (tempHP <= 0)
            {
                int DropCnt = Random.Range(1, getMaxDropCnt() + 1);
               //アイテムを落とす
                DropItem(this.gameObject.transform.position, DropCnt);
                //フラグをfalseにする
                setAppearFlag(false);

                //エフェクトを表示
                par.PlayAround(destEff, this.gameObject.transform.position);
                //SEの再生
                se.Play(SE_Explos);

                //消滅する
                DestroyMe();
            }
            //HPを変更する
            setHP(tempHP);
        }
        //被ダメフラグを立てる
        setDamageHitFlag(true);
    }
    //-------------------------------------------------------------------------------------------
    void DestroyMe()
    {
        GameObject Parent = transform.parent.gameObject;

        //呼び出し状態で無くす
        CameSc.setCallEnemyFlag(false);

        //消滅する
        kill(Parent);
    }
    //-------------------------------------------------------------------------------------------
    void LoadRoot()
    {
        //周回ルートを読み込む
        string[,] RootTemp = new string[3, 3];
        RootLoad RL = this.gameObject.GetComponent<RootLoad>();
        RootTemp = RL.LoadRoot();

        //列分for文を回す
        for (int i = 0; i < RootTemp.Length / 2; i++)
        {
            //方向
            Root.Add(RootTemp[i, 0]);
            //秒数
            RTime.Add(int.Parse(RootTemp[i, 1]));
        }

        this.gameObject.transform.position = RL.getInsPos();
    }
    //-------------------------------------------------------------------------------------------
    void GetObjects()
    {
        //オブジェクト取得クラスをインスタンス化
        ObjectGetClass GetObj = new ObjectGetClass();

        //プレイヤの取得
        player = GetObj.GetGameObject("player");
        //rigidboy2Dを取得
        rigid = GetObj.GetRigid2D(this.gameObject);

        //探索用コライダーのスクリプトを取得
        GameObject SearchObj = GetObj.GetChild_Obj(this.gameObject, "SearchArea");
        SearchAreaCol = SearchObj.GetComponent<ColliderScript>();
        //攻撃範囲コライダーのスクリプトを取得
        GameObject AttackAreaObj = GetObj.GetChild_Obj(this.gameObject, "AttackArea");
        AttackAreaCol = AttackAreaObj.GetComponent<ColliderScript>();

        Nav = this.gameObject.GetComponent<NavMeshAgent>();
    }
    //-------------------------------------------------------------------------------------------
    void GetScripts()
    {
        //絵のスクリプトを取得
        ImgScript = enemyImg.GetComponent<EnemyImg>();

        //カメラスクリプ
        CameSc = GameObject.Find("Main Camera").GetComponent<CameraScript>();
    }
    //-------------------------------------------------------------------------------------------
    void ZeroClear()
    {
        //カウンタの初期化
        dirCnt = 0;
        dirTimer = 0;
        DamageTimer = 0;
        preDamage = 0;
        targetPos = Vector2.zero;
    }
    //-------------------------------------------------------------------------------------------
    void ImgTurn()
    {
        Vector2 playerVector = player.transform.position - transform.position;
        bool isFlip = playerVector.x < 0;

       　//向きを変更する関数を呼ぶ
       　ImgScript.ImgTurn(isFlip);
    }
    //-------------------------------------------------------------------------------------------
}
