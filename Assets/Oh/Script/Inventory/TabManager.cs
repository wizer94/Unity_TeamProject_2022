using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory {

    [System.Serializable]
    public class TabManager : MonoBehaviour {

        [Serializable]
        public class TabProperty {
            public string tabName; //タブの名前
            public int capacity; //タブのアイテムストック
        }

        [Tooltip ("タブのリスト(Tab List)（最低限一個以上のタブが必要）")]
        public List<TabProperty> tabList = new List<TabProperty> ();

        private static List<string> tabNameList = new List<string> (); //タブの名前のリスト
        private static Dictionary<string, InventoryTab> TabDictionary { get; } = new Dictionary<string, InventoryTab> (); //タブのDictionary

        private void Awake () {

            //タブの名前リストとDictionaryにタブを追加
            for (int i = 0; i < tabList.Count; i++) {
                tabNameList.Add (tabList[i].tabName);
                TabDictionary.Add (tabList[i].tabName, new InventoryTab (tabList[i].tabName, tabList[i].capacity));
            }
        }


		//タブの名前でタブに接触
		public static InventoryTab GetTab (string tabName) {
            if (tabNameList.Contains (tabName))
                return TabDictionary[tabName];
            else {
                Debug.LogError ("インベントリのタブ：正義されてないインベントリタブです。");
                return null;
            }
        }

        //Indexでタブに接触
        public static InventoryTab GetTab (int index) {
            if ((index >= 0) && (index < tabNameList.Count))
                return TabDictionary[tabNameList[index]];
            else {
                Debug.LogError ("インベントリのタブ：Indexの範囲を超えました。");
                return null;
            }
        }
    }

    public class InventoryTab {
        public string TabName { get; } //タブの名前
        public int Capacity { get; private set; } //最大のストック

        public List<SlotItem> ItemTable { get; private set; } //アイテムのリスト

        //アイテムIndexでアイテムに接触および設定（Indexer）
        public SlotItem this[int itemIndex] {
            get { return ItemTable[itemIndex]; }
            set { ItemTable[itemIndex] = value; }
        }

        //アイテムIDでアイテムに接触（Indexer）
        public SlotItem this[string id] {
            get {
                foreach (var item in ItemTable) {
                    if (item != null && item.Id == id) return item;
                }
                return null;
            }
        }

        //アイテムの名前でアイテムを戻す
        public SlotItem[] GetItemsByName (string itemName) {
            List<SlotItem> items = new List<SlotItem> ();
            foreach (var item in ItemTable) {
                if (item != null && item.Name == itemName) items.Add (item);
            }
            return items.ToArray ();
        }

        //すべてのアイテムを戻す（Save用）
        public SlotItem[] GetItemsAll () {
            List<SlotItem> items = new List<SlotItem> ();
            foreach (var item in ItemTable) {
                if (item != null) items.Add (item);
            }
            return items.ToArray ();
        }

        public int Count { //現在のアイテムの数
            get {
                int count = 0;
                foreach (var item in ItemTable) {
                    if (item != null) count += 1;
                }
                return count;
            }
        }

        public bool IsEmpty { //空いているかを確認してRetrun
            get { return Count == 0; }
        }

        public bool IsFull { //Full画を確認してReturn
            get { return Count == Capacity; }
        }

        public int GetNextIndex () { //順番に空いてるSlotを検索
            int idx;
            for (idx = 0; idx < Capacity; idx++)
                if (ItemTable[idx] == null) break;
            return idx;
        }

        public InventoryTab (string tabName, int capacity) {
            TabName = tabName; //名前の設定
            Capacity = capacity; //最大のストックの設定

            //アイテムテーブルの初期化
            ItemTable = new List<SlotItem> (Capacity);
            for (int i = 0; i < Capacity; i++)
                ItemTable.Add (null);
        }

        //リストにアイテムの追加（自動で合成する）
        public void Add (SlotItem item, bool autoMerge = true, Action addFailEvent = null) {

            //自動合成
            if (autoMerge && item.MaxCount > 1) {
                SlotItem[] targetItems = GetItemsByName (item.Name);
                foreach (var target in targetItems) {
                    if (target.MaxCount > target.Count) {
                        int valid = target.MaxCount - target.Count;
                        if (item.Count <= valid) {
                            target.Count += item.Count;
                            item.Count = 0;
                        } else {
                            target.Count += valid;
                            item.Count -= valid;
                        }
                    }
                }        
                if (item.Count <= 0) return;
            }

            //アイテム追加
            if (!IsFull) {
                item.Tab = this;
                int idx = GetNextIndex ();
                ItemTable[idx] = item;
                item.Index = idx;
            } else addFailEvent?.Invoke ();
        }

        //リストにアイテム追加（Indexを直接設定、Indexから読み込み用）
        public void Add (SlotItem item, int index) {
            item.Tab = this;
            ItemTable[index] = item;
            item.Index = index;
        }

        //リストからアイテム削除
        public void Remove (SlotItem item) {
            if (ItemTable.Contains (item)) 
                ItemTable[item.Index] = null;
        }

        //リスト拡散
        public void Extend (int capacity) {
            ItemTable.Capacity += capacity;
            for (int i = 0; i < capacity; i++) {
                ItemTable.Add (null);
            }
            Capacity += capacity;
        }
    }
}
