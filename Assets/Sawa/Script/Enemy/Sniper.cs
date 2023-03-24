using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : EnemyClass
{   
    //�ړ��N���X���C���X�^���X��
    EMove move = new EMove();

    //����
    [SerializeField] GameObject Weapon;
    //�U���N���X
    [SerializeField] EAttack attack;

    //�G�̃X�N���v�g
    [SerializeField] EnemyImg ImgScript;

    [SerializeField] float TakeDist;    //�U�����@�v���C���Ƃ̊Ԃ̋���

    bool[] NoMoveDir = new bool[4] { true, true, true, true };
    [SerializeField] ColliderScript[] MoveCol = new ColliderScript[4];     //�X�N���v�g�i�[

    float DamageTimer;

    //---------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        //�o��������
        setAppearFlag(true);

        //������
        Initialize();
        move.EMoveInitialize();

        //�I�u�W�F�N�g�擾
        GetObjects();

        //�X�N���v�g�擾
        GetScripts();

        //�X�i�C�p�[����SE�̉��ʂ��グ��
        se.setVolume(0.2f);
    }
    //---------------------------------------------------------------------
    // Update is called once per frame
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
    //---------------------------------------------------------------------
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
            //�U�����~�߂�
            attack.setAttackFlag(false);
            attack.setLayActive(false);

            //�R���C�_�[��؂�
            ImgScript.setActiveCollider(false);

            //����ݒ肷��
            ImgScript.ChengeAlphaDec();
        }

        //�摜�̌����̕ύX
        ImgTurn();
    }
    //---------------------------------------------------------------------
    void Initialize()
    {
        //�[���N���A
        ZeroClear();

        //�ړ����x
        setMoveSpeed(getMoveSpeed());
        //��Ԃ̐ݒ�
        setState(State.Non);
        //HP
        InitializeHP();
        //������HP
        setEscHP(0.1f * getMaxHp());
    }
    //---------------------------------------------------------------------
    public override void Move()
    {
        //�ړ���
        Vector2 vec;

        //���g�̏�Ԃ��擾���ĕ��򂷂�
        switch (getState())
        {
            //�������-----------------------------
            case State.Non:
                //�����Ȃ�
                break;
            //�T�����-----------------------------
            case State.Search:
                //�����Ȃ�
                break;
            //�ǔ����-----------------------------
            case State.AimPlayer:
                //�����Ȃ�
                break;
            //��~���-----------------------------
            case State.Stop:

                if (!OneceFlag)
                {
                    attack.setAttackFlag(false);

                    //���C������
                    attack.setLayActive(false);

                    //�摜��ύX����
                    ImgScript.ChangeImage(false);

                    OneceFlag = true;
                }

                break;
            //�U�����-----------------------------
            case State.Attack:
                if (!OneceFlag)
                {
                    attack.setAttackFlag(true);
                    //���C���o��
                    attack.setLayActive(true);

                    //�摜��ύX����
                    ImgScript.ChangeImage(true);

                    OneceFlag = true;
                }

                attack.Fire(false);

                //��苗�������
                if (HitCircle(this.gameObject, player, TakeDist))
                {
                    if (CheckNoMoveDir())
                    {
                        vec = move.TakeDistance(player, this.gameObject, getMoveSpeed());
                        gameObject.transform.Translate(vec);
                    }
                }

                break;
            //�������-----------------------------
            case State.Escape:
                //�����Ȃ�
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
    //---------------------------------------------------------------------
    public override void Think()
    {
        //��Ԃ��ꎞ�ۑ�����ϐ�
        State st = getState();

        //���g�̏�Ԃ��擾���ĕ��򂷂�
        switch (getState())
        {
            //�������-----------------------------
            case State.Non:

                //������
                st = State.Stop;

                break;
            //�T�����-----------------------------
            case State.Search:
                //�����Ȃ�
                break;
            //�ǔ����-----------------------------
            case State.AimPlayer:
                //�����Ȃ�
                break;
            //��~���-----------------------------
            case State.Stop:

                //�v���C�������͈͓��ɂ���
                if (HitCircle(this.gameObject, player, 25))
                {
                    st = State.Attack;
                }
                break;
            //�U�����-----------------------------
            case State.Attack:
                //�v���C�������͈͓��ɂ��Ȃ�
                if (!HitCircle(this.gameObject, player, 25))
                {
                    st = State.Stop;
                }

                break;
            //�������-----------------------------
            case State.Escape:
                //�����Ȃ�
                break;
            //������-----------------------------
            case State.Avoidance:
                //�����Ȃ�
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
    //---------------------------------------------------------------------
    void setEnemyImg(GameObject g)
    {
        enemyImg = g;
    }
    public GameObject getEnemyImg()
    {
        return enemyImg;
    }
    //---------------------------------------------------------------------
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

                //�G�t�F�N�g��\��
                par.PlayAround(destEff, this.gameObject.transform.position);
                //SE�̍Đ�
                se.setVolume(0.2f);
                se.Play(SE_Explos);

                //���ł���
                DestroyMe();
            }

            //��_���t���O�𗧂Ă�
            setDamageHitFlag(true);
        }
    }
    //---------------------------------------------------------------------
    void DestroyMe()
    {
        GameObject Parent = transform.parent.gameObject;

        //���ł���
        kill(Parent);
    }
    //---------------------------------------------------------------------
    void ImgTurn()
    {
        Vector2 playerVector = player.transform.position - transform.position;
        bool isFlip = playerVector.x < 0;

       �@//������ύX����֐����Ă�
       �@ImgScript.ImgTurn(isFlip);
    }
    //---------------------------------------------------------------------
    //Start���ɃI�u�W�F�N�g���擾����
    void GetObjects()
    {
        //�I�u�W�F�N�g�擾�N���X���C���X�^���X��
        ObjectGetClass GetObj = new ObjectGetClass();

        //�v���C���̎擾
        player = GetObj.GetGameObject("player");

        //�G�ƃI�u�W�F�N�g�𓯊�������
        GameObject ImgObj = transform.parent.transform.GetChild(0).gameObject;
        setEnemyImg(ImgObj);
    }
    //---------------------------------------------------------------------
    void GetScripts()
    {
        //�G�̃X�N���v�g���擾
        ImgScript = enemyImg.GetComponent<EnemyImg>();


        //�q�I�u�W�F�N�g
        GameObject[] temp = new GameObject[4] {
            transform.Find("MoveColLeft").gameObject,
            transform.Find("MoveColRight").gameObject,
            transform.Find("MoveColUp").gameObject,
            transform.Find("MoveColDown").gameObject
        };
        //  �X�N���v�g�擾
        for (int i = 0; i < 4; i++)
        {
            MoveCol[i] = temp[i].GetComponent<ColliderScript>();
        }
        //�U���X�N���v�g
        attack = Weapon.GetComponent<EAttack>();
    }
    //---------------------------------------------------------------------
    void ZeroClear()
    {
        DamageTimer = 0;
    }
    //---------------------------------------------------------------------
    bool CheckNoMoveDir()
    {
        //�l�������m�F
        for (int i = 0; i < 4; ++i)
        {
            NoMoveDir[i] = MoveCol[i].getMoveFlag();
        }

        //�l�����m�F����
        return NoMoveDir[0] && NoMoveDir[1] && NoMoveDir[2] && NoMoveDir[3];
    }
    //---------------------------------------------------------------------
}
