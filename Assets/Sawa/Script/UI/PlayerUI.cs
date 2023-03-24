using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerUI : MonoBehaviour
{
    GameObject player;
    WeaponScript WS;
    GameObject Weapon;

    //HPBer
    [SerializeField] Image HP_Gauge;
    [SerializeField] Image Damage_Gauge;
    [SerializeField] Image Heal_Gauge;
    bool isShaveGauge;
    bool isHealGauge;

    //使用中の武器
    [SerializeField] Image UsingWeapon;
    [SerializeField] Sprite[] WeaponsImg;
    [SerializeField] Text Magazine;

    //マップ
    [SerializeField] Image[] Maps;
    [SerializeField] GameObject NowMap;
    [SerializeField] Sprite[] MapImgs_waku;
    [SerializeField] Sprite[] MapImgs;
    bool isExpansMap;   //マップの拡大表示フラグ
    int NowMapNum;
    bool[] EnteredMaps; //侵入したマップ
    //現在位置表示
    [SerializeField] GameObject PlayerMark;
    [SerializeField] SE SE;
    [SerializeField] AudioClip clip;

    int a = 0;

    // Start is called before the first frame update
    void Start()
    {
        //playerの取得
        player = GameObject.FindGameObjectWithTag("player");

        //HPバー
        //フラグの初期化
        isShaveGauge = false;
        isHealGauge = false;
        Damage_Gauge.enabled = false;
        Heal_Gauge.enabled = false;

        //使用中の武器
        //武器名の取得
        getWeaponObjs();
        WeaponUI(Weapon.name);

        //マップ　現在位置を変更
        NowMapNum = 0;
        isExpansMap = false;
        NowMap.SetActive(false);
        EnteredMaps = new bool[] { false, false, false, false, false, false };
        //マップを透明にする
        for(int i = 0;i < Maps.Length; ++i)
        {
            Maps[i].sprite = null;
            Maps[i].color = new Color(1, 1, 1, 0);
        }
        setMapUI(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        //マップを表示
        if (Input.GetKeyDown(KeyCode.Tab) && Time.timeScale != 0)
        {           
            //フラグを変更
            isExpansMap = !isExpansMap;
            //NowStageを変更する
            NowMap.SetActive(isExpansMap);
            //SEを再生
            SE.Play(clip);
        }
        //ゲージを削る処理
        if (getShaveGauge())
        {
            Shave_HPGauge();
        }
        //ゲージを回復する処理
        if (getisHealGauge())
        {
            Heal_HPgauge();
        }

        if (WS) {
            //残段数
            setMagazineText(WS.GetMagazine());
            UsingWeapon.fillAmount = WS.GetReloadCompleteRate();
        }
        PlayerMark.transform.position = player.transform.position;
    }
    //---------------------------------------------------------
    //setter
    void setShaveGauge(bool Flag)
    {
        isShaveGauge = Flag;
    }
    //---------------------------------------------------------
    //getter
    public bool getShaveGauge()
    {
        return isShaveGauge;
    }
    public bool getisHealGauge()
    {
        return isHealGauge;
    }

    //HPBer
    //---------------------------------------------------------
    void Shave_HPGauge()
    {
        //ダメージゲージを削る
        Damage_Gauge.fillAmount -= Time.deltaTime / 5;

        //ダメージゲージが現在のHp未満の長さになったら
        if (Damage_Gauge.fillAmount < HP_Gauge.fillAmount)
        {
            //削る処理を終了する
            setShaveGauge(false);

            //削り終わったのでゲージを非表示にする
            Damage_Gauge.enabled = false;
        }
    }
    void Heal_HPgauge()
    {
        //HPゲージを少しずつ増やす
        HP_Gauge.fillAmount += Time.deltaTime / 5;

        //HPゲージが回復ゲージより多くなったら
        if (HP_Gauge.fillAmount >= Heal_Gauge.fillAmount)
        {
            //増やす処理を終了する
            isHealGauge = false;

            //削り終わったのでゲージを非表示にする
            Heal_Gauge.enabled = false;
        }
    }
    //ダメージを受けた時にUIを変更する
    public void ChDamage_HPGauge(float hp, float max_Hp)
    {
        Damage_Gauge.enabled = true;                        //ダメージゲージを使用するので表示する
        Damage_Gauge.fillAmount = HP_Gauge.fillAmount;      //ダメージゲージのFillを緑と同じ長さにする

        //現在のHpを更新する（緑のゲージをいきなり削るのでここで処理を行う）
        HP_Gauge.fillAmount = hp / max_Hp;  //UIを更新する
        setShaveGauge(true);                //フグを立てる　→　赤いゲージ（ダメージゲージ）を削る

        //残りゲージで色を変える
        if (HP_Gauge.fillAmount <= 0.2f)
        {
            //半分なら赤に変更する
            HP_Gauge.color = new Color(1, 0.5f, 0);
        }
        else if (HP_Gauge.fillAmount <= 0.5f)
        {
            //半分なら黄色に変更する
            HP_Gauge.color = new Color(1, 1, 0);
        }        
    }
    //HPを回復したときにUiを変更する
    public void ChHeal_HPGauge(float HP,float maxHP)
    {
        Heal_Gauge.enabled = true;            //水色のゲージを使用するため、表示する
        //水色のゲージを回復後の値にする（水色のゲージをいきなり増やすのでここで処理を行う）
        Heal_Gauge.fillAmount = HP / maxHP;  //UIを更新する
        isHealGauge = true;                //フラグを立てる　→　緑のゲージを少しずつ増やす

        //ゲージの色を変更する
        if(Heal_Gauge.fillAmount >= 0.5f)
        {
            //緑に変更する
            HP_Gauge.color = new Color(0, 1, 0);
        }
        else if (Heal_Gauge.fillAmount >= 0.2f)
        {
            //黄色に変更する
            HP_Gauge.color = new Color(1, 1, 0);
        }
    }
    //緑ゲージを削る
    public void DecHPGauge(float HP,float maxHP)
    {
        HP_Gauge.fillAmount = HP / maxHP;
    }

    //使用中の武器
    //---------------------------------------------------------
    public void WeaponUI(string WeaponName)
    {
        int outNum = 0;
        switch (WeaponName)
        {
            case "AssaulRifle":
                outNum = 0;
                break;
            case "Handgun":
                outNum = 1;
                break;
            case "Machinegun":
                outNum = 2;
                break;
            case "Revolver":
                outNum = 3;
                break;
            case "Shotgun":
                outNum = 4;
                break;
            case "Sniper":
                outNum = 5;
                break;
            case "Submachinegun":
                outNum = 6;
                break;
        }
        //画像を設定
        UsingWeapon.sprite = WeaponsImg[outNum];
    }
    //残段数表示
    void setMagazineText(int magazine)
    {
        Magazine.text = magazine.ToString();
    }
    //---------------------------------------------------------
    //マップ現在地を変更
    public void setMapUI(string Name)
    {
        //侵入したマップを確認する
        switch (Name)
        {
            case "GameScene01":
                if (!EnteredMaps[0])
                {
                    EnteredMaps[0] = true;
                }
                break;
            case "GameScene04":
                if (!EnteredMaps[1])
                {
                    EnteredMaps[1] = true;
                }
                break;
            case "GameScene05":
                if (!EnteredMaps[2])
                {
                    EnteredMaps[2] = true;
                }
                break;
            case "GameScene06":
                if (!EnteredMaps[3])
                {
                    EnteredMaps[3] = true;
                }
                break;
            case "GameScene08":
                if (!EnteredMaps[4])
                {
                    EnteredMaps[4] = true;
                }
                break;
            case "GameScene09":
                if (!EnteredMaps[5])
                {
                    EnteredMaps[5] = true;
                }
                break;
        }
        for(int i = 0; i < Maps.Length; ++i)
        {
            if (EnteredMaps[i])
            {
                Maps[i].sprite = MapImgs[i];
                Maps[i].color = new Color(1, 1, 1, 1);
            }
        }
        //シーン名でマップの画像を変更する
        switch (Name)
        {
            case "GameScene01":
                Maps[0].sprite = MapImgs_waku[0];
                NowMapNum = 0;
                break;
            case "GameScene04":
                Maps[1].sprite = MapImgs_waku[1];
                NowMapNum = 1;
                break;
            case "GameScene05":
                Maps[2].sprite = MapImgs_waku[2];
                NowMapNum = 2;
                break;
            case "GameScene06":
                Maps[3].sprite = MapImgs_waku[3];
                NowMapNum = 3;
                break;
            case "GameScene08":
                Maps[4].sprite = MapImgs_waku[4];
                NowMapNum = 4;
                break;
            case "GameScene09":
                Maps[5].sprite = MapImgs_waku[5];
                NowMapNum = 5;
                break;
        }
    }
    //---------------------------------------------------------
    public void getWeaponObjs()
    {
        try {
            Weapon = player.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            WS = Weapon.GetComponent<WeaponScript>();
        }
		catch { }
    }
    //---------------------------------------------------------
    public void setHPGaugeColor(Color color)
    {
        HP_Gauge.color = color;
    }
    //---------------------------------------------------------
}
