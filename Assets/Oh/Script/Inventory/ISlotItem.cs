using System;
using UnityEngine;

namespace Inventory {

    public interface ISlotItem {

        InventoryTab Tab { get; set; } //アイテムを持ったTABの名前
        int Index { get; set; } //INVENTORYのSLOT順番
    }

    public interface IItemProperty {

        bool Equip { get; set; }
        int MaxCount { get; set; } //アイテム最大の数
        int Count { get; set; } //現在のアイテム数

        string Id { get; set; } //アイテムのID
        string Name { get; set; } //アイテムのNAME
        float Stat { get; set; }    // アイテムの数値
        string Type { get; set; } //アイテムのタイプ
        string Equip_Type { get; set; } //アイテム装備・解除時のStat判定用
        string Description { get; set; } //アイテムの説明

        Sprite Icon { get; set; } //表示アイコン

        void SetCount (int maxCount, int count); //最大と現在の数を設定するメソッド
        void SetProperty (string id, string name, float stat, string type, string equip_Type, string description); //アイテムのプロパティ設定
    }

    //ショップの設定
    //public interface ITradable {

    //    int BuyPrice { get; set; } //購入値段
    //    int SellPrice { get; set; } //販売値段

    //    void SetPrice (int buyPrice, int sellPrice); //購入と販売の値段
    //}

    //使用可能にする
    public interface IUsable {
        bool Usable { get; set; }
        Action<SlotItem> UseEvent { get; set; }
    }

    public interface IEquipment {
        bool Usable { get; set; }
        InventorySlot TargetSlot { get; set; } //使用すると装備するタゲットのSLOT
        Action<SlotItem> UseEvent { get; set; }
    }

    // ショップに関するものだと思う
    public interface IConsumable {
        bool Usable { get; set; }
        Action<SlotItem> UseEvent { get; set; }
    }
}
