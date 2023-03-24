using UnityEngine;
using Inventory;
using System;

/// <summary>
/// SlotItem 을 상속해야 인벤토리에서 사용이 가능합니다.
/// 
/// - IUsable : 가장 일반적인 아이템 사용 인터페이스
/// - IEquipment : 사용시 지정된 슬롯으로 이동 또는 교체
/// - IConsumeable : 사용시 갯수가 줄어들며 0개가 되면 탭에서 자동 제거
/// </summary>

public class Item : SlotItem, IUsable {

    public Item (string id, string name, float stat, string type, string equip_Type, string description, Sprite icon) {
        SetCount ();
        SetProperty (id, name, stat, type, equip_Type, description);
        Icon = icon;
        Usable = true;
    }

    public Item(GameObject obj,string name, string description, int level, Sprite icon, Color color) {
        SetCount();
        SetWeaponChipProperty(obj, name, description, level);
        Type = "WeaponChip";
        Icon = icon;
        Color = color;
        Usable = true;
    }

    public bool Usable { get; set; }
    public Action<SlotItem> UseEvent { get; set; }
}

public class Equipment : SlotItem, IEquipment{

    public Equipment (string id, string name, float stat, string type,  string equip_Type, string description, Sprite icon) {
        SetCount ();
        SetProperty (id, name, stat, type, equip_Type, description);
        Icon = icon;
        Usable = true;
    }

    public bool Usable { get; set; }
    public InventorySlot TargetSlot { get; set; }
    public Action<SlotItem> UseEvent { get; set; }
}

public class Consumable : SlotItem, IConsumable{

    public Consumable (int maxCount, int count, string id, string name, float stat, string type, string equip_Type, string description, Sprite icon) {
        SetCount (maxCount, count);
        SetProperty (id, name, stat, type, equip_Type, description);
        Icon = icon;
        Usable = true;
    }

    public bool Usable { get; set; }
    public Action<SlotItem> UseEvent { get; set; }
}
