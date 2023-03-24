using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------------------------------
//敵　攻撃クラス
//-----------------------------------------------------------------

public class EAttack : MonoBehaviour
{
    //使用武器　インスペクターから指定
    enum WeaponType
    {
        Hand_gun,
        Machine_gun,
        Shot_gun,
        Sniper_gun
    }
    [SerializeField] WeaponType WT;

    enum Angle
    {
        Left,
        Right
    }
    [SerializeField] Angle angle;

    //攻撃フラグ
    bool AttackFlag;
    //プレイヤ
    GameObject player;
    [SerializeField] GameObject bullet_;

    //ダメージ
    [SerializeField] float ShotDamage;

    //攻撃間隔
    float AtTime;
    //残段数
    int ShotCnt;
    [Header("MaxShot -> ハンドガン = 6, マシンガン = 30, ショットガン = 5,スナイパー = 1")]
    [SerializeField] int maxShot;   //最大数

    //リロード時間
    float ReloadTime;
    [SerializeField] float ReloadSpan;
    [SerializeField] bool ReloadFlag;

    [SerializeField] float MGBlurAngle;

    SpriteRenderer WSRen;
    //レイのRender
    [SerializeField] SpriteRenderer LaySRen;

    bool isFlip = false;

    //エフェクト
    [SerializeField] PlayParticle particle;
    [SerializeField] ParticleSystem Steam;

    //SE
    [SerializeField] SE SE;
    [SerializeField] AudioClip clip;

    //-----------------------------------------------------------------------------------------------
    void Start()
    {
        //初期は移動しているのでfalse
        AttackFlag = false;

        ReloadFlag = false;

        player = GameObject.FindGameObjectWithTag("player");
        AtTime = 0;

        ShotCnt = maxShot;

        //とりあえず初期化
        angle = Angle.Left;

        WSRen = this.gameObject.GetComponent<SpriteRenderer>();

        InitializeAttack();
    }

    void Update()
    {
        //攻撃フラグがtrueの時プレイヤの位置に向く
        if (AttackFlag)
        {
            //武器の向き変更
            SetAngleOffset();

            //武器の向いている方向を調べる　Imgの向いている方向
            if (transform.parent.transform.localScale.x <= 0)
            {
                //マイナスの場合
                angle = Angle.Left;
            }
            else
            {
                //プラスの場合
                angle = Angle.Right;
            }

            //スナイパーの時のみの処理
            if(WT == WeaponType.Sniper_gun)
            {
                //レイのalpha値を同期する
                LaySRen.color = WSRen.color;
            }


            //時間をカウントする
            AtTime += Time.deltaTime;
        }
    }

    //攻撃関数(敵ごとに関数作成)--------------------------------------------------------------------
    void HandGunAttack()     //通常の攻撃
    {
        //リロード状態ではない
        if (!ReloadFlag)
        {
            HGAttack(0.8f);
        }
        //リロード状態なら
        else
        {
            if(ReloadTime >= ReloadSpan)
            {
                Reload();
            }

            ReloadTime += Time.deltaTime;
        }
    }

    //-----------------------------------------------------------------------------------------------
    //マシンガン処理
    void MachineGunAttack(bool BossAttack)
    {
        //リロード状態ではない
        if (!ReloadFlag)
        {
            if (BossAttack)
            {
                MGAttack(2f,0.3f, MGBlurAngle);
            }
            else
            {
                MGAttack();
            }
        }
        //リロード状態なら
        else
        {
            if (ReloadTime >= ReloadSpan)
            {
                Reload();
            }

            ReloadTime += Time.deltaTime;
        }
    }
    //-----------------------------------------------------------------------------------------------
    //ショットガン処理
    void ShotGunAttack()
    {
        //リロード状態ではない
        if (!ReloadFlag)
        {
            ShGAttack();
        }
        //リロード状態なら
        else
        {
            if (ReloadTime >= ReloadSpan)
            {
                Reload();
            }

            ReloadTime += Time.deltaTime;
        }
    }
    //-----------------------------------------------------------------------------------------------
    void SniperGunAttack()
    {
        //リロード状態ではない
        if (!ReloadFlag)
        {
            SnGAttack();
        }
        //リロード状態なら
        else
        {
            if (ReloadTime >= ReloadSpan)
            {
                Reload();
            }

            ReloadTime += Time.deltaTime;
        }
    }
    //-----------------------------------------------------------------------------------------------
    public void setAttackFlag(bool Flag)
    {
        //プレイヤが攻撃範囲内にいる  攻撃範囲を判定しているスクリプトで変更
            //　true　 →　攻撃する
            //　flase　→　攻撃しない
        AttackFlag = Flag;
    }
    //-----------------------------------------------------------------------------------------------
    void SetAngleOffset()
    {
        //プレイヤと反対方向を向いている(銃は表示しないため問題ない)
        //スナイパーのレイをプレイヤの反対に表示する　→　敵画像を反転したときに正しい方向にレイが向く
        Vector2 playerVector = (Vector2)player.transform.position - (Vector2)transform.position;
        isFlip = playerVector.x > 0;

        float rotateAngle = Mathf.Atan2(playerVector.y, playerVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotateAngle + (isFlip ? 180 : 0), Vector3.forward);
    }
    //-----------------------------------------------------------------------------------------------
    public void Fire(bool BossAttack)
    {
        //武器の種類で分岐
        switch (WT)
        {
            //ハンドガン------------------
            case WeaponType.Hand_gun:
                HandGunAttack();
                break;

            //マシンガン------------------
            case WeaponType.Machine_gun:
                MachineGunAttack(BossAttack);
                break;

            //ショットガン----------------
            case WeaponType.Shot_gun:
                ShotGunAttack();
                break;
            //----------------------------
            case WeaponType.Sniper_gun:
                SniperGunAttack();
                break;
            //----------------------------
            default:
                //基本ここに来ることはない
                Debug.Log("敵　使用武器エラー");
                break;
        }
    }
    //-----------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------
    //弾を発射する処理
    void OneShot(float BlurAngle, float OffsetAngle,Vector2 OffPos)    //OffsetAngle → ショットガン以外は0
    {
        if(angle == Angle.Left)
        {
            //左を向いていたらマイナスにする
            OffPos.x *= -1;
        }
        Vector2 offsetPos = OffPos;
        Vector2 shotPos = (Vector2)transform.position + offsetPos;

        //生成
        GameObject bullet = Instantiate(bullet_, shotPos, Quaternion.identity);

        //サイズ設定
        bullet.transform.localScale *= 0.08f;

        EnemyShot ec = bullet.GetComponent<EnemyShot>();

        //寿命の設定
        ec.liveTime = 0.8f;
        //ダメージ設定
        ec.damage = ShotDamage;
        //最大ブレ角の設定
        ec.setMaxBlurAngle(BlurAngle);

        Vector2 shotDir = player.transform.position - this.gameObject.transform.position;

        //角度を求め、弾にセットする
        ec.setAngle(Mathf.Atan2(shotDir.y, shotDir.x) + OffsetAngle * Mathf.Deg2Rad);
    }
    //-----------------------------------------------------------------------------------------------
    void OneShot(float BlurAngle, float OffsetAngle, Vector2 OffPos,float LiveTime,float ShotSpeed)    //OffsetAngle → ショットガン以外は0
    {
        if (angle == Angle.Left)
        {
            //左を向いていたらマイナスにする
            OffPos.x *= -1;
        }
        Vector2 offsetPos = OffPos;
        Vector2 shotPos = (Vector2)transform.position + offsetPos;

        //生成
        GameObject bullet = Instantiate(bullet_, shotPos, Quaternion.identity);

        //サイズ設定
        bullet.transform.localScale *= 0.08f;

        EnemyShot ec = bullet.GetComponent<EnemyShot>();

        //寿命の設定
        ec.liveTime = LiveTime;
        //ダメージ設定
        ec.damage = ShotDamage;
        //弾速
        ec.ShotSpeed = ShotSpeed;
        //最大ブレ角の設定
        ec.setMaxBlurAngle(BlurAngle);

        Vector2 shotDir = player.transform.position - this.gameObject.transform.position;

        //角度を求め、弾にセットする
        ec.setAngle(Mathf.Atan2(shotDir.y, shotDir.x) + OffsetAngle * Mathf.Deg2Rad);
    }
    //-----------------------------------------------------------------------------------------------
    //レイのオンオフ
    public void setLayActive(bool Flag)
    {
        if (Flag)
        {
            LaySRen.color = new Color(1, 1, 1, 1);
        }
        else
        {
            LaySRen.color = new Color(1, 1, 1, 0);
        }
    }
    //-----------------------------------------------------------------------------------------------
    //ハンドガン攻撃
    void HGAttack(float AtSpan)
    {
        if (AtTime >= AtSpan)
        {
            //ブレ角【-13～13】
            OneShot(13.0f, 0, new Vector2(0.2f, 0.1f));

            //残弾数を減らす
            ShotCnt--;
            if (ShotCnt <= 0)
            {
                ReloadFlag = true;
                //時間を設定する(必ず生成よりも先)
                particle.setDuartion(ReloadSpan);
                //エフェクトを出す(追尾)
                particle.Play(Steam, this.gameObject.transform.position, this.gameObject);
            }
            //SEを再生
            SE.Play(clip);
            AtTime = 0;
        }
    }
    //-----------------------------------------------------------------------------------------------
    //マシンガン攻撃
    void MGAttack()
    {
        if (AtTime >= 0.05f)
        {
            //ブレ角の設定　ハンドガンより小さめ
            OneShot(10.0f, 0, new Vector2(0.5f, 0.1f));

            //残弾数を減らす
            ShotCnt--;
            if (ShotCnt <= 0)
            {
                ReloadFlag = true;

                //時間を設定する(必ず生成よりも先)
                particle.setDuartion(ReloadSpan);
                //エフェクトを出す(追尾)
                particle.Play(Steam, this.gameObject.transform.position, this.gameObject);
            }
            //SEを再生
            SE.Play(clip);
            AtTime = 0;
        }
    }
    //マシンガン攻撃(弾の寿命を伸ばす)
    void MGAttack(float limit,float shotSpped, float MGBlurAngle)
    {
        if (AtTime >= 0.05f)
        {
            //ブレ角の設定　ハンドガンより小さめ
            OneShot(MGBlurAngle, 0, new Vector2(0.5f, 0.1f), limit, shotSpped);

            //残弾数を減らす
            ShotCnt--;
            if (ShotCnt <= 0)
            {
                ReloadFlag = true;

                //時間を設定する(必ず生成よりも先)
                particle.setDuartion(ReloadSpan);
                //エフェクトを出す(追尾)
                particle.Play(Steam, this.gameObject.transform.position, this.gameObject);
            }
            //SEを再生
            SE.Play(clip);
            AtTime = 0;
        }
    }
    //-----------------------------------------------------------------------------------------------
    //ショットガン攻撃
    void ShGAttack()
    {
        if (AtTime >= 0.8f)
        {
            float tempAngle = -3;

            for (int i = 0; i < 10; ++i)
            {
                OneShot(3.0f, tempAngle, new Vector2(1.0f, -0.2f));

                //2度ずらす
                tempAngle += 0.5f;
            }

            //残弾数を減らす
            ShotCnt--;
            if (ShotCnt <= 0)
            {
                ReloadFlag = true;

                //時間を設定する(必ず生成よりも先)
                particle.setDuartion(ReloadSpan);
                //エフェクトを出す(追尾)
                particle.Play(Steam, this.gameObject.transform.position, this.gameObject);
            }
            //SEを再生
            SE.Play(clip);
            AtTime = 0;
        }
    }
    //-----------------------------------------------------------------------------------------------
    //スナイパー攻撃
    void SnGAttack()
    {
        if (AtTime >= 0.8f)
        {
            OneShot(3.0f, 0, new Vector2(1.0f, -0.2f), 10, 0.5f);

            //残弾数を減らす
            ShotCnt--;
            if (ShotCnt <= 0)
            {
                ReloadFlag = true;

                //時間を設定する(必ず生成よりも先)
                particle.setDuartion(ReloadSpan);
                //エフェクトを出す(追尾)
                particle.Play(Steam, this.gameObject.transform.position, this.gameObject);
            }
            //SEを再生
            SE.Play(clip);
            AtTime = 0;
        }
    }
    //-----------------------------------------------------------------------------------------------
    void Reload()
    {
        //残段数を回復
        ShotCnt = maxShot;

        //フラグを戻す
        ReloadFlag = false;

        ReloadTime = 0;
    }
    //-----------------------------------------------------------------------------------------------
    void InitializeAttack()
    {
        // 1.6681f＝100の9乗根。最大難易度で100倍のHP。
        ShotDamage = ShotDamage + (StaticVariable.Level * 10);
    }
    //-----------------------------------------------------------------------------------------------
}
