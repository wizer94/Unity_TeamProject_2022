using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Inventory;

public class MenuViewer : ContentViewer {

    [Space (8f)]
    public InventoryManager inventory;
    public Text useText;
    public Text removeText;
    public Button removeButton;
    public Button sellButton;

    private InventorySlot slot;

    protected override void EventCall () {
        foreach (var handler in ItemHandler.HandlerList) {
            handler.OnSlotClick += OnDisplay;
        }
    }

    protected override void OnDisplay (PointerEventData eventData, InventorySlot slot) {

        if (slot.Item == null || eventData.button == PointerEventData.InputButton.Left ||
            eventData.button == PointerEventData.InputButton.Middle) {
            Cancel ();
        }

        if (slot.Item != null && eventData.button == PointerEventData.InputButton.Right) {
            ViewerEnable (slot);
        }
    }

    protected override void OnDisappear (PointerEventData eventData, InventorySlot slot) {
        
    }

    protected override void DrawContent (InventorySlot slot) {

        this.slot = slot; //Slot登録
        SlotItem item = slot.Item; //アイテム呼び出し

        //Slotのアイテムのタブの名前によってソート順とテキスト変更
        if (slot.Item.Tab.TabName == "ShopTab") {
            anchor = ViewerAnchor.BottomRight;
            removeText.text = "捨てる";
            removeButton.interactable = false;
            //sellButton.interactable = false;

        } else if (slot.Item.Tab.TabName == "PlayerChipTab") {
            anchor = ViewerAnchor.TopRight;
            removeText.text = "捨てる";
            removeButton.interactable = true;
            //sellButton.interactable = false;

        } else {
            anchor = ViewerAnchor.BottomRight;
            removeText.text = "捨てる";
            removeButton.interactable = true;
            //sellButton.interactable = true;
        }

        //상점 아이템일 경우
        //if (slot.Item.Tab.TabName == "ShopTab") {
        //    useText.text = "구매";

        //if (slot.Item.Tab.TabName != "ShopTab") { 
        //            //装備によって使用テキスト変更
        //    if (item is IEquipment) useText.text = "装備";
        //    else useText.text = "使用";
        //}
    }

    public void Use () {
        if (slot != null) {
            slot.itemHandler.Use (slot.Item);
            SlotManager.RefreshAll ();
            Cancel ();
        }
    }

    public void Remove () {
        if (slot != null) {
            slot.Item.Tab.Remove (slot.Item);
            SlotManager.RefreshAll ();
            Cancel ();
        }
    }

    public void Cancel () {
        ViewerDisable ();
        slot = null;
    }
}
