using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory {

    public class QuickSlotHandler : MonoBehaviour {

        [Serializable]
        public class ShortcutSlot {
            public InventorySlot slot;
            public KeyCode key;
        }

        public ItemHandler itemHandler; //アイテムハンドラ
        public List<ShortcutSlot> quickSlotList = new List<ShortcutSlot> (); //SlotList

        private Dictionary<KeyCode, InventorySlot> quickSlotDctn = new Dictionary<KeyCode, InventorySlot> (); //SlotDictionary

        private void Awake () {

            //Dictionaryに登録
            foreach (var slot in quickSlotList) {
                quickSlotDctn.Add (slot.key, slot.slot);
            }
        }

        //private void Update () {

        //    if (Input.anyKeyDown) {

        //        foreach (var slot in quickSlotList) {

        //            //Slotのキーが押された場合押されたSlotのアイテム使用
        //            if (Input.GetKeyDown (slot.key)) {
        //                InventorySlot usedSlot = quickSlotDctn[slot.key];
        //                if (usedSlot.Item != null) {
        //                    itemHandler.Use (usedSlot.Item);
        //                    usedSlot.slotManager.Refresh (usedSlot.slotManager.LastRefreshedTab);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
