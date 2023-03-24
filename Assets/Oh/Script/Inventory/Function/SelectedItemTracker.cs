using UnityEngine;
using UnityEngine.UI;

namespace Inventory {

    public class SelectedItemTracker : MonoBehaviour {

        [SerializeField] Canvas canvas;
        public Vector2 trackerSize; //トラッキングイメージサイズ
        public Color trackerColor; //トラッキングイメージ色
        public Color selectedSlotColor; //選択したSlotの色


        private InventorySlot lastSlot;
        private SlotItem lastItem;
        private Image trackingImage;
        private RectTransform tracker_rt;
        private Color lastSlotColor;

        private void Start () {

            //イベントハンドラ追加
            foreach (var handler in ItemHandler.HandlerList) {
                handler.OnItemSelected += OnItemSelected;
                handler.OnEventEnded += OnEventEnded;
            }

            //トラッカ（Tracker）イメージ生成
            GameObject tracker = new GameObject ("PointerTracker", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            trackingImage = tracker.GetComponent<Image> ();
            trackingImage.color = trackerColor;
            trackingImage.raycastTarget = false;

            tracker_rt = tracker.GetComponent<RectTransform> ();
            tracker_rt.sizeDelta = trackerSize;
            tracker_rt.gameObject.SetActive (false);
            tracker_rt.SetParent (canvas.transform);

        }

        //Onにする間マウストラッキング
        private void LateUpdate () {
            if (tracker_rt.gameObject.activeSelf) {
                tracker_rt.position = Input.mousePosition;

                if (lastSlot != null) {
                    if (lastItem == lastSlot.Item) lastSlot.slotIcon.color = new Color(lastSlot.slotIcon.color.r, lastSlot.slotIcon.color.g, lastSlot.slotIcon.color.b, 0.5f);
                    else lastSlot.slotIcon.color = lastSlotColor;
                }
            }
        }

        //アイテム選択イベント
        public void OnItemSelected (InventorySlot slot, SlotItem item) {
            if (slot.Item != null) {
                lastSlot = slot;
                lastItem = slot.Item;
                lastSlotColor = slot.slotIcon.color;

                slot.slotIcon.color = new Color(lastSlot.slotIcon.color.r, lastSlot.slotIcon.color.g, lastSlot.slotIcon.color.b, 0.5f);
                trackingImage.sprite = item.Icon;
                trackingImage.color = slot.slotIcon.color;
                tracker_rt.gameObject.SetActive (true);
            }
        }

        //イベント終了時イベント
        public void OnEventEnded () {
            if (lastSlot != null) lastSlot.slotIcon.color = lastSlotColor;
            tracker_rt.gameObject.SetActive (false);
        }

        public Image GetTracker () {
            if (trackingImage != null) return trackingImage;
            else return null;
        }
    }
}
