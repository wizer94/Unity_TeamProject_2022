using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//-------------------------------------------------------------------------------------------
//�G�@�h���[��
//-------------------------------------------------------------------------------------------

public class Drone : EnemyClass
{
    //�ړ��N���X���C���X�^���X��
    EMove move = new EMove();

    //�G�̃X�N���v�g
    [SerializeField] EnemyImg ImgScript;

    //���񃋁[�g�z��(�ϒ��z��)
    [SerializeField] List<string> Root = new List<string>();
    [SerializeField]List<int> RTime = new List<int>();
    int dirCnt;     //�ړ������J�E���^
    float dirTimer; //�ړ����ԃJ�E���^

    //�T���֘A�̕ϐ�
    ColliderScript SearchAreaCol;
    //�U���֘A�̕ϐ�
    ColliderScript AttackAreaCol;

    //�ǔ��p
    NavMeshAgent Nav;
    //����֘A�̕ϐ�
    //BoxCollider2D col;

    //���̈ʒu�ɖ߂邽�߂̕ϐ�
    Vector2 targetPos;
    bool RetTarget;

    Rigidbody2D rigid;

    float DamageTimer;
    float preDamage;

    //-------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        //�o��������
        setAppearFlag(true);

        //������
        Initialize();

        //���񃋁[�g�ǂݍ���
        LoadRoot();

        //�G�Ɩ{�̂𓯊�������
        GameObject Parent = transform.parent.gameObject;
        setEnemyImg(Parent.transform.Find("DroneImg").gameObject);

        //�I�u�W�F�N�g�擾
        GetObjects();

        //�X�N���v�g�擾
        GetScripts();

        //�t���O�̏�����
        Nav.enabled = false;
        //����t���O�ݒ�i���e�͊m�����j
        setAvoidFlag(true);
        RetTarget = false;
    }
    //-------------------------------------------------------------------------------------------
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
            Nav.enabled = false;

            //�R���C�_�[��؂�
            ImgScript.setActiveCollider(false);

            //����ݒ肷��
            ImgScript.ChengeAlphaDec();
        }
    }
    private void Update()
    {
        //���G����
        if (getDamageHitFlag())
        {
            //����Ȃ炱�̏������甲����
            if (getState() == State.Avoidance)
            {
                DamageTimer = 1;
            }

            //�ŏ��Ȃ�
            if (!ImgScript.getDamageEffFlag())
            {
                //�t���O��ς���
                ImgScript.setDamageEffFlag(true);
            }

            //���G���Ԃ̔���
            if (DamageTimer >= 0.2f)
            {
                DamageTimer = 0;
                setDamageHitFlag(false);

                //�G�t�F�N�g���I������
                ImgScript.setDamageEffFlag(false);
            }
            DamageTimer += Time.deltaTime;
        }

        //�摜�̌����̕ύX
        ImgTurn();
    }
    //-------------------------------------------------------------------------------------------
    void Initialize()   //������
    {
        //�e����̐ݒ�
        setMoveSpeed(0.05f);   //�ړ����x
        //��Ԃ̐ݒ�
        setState(State.Non);
        //HP
        InitializeHP();
        //������HP
        setEscHP(0.1f * getMaxHp());

        //�[���N���A
        ZeroClear();
    }
    //-------------------------------------------------------------------------------------------
    public override void Move()
    {
        //�ړ���
        Vector2 vec = Vector2.zero;

        //���g�̏�Ԃ��擾���ĕ��򂷂�
        switch (getState())
        {
            //�������-----------------------------
            case State.Non:

                break;
            //�T�����-----------------------------
            case State.Search:

                //���񃋁[�g����O��Ă��Ȃ���
                if (!RetTarget)
                {
                    //��胋�[�g����
                    //�ړ��ʂ̎擾
                    vec = move.SearchDrone(Root[dirCnt], getMoveSpeed());
                    //�ړ�
                    gameObject.transform.Translate(vec);

                    //�ړ�������ύX����J�E���^
                    if (dirTimer >= RTime[dirCnt])
                    {
                        dirTimer = 0;
                        dirCnt++;

                        //�����z��̍Ō�܂ŗ�����[dirCnt]������������
                        if (dirCnt >= RTime.Count)
                        {
                            dirCnt = 0;
                        }
                    }
                    dirTimer += Time.deltaTime;
                }
                //���񃋁[�g�ɖ߂�
                else
                {
                    vec = move.ReturnDrone(this.gameObject.transform.position, targetPos, getMoveSpeed());
                    //�ړ�
                    gameObject.transform.Translate(vec);

                    //���̈ʒu�ɖ߂��������ׂ�@�l����苗���ɓ�������
                    if( this.gameObject.transform.position.x >= targetPos.x - 0.5f &&   //����
                        this.gameObject.transform.position.x <= targetPos.x + 0.5f &&   //�E��
                        this.gameObject.transform.position.y <= targetPos.y + 0.5f &&   //�㑤
                        this.gameObject.transform.position.y >= targetPos.y - 0.5f)     //����
                    {
                        //�t���O��߂�
                        RetTarget = false;
                    }
                }
                break;
            //�ǔ����-----------------------------
            case State.AimPlayer:
                //�i�r���b�V�����N��������
                if (!Nav.enabled)
                {
                    Nav.enabled = true;

                    //�߂��Ă���r���łȂ� and �O�̏�Ԃ��U���łȂ�
                    if (!RetTarget && getPreState() != State.Attack)
                    {
                        //�߂���W��ۑ�����
                        targetPos = this.gameObject.transform.position;
                    }
                }
                //�i�r���b�V�����N�����Ă���Ȃ�
                this.Nav.SetDestination(player.transform.position);

                break;
            //��~���-----------------------------
            case State.Stop:
                //�i�r���b�V���𖳌�������
                Nav.enabled = false;

                //��~
                vec = move.Stop();
                break;
            //�U�����-----------------------------
            case State.Attack:
                if (!OneceFlag)
                {
                    CameSc.setCallEnemyFlag(true);
                    OneceFlag = true;
                }

                //��������鏈��
                //��苗�������
                if (HitCircle(this.gameObject, player, 4))
                {
                    vec = move.TakeDistance(player, this.gameObject, getMoveSpeed());
                    gameObject.transform.Translate(vec);
                }

                break;
            //�������-----------------------------
            case State.Escape:
                //�v���C���Ɣ��Ε����Ɉړ�
                vec = move.Escape(player, this.gameObject, getMoveSpeed());
                gameObject.transform.Translate(vec);

                //��莞�Ԃŏ�����
                if (MoveTimer >= 10)
                {
                    //����ݒ肷��
                    ImgScript.ChengeAlphaDec();

                    //0.1f�ȉ��Ȃ��\���ɂ���
                    if (ImgScript.getAlpha() <= 0.1f)
                    {
                        //���ł���
                        DestroyMe();
                    }
                }
                break;
            //������-----------------------------
            case State.Avoidance:
                //��x�����͂�������
                if (!OneceFlag)
                {
                    vec = move.Avoidance();
                    rigid.AddForce(vec);

                    OneceFlag = true;
                }

                break;
            //�f�t�H���g-----------------------------
            default:
                //��{�����ɗ��邱�Ƃ͂Ȃ�
                Debug.Log("Move-�f�t�H���g");
                break;
        }
    }
    //-------------------------------------------------------------------------------------------
    public override void Think()
    {
        //��Ԃ��ꎞ�ۑ�����ϐ�
        State st = getState();

        //���g�̏�Ԃ��擾���ĕ��򂷂�
        switch (getState())
        {
            //�������-----------------------------
            case State.Non:
                //�������ŒT����
                st = State.Search;

                break;
            //�T�����-----------------------------
            case State.Search:
                //�ǔ�
                //�v���C�����T���͈͂ɓ�������
                if (SearchAreaCol.getSearchFlag())
                {
                    st = State.AimPlayer;
                }
                //���
                //����\�@&&�@�U�����󂯂�
                if (getAvoidFlag() && getDamageHitFlag())
                {
                    st = State.Avoidance;
                }
                //����(���̗͂����������)
                if (getEscHP() >= getHp())
                {
                    st = State.Escape;
                }

                break;
            //�ǔ����-----------------------------
            case State.AimPlayer:

                //�T��
                //�ǔ��͈͂���v���C�����o����
                if (!SearchAreaCol.getSearchFlag())
                {
                    st = State.Non;
                    this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    Nav.enabled = false;

                    //���񃋁[�g�ɖ߂�t���O�𗧂Ă�
                    RetTarget = true;
                }

                //��~
                //�v���C�����U���͈͂ɓ�������
                if (AttackAreaCol.getAttackFlag())
                {
                    st = State.Stop;
                    this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    Nav.enabled = false;
                }

                //���
                //����\�@&&�@�U�����󂯂�
                if (getAvoidFlag() && getDamageHitFlag())
                {
                    st = State.Avoidance;
                    this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    Nav.enabled = false;
                }

                //����(���̗͂����������)
                if (getEscHP() >= getHp())
                {
                    st = State.Escape;
                    this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    Nav.enabled = false;
                }

                break;
            //��~���-----------------------------
            case State.Stop:
                //���G�@�v���C�����U���͈͂���o���@or�@��莞�Ԍo�߂���
                if (!AttackAreaCol.getAttackFlag())
                {
                    st = State.Search;

                    //�ړ��������Ē��I����
                    move.setMoveDir(true);
                }

                //�U���@�U���͈͓��Ƀv���C����������
                if (AttackAreaCol.getAttackFlag())
                {
                    st = State.Attack;
                }

                //���
                //����\�@&&�@�U�����󂯂�
                if (getAvoidFlag() && getDamageHitFlag())
                {
                    st = State.Avoidance;
                }

                //����(���̗͂����������)
                if (getEscHP() >= getHp())
                {
                    st = State.Escape;
                }

                break;
            //�U�����-----------------------------
            case State.Attack:
                //�ǔ�
                //�v���C�����U���͈͂���o����
                if (!AttackAreaCol.getAttackFlag())
                {
                    st = State.AimPlayer;

                    CameSc.setCallEnemyFlag(false);
                }

                //���
                //����\�@&&�@�U�����󂯂�
                if (getAvoidFlag() && getDamageHitFlag())
                {
                    st = State.Avoidance;
                }

                //����(���̗͂����������)
                if (getEscHP() >= getHp())
                {
                    st = State.Escape;
                }

                break;
            //�������-----------------------------
            case State.Escape:
                //��ԑJ�ڂȂ�
                //�@���@����������

                break;
            //������-----------------------------
            case State.Avoidance:
                //��莞�Ԍo�ߌ�@���@��~��ԂɂȂ�
                if (MoveTimer >= 0.2f)
                {
                    st = State.Stop;
                    rigid.velocity = Vector3.zero;

                    //�󂯂��_���[�W��␳����
                    setHP(getHp() + preDamage);

                    //����t���O��ύX����
                    AvoidRateRandom();
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
    //-------------------------------------------------------------------------------------------
    //�L�����E�������̊֐�
    //�L�����̓J�����̃X�N���v�g�ōs��
    public void ActiveFalse()
    {
        //�{�̖̂�����
        this.gameObject.SetActive(false);
    }
    //-------------------------------------------------------------------------------------------
    void setEnemyImg(GameObject g)
    {
        enemyImg = g;
    }
    public GameObject getEnemyImg()
    {
        return enemyImg;
    }
    //-------------------------------------------------------------------------------------------
    public void HitDamageDrone(float damage)
    {
        //���g�̏�Ԃ�����łȂ��� && ����ł��Ȃ��� && �U�����󂯂Ă��Ȃ��� �ɏ������s��
        if (!getAvoidFlag() && !getDamageHitFlag())
        {
            float tempHP = getHp() - damage;
            preDamage = damage;     //�_���[�W��ۑ�

            //�G�t�F�N�g��\��
            par.Play(HitDamageEff, this.gameObject.transform.position);
            //SE�Đ�
            se.Play(SE_hit);

            //�c��HP�̔���
            if (tempHP <= 0)
            {
                int DropCnt = Random.Range(1, getMaxDropCnt() + 1);
               //�A�C�e���𗎂Ƃ�
                DropItem(this.gameObject.transform.position, DropCnt);
                //�t���O��false�ɂ���
                setAppearFlag(false);

                //�G�t�F�N�g��\��
                par.PlayAround(destEff, this.gameObject.transform.position);
                //SE�̍Đ�
                se.Play(SE_Explos);

                //���ł���
                DestroyMe();
            }
            //HP��ύX����
            setHP(tempHP);
        }
        //��_���t���O�𗧂Ă�
        setDamageHitFlag(true);
    }
    //-------------------------------------------------------------------------------------------
    void DestroyMe()
    {
        GameObject Parent = transform.parent.gameObject;

        //�Ăяo����ԂŖ�����
        CameSc.setCallEnemyFlag(false);

        //���ł���
        kill(Parent);
    }
    //-------------------------------------------------------------------------------------------
    void LoadRoot()
    {
        //���񃋁[�g��ǂݍ���
        string[,] RootTemp = new string[3, 3];
        RootLoad RL = this.gameObject.GetComponent<RootLoad>();
        RootTemp = RL.LoadRoot();

        //��for������
        for (int i = 0; i < RootTemp.Length / 2; i++)
        {
            //����
            Root.Add(RootTemp[i, 0]);
            //�b��
            RTime.Add(int.Parse(RootTemp[i, 1]));
        }

        this.gameObject.transform.position = RL.getInsPos();
    }
    //-------------------------------------------------------------------------------------------
    void GetObjects()
    {
        //�I�u�W�F�N�g�擾�N���X���C���X�^���X��
        ObjectGetClass GetObj = new ObjectGetClass();

        //�v���C���̎擾
        player = GetObj.GetGameObject("player");
        //rigidboy2D���擾
        rigid = GetObj.GetRigid2D(this.gameObject);

        //�T���p�R���C�_�[�̃X�N���v�g���擾
        GameObject SearchObj = GetObj.GetChild_Obj(this.gameObject, "SearchArea");
        SearchAreaCol = SearchObj.GetComponent<ColliderScript>();
        //�U���͈̓R���C�_�[�̃X�N���v�g���擾
        GameObject AttackAreaObj = GetObj.GetChild_Obj(this.gameObject, "AttackArea");
        AttackAreaCol = AttackAreaObj.GetComponent<ColliderScript>();

        Nav = this.gameObject.GetComponent<NavMeshAgent>();
    }
    //-------------------------------------------------------------------------------------------
    void GetScripts()
    {
        //�G�̃X�N���v�g���擾
        ImgScript = enemyImg.GetComponent<EnemyImg>();

        //�J�����X�N���v
        CameSc = GameObject.Find("Main Camera").GetComponent<CameraScript>();
    }
    //-------------------------------------------------------------------------------------------
    void ZeroClear()
    {
        //�J�E���^�̏�����
        dirCnt = 0;
        dirTimer = 0;
        DamageTimer = 0;
        preDamage = 0;
        targetPos = Vector2.zero;
    }
    //-------------------------------------------------------------------------------------------
    void ImgTurn()
    {
        Vector2 playerVector = player.transform.position - transform.position;
        bool isFlip = playerVector.x < 0;

       �@//������ύX����֐����Ă�
       �@ImgScript.ImgTurn(isFlip);
    }
    //-------------------------------------------------------------------------------------------
}
