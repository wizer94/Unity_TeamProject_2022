using UnityEngine;
using UnityEngine.UI;
using Inventory;
using UnityEngine.EventSystems;

public class InfoViewer : ContentViewer {

    [Space(8f)]
    public Text nameText;
    public Text typeText;
    public Text descText;
    public Text priceText;
    public MenuViewer itemMenuViewer;

    protected override void EventCall () {
        foreach (var handler in ItemHandler.HandlerList) {
            handler.OnSlotEnter += OnDisplay;
            //handler.OnSlotUp += OnDisplay;
            handler.OnSlotExit += OnDisappear;
            //handler.OnPointerDown += OnDisappear;
        }
    }

    protected override void OnDisplay (PointerEventData eventData, InventorySlot slot) {
        if (slot.Item != null && !itemMenuViewer.IsEnabled) {
            ViewerEnable (slot);
        }
    }

    protected override void OnDisappear (PointerEventData eventData, InventorySlot slot) {
        ViewerDisable ();
    }

    protected override void DrawContent (InventorySlot slot) {
        SlotItem item = slot.Item;

        //アイテムがない場合Return
        if (item == null) return;

        //Slotのアイテムタブの名前によってソート順設定
        //if (item.Tab.TabName == "ShopTab") anchor = ViewerAnchor.TopRight;
        if (item.Tab.TabName == "PlayerChipTab" || item.Tab.TabName == "WeaponTab") anchor = ViewerAnchor.BottomRight;
        else anchor = ViewerAnchor.TopLeft;

        //アイテムの数によって名前と数表示
        if (item.Count == 1) nameText.text = item.Name;
        else nameText.text = $"{item.Name}({item.Count})";

        //アイテムのタイプによって色変更
        switch (item.Type) {
            case "PlayerChip": typeText.color = new Color32 (50, 160, 160, 255); break;
            case "WeaponChip": typeText.color = new Color32 (160, 50, 160, 255); break;
            case "Potion": typeText.color = new Color32 (160, 160, 50, 255); break;
            default: typeText.color = new Color32 (50, 50, 50, 255); break;
        }

        //アイテムタイプおよび説明表示
        typeText.text = slot.Item.Type;
        descText.text = slot.Item.Description;
    }
}
