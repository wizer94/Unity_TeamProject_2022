using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImg : MonoBehaviour
{
    //アタッチすること
    public GameObject enemyObj;
    [SerializeField] GameObject Weapon;

    SpriteRenderer SRen;
    SpriteRenderer WSRen;

    bool DamageEffFlag;

    //敵の種類
    [SerializeField] string EnemyType;
    //画像
    [SerializeField] Sprite SearchImg;  //通常時の画像
    [SerializeField] Sprite AttackImg;  //攻撃時の画像
    //エフェクト
    [SerializeField] TrackEff particle;

    [SerializeField] private int AttakPoint;     //攻撃力

    void Start()
    {
        SRen = this.gameObject.GetComponent<SpriteRenderer>();
        WSRen = Weapon.GetComponent<SpriteRenderer>();

        DamageEffFlag = false;

        //敵の種類を取得
        EnemyType = this.gameObject.name;
    }
    //---------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        //位置だけ同期して動かす
        this.gameObject.transform.position = enemyObj.transform.position;

        //ダメージエフェクトの処理
        if (getDamageEffFlag())
        {
            DamageEff();
        }
    }
    //---------------------------------------------------------------------------
    public void ChengeAlphaDec()
    {
        Color color = SRen.color;
        color.a -= Time.deltaTime;
        SRen.color = color;

        //本体に合わせて銃も消える
        WSRen.color = color;
        //エフェクトを消す
        if(EnemyType != "DroneImg")
        {
            setParticleActive(false);
        }
    }
    //---------------------------------------------------------------------------
    public float getAlpha()
    {
        return SRen.color.a;
    }
    //---------------------------------------------------------------------------
    void DamageEff()
    {
        SRen.color = new Color(1, 1, 1, 0);
        WSRen.color = new Color(1, 1, 1, 0);        
    }
    //---------------------------------------------------------------------------
    public void setDamageEffFlag(bool Flag)
    {
        DamageEffFlag = Flag;

        //falseがセットされたら
        if (!Flag)
        {
            //alphaを戻す
            SRen.color = new Color(1, 1, 1, 1);
            WSRen.color = new Color(1, 1, 1, 1);
        }
    }
    //---------------------------------------------------------------------------
    public bool getDamageEffFlag()
    {
        return DamageEffFlag;
    }
    //---------------------------------------------------------------------------
    public void setPos(Vector3 pos)
    {
        this.transform.position = pos;
    }
    //---------------------------------------------------------------------------
    public void ImgTurn(bool isFlip)
    {
        float setSize = 0;

        switch (EnemyType)
        {
            //通常の敵
            case "NormalEnemyImg":
                setSize = 0.3f;
                break;
            //大きい敵
            case "BigEnemyImg":
                setSize = 0.5f;
                break;
            //ドローン
            case "DroneImg":
                setSize = 0.18f;
                break;
            //特攻兵
            case "RushEnemyImg":
                setSize = 0.15f;
                break;
            //Boss
            case "BossImg":
                setSize = 0.4f;
                break;
            //スナイパー
            case "SniperImg":
                setSize = 0.3f;
                break;
            //ダミー
            case "DummyImg":
                setSize = 0.3f;
                break;
        }

        //引用箇所
        if(EnemyType != "BigEnemyImg" && EnemyType != "BossImg")    //画像によって向きが違う
        {
            //通常
            this.gameObject.transform.localScale = new Vector3(isFlip ? setSize : setSize * -1, setSize, 1);
        }
        else
        {
            //大きい敵　+　ボス
            this.gameObject.transform.localScale = new Vector3(isFlip ? setSize * -1: setSize, setSize, 1);
        }

        if(EnemyType != "DroneImg")
        {
            if (EnemyType != "BossImg")
            {
                //エフェクトを反転
                particle.TurnEff(isFlip, isFlip ? 1 : -1);
            }
            else
            {
                //エフェクトを反転
                particle.TurnEff(isFlip, isFlip ? -1 : 1);
            }
        }
    }
    //---------------------------------------------------------------------------
    public void CollHitDamage(float damage)
    {
        //ダメージ処理を呼び出す
        switch (EnemyType)
        {
            //通常の敵
            default:
                enemyObj.GetComponent<NormalEnemy>().HitDamage(damage);
                break;
            //ボス
            case "BossImg":
                enemyObj.GetComponent<Boss>().HitDamage(damage);
                break;
            //スナイパー
            case "SniperImg":
                enemyObj.GetComponent<Sniper>().HitDamage(damage);
                break;
            //突進
            case "RushEnemyImg":
                enemyObj.GetComponent<RushEnemy>().HitDamage(damage);
                break;
            //ドローン
            case "DroneImg":
                enemyObj.GetComponent<Drone>().HitDamageDrone(damage);
                break;
            //ダミー
            case "DummyImg":
                enemyObj.GetComponent<Dummy>().HitDamage(damage);
                break;
        }
    }
    //---------------------------------------------------------------------------
    //コライダーの有・無効処理
    public void setActiveCollider(bool Flag)
    {
        BoxCollider2D col = gameObject.GetComponent<BoxCollider2D>();

        col.enabled = Flag;
    }
    //---------------------------------------------------------------------------
    void OnCollisionEnter2D(Collision2D col)
    {
        //プレイヤと接触していたら
        if(col.gameObject.tag == "player")
        {
            //普通と違う処理がある場合のみ書く
            switch (EnemyType)
            {
                //-------------------------------
                case "RushEnemyImg":
                    //自身にもダメージを与える（最大HPをダメージとする）
                    CollHitDamage(enemyObj.GetComponent<RushEnemy>().getMaxHp());
                    break;
                //-------------------------------
                default:
                    //通常の処理(基本はここに来る)
                    //ダミーは含まない
                    if(EnemyType != "DummyImg")
                    {
                        col.gameObject.GetComponent<PlayerController>().HitDamage(10);
                    }

                    break;
            }
        }
    }
    //---------------------------------------------------------------------------
    //画像を変更するメソッド
    public void ChangeImage(bool isAttack)
    {
        //攻撃時なら
        if (isAttack)
        {
            SRen.sprite = AttackImg;
        }
        //通常なら
        else
        {
            SRen.sprite = SearchImg;
        }
    }
    //---------------------------------------------------------------------------
    void setParticleActive(bool Flag)
    {
        particle.setActive(Flag);
    }
    public void setAttackPoint(int AttakPoint)
    {
        this.AttakPoint = AttakPoint;
    }
    //---------------------------------------------------------------------------
    public int getAttakPoint()
    {
        return AttakPoint;
    }

}
