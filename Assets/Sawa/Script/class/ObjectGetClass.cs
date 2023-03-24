using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//-----------------------------------------------------------------
//�I�u�W�F�N�g�@�擾�N���X
//-----------------------------------------------------------------

public class ObjectGetClass : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------------------
    //�I�u�W�F�N�g���擾���郁�\�b�h
    public GameObject GetGameObject(string ObjName)
    {
        //�I�u�W�F�N�g���擾����
        GameObject Obj = GameObject.Find(ObjName);

        //������Ȃ������ꍇ�Q�[�����I������
        if (Obj == null)
        {
            Debug.Log("Object is null");
        }

        return Obj;
    }
    //---------------------------------------------------------------------------------------------------------
    //�I�u�W�F�N�g���^�O�Ŏ擾���郁�\�b�h
    public GameObject GetGameObject_Tag(string ObjName)
    {
        //�I�u�W�F�N�g���擾����
        GameObject Obj = GameObject.FindWithTag(ObjName);

        //������Ȃ������ꍇ�Q�[�����I������
        if (Obj == null)
        {
            Debug.Log("ObjectTag is null");
        }

        return Obj;
    }
    //---------------------------------------------------------------------------------------------------------
    //�q�I�u�W�F�N�g���擾���郁�\�b�h
    public GameObject GetChild_Obj(GameObject Obj, string ChildName)
    {
        //�q�I�u�W�F�N�g���擾����
        GameObject child = Obj.transform.Find(ChildName).gameObject;

        //������Ȃ������ꍇ�Q�[�����I������
        if (child == null)
        {
            Debug.Log("Children is null");
        }

        //�����̖��O�̎q�I�u�W�F�N�g��Ԃ�
        return child;
    }
    //---------------------------------------------------------------------------------------------------------
    // �e�I�u�W�F�N�g���擾���郁�\�b�h
    public GameObject GetParent()
    {
        GameObject par = transform.parent.gameObject;

        if(par == null)
        {
            Debug.Log("�e�I�u�W�F�N�g���擾�ł��܂���ł���");
        }

        return par;
    }
    //---------------------------------------------------------------------------------------------------------
    //SpriteRenderer���擾���\�b�h
    public SpriteRenderer Get_SRen(GameObject Obj)
    {
        //�����̃I�u�W�F�N�g��SpriteRenderer���擾����
        SpriteRenderer sr = Obj.GetComponent<SpriteRenderer>();

        //������Ȃ������ꍇ�Q�[�����I������
        if (sr == null)
        {
            Debug.Log("SpriteRenderer is null");
        }

        //SpriteRenderer��Ԃ�
        return sr;
    }

    //---------------------------------------------------------------------------------------------------------
    //Rigidbody���擾���\�b�h
    public Rigidbody2D GetRigid2D(GameObject obj)
    {
        Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();

        //������Ȃ������ꍇ�Q�[�����I������
        if (rigid == null)
        {
            Debug.Log("Rigidbody is null");
        }

        return rigid;
    }

    //---------------------------------------------------------------------------------------------------------
    //Collider���擾���\�b�h
    public Collider2D GetCollider2D(GameObject obj)
    {
        Collider2D Col = obj.GetComponent<Collider2D>();

        //������Ȃ������ꍇ�Q�[�����I������
        if (Col == null)
        {
            Debug.Log("Collider2D is null");
        }

        return Col;
    }
    //---------------------------------------------------------------------------------------------------------
    //CapsuleCollider���擾���\�b�h
    public CapsuleCollider2D GetCapsuleCollider2D(GameObject obj)
    {
        CapsuleCollider2D col = obj.GetComponent<CapsuleCollider2D>();

        if (col == null)
        {
            Debug.Log("CapsuleCollider2D is null");
        }

        return col;
    }
    //---------------------------------------------------------------------------------------------------------
    //BoxCollider���擾���\�b�h
    public BoxCollider2D GetBoxCollider2D(GameObject obj)
    {
        BoxCollider2D Col = obj.GetComponent<BoxCollider2D>();

        //������Ȃ������ꍇ�Q�[�����I������
        if (Col == null)
        {
            Debug.Log("BoxCollider2D is null");
        }

        return Col;
    }
    //---------------------------------------------------------------------------------------------------------
    //�A�j���[�^�[���擾���郁�\�b�h
    public Animator GetAnim(GameObject obj)
    {
        return obj.GetComponent<Animator>();
    }
    //---------------------------------------------------------------------------------------------------------
    //Image���擾���郁�\�b�h
    public Image GetImage(GameObject obj)
    {
        return obj.GetComponent<Image>();
    }
    //---------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------
}
