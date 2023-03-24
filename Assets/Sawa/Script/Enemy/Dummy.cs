using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : EnemyClass
{
    //�G�̃X�N���v�g
    [SerializeField] EnemyImg ImgScript;

    float DamageTimer;

    //-----------------------------------------------------------------    
    void Start()
    {
        //�o��������
        setAppearFlag(true);

        //������
        Initialize();

        //�I�u�W�F�N�g�擾
        GetObjects();

        //�X�N���v�g�擾
        GetScripts();
    }

    //-----------------------------------------------------------------
    void Update()
    {
        //�G�t�F�N�g����
        if (getDamageHitFlag())
        {
            //�ŏ��Ȃ�
            if (!ImgScript.getDamageEffFlag())
            {
                //�t���O��ς���
                ImgScript.setDamageEffFlag(true);
            }

            //�G�t�F�N�g���Ԃ̔���
            if (DamageTimer >= 0.2f)
            {
                DamageTimer = 0;
                setDamageHitFlag(false);

                //�G�t�F�N�g���I������
                ImgScript.setDamageEffFlag(false);
            }
            DamageTimer += Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        //�����E�������̔���
        if (getAppearFlag())
        {
            Think();
            Move();
        }
        else
        {
            //�R���C�_�[��؂�
            ImgScript.setActiveCollider(false);

            //����ݒ肷��
            ImgScript.ChengeAlphaDec();
        }

        //�摜�̌����̕ύX
        ImgTurn();
    }
    //-----------------------------------------------------------------
    //�e����̐ݒ�
    void Initialize()
    {
        //�[���N���A
        ZeroClear();

        //�ړ����x
        setMoveSpeed(getMoveSpeed());
        //��Ԃ̐ݒ�
        setState(State.Non);
        //HP
        setHP(getMaxHp());
    }
    //-----------------------------------------------------------------
    public override void Move()
    {
        //���g�̏�Ԃ��擾���ĕ��򂷂�
        switch (getState())
        {
            //�������-----------------------------
            case State.Non:
                break;
            //�T�����-----------------------------
            case State.Search:
                break;
            //�ǔ����-----------------------------
            case State.AimPlayer:
                break;
            //��~���-----------------------------
            case State.Stop:
                break;
            //�U�����-----------------------------
            case State.Attack:
                break;
            //�������-----------------------------
            case State.Escape:
                break;
            //������-----------------------------
            case State.Avoidance:
                //�����Ȃ�
                break;
            //�f�t�H���g-----------------------------
            default:
                //��{�����ɗ��邱�Ƃ͂Ȃ�
                Debug.Log("Move-�f�t�H���g");
                break;
        }
    }

    //-----------------------------------------------------------------
    public override void Think()
    {
        //��Ԃ��ꎞ�ۑ�����ϐ�
        State st = getState();

        //���g�̏�Ԃ��擾���ĕ��򂷂�
        switch (getState())
        {
            //�������-----------------------------
            case State.Non:

                //�������Œ�~��Ԃɂ���
                st = State.Stop;
                break;
            //�T�����-----------------------------
            case State.Search:
                break;
            //�ǔ����-----------------------------
            case State.AimPlayer:
                break;
            //��~���-----------------------------
            case State.Stop:
                break;
            //�U�����-----------------------------
            case State.Attack:
                break;
            //�������-----------------------------
            case State.Escape:
                break;
            //������-----------------------------
            case State.Avoidance:
                break;
            //�f�t�H���g-----------------------------
            default:
                //��{�����ɗ��邱�Ƃ͂Ȃ�
                Debug.Log("Think-�f�t�H���g");
                break;
        }

        //�^�C�}�[���J�E���g
        MoveTimer += Time.deltaTime;

        //��Ԃ�ύX����֐����Ă�
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
        //�����ԂłȂ� && �����Ă���
        if (getAppearFlag())
        {
            float tempHP = getHp() - damage;

            //HP��ύX����
            setHP(tempHP);

            //�G�t�F�N�g��\��
            par.Play(HitDamageEff, this.gameObject.transform.position);
            //SE�Đ�
            se.Play(SE_hit);

            //�c��HP�̔���@&&�@�������Ă�����
            if (getHp() <= 0 && getAppearFlag())
            {
                //�A�C�e���𗎂Ƃ�
                int DropCnt = Random.Range(1, getMaxDropCnt() + 1);
                DropItem(this.gameObject.transform.position, DropCnt);
                //�t���O��false�ɂ���
                setAppearFlag(false);

                //���ł���
                DestroyMe();

                //�G�t�F�N�g��\��
                par.PlayAround(destEff, this.gameObject.transform.position);
                //SE�̍Đ�
                se.Play(SE_Explos);
            }

            //��_���t���O�𗧂Ă�
            setDamageHitFlag(true);
        }
    }
    //-----------------------------------------------------------------
    void DestroyMe()
    {
        GameObject Parent = transform.parent.gameObject;
       
        //���ł���i�_�~�[�̓L�����Ɋ܂܂Ȃ��̂ł��̂܂�Destroy����j
        Destroy(Parent, 2);
    }
    //-----------------------------------------------------------------
    void ImgTurn()
    {
        Vector2 playerVector = player.transform.position - transform.position;
        bool isFlip = playerVector.x < 0;

       �@//������ύX����֐����Ă�
       �@ImgScript.ImgTurn(isFlip);
    }
    //-----------------------------------------------------------------
    //Start���ɃI�u�W�F�N�g���擾����
    void GetObjects()
    {
        //�v���C���̎擾
        player = GameObject.FindGameObjectWithTag("player");

        //�G�ƃI�u�W�F�N�g�𓯊�������
        GameObject ImgObj = transform.parent.transform.GetChild(0).gameObject;
        setEnemyImg(ImgObj);
    }
    //-----------------------------------------------------------------
    void GetScripts()
    {
        //�G�̃X�N���v�g���擾
        ImgScript = enemyImg.GetComponent<EnemyImg>();
    }
    //-----------------------------------------------------------------
    void ZeroClear()
    {
        DamageTimer = 0;
    }
    //-----------------------------------------------------------------
    //-----------------------------------------------------------------
}
