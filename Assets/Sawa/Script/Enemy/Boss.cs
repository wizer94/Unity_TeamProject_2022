using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//-----------------------------------------------------------------
//�G �{�X
//-----------------------------------------------------------------

public class Boss : EnemyClass
{
    //�ړ��N���X���C���X�^���X��
    EMove move = new EMove();

    //����
    [SerializeField] GameObject Weapon;
    [SerializeField] GameObject Weapon2;
    //�U���N���X
    EAttack attack_Main;
    EAttack attack_Sub;

    //�G�̃X�N���v�g
    [SerializeField] EnemyImg ImgScript;

    [SerializeField] float TakeDist;    //�U�����@�v���C���Ƃ̊Ԃ̋���

    //�U���֘A�̕ϐ�
    ColliderScript AttackAreaCol;

    NavMeshAgent Nav;

    bool[] NoMoveDir = new bool[4] { true, true, true, true };
    [SerializeField] ColliderScript[] MoveCol = new ColliderScript[4];     //�X�N���v�g�i�[

    float DamageTimer;

    bool isRush = false;
    Vector3 RushPoint;               //�ːi�ڕW�n�_  
    [SerializeField] ParticleSystem SuctionEff;
    bool isInsRushEff = false;

    //-----------------------------------------------------------------    
    //�ύX�_
    public GameObject result;    
    //-----------------------------------------------------------------    
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

        //NavMesh�̖�����
        Nav.enabled = false;

        //�X�i�C�p�[�̃��C������
        attack_Main.setLayActive(false);

        //�ύX�_
        result.SetActive(false);
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

                //HP�Q�[�W��ύX����
                this.gameObject.GetComponent<HPBar>().Change_HPGauge(getHp(), getMaxHp());

            }

            //�G�t�F�N�g���Ԃ̔���
            if (DamageTimer >= 0.1f)
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
            //NavMesh��؂�
            Nav.enabled = false;

            //�U�����~�߂�
            attack_Main.setAttackFlag(false);
            attack_Sub.setAttackFlag(false);
            //���C������
            attack_Main.setLayActive(false);
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
        InitializeHP();
        //������HP
        setEscHP(0.1f * getMaxHp());
    }
    //-----------------------------------------------------------------
    public override void Move()
    {
        //�ړ���
        Vector2 vec;

        //���g�̏�Ԃ��擾���ĕ��򂷂�
        switch (getState())
        {
            //�������-----------------------------
            case State.Non:
                //���G�̕��������Z�b�g����
                move.setMoveDir(true);

                //HP�Q�[�W������
                this.gameObject.GetComponent<HPBar>().Enabled_HPGauge(false);

                //�摜��ύX����
                ImgScript.ChangeImage(false);
                break;
            //�T�����-----------------------------
            case State.Search:
                break;
            //�ǔ����-----------------------------
            case State.AimPlayer:
                //�i�r���b�V�����N��������
                if (!Nav.enabled)
                {
                    Nav.enabled = true;

                    attack_Main.setAttackFlag(true);
                    attack_Sub.setAttackFlag(true);
                    //���C���o��
                    attack_Main.setLayActive(true);
                }

                //�ڕW���w��
                Nav.SetDestination(player.transform.position);

                //�ǔ������U�����s��
                attack_Main.Fire(true);
                attack_Sub.Fire(true);

                break;
            //��~���-----------------------------
            case State.Stop:
                //��~
                vec = move.Stop();

                //�ːi�̏������s��
                if (isRush)
                {
                    if (!OneceFlag)
                    {
                        //�摜��ύX����
                        ImgScript.ChangeImage(true);
                        //�e�ɂ��U���͎~�߂�
                        attack_Main.setAttackFlag(false);
                        attack_Sub.setAttackFlag(false);
                        attack_Main.setLayActive(false);
                        OneceFlag = true;
                    }
                }
                break;
            //�U�����-----------------------------
            case State.Attack:
                //�ːi�łȂ��ꍇ
                //�e�ɂ��U��
                if (!OneceFlag)
                {
                    //�摜��ύX����
                    ImgScript.ChangeImage(true);
                    //�U���J�n
                    attack_Main.setAttackFlag(true);
                    attack_Sub.setAttackFlag(true);
                    attack_Main.setLayActive(true);
                    OneceFlag = true;
                }
                //�U���@�C���X�^���X����������g��
                attack_Main.Fire(true);
                attack_Sub.Fire(true);

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
            //�ːi���-----------------------------
            case State.Rush:
                //�ڕW�n�_���߂�
                if (!OneceFlag)
                {
                    RushPoint = player.transform.position;
                    OneceFlag = true;
                }
                //�ːi���s��
                if (CheckNoMoveDir())
                {
                    vec = move.Rush(RushPoint, this.gameObject, getMoveSpeed() + 0.2f);
                    gameObject.transform.Translate(vec);
                }
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
                //�������Œ�~�ɂ���
                st = State.Stop;

                break;
            //�T�����-----------------------------
            case State.Search:               
                break;
            //�ǔ����-----------------------------
            case State.AimPlayer:

                //��~
                //�v���C�����U���͈͂ɓ�������
                if (AttackAreaCol.getAttackFlag())
                {
                    st = State.Stop;
                    this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    Nav.enabled = false;
                }
                //�v���C����10�ȏ㗣��Ă�����
                if(getPlayerDist() >= 10)
                {
                    this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    Nav.enabled = false;

                    //��Ԃ�Stop�ɕύX����
                    st = State.Stop;
                    //�ːi�t���O�𗧂Ă�
                    isRush = true;

                    //�G�t�F�N�g���o��
                    if(!isInsRushEff)
                    {
                        ParticleSystem par = Instantiate(SuctionEff, gameObject.transform.position, Quaternion.identity);
                        par.GetComponent<TrackEff>().setTackObj(gameObject);
                        isInsRushEff = true;
                    }
                }
                break;
            //��~���-----------------------------
            case State.Stop:
                //�U�����󂯂���
                if (getDamageHitFlag())
                {
                    st = State.AimPlayer;

                    //HP�Q�[�W���o��
                    this.gameObject.GetComponent<HPBar>().Enabled_HPGauge(true);
                }
                //�U���@�U���͈͓��Ƀv���C��������
                if (getPlayerDist() <= 10)
                {
                    st = State.Attack;

                    //HP�Q�[�W���o��
                    this.gameObject.GetComponent<HPBar>().Enabled_HPGauge(true);
                }
                //�ːi
                if(MoveTimer >= 2 && isRush)
                {
                    st = State.Rush;
                }
                break;
            //�U�����-----------------------------
            case State.Attack:
                //�ǔ�
                //�v���C�����U���͈͂���o����
                if (!AttackAreaCol.getAttackFlag())
                {
                    st = State.AimPlayer;

                    //�U�����~�߂�
                    attack_Main.setAttackFlag(false);
                }

                break;
            //�������-----------------------------
            case State.Escape:
                //��ԑJ�ڂȂ�
                //�@���@����������

                break;
            //������-----------------------------
            case State.Avoidance:
                //�����Ȃ�
                break;
            case State.Rush:
                //4�b�ԓːi����
                if(MoveTimer >= 4)
                {
                    st = State.Stop;
                    isInsRushEff = false;
                }
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
                DropItem(this.gameObject.transform.position, getMaxDropCnt() + 1);
                //�t���O��false�ɂ���
                setAppearFlag(false);

                //HP�Q�[�W������
                this.gameObject.GetComponent<HPBar>().Enabled_HPGauge(false);

                //�G�t�F�N�g��\��
                par.PlayAround(destEff, this.gameObject.transform.position);
                //SE�̍Đ�
                se.Play(SE_Explos);

                //���ł���
                DestroyMe();
            }

            //��_���t���O�𗧂Ă�
            setDamageHitFlag(true);
        }
    }
    //-----------------------------------------------------------------
    //-----------------------------------------------------------------
    void DestroyMe()
    {
        GameObject Parent = transform.parent.gameObject;

        //���ł���
        kill(Parent);
    }
    //-----------------------------------------------------------------
    void ImgTurn()
    {
        Vector2 playerVector = player.transform.position - transform.position;
        bool isFlip = playerVector.x > 0;

       �@//������ύX����֐����Ă�
       �@ImgScript.ImgTurn(isFlip);
    }
    //-----------------------------------------------------------------
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
    //-----------------------------------------------------------------
    //Start���ɃI�u�W�F�N�g���擾����
    void GetObjects()
    {
        //�I�u�W�F�N�g�擾�N���X���C���X�^���X��
        ObjectGetClass GetObj = new ObjectGetClass();


        //�U���͈̓R���C�_�[�̃X�N���v�g���擾
        GameObject AttackAreaObj = GetObj.GetChild_Obj(this.gameObject, "AttackArea");
        AttackAreaCol = AttackAreaObj.GetComponent<ColliderScript>();

        //�v���C���̎擾
        player = GetObj.GetGameObject("player");
        //NavMesh�̎擾
        Nav = this.gameObject.GetComponent<NavMeshAgent>();

        //�G�ƃI�u�W�F�N�g�𓯊�������
        GameObject ImgObj = transform.parent.transform.GetChild(0).gameObject;
        setEnemyImg(ImgObj);

        //�G�ɂ����NavMesh�̍ō����x�����߂�
        if (ImgObj.name == "NormalEnemyImg")
        {
            Nav.speed = 4.0f;
        }
        else if (ImgObj.name == "BigEnemyImg")
        {
            Nav.speed = 2.0f;
        }
    }
    //-----------------------------------------------------------------
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

        //�J�����X�N���v�g�擾
        CameSc = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        //�U���X�N���v�g
        attack_Main = Weapon.GetComponent<EAttack>();
        attack_Sub = Weapon2.GetComponent<EAttack>();
    }
    //-----------------------------------------------------------------
    void ZeroClear()
    {
        DamageTimer = 0;
        RushPoint = Vector2.zero;
    }
    //-----------------------------------------------------------------
    float getPlayerDist()
    {
        //�v���C���Ƃ̋������v�Z
        Vector2 dt = transform.position - player.transform.position;
        float dir = Mathf.Sqrt(dt.x * dt.x + dt.y * dt.y);

        return Mathf.Abs(dir);
    }
    //-----------------------------------------------------------------
    public void OnDestroy()
    {
        result.SetActive(true);
    }
    //-----------------------------------------------------------------

}
