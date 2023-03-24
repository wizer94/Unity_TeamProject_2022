using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savedata : MonoBehaviour
{
    //�`�b�v�Ǘ��̃N���X
    public class WeaponChip
    {
        public bool have;        //�������Ă��邩�ǂ���
        public int Chips;        //����̏����`�b�v
        public int Level;        //����`�b�v���x��
        public int Quaantity;    //����`�b�v������
    }

    //�v���C���[�̊Ǘ��N���X
    public class PlayerChip
    {
        public bool have;        //�������Ă��邩�ǂ���
        public int Chips;        //�v���C���[�̏����`�b�v
        public int Level;        //�v���C���[�`�b�v���x��
        public int Quaantity;    //�v���C���[�`�b�v������
    }

    [SerializeField]
    public class SaveData
    {
        public string playerName;               //��l���̖��O
        public WeaponChip[] weaponChips;
        public int[] weapons;                   //����̎��
        public WeaponChip[] weaponChipEquip;    //����ɂ��Ă���`�b�v
        public PlayerChip[] playerChips;
        public PlayerChip[] playerChipEquip;    //�v���C���[�ɂ��Ă���`�b�v
        //public string[] SavePoint;            //�Z�[�u�|�C���g
        public float playtime;                  //�v���C����
    }

    public void SaveWeapons()
    {

    }

    public void SavePlayer()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
