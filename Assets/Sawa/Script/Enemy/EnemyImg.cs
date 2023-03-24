using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImg : MonoBehaviour
{
    //�A�^�b�`���邱��
    public GameObject enemyObj;
    [SerializeField] GameObject Weapon;

    SpriteRenderer SRen;
    SpriteRenderer WSRen;

    bool DamageEffFlag;

    //�G�̎��
    [SerializeField] string EnemyType;
    //�摜
    [SerializeField] Sprite SearchImg;  //�ʏ펞�̉摜
    [SerializeField] Sprite AttackImg;  //�U�����̉摜
    //�G�t�F�N�g
    [SerializeField] TrackEff particle;

    [SerializeField] private int AttakPoint;     //�U����

    void Start()
    {
        SRen = this.gameObject.GetComponent<SpriteRenderer>();
        WSRen = Weapon.GetComponent<SpriteRenderer>();

        DamageEffFlag = false;

        //�G�̎�ނ��擾
        EnemyType = this.gameObject.name;
    }
    //---------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        //�ʒu�����������ē�����
        this.gameObject.transform.position = enemyObj.transform.position;

        //�_���[�W�G�t�F�N�g�̏���
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

        //�{�̂ɍ��킹�ďe��������
        WSRen.color = color;
        //�G�t�F�N�g������
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

        //false���Z�b�g���ꂽ��
        if (!Flag)
        {
            //alpha��߂�
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
            //�ʏ�̓G
            case "NormalEnemyImg":
                setSize = 0.3f;
                break;
            //�傫���G
            case "BigEnemyImg":
                setSize = 0.5f;
                break;
            //�h���[��
            case "DroneImg":
                setSize = 0.18f;
                break;
            //���U��
            case "RushEnemyImg":
                setSize = 0.15f;
                break;
            //Boss
            case "BossImg":
                setSize = 0.4f;
                break;
            //�X�i�C�p�[
            case "SniperImg":
                setSize = 0.3f;
                break;
            //�_�~�[
            case "DummyImg":
                setSize = 0.3f;
                break;
        }

        //���p�ӏ�
        if(EnemyType != "BigEnemyImg" && EnemyType != "BossImg")    //�摜�ɂ���Č������Ⴄ
        {
            //�ʏ�
            this.gameObject.transform.localScale = new Vector3(isFlip ? setSize : setSize * -1, setSize, 1);
        }
        else
        {
            //�傫���G�@+�@�{�X
            this.gameObject.transform.localScale = new Vector3(isFlip ? setSize * -1: setSize, setSize, 1);
        }

        if(EnemyType != "DroneImg")
        {
            if (EnemyType != "BossImg")
            {
                //�G�t�F�N�g�𔽓]
                particle.TurnEff(isFlip, isFlip ? 1 : -1);
            }
            else
            {
                //�G�t�F�N�g�𔽓]
                particle.TurnEff(isFlip, isFlip ? -1 : 1);
            }
        }
    }
    //---------------------------------------------------------------------------
    public void CollHitDamage(float damage)
    {
        //�_���[�W�������Ăяo��
        switch (EnemyType)
        {
            //�ʏ�̓G
            default:
                enemyObj.GetComponent<NormalEnemy>().HitDamage(damage);
                break;
            //�{�X
            case "BossImg":
                enemyObj.GetComponent<Boss>().HitDamage(damage);
                break;
            //�X�i�C�p�[
            case "SniperImg":
                enemyObj.GetComponent<Sniper>().HitDamage(damage);
                break;
            //�ːi
            case "RushEnemyImg":
                enemyObj.GetComponent<RushEnemy>().HitDamage(damage);
                break;
            //�h���[��
            case "DroneImg":
                enemyObj.GetComponent<Drone>().HitDamageDrone(damage);
                break;
            //�_�~�[
            case "DummyImg":
                enemyObj.GetComponent<Dummy>().HitDamage(damage);
                break;
        }
    }
    //---------------------------------------------------------------------------
    //�R���C�_�[�̗L�E��������
    public void setActiveCollider(bool Flag)
    {
        BoxCollider2D col = gameObject.GetComponent<BoxCollider2D>();

        col.enabled = Flag;
    }
    //---------------------------------------------------------------------------
    void OnCollisionEnter2D(Collision2D col)
    {
        //�v���C���ƐڐG���Ă�����
        if(col.gameObject.tag == "player")
        {
            //���ʂƈႤ����������ꍇ�̂ݏ���
            switch (EnemyType)
            {
                //-------------------------------
                case "RushEnemyImg":
                    //���g�ɂ��_���[�W��^����i�ő�HP���_���[�W�Ƃ���j
                    CollHitDamage(enemyObj.GetComponent<RushEnemy>().getMaxHp());
                    break;
                //-------------------------------
                default:
                    //�ʏ�̏���(��{�͂����ɗ���)
                    //�_�~�[�͊܂܂Ȃ�
                    if(EnemyType != "DummyImg")
                    {
                        col.gameObject.GetComponent<PlayerController>().HitDamage(10);
                    }

                    break;
            }
        }
    }
    //---------------------------------------------------------------------------
    //�摜��ύX���郁�\�b�h
    public void ChangeImage(bool isAttack)
    {
        //�U�����Ȃ�
        if (isAttack)
        {
            SRen.sprite = AttackImg;
        }
        //�ʏ�Ȃ�
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
