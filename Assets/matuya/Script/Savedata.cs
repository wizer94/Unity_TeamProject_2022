using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savedata : MonoBehaviour
{
    //チップ管理のクラス
    public class WeaponChip
    {
        public bool have;        //所持しているかどうか
        public int Chips;        //武器の所持チップ
        public int Level;        //武器チップレベル
        public int Quaantity;    //武器チップ所持数
    }

    //プレイヤーの管理クラス
    public class PlayerChip
    {
        public bool have;        //所持しているかどうか
        public int Chips;        //プレイヤーの所持チップ
        public int Level;        //プレイヤーチップレベル
        public int Quaantity;    //プレイヤーチップ所持数
    }

    [SerializeField]
    public class SaveData
    {
        public string playerName;               //主人公の名前
        public WeaponChip[] weaponChips;
        public int[] weapons;                   //武器の種類
        public WeaponChip[] weaponChipEquip;    //武器についているチップ
        public PlayerChip[] playerChips;
        public PlayerChip[] playerChipEquip;    //プレイヤーについているチップ
        //public string[] SavePoint;            //セーブポイント
        public float playtime;                  //プレイ時間
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
