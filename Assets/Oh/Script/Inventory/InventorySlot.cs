using UnityEngine;
using UnityEngine.UI;

namespace Inventory {
    [System.Serializable]
    [RequireComponent (typeof (RectTransform))]
    public class InventorySlot : MonoBehaviour {

        public SlotManager slotManager; //slot manager
        public Image slotIcon;          //slot icon
        public Text itemCountText;      //item数表示
        public bool isEquip;            //装備しているか確認
        public enum SlotType {
            Inventory,
            WeaponChip,
            PlayerChip
		}
        public SlotType slottype;
        public string[] allowTypes;     //許可されるアイテムタイプ（空白の場合すべて許可）

        internal ItemHandler itemHandler; //アイテムハンドラ

        public int Index { get; internal set; } //SLOT INDEX
        public SlotItem Item { get; internal set; } //現在SLOT Item


        GameObject WeaponChipMain, WeaponChipSub, WeaponChipInv;

        private void Awake () {

            //SLOT MANAGER ないときデバッグ
            if (slotManager == null) {
                Debug.Log ("SLOTMANAGERがキャッシングされてません。");
                return;
            }

            //イメージがないときイメージ読み込み
            if (slotIcon == null) {
                Image image;
                if ((image = GetComponent<Image> ()) != null)
                    slotIcon = image;
            }

                    //Tab ManagerにSLOT登録
            if (!slotManager.slotList.Contains (this))
                slotManager.slotList.Add (this);

            //アイテムハンドラ設定
            itemHandler = slotManager.itemHandler;

            isEquip = false;
        }

        private void Start() {
            WeaponChipMain = GameObject.Find("Main");
            WeaponChipSub = GameObject.Find("Sub");
            WeaponChipInv = GameObject.Find("WeaponChipInventory");
		}

        //SLOT 設定
        internal void SetSlot (SlotItem slotItem) {
            
            if (slotItem != null) {
                this.Item = slotItem;
                if (slotIcon != null)
                {
                    slotIcon.sprite = slotItem.Icon; //icon 設定
                    slotIcon.color = slotItem.Color;
                    isEquip = true;
                }
                if (itemCountText != null) { //数テキスト設定
                    if (slotItem.MaxCount == 1) itemCountText.text = "";
                    else itemCountText.text = slotItem.Count.ToString ();
                }
                if(slotItem.Type == "WeaponChip") {
                    if (slottype == SlotType.WeaponChip) {
                        slotItem.Object.transform.SetParent(WeaponChipMain.transform.GetChild(0).transform);
                    }
                    else {
                        slotItem.Object.transform.SetParent(WeaponChipInv.transform);
                    }
                }

            } else {
                //SLOTを基本の数値に初期化
                this.Item = null;
                isEquip = false;
                if (slotIcon != null) {
                    slotIcon.sprite = slotManager.defaultIcon;
                    slotIcon.color = Color.white;
                }
                if (itemCountText != null) itemCountText.text = "";
            }
        }

        //できるタイプか確認
        public bool HasType (string type) {
            foreach (var s in allowTypes) {
                if (s == type)
                    return true;
            }
            return false;
        }
    }
}
