using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory {

    public class SlotManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

        [SerializeField] static List<SlotManager> ManagerList = new List<SlotManager> ();

        [Tooltip ("アイテムSlot List（必要だったら登録）")]
        public List<InventorySlot> slotList = new List<InventorySlot> (); //Slot List
        private List<RectTransform> slotRects; //Slot RectTransform List（Pointイベント感知用）
        
        [Tooltip ("名前順にSlotSort")]
        public bool sortByName = true;

        [Space(8f)]
        [Tooltip("アイテムがない時に表示するアイコン")]
        public Sprite defaultIcon;

        [Tooltip ("アイテムハンドラ")]
        public ItemHandler itemHandler;

        public InventoryTab LastRefreshedTab { get; internal set; } //最近ReFreshしたタブ

        internal static InventorySlot SelectedSlot { get; set; }

        private InventorySlot lastEnteredSlot;
        private bool pointerEntered;

        private void Awake () {
            if (!ManagerList.Contains (this)) ManagerList.Add (this);
        }

		private void Start()
		{
			SlotSort(); //最初のソート
		}

		//Slot名前順にソートおよびIndex指定
		public void SlotSort()
		{
			if (sortByName) slotList.Sort((InventorySlot x, InventorySlot y) => x.name.CompareTo(y.name));
			for (int i = 0; i < slotList.Count; i++) slotList[i].Index = i;

			//SlotのRectTransformをListに追加
			slotRects = new List<RectTransform>(slotList.Count);
			foreach (var slot in slotList)
			{
				slotRects.Add(slot.GetComponent<RectTransform>());
			}
		}

		//アイテムテーブルRefresh（タブのアイテムList視覚化）
		public void Refresh (InventoryTab tab) {
            //タブがnullの場合終了（returnする）
            if (tab == null) return;

            if (tab.Capacity > slotList.Count) {
                Debug.Log ("タブのアイテムのストックがスロットの数を超えました。： " + tab.TabName);
                return;
            }

            //Slotアイテムおよび現在のタブの設定
            for (int i = 0; i < tab.Capacity; i++) {
                slotList[i].SetSlot (tab.ItemTable[i]);

                // 装備している場合EquipがTrueのために追加したもの
                if (tab.TabName != "PlayerChipTab" && slotList[i].Item != null)
                {
                    slotList[i].Item.Equip = false;
                }
            }

            LastRefreshedTab = tab;
        }

        //すべてのアイテムのテーブルを最後のタブにReFresh
        public static void RefreshAll () {
            foreach (var manager in ManagerList) {
                if (manager.LastRefreshedTab != null) 
                    manager.Refresh (manager.LastRefreshedTab);
            }
        }

        //すべてのSlotを許可タイプに設定
        public void SetSlotType (string[] types) {
            foreach (var slot in slotList) {
                slot.allowTypes = new string[types.Length];
                for (int i = 0; i < types.Length; i++) {
                    slot.allowTypes[i] = types[i];
                }
            }
        }

        //ポインタダウンイベント（Pointer Down Event）（アイテム選択）
        public void OnPointerDown (PointerEventData eventData) {
            //左クリックイベント
            if (eventData.button == PointerEventData.InputButton.Left) {
                InventorySlot slot = GetSlotFromPointer ();

                if (slot != null) {
                    itemHandler.OnSlotDown?.Invoke (eventData, slot);

                    //選択アイテムの登録（移動する時）
                    if (itemHandler.movable) {
                        if (slot.Item != null) {
                            ItemHandler.SelectedItem = slot.Item;
                            SelectedSlot = slot;
                            itemHandler.OnItemSelected?.Invoke (slot, ItemHandler.SelectedItem);
                        }
                    }
                }
            }
        }

        //ポインタアップイベント（Pointer Up Event）（アイテムドロップ）
        public void OnPointerUp (PointerEventData eventData) {
            ItemHandler.RequestItemResetStop = true; //アイテムハンドラのアイテム初期化感知OFF

            //左クリックイベント
            if (eventData.button == PointerEventData.InputButton.Left) {
                InventorySlot targetSlot = GetSlotFromPointer ();

                if (targetSlot != null) {
                    itemHandler.OnSlotUp?.Invoke (eventData, targetSlot);

                    //移動ONおよび選択したアイテムがあったらイベント実行
                    if (itemHandler.movable && ItemHandler.SelectedItem != null) {
                        ItemHandler.TargetItem = targetSlot.Item; //対象アイテム登録

                        //お互いのSlot Managerが違う場合イベント処理
                        if (SelectedSlot.slotManager != this) {
                            if (!itemHandler.moveToOtherSlot || !targetSlot.itemHandler.moveToOtherSlot) {
                                itemHandler.SlotMoveFailEvent?.Invoke (ItemHandler.SelectedItem);
                                itemHandler.ResetItems ();
                                return;
                            }
                        }

                        //選択アイテムとターゲットアイテムが違う場合イベント実行
                        if (ItemHandler.SelectedItem != ItemHandler.TargetItem) {

                            //ターゲットがない時アイテム移動
                            if (ItemHandler.TargetItem == null) {

                                //ターゲットSlotにタイプがないもしくは新貝のタイプが一致すると実行
                                if (targetSlot.allowTypes.Length == 0 || targetSlot.HasType (ItemHandler.SelectedItem.Type)) {

                                    itemHandler.Move (ItemHandler.SelectedItem, targetSlot);
                                } else
                                    itemHandler.TypeNotMatchEvent?.Invoke (ItemHandler.SelectedItem, targetSlot);

                            } else {
                                //アイテム合成するときイベント実行
                                if (itemHandler.merging && (ItemHandler.SelectedItem.Name == ItemHandler.TargetItem.Name)) {
                                    itemHandler.Merge (ItemHandler.SelectedItem, ItemHandler.TargetItem);

                                    //アイテムの位置交換
                                } else if (itemHandler.switching) {
                                    if (targetSlot.allowTypes.Length == 0 || targetSlot.HasType (ItemHandler.SelectedItem.Type)) {
                                        itemHandler.Switch (ItemHandler.SelectedItem, ItemHandler.TargetItem);

                                    } else
                                        itemHandler.TypeNotMatchEvent?.Invoke (ItemHandler.SelectedItem, targetSlot);
                                }
                            }

                            //選択したSlot Managerおよび対象Slot Manager ReFresh
                            SelectedSlot.slotManager.Refresh (SelectedSlot.slotManager.LastRefreshedTab);
                            if (SelectedSlot.slotManager != this)
                                Refresh (LastRefreshedTab);
                        }
                    }
                }
            }

            ItemHandler.RequestItemResetStop = false; //アイテムハンドラの初期化感知ON
            ItemHandler.CallEventEnd (); //初期化実行
            SelectedSlot = null;
        }

        //Pointer Click Event（アイテム使用）
        public void OnPointerClick (PointerEventData eventData) {
            InventorySlot slot = GetSlotFromPointer ();
            if (slot != null) {
                itemHandler.OnSlotClick?.Invoke (eventData, slot);

                //左ダブルクリックイベント
                if (eventData.button == PointerEventData.InputButton.Left) {

#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
                    if (!(eventData.clickCount >= 2))
                        return; //ダブルクリックチェック（PC）
#endif

                    //Slotにアイテムがある場合実行
                    if (itemHandler.usable && slot.Item != null) {
                        itemHandler.Use (slot.Item);
                        Refresh (LastRefreshedTab);
                    }
                }
            }
        }

        //Pointer Enter Event
        public void OnPointerEnter (PointerEventData eventData) {
            //Pointer イベントたちは基本的に一のオブジェクトの中にしか動くかない
            //Enterのたびに現在認識されたオブジェクトを変更して固定防止
            eventData.pointerPress = gameObject;

            if (itemHandler.enablePointerEnterAndExitEvent) {
                pointerEntered = true;
                StartCoroutine (PointerUpdate (eventData));
            }
        }

        //Pointer Exit Event
        public void OnPointerExit (PointerEventData eventData) {
            eventData.pointerPress = null; //オブジェクト固定防止
            if (pointerEntered) {
                pointerEntered = false; //Pointer Enter, Exit イベント終了
            }

            if (itemHandler.enablePointerEnterAndExitEvent) {

                if (lastEnteredSlot != null) { //最後のSlotがあればExitイベント呼び出し
                    itemHandler.OnSlotExit (eventData, lastEnteredSlot);
                    lastEnteredSlot = null;
                }
            }
        }

        //Enter, ExitイベントのためにPointer更新およびイベント呼び出し
        IEnumerator PointerUpdate (PointerEventData eventData) {
            WaitForSeconds interval = new WaitForSeconds (itemHandler.pointerUpdateInterval); 
            while (pointerEntered) {
                InventorySlot slot = GetSlotFromPointer ();

                if (slot != null) { //PointerにSlotがある場合
                    itemHandler.OnSlotEnter (eventData, slot); //Enterイベント呼び出し

                    //最後のSlotと現在のSlotが違う場合最後のSlotでExitイベント呼び出し
                    if (lastEnteredSlot != null && lastEnteredSlot != slot) {
                        itemHandler.OnSlotExit (eventData, lastEnteredSlot);
                    }

                    lastEnteredSlot = slot; //最後のSlot設定

                } else { //PointerにSlotがない場合
                    if (lastEnteredSlot != null) { //最後のSlotがあればExitイベント呼び出し
                        itemHandler.OnSlotExit (eventData, lastEnteredSlot);
                        lastEnteredSlot = null;
                    }
                }

                yield return interval; //待機
            }
        }

        //Pointerの位置からSlot読み込み
        private InventorySlot GetSlotFromPointer () {
            Vector2 pointer = GetPointerPosition (); //Pointerの位置

            //Slotの位置と距離
            Vector2 pos;
            float xDist, yDist;

            for (int i = 0; i < slotRects.Count; i++) {
                pos = slotRects[i].position;
                xDist = slotRects[i].sizeDelta.x * 0.5f;
                yDist = slotRects[i].sizeDelta.y * 0.5f;

                //PointerがSlotの領域ないにある場合この時のIndex戻す
                if (pointer.x >= pos.x - xDist && pointer.x <= pos.x + xDist && 
                    pointer.y >= pos.y - yDist && pointer.y <= pos.y + yDist) {
                    return slotList[i];
                }
            }

            return null;
        }

        //PC、モバイルによってPointer位置読み込み
        private Vector2 GetPointerPosition () {
            //Mobile
#if (UNITY_IOS && !UNITY_EDITOR) || (UNITY_ANDROID && !UNITY_EDITOR)
            Touch touch = Input.GetTouch (0);
                return touch.position;
#endif
            //PC
            return Input.mousePosition;
        }
    }
}
