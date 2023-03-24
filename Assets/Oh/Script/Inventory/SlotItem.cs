using System;
using UnityEngine;
using UnityEngine.Events;

namespace Inventory {

    /// <summary>
    /// - Slot表示のために必要なSlotアイテム
    /// - いるならもらってほかのアイテム実装
    /// </summary>
    [System.Serializable]
    public class SlotItem : ISlotItem, IItemProperty {
        public bool Equip { get; set; }
        public InventoryTab Tab { get; set; }
        public int Index { get; set; }

        public int MaxCount { get; set; }
        public int Count { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public float Stat { get; set; }
        public string Type { get; set; }        // PlayerChip判定用
        public string Equip_Type { get; set; }  // Stat加減時に使う
        public string Description { get; set; }

        public Sprite Icon { get; set; }

        public Color Color { get; set; }
        public GameObject Object { get; set; }
        public int Level { get; set; }

        public void SetCount (int maxCount = 1, int count = 1) {
            MaxCount = maxCount;
            Count = count;
        }

        public void SetProperty (string id, string name, float stat, string type, string equip_Type, string description) {
            Id = id;
            Name = name;
            Stat = stat;
            Type = type;
            Equip_Type = equip_Type;
            Description = description;
            Color = Color.white;
            Equip = false;
        }

        public void SetWeaponChipProperty(GameObject obj, string name, string description, int level) {
            Object = obj;
            Name = name;
            Type = "WeaponChip";
            Description = description;
            Color = Color.white;
            Level = level;
            Equip = false;
        }
    }
}
