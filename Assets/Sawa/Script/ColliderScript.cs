using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------------------------------
//�R���C�_�[�p�X�N���v�g
//-----------------------------------------------------------------

public class ColliderScript : MonoBehaviour
{
    GameObject parent;
    //�ړ�
    [SerializeField] bool MoveFlag = true;
    //�ǔ�
    [SerializeField] bool SearchFlag = false;
    //�U��
    [SerializeField] bool AttackFlag = false;

    string ObjName;

    void Start()
    {
        ObjName = this.gameObject.name;

        parent = transform.root.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //�ړ��p�R���C�_�[
        if(ObjName == "MoveColLeft" || ObjName == "MoveColRight" ||
           ObjName == "MoveColUp" || ObjName == "MoveColDown")
        {
            //�� or �G�ƐڐG���Ă�����
            if (col.transform.tag == "Wall" || col.transform.tag == "Enemy")
            {
                MoveFlag = false;

                //�{�X�̏ꍇ��莞�Ԃ�true�ɖ߂�
                if(parent.name == "BossPrefab")
                {
                    Invoke("ReTrue", 3);
                }
            }
        }

        //�T���͈�
        if(ObjName == "SearchArea" && col.gameObject.tag == "player")
        {
            SearchFlag = true;
        }

        //�U���͈�
        if(ObjName == "AttackArea" && col.gameObject.tag == "player")
        {
            AttackFlag = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        //�ǂƐڐG���Ȃ��Ȃ�����
        if (ObjName == "MoveColLeft" || ObjName == "MoveColRight" ||
           ObjName == "MoveColUp" || ObjName == "MoveColDown")
        {
            //�� or �G�ƐڐG���Ă�����
            if (col.transform.tag == "Wall" || col.transform.tag == "Enemy")
            {
                MoveFlag = true;
            }
        }
        //�T���͈�
        if (ObjName == "SearchArea" && col.gameObject.tag == "player")
        {
            SearchFlag = false;
        }

        //�U���͈�
        if (ObjName == "AttackArea" && col.gameObject.tag == "player")
        {
            AttackFlag = false;
        }
    }

    public bool getMoveFlag()
    {
        return MoveFlag;
    }
    public bool getSearchFlag()
    {
        return SearchFlag;
    }
    public bool getAttackFlag()
    {
        return AttackFlag;
    }

    //Invoke�p
    void ReTrue()
    {
        MoveFlag = true;
    }
}
