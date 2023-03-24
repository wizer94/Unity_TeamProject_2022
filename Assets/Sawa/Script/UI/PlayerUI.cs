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

    //�g�p���̕���
    [SerializeField] Image UsingWeapon;
    [SerializeField] Sprite[] WeaponsImg;
    [SerializeField] Text Magazine;

    //�}�b�v
    [SerializeField] Image[] Maps;
    [SerializeField] GameObject NowMap;
    [SerializeField] Sprite[] MapImgs_waku;
    [SerializeField] Sprite[] MapImgs;
    bool isExpansMap;   //�}�b�v�̊g��\���t���O
    int NowMapNum;
    bool[] EnteredMaps; //�N�������}�b�v
    //���݈ʒu�\��
    [SerializeField] GameObject PlayerMark;
    [SerializeField] SE SE;
    [SerializeField] AudioClip clip;

    int a = 0;

    // Start is called before the first frame update
    void Start()
    {
        //player�̎擾
        player = GameObject.FindGameObjectWithTag("player");

        //HP�o�[
        //�t���O�̏�����
        isShaveGauge = false;
        isHealGauge = false;
        Damage_Gauge.enabled = false;
        Heal_Gauge.enabled = false;

        //�g�p���̕���
        //���햼�̎擾
        getWeaponObjs();
        WeaponUI(Weapon.name);

        //�}�b�v�@���݈ʒu��ύX
        NowMapNum = 0;
        isExpansMap = false;
        NowMap.SetActive(false);
        EnteredMaps = new bool[] { false, false, false, false, false, false };
        //�}�b�v�𓧖��ɂ���
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
        //�}�b�v��\��
        if (Input.GetKeyDown(KeyCode.Tab) && Time.timeScale != 0)
        {           
            //�t���O��ύX
            isExpansMap = !isExpansMap;
            //NowStage��ύX����
            NowMap.SetActive(isExpansMap);
            //SE���Đ�
            SE.Play(clip);
        }
        //�Q�[�W����鏈��
        if (getShaveGauge())
        {
            Shave_HPGauge();
        }
        //�Q�[�W���񕜂��鏈��
        if (getisHealGauge())
        {
            Heal_HPgauge();
        }

        if (WS) {
            //�c�i��
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
        //�_���[�W�Q�[�W�����
        Damage_Gauge.fillAmount -= Time.deltaTime / 5;

        //�_���[�W�Q�[�W�����݂�Hp�����̒����ɂȂ�����
        if (Damage_Gauge.fillAmount < HP_Gauge.fillAmount)
        {
            //��鏈�����I������
            setShaveGauge(false);

            //���I������̂ŃQ�[�W���\���ɂ���
            Damage_Gauge.enabled = false;
        }
    }
    void Heal_HPgauge()
    {
        //HP�Q�[�W�����������₷
        HP_Gauge.fillAmount += Time.deltaTime / 5;

        //HP�Q�[�W���񕜃Q�[�W��葽���Ȃ�����
        if (HP_Gauge.fillAmount >= Heal_Gauge.fillAmount)
        {
            //���₷�������I������
            isHealGauge = false;

            //���I������̂ŃQ�[�W���\���ɂ���
            Heal_Gauge.enabled = false;
        }
    }
    //�_���[�W���󂯂�����UI��ύX����
    public void ChDamage_HPGauge(float hp, float max_Hp)
    {
        Damage_Gauge.enabled = true;                        //�_���[�W�Q�[�W���g�p����̂ŕ\������
        Damage_Gauge.fillAmount = HP_Gauge.fillAmount;      //�_���[�W�Q�[�W��Fill��΂Ɠ��������ɂ���

        //���݂�Hp���X�V����i�΂̃Q�[�W�������Ȃ���̂ł����ŏ������s���j
        HP_Gauge.fillAmount = hp / max_Hp;  //UI���X�V����
        setShaveGauge(true);                //�t�O�𗧂Ă�@���@�Ԃ��Q�[�W�i�_���[�W�Q�[�W�j�����

        //�c��Q�[�W�ŐF��ς���
        if (HP_Gauge.fillAmount <= 0.2f)
        {
            //�����Ȃ�ԂɕύX����
            HP_Gauge.color = new Color(1, 0.5f, 0);
        }
        else if (HP_Gauge.fillAmount <= 0.5f)
        {
            //�����Ȃ物�F�ɕύX����
            HP_Gauge.color = new Color(1, 1, 0);
        }        
    }
    //HP���񕜂����Ƃ���Ui��ύX����
    public void ChHeal_HPGauge(float HP,float maxHP)
    {
        Heal_Gauge.enabled = true;            //���F�̃Q�[�W���g�p���邽�߁A�\������
        //���F�̃Q�[�W���񕜌�̒l�ɂ���i���F�̃Q�[�W�������Ȃ葝�₷�̂ł����ŏ������s���j
        Heal_Gauge.fillAmount = HP / maxHP;  //UI���X�V����
        isHealGauge = true;                //�t���O�𗧂Ă�@���@�΂̃Q�[�W�����������₷

        //�Q�[�W�̐F��ύX����
        if(Heal_Gauge.fillAmount >= 0.5f)
        {
            //�΂ɕύX����
            HP_Gauge.color = new Color(0, 1, 0);
        }
        else if (Heal_Gauge.fillAmount >= 0.2f)
        {
            //���F�ɕύX����
            HP_Gauge.color = new Color(1, 1, 0);
        }
    }
    //�΃Q�[�W�����
    public void DecHPGauge(float HP,float maxHP)
    {
        HP_Gauge.fillAmount = HP / maxHP;
    }

    //�g�p���̕���
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
        //�摜��ݒ�
        UsingWeapon.sprite = WeaponsImg[outNum];
    }
    //�c�i���\��
    void setMagazineText(int magazine)
    {
        Magazine.text = magazine.ToString();
    }
    //---------------------------------------------------------
    //�}�b�v���ݒn��ύX
    public void setMapUI(string Name)
    {
        //�N�������}�b�v���m�F����
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
        //�V�[�����Ń}�b�v�̉摜��ύX����
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
