using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory {

    /// <summary>
    ///  - アイテムの移動、位置交換、合成 : Drag And Drop
    ///  - アイテムの使用 : ダブルクリック (PC), 一回タブ(モバイル)
    /// </summary>

    [RequireComponent(typeof(SlotManager))]
    public class ItemHandler : MonoBehaviour {

        //アイテムハンドラリスト
        public static List<ItemHandler> HandlerList { get; private set; } = new List<ItemHandler> ();

        //public SlotManager slotManager;

        public static SlotItem SelectedItem { get; internal set; } //選択されたアイテム
        public static SlotItem TargetItem { get; internal set; } //ターゲットアイテム

        internal static bool RequestItemResetStop { get; set; }

        [Header ("Item Management Option")]
        [Tooltip ("アイテム移動可能有無")]
        public bool movable = true;

        [Tooltip ("他のタブに移動可能有無")]
        public bool moveToOtherSlot = true; //移動できるとき使える

        [Tooltip ("あ互いのアイテムの位置交換可能有無")]
        public bool switching = true; //移動できるとき使える

        [Tooltip ("アイテム合成可能有無")]
        public bool merging = true; //移動できるとき使える

        [Tooltip ("アイテム使用可能有無")]
        public bool usable = true;

        [Header ("Pointer Event And Exit Settings")]
        [Tooltip ("Pointer Event And Exit Settings ON")]
        public bool enablePointerEnterAndExitEvent = true;

        [Tooltip ("Pointer Event更新待機時間")]
        public float pointerUpdateInterval = 0.1f;

        public Action<InventorySlot, SlotItem> OnItemSelected { get; set; } //アイテム選択イベント（選択SLOT、選択アイテム）
        public Action OnEventEnded { get; set; } //イベント終了お知らせイベント

        public Action<SlotItem> OnItemMoved { get; set; } //アイテム移動イベント（選択アイテム）
        public Action<SlotItem, SlotItem> OnItemSwitched { get; set; } //アイテム位置交換イベント（選択アイテム・対象アイテム）
        public Action<SlotItem> OnItemMerged { get; set; } //アイテム合成イベント（対象アイテム）
        public Action<SlotItem> OnItemUsed { get; set; } //アイテム使用イベント（選択アイテム）

        public Action<SlotItem> DragOutEvent { get; set; } //アイテムをインベントリの外にDRAGした時のイベント
        public Action<SlotItem, InventorySlot> TypeNotMatchEvent { get; set; } //タイプが合わないよ期のイベント
        public Action<SlotItem> SlotMoveFailEvent { get; set; } //SLOTとSLOTに移動ができない時のイベント

        public Action<PointerEventData, InventorySlot> OnSlotDown { get; set; } //ポインタダウンイベント
        public Action<PointerEventData, InventorySlot> OnSlotUp { get; set; } //ポインタアップイベント
        public Action<PointerEventData, InventorySlot> OnSlotClick { get; set; } //ポインタクリックイベント
        public Action<PointerEventData, InventorySlot> OnSlotEnter { get; set; } //ポインタENTERイベント
        public Action<PointerEventData, InventorySlot> OnSlotExit { get; set; } //ポインタEXITイベント

        GameObject inventory;
        AudioSource audiosource;
        AudioClip mergeSound;

        public static bool playerchip_move;     // chipを空間に移動する
        public static bool playerchip_change;   // chipの間のいり替え

        public static int target_index = 0;
        public static int selected_index = 0;

        private void Awake () {

            //ハンドラリストに現在のハンドラを追加
            if (!HandlerList.Contains (this)) HandlerList.Add (this);

            //アイテム移動不可の時タブ移動・位置交換・合成OFF
            if (!movable)
            {
                moveToOtherSlot = false;
                switching = false;
                merging = false;
            }
        }

		private void Start() {
            inventory = GameObject.FindGameObjectWithTag("Inventory");
            audiosource = inventory.GetComponent<AudioSource>();
            mergeSound = Resources.Load("SoundEffect/merge") as AudioClip;
            playerchip_change = false;
		}

		/*
		private void Start() {
            slotManager = GetComponent<SlotManager>();
		}
        */

		//外部アップデート関数から呼び出し必要
		public static void RequestItemHandle () {
            //イベント終了呼び出し（モバイル）
#if (UNITY_IOS && !UNITY_EDITOR) || (UNITY_ANDROID && !UNITY_EDITOR)
            Touch touch = Input.GetTouch (0);
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                CallEventEnd ();
                return;
            }
#endif

            //イベント終了後呼び出し（PC）
            if (Input.GetMouseButtonUp (0)) {
                CallEventEnd ();
            }
        }

        //選択、対象アイテムの初期化およびDragOutイベント呼び出し
        internal static void CallEventEnd () {
            if (!RequestItemResetStop && SelectedItem != null) {
                InventorySlot slot = SlotManager.SelectedSlot;

                if (!EventSystem.current.IsPointerOverGameObject()) //DragOutイベント呼び出し
                    slot.itemHandler.DragOutEvent?.Invoke(slot.Item);

                slot.itemHandler.ResetItems (); //アイテムReset
            }
        }

        //アイテム移動（アイテムー＞空いてるSlot）
        public void Move (SlotItem selectedItem, InventorySlot targetSlot) {
            if (selectedItem.Tab.TabName == "PlayerChipTab" && targetSlot.Item == null)
            {
                playerchip_move = true;
            }
            int originIdx = selectedItem.Index;
            selected_index = originIdx;
            targetSlot.slotManager.LastRefreshedTab.ItemTable[targetSlot.Index] = selectedItem;
            target_index = targetSlot.Index;
            selectedItem.Tab[originIdx] = null;

            selectedItem.Index = targetSlot.Index;
            selectedItem.Tab = targetSlot.slotManager.LastRefreshedTab;

            //武器チップを外した時
            if (selectedItem.Type == "WeaponChip" && targetSlot.slottype != InventorySlot.SlotType.WeaponChip) {
                WeaponChip wc = selectedItem.Object.GetComponent<WeaponChip>();
                wc.SetWS(null);
                wc.SetDescription();
                selectedItem.Description = wc.description;
                RefreshWeaponMerge();
            }
            //武器チップをつけた時
            if (selectedItem.Type == "WeaponChip" && targetSlot.slottype == InventorySlot.SlotType.WeaponChip) {

                RefreshWeaponMerge();
            }
            OnItemMoved?.Invoke (selectedItem);
        }

        void RefreshWeaponMerge() {

            InventoryTab tab = TabManager.GetTab("WeaponTab");

            bool[] merged = new bool[5];

            for (int i = 0; i < 5; i++) {
                //空じゃない
                if (tab.ItemTable[i] != null) {


                    WeaponChip self = tab.ItemTable[i].Object.GetComponent<WeaponChip>();

                    List<SlotItem> slotitems = new List<SlotItem>();
                    List<WeaponChip> wcs = new List<WeaponChip>();
                    List<int> levels = new List<int>();

                    for (int j = 0; j < 5; j++) {
                        //空じゃなく、未参照
                        if (tab.ItemTable[j] != null && !merged[j]) {

                            WeaponChip other = tab.ItemTable[j].Object.GetComponent<WeaponChip>();
                            //同じチップ
                            if (self.Equals(other)) {
                                slotitems.Add(tab.ItemTable[j]);
                                wcs.Add(other);
                                levels.Add(other.level);
                                merged[j] = true;
                            }

                        }
                    }

                    for (int k = 0; k < slotitems.Count; k++) {
                        wcs[k].SetMergeDescription(levels);
                        slotitems[k].Description = wcs[k].description;

                    }
                }
            }
            SlotManager.RefreshAll();
        }

        //アイテムとアイテムの位置交換（アイテム＜ー＞アイテム）
        public void Switch (SlotItem selectedItem, SlotItem targetItem) {
            //playerchip_change = true;
            if (selectedItem.Tab.TabName == "PlayerChipTab")
            {
                if (targetItem.Type == "WeaponChip")
                {
                    ResetItems();
                    return;
                }
            }

            else if(selectedItem.Tab.TabName == "WeaponTab")
			{
                if(targetItem.Type == "PlayerChip")
                {
                    ResetItems();
                    return;
                }
			}

            if(selectedItem.Tab.TabName == "PlayerChipTab" && targetItem.Tab.TabName == "PlayerChipTab")
                playerchip_change = true;

            // PlayerChipもしくはWeaponchipタブの場合入れ替え防止
            if (targetItem.Tab.TabName != "PlayerChipTab" || targetItem.Tab.TabName != "WeaponTab")
            {
                Swap(ref selectedItem, ref targetItem);
            }

            //武器チップを外した時
            if (selectedItem.Tab.TabName == "WeaponTab" && targetItem.Tab.TabName != "WeaponTab") {
                WeaponChip wc = targetItem.Object.GetComponent<WeaponChip>();
                wc.SetWS(null);
                wc.SetDescription();
                targetItem.Description = wc.description;
                RefreshWeaponMerge();
            }
            //武器チップをつけた時
            if (selectedItem.Tab.TabName != "WeaponTab" && targetItem.Tab.TabName == "WeaponTab") {
                WeaponChip wc = selectedItem.Object.GetComponent<WeaponChip>();
                wc.SetWS(null);
                wc.SetDescription();
                selectedItem.Description = wc.description;
                RefreshWeaponMerge();
            }
            OnItemSwitched?.Invoke (selectedItem, targetItem);
        }

        //インベントリ内の位置交換メソッド
        public void Swap(ref SlotItem item1, ref SlotItem item2) {
            selected_index = item1.Index;
            target_index = item2.Index;

            item1.Tab[item1.Index] = item2;
            item2.Tab[item2.Index] = item1;

            int targetIdx = item2.Index;
            item2.Index = item1.Index;
            item1.Index = targetIdx;

            InventoryTab targetTab = item2.Tab;
            item2.Tab = item1.Tab;
            item1.Tab = targetTab;

            SlotManager.RefreshAll();
        }

        //アイテム合成（アイテムー＞アイテム）
        public void Merge (SlotItem selectedItem, SlotItem targetItem) {

            if(selectedItem.Type == "WeaponChip" && targetItem.Type == "WeaponChip") {
                if (selectedItem.Tab.TabName != "WeaponTab" && targetItem.Tab.TabName != "WeaponTab") { 
                    WeaponChip selected = selectedItem.Object.GetComponent<WeaponChip>();
                    WeaponChip target = targetItem.Object.GetComponent<WeaponChip>();
                    if (target.Equals(selected) && selected.level == target.level && target.level < 5) {
                        if (targetItem.Tab.TabName == "WeaponTab")
                            target.UnLoadChip();
                        selected.UnLoadChip();
                        int preLevel = target.level;
                        target.ChangeLevel(target.level + 1);
                        targetItem.Level = target.level;
                        targetItem.Color = target.GetColorFromLevel();
                        if (targetItem.Tab.TabName == "WeaponTab")
                            target.LoadChip();
                        targetItem.Description = target.description;
                        Destroy(selectedItem.Object);
                        selectedItem.Tab.Remove(selectedItem);
                        float pitch = Mathf.Pow(2f, (preLevel - 1) / 6f);
                        PlaySound(mergeSound, pitch);

                        return;
                    }
                }
            }

            if (targetItem.MaxCount > targetItem.Count) {
                int valid = targetItem.MaxCount - targetItem.Count;

                if (selectedItem.Count <= valid) {
                    targetItem.Count += selectedItem.Count;
                    selectedItem.Tab.Remove(selectedItem);

                } else {
                    targetItem.Count += valid;
                    selectedItem.Count -= valid;
                }

                OnItemMerged?.Invoke (targetItem);
            } else Switch (selectedItem, targetItem);
        }

        //アイテム使用
        public void Use (SlotItem usedItem) {
            //使用イベントがある時実行
            if (usedItem is IUsable) {
                IUsable usableItem = usedItem as IUsable;

                if (usableItem.Usable) {
                    usableItem.UseEvent?.Invoke (usedItem); //アイテム使用
                    OnItemUsed?.Invoke (usedItem);
                }

            //装備の場合追加処理
            } else if (usedItem is IEquipment) {
                IEquipment equipment = usedItem as IEquipment;

                if (equipment.Usable) {

                    //現在のSlotと対象Slotが違うときイベント実行
                    if (equipment.TargetSlot.Item != usedItem) {
                        if (equipment.TargetSlot.Item == null) 
                            Move (usedItem, equipment.TargetSlot);
                        else 
                            Switch (usedItem, equipment.TargetSlot.Item);

                        equipment.UseEvent?.Invoke (usedItem);
                        equipment.TargetSlot.slotManager.Refresh (usedItem.Tab);
                        OnItemUsed?.Invoke (usedItem);
                    }
                }

            //消費用品の場合追加処理
            } else if (usedItem is IConsumable) {
                IConsumable consumableItem = usedItem as IConsumable;

                if (consumableItem.Usable) {
                    consumableItem.UseEvent?.Invoke (usedItem);
                    usedItem.Count -= 1;

                    if (usedItem.Count <= 0) 
                        usedItem.Tab.Remove (usedItem);

                    OnItemUsed?.Invoke (usedItem);
                }
            }
        }

        //選択および対象アイテム初期化
        internal void ResetItems () {
            SelectedItem = null;
            TargetItem = null;
            OnEventEnded?.Invoke ();
        }

        public void PlaySound(AudioClip ac, float pitch) {
            audiosource.pitch = pitch;
            audiosource.PlayOneShot(ac);
        }
    }
}
