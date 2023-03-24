using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------------------------------
//敵　基底クラス
//-----------------------------------------------------------------

public class EnemyClass : MonoBehaviour
{
    public enum State      //敵の状態
    {
        Non,        //なし
        Search,     //索敵
        AimPlayer,  //プレイヤへ移動
        Stop,       //停止
        Attack,     //攻撃
        Escape,     //にげる    
        Avoidance,  //回避
        Rush,       //突進(Bossのみ使用)
    }
    //状態
    [SerializeField] State state = State.Non;
    [SerializeField] State preState = State.Non;

    [SerializeField] private float   HP;             //HP
    [SerializeField] private float maxHP = 0;       //最大HP
    [SerializeField] private float MoveSpeed;      //移動速度
    private int   HitRate;        //命中率
    private float   EscHP;          //逃げるHp

    private bool AppearFlag;      //出現フラグ
    private bool AvoidFlag;       //回避フラグ
    private bool DamegeHitFlag;   //被ダメフラグ

    public float MoveTimer;       //行動タイマー
    public bool OneceFlag;        //一度だけ行動したい時に使うフラグ

    //セットの絵を保存する
    public GameObject enemyImg;
    public GameObject player;

    //ドロップアイテムテーブル
    [SerializeField] int maxDropCnt;
    /*
    [SerializeField] List<GameObject> R_ItemTable = new List<GameObject>();
    [SerializeField] List<GameObject> SR_ItemTable = new List<GameObject>();
    [SerializeField] List<GameObject> SSR_ItemTable = new List<GameObject>();
    */
    [SerializeField] List<DropItemClass> dropItemList = new List<DropItemClass>();

    public CameraScript CameSc;

    [Header("エフェクト")]
    //エフェクト
    [SerializeField] public PlayParticle par;
    [SerializeField] public ParticleSystem HitDamageEff;
    [SerializeField] public ParticleSystem destEff;

    //SE
    [SerializeField] public SE se;
    [SerializeField] public AudioClip SE_Explos;
    [SerializeField] public AudioClip SE_hit;

    void Start()
    {
        //ゼロクリア
        ZeroClear();
        //フラグの初期化
        AppearFlag = false;
        OneceFlag = false;
        AvoidFlag = false;
        DamegeHitFlag = false;
    }

    //このクラス内でのみ使う関数------------------------------------------------------------
    private void ZeroClear()        //ゼロクリア
    {
        HP = 0;
        MoveSpeed = 0;
        HitRate = 0;
        MoveTimer = 0;
    }    
    //仮想関数------------------------------------------------------------------------------    
    public virtual void Think()     //状態変更
    {
        //処理なし（ダミー）
    }

    public virtual void Move()      //状態ごとの行動
    {
        //処理なし（ダミー）
    }

    public void ChangeState(State st)   //状態が変わるか調べる関数
    {
        //引数と状態が違ったら
        if(state != st)
        {
            //前の状態を保存
            preState = getState();
            //状態を変更
            setState(st);

            MoveTimer = 0;
            OneceFlag = false;
        }
    }

    //Setter--------------------------------------------------------------------------------
    public void setHP(float HP)
    {
        this.HP = HP;
        if (HP > maxHP)
            maxHP = HP;
    }
    public void setMaxHp(float maxHP)
    {
        this.maxHP = maxHP;
    }
    public void InitializeHP() {
        // 1.6681f＝100の9乗根。最大難易度で100倍のHP。
        setHP(getHp() * Mathf.Pow(1.6681f, StaticVariable.Level));
    }
    public void setMoveSpeed(float MoveSpeed)
    {
        this.MoveSpeed = MoveSpeed;
    }
    public void setHitRate(int HitRate)
    {
        this.HitRate = HitRate;
    }
    public void setAppearFlag(bool Flag)
    {
        AppearFlag = Flag;
    }
    public void setState(State state)
    {
        this.state = state;
    }
    public void setEscHP(float Hp)
    {
        EscHP = Hp;
    }
    public void setAvoidFlag(bool AvoidFlag)
    {
        this.AvoidFlag = AvoidFlag;
    }
    public void setDamageHitFlag(bool DamegeHitFlag)
    {
        this.DamegeHitFlag = DamegeHitFlag;
    } 
    //Getter--------------------------------------------------------------------------------
    public float getMoveSpeed()
    {
        return MoveSpeed;
    }
    public State getState()
    {
        return state;
    }
    public State getPreState()
    {
        return preState;
    }
    public float getHp()
    {
        return HP;
    }
    public float getMaxHp()
    {
        return maxHP;
    }
    public int getHitRate()
    {
        return HitRate;
    }
    public bool getAppearFlag()
    {
        return AppearFlag;
    }
    public float getEscHP()
    {
        return EscHP;
    }
    public bool getAvoidFlag()
    {
        return AvoidFlag;
    }
    public bool getDamageHitFlag()
    {
        return DamegeHitFlag;
    }
    public int getMaxDropCnt()
    {
        return maxDropCnt;
    }
    //-----------------------------------------------------------------
    //消滅
    public void Destroy()
    {
        //自身の消滅関数
        this.gameObject.SetActive(false);
    }
    //-----------------------------------------------------------------
    //ランダムで回避フラグをtrueにする関数
    public void AvoidRateRandom()
    {
        int rnd = Random.Range(0, 101);
        //判定（三項演算）
        setAvoidFlag((rnd < HitRate) ? true : false);
    }
    //-----------------------------------------------------------------
    public bool HitCircle(GameObject me, GameObject you, float JudgeR)
    {
        //円の接触判定
        Vector2 dt = me.transform.position - you.transform.position;

        float r = Mathf.Sqrt(dt.x * dt.x + dt.y * dt.y);

        return r <= JudgeR;
    }
    //-----------------------------------------------------------------
    public void DropItem(Vector2 vec,int DropCnt)
    {
        if (dropItemList.Count <= 0)
            return;

        for (int c = 0; c < DropCnt; c++) {
            Vector2 DropPos = vec;
            Vector2 rndPos = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
            DropPos += rndPos;

            int rtv = 0;
            {
                int weightSum = 0;
                for (int i = 0; i < dropItemList.Count; i++)
                    weightSum += dropItemList[i].weight;
                int rand = Random.Range(1, weightSum + 1);
                int w = 0;
                while(rtv < dropItemList.Count) {
                    int weight = dropItemList[rtv].weight;
                    if (w + weight >= rand)
                        break;
                    rtv++;
                    w += weight;
                }
            }

            if (dropItemList[rtv].item.CompareTag("WeaponChip")) {
                int level = 1;
                if (StaticVariable.Level >= 1) {
                    float[] levelWeight = new float[5];

                    int pow = 6 - StaticVariable.Level;
                    for (int x = 1; x <= levelWeight.Length; x++)
                        levelWeight[x - 1] = Pow(-x + 6, pow);

                    int index = 0;
                    {
                        float weightSum = 0;
                        for (int i = 0; i < levelWeight.Length; i++)
                            weightSum += levelWeight[i];
                        float rand = Random.Range(0f, weightSum);
                        float w = 0;
                        while (index < levelWeight.Length) {
                            float weight = levelWeight[index];
                            if (w + weight >= rand)
                                break;
                            index++;
                            w += weight;
                        }
                    }


                    level = index + 1;
                }
                dropItemList[rtv].item.GetComponent<WeaponChip>().ChangeLevel(level);
            }
            Instantiate(dropItemList[rtv].item, DropPos, Quaternion.identity);
        }

    }

    float Pow(float b, int p) {
        float rtv = 1;

        //軽量化のためp=0、p=1のときは計算しない
        if (p == 0)
            return 1;
        else if (p == 1)
            return b; 
        else if (p > 1) {
            for (int i = 0; i < p; i++)
                rtv *= b;
        }
		else {
            for (int i = 0; i < -p; i++)
                rtv /= b;
        }
        return rtv;
	}


    //-----------------------------------------------------------------
    public void kill(GameObject obj)
    {
        //消滅する
        Destroy(obj, 2);
    }
    //-----------------------------------------------------------------
}