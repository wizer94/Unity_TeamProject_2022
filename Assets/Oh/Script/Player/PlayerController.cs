using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    PlayerStat P_Stat;
    Rigidbody2D rigid2d;
    SpriteRenderer sprite;
    public Animator anim;
    Color color;

    Vector2 movement;
    Vector2 pre_movement;

    public float dodgeTime;
    public float speed;

    public float dt;
    public float next_dodgetime;
    public float dodge_enalbetime;
    public float stay_longtime;

    public bool isDamage;
    public bool isMove;
    public bool isDash;
    public bool isDodge;

    public float muteki_time;

    public float moveMultiply = 1f; //移動速度の乗算倍率

    AudioSource audiosource;
    [SerializeField] AudioClip damageSound;

    //KeyConfig
    GetKeyCode getkey;
    private int []keyCode = new int[15];

    // Start is called before the first frame update
    void Start()
    {
        P_Stat = GetComponent<PlayerStat>();
        rigid2d = this.gameObject.GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();

        color = sprite.color;

        isDamage = false;
        isDodge = false;
        isMove = false;
        dodgeTime = 0.25f;
        dodge_enalbetime = 1.5f;
        muteki_time = 0.5f;
        next_dodgetime = dodge_enalbetime;
        stay_longtime = 0.0f;

        speed = P_Stat.speed;

        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
	{
        DodgeTime();

        stay_longtime += Time.deltaTime;
        Stay_LongTime();
    }

    void Stay_LongTime()
	{
        if (movement == Vector2.zero)
        {
            if (stay_longtime >= 6.0f)
            {
                anim.SetBool("isStay", true);
                Invoke("Stop_StayTime", 1.8f);
            }
        }
        else {
            stay_longtime = 0.0f;
            anim.SetBool("isStay", false);
        }
	}

    void Stop_StayTime()
	{
        stay_longtime = 0.0f;
        anim.SetBool("isStay", false);
    }

	public void GetMoveInput()
    {
        //stay_longtime = 0.0f;
        // 動き判定
        if (movement != Vector2.zero) {
            isMove = true;
        }
            
        else
            isMove = false;

        // 動きなしに初期化
        movement = Vector2.zero;
        anim.SetBool("isWalk", false);

        // 移動
        if (!anim.GetBool("isDodge"))
        {
            if (KeyScript.InputOn(KeyScript.Dir.Up)) {
                movement += new Vector2(0, +1);
                anim.SetBool("isWalk", true);
            }
            if (KeyScript.InputOn(KeyScript.Dir.Down)) {
                movement += new Vector2(0, -1);
                anim.SetBool("isWalk", true);
            }
            if (KeyScript.InputOn(KeyScript.Dir.Left)) {
                movement += new Vector2(-1, 0);
                anim.SetBool("isWalk", true);
            }
            if (KeyScript.InputOn(KeyScript.Dir.Right)) {
                movement += new Vector2(+1, 0);
                anim.SetBool("isWalk", true);
            }

            movement.y *= 0.5f; //Y速度を半分にしてマップのグリッドに添わせる
            if (movement.x * movement.y != 0)
                movement *= 0.7071f; //斜め入力のときは1/√2倍
        }
        if (movement != Vector2.zero)
            pre_movement = movement;

        // ダッジ入力        
        if (KeyScript.InputOn(KeyScript.Dir.Dodge) && movement != Vector2.zero)
        {
            isDodge = true;
            GetComponent<UI>().setAvoidUI();
        }
        // ダッシュ入力
        if (KeyScript.InputDown(KeyScript.Dir.Dash)) isDash = true;
        if (KeyScript.InputUp(KeyScript.Dir.Dash)) isDash = false;
    }

    public void Move()	{
        rigid2d.velocity = movement.normalized * speed * moveMultiply;
        if (anim.GetBool("isDodge"))
            rigid2d.velocity = pre_movement.normalized * speed * moveMultiply;
    }

    public void Turn() {
        Vector2 mouseVector = GetMousePos() - transform.position;
        bool isFlip = mouseVector.x < 0;
        transform.localScale = new Vector3(isFlip ? -1 : 1, 1, 1);
    }

    Vector3 GetMousePos() {
        Vector2 inp = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(new Vector3(inp.x, inp.y, 10));
    }

    public void Dash() {
        anim.SetBool("isADS", false);
        if (!isDodge)
        {
            if (isDash)
            {
                if (movement != Vector2.zero)
                    anim.SetBool("isRun", true);
                else
                    anim.SetBool("isRun", false);
                    speed = P_Stat.dash;
            }

            else
            {
                anim.SetBool("isRun", false);
                    speed = P_Stat.speed;
            }
        }
        else
            isDash = false;
    }

    public void Dodge() {
        if (next_dodgetime < dodge_enalbetime) {
            isDodge = false;
            return;
        }
        if (isDodge && movement != Vector2.zero)
        {
            anim.SetBool("isDodge", true);
            speed = P_Stat.speed * (4.0f * (1 + P_Stat.Avoid_up));
			isDodge = true;
            dodgeTime += P_Stat.Invincible;
            Invoke("DodgeOut", dodgeTime);
        }
    }

	public void DodgeOut() {
        next_dodgetime = 0.0f;
        anim.SetBool("isDodge", false);
        movement = Vector2.zero;
        dodgeTime -= P_Stat.Invincible;
        isDodge = false;
        if (KeyScript.InputOn(KeyScript.Dir.Dash))
            isDash = true;
    }

    void DodgeTime() {
        if(next_dodgetime < 5.0f)
            next_dodgetime += Time.deltaTime;

    }

    public void HitDamage(float damage)
    {
        P_Stat.HP -= damage * (1.0f - P_Stat.damageCut);
        //Debug.Log(P_Stat.damageCut);

        isDamage = true;

        audiosource.PlayOneShot(damageSound);

        //UIの変更
        this.gameObject.GetComponent<UI>().ChangeHPGauge(P_Stat.HP, P_Stat.Max_HP);

        //残りHPの判定
        if (P_Stat.HP <= 0)
        {
            //エフェクト
            GetComponent<UI>().setGameOverEff();
            this.gameObject.SetActive(false);
            Invoke("Return_Title", 3.0f);       // Titleに戻る
        }
    }

    void Return_Title()
	{
        SceneManager.LoadScene("Title");
        //Destroy(this.gameObject);
    }
    public void DamageEffect()
    {
        if (isDamage)
        {
            dt += Time.deltaTime;

            if (dt % 0.2f < 0.1f)
            {
                color.a -= dt * 3.0f;
                sprite.color = color;
            }
            else
            {
                color.a += dt * 3.0f;
                sprite.color = color;
            }

            if (dt > muteki_time)
            {
                dt = 0.0f;
                color.a = 1;
                sprite.color = new Color(1, 1, 1, 1);
                isDamage = false;
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D col)
    {
        // 敵に当たった
        if ((col.gameObject.tag == "Enemy" || col.gameObject.tag == "OtherEnemy") && !isDamage)
        {
            HitDamage(col.gameObject.GetComponent<EnemyImg>().getAttakPoint());
        }
    }

	private void OnTriggerEnter2D(Collider2D col)
	{
        // 弾に当たったら
        if (col.gameObject.tag == "EnemyBullet" && !isDamage)
        {
            HitDamage(col.GetComponent<EnemyShot>().damage);
        }
    }


}
