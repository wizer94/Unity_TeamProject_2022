using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//-----------------------------------------------------------------
//�G ���U��
//-----------------------------------------------------------------

public class RushEnemy : EnemyClass
{
    //�ړ��N���X���C���X�^���X��
    EMove move = new EMove();

    //�G�̃X�N���v�g
    [SerializeField] EnemyImg ImgScript;

    //�T���֘A�̕ϐ�
    ColliderScript SearchAreaCol;

    NavMeshAgent Nav;

    bool[] NoMoveDir = new bool[4] { true, true, true, true };
    [SerializeField] ColliderScript[] MoveCol = new ColliderScript[4];     //�X�N���v�g�i�[

    float DamageTimer;

    bool isSearch;  //�T����ԂɂȂ邩

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

        //NavMesh�̖�����
        Nav.enabled = false;

        setIsSearch(false);
    }
    //-----------------------------------------------------------------
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
    //-----------------------------------------------------------------
    private void FixedUpdate()
    {
        //�����E�������̔���
        if (getAppearFlag())
        {
            //�v���C���Ƃ̋������v��
            if(HitCircle(this.gameObject, player, 12))
            {
                //�v���C���̋߂��ɂ�����
                setIsSearch(true);
            }
            else
            {
                setIsSearch(false);
            }

            Think();
            Move();
        }
        else
        {
            //NavMesh��؂�
            Nav.enabled = false;

            //�R���C�_�[��؂�
            ImgScript.setActiveCollider(false);

            //����ݒ肷��
            ImgScript.ChengeAlphaDec();
        }

        //�摜�̌����̕ύX
        ImgTurn();
    }
    //-----------------------------------------------------------------
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
                //�摜��ύX����
                ImgScript.ChangeImage(false);

                break;
            //�T�����-----------------------------
            case State.Search:
                //�ړ��̕ϐ����ĂԑO�Ɉړ������̊m�F
                for (int i = 0; i < 4; ++i)
                {
                    NoMoveDir[i] = MoveCol[i].getMoveFlag();
                }

                vec = move.Search(getMoveSpeed(), NoMoveDir);
                gameObject.transform.Translate(vec);

                break;
            //�ǔ����-----------------------------
            case State.AimPlayer:
                if (!OneceFlag)
                {
                    //�摜��ύX����
                    ImgScript.ChangeImage(true);
                    OneceFlag = true;
                }
                //�i�r���b�V�����N�����Ă��Ȃ��Ȃ�
                if (!Nav.enabled)
                {
                    //�i�r���b�V�����N��������
                    Nav.enabled = true;
                }
                //�ڕW��ݒ肷��
                Nav.SetDestination(player.transform.position);
                break;
            //��~���-----------------------------
            case State.Stop:
                //�����Ȃ�
                break;
            //�U�����-----------------------------
            case State.Attack:
                //�����Ȃ�
                break;
            //�������-----------------------------
            case State.Escape:
                //�ǂ��m�F����
                if (CheckNoMoveDir())
                {
                    vec = move.Escape(player, this.gameObject, getMoveSpeed());
                    gameObject.transform.Translate(vec);
                }

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
                //�������ŒT���ɂ���
                if (getIsSearch())
                {
                    st = State.Search;
                }

                break;
            //�T�����-----------------------------
            case State.Search:
                //�J�����O�ɂ���Ȃ�
                if (!getIsSearch())
                {
                    st = State.Non;
                }

                //��~
                //��莞�Ԉړ�������~�܂�
                if (MoveTimer >= 2)
                {
                    st = State.Stop;
                }

                //�ǔ�
                //�v���C�����T���͈͂ɓ�������
                if (SearchAreaCol.getSearchFlag())
                {
                    st = State.AimPlayer;
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
                //�ǔ��͈͂���v���C�����o���� && �h���[���ɌĂ΂�Ă��Ȃ���
                if (!SearchAreaCol.getSearchFlag() && !CameSc.getCallEnemyFlag())
                {
                    st = State.Non;
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
                //�����Ȃ��@�������̂���Non�ɂ���
                st = State.Non;
                break;
            //�U�����-----------------------------
            case State.Attack:
                //�����Ȃ��@�������̂���Non�ɂ���
                st = State.Non;
                break;
            //�������-----------------------------
            case State.Escape:
                //�����Ȃ�
               //   ���@����������

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
    public void setState_AimPlayer()
    {
        setState(State.AimPlayer);
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

        //�T���p�R���C�_�[�̃X�N���v�g���擾
        GameObject SearchObj = GetObj.GetChild_Obj(this.gameObject, "SearchArea");
        SearchAreaCol = SearchObj.GetComponent<ColliderScript>();

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
    }
    //-----------------------------------------------------------------
    void ZeroClear()
    {
        DamageTimer = 0;
    }
    //-----------------------------------------------------------------
    void setIsSearch(bool Flag)
    {
        isSearch = Flag;
    }
    bool getIsSearch()
    {
        return isSearch;
    }
    //-----------------------------------------------------------------
}
