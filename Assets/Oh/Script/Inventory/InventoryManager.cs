using Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class InventoryManager : MonoBehaviour {

    public GameObject inventoryPanel;
    public bool bViewInventoryPanel;
    public static bool openingInventory = false;

    public SlotManager slotManager;
    public SlotManager WeaponChipSlotManager;       // weaponChipManager
    public SlotManager playerChipSlotManager;       // playerChipManager
    //public SlotManager shopSlotManager;

    public List<GameObject> tabList = new List<GameObject> (); //タブリスト
    public List<GameObject> extraSlotList = new List<GameObject> ();
    //public List<Item> inventoryItemList = new List<Item> ();    // inventoryのアイテムリスト

    private Dictionary<GameObject, Image> tabImgDctn = new Dictionary<GameObject, Image> (); //タブイメージ
    private Dictionary<GameObject, Button> tabBtnDctn = new Dictionary<GameObject, Button> (); //タブボタン

    //public List<Sprite> spriteList = new List<Sprite> ();

    public static bool remove_check;
    public MenuViewer itemMenuViewer;
    public InventoryTab tab1;
    public InventoryTab tab2;
    public InventoryTab tab3;

    private void Awake () {

        //タブイメージ・ボタン追加
        foreach (var tab in tabList) {
            tabImgDctn.Add (tab, tab.GetComponent<Image> ());
            tabBtnDctn.Add (tab, tab.GetComponent<Button> ());
        }
    }

    private void Start () {
        CreateTab1 ();
        CreateTab2 ();
        CreateTab3 ();

        //slot manager refresh(初期 refresh 必修)
        slotManager.Refresh (TabManager.GetTab ("Tab1"));
        WeaponChipSlotManager.Refresh(TabManager.GetTab("WeaponTab"));
        playerChipSlotManager.Refresh (TabManager.GetTab ("PlayerChipTab"));

        //item handler event 設定
        foreach (var handler in ItemHandler.HandlerList) {
            //handler.DragOutEvent = (item) => Debug.Log ("Drag Out: " + item.Name);
            handler.SlotMoveFailEvent = (item) => Debug.Log ("Slot Move Fail: " + item.Name);
            handler.TypeNotMatchEvent = (item, slot) => Debug.Log ("Type doesn't match: " + item.Type);
        }

        foreach (var slot in extraSlotList) {
            slot.SetActive (true);
        }


        //タブ拡大
        TabManager.GetTab ("Tab1").Extend (5);
        TabManager.GetTab ("Tab2").Extend (5);
        TabManager.GetTab ("Tab3").Extend (5);

        //SlotList 再設定
        slotManager.SlotSort ();
        bViewInventoryPanel = false;
    }

    private void Update()
    {

        ItemHandler.RequestItemHandle(); //event end
        SlotManager.RefreshAll();
        InventoryOnOff();
    }

    public void RemoveItem()
    {
        remove_check = true;

        InventoryTab tab;
        for (int i = 0; i < 25 * 3; i++)
        {
            int index = i % 25;
            if (i < 25 * 1)
                tab = tab1;
            else if (i < 25 * 2)
                tab = tab2;
            else
                tab = tab3;

            tab.ItemTable[index] = null;
        }

        tab = TabManager.GetTab("PlayerChipTab");
        for (int i = 0; i < 5; ++i)
            tab.ItemTable[i] = null;

        tab = TabManager.GetTab("WeaponTab");
        for (int i = 0; i < 5; ++i)
            tab.ItemTable[i] = null;

        remove_check = false;
    }
    public void OnClickSortButton() {
        Sort();
	}

    const int capacity = 25;

    public void Sort() {
        List<SlotItem> all = GetAllItemsInInventory();
        List<SlotItem> others = new List<SlotItem>();
        List<SlotItem> playerChips = new List<SlotItem>();
        List<SlotItem> weaponChips = new List<SlotItem>();
        foreach(SlotItem one in all) {
            if (one == null) continue;
			switch (one.Type) {
                case "PlayerChip":
                    playerChips.Add(one);
                    break;
                case "WeaponChip":
                    weaponChips.Add(one);
                    break;
                default:
                    others.Add(one);
                    break;
            }
		}

        var othersOrdered = others.OrderBy(x => x.Name);
        var playerChipsOrdered = playerChips.OrderByDescending(x => x.Name);
        var weaponChipsOrdered = weaponChips.OrderBy(x => x.Name);
        weaponChipsOrdered = weaponChipsOrdered.ThenByDescending(x => x.Level);

        List<SlotItem> sorted = new List<SlotItem>();
        foreach (SlotItem one in othersOrdered)
            sorted.Add(one);
        foreach (SlotItem one in playerChipsOrdered)
            sorted.Add(one);
        foreach (SlotItem one in weaponChipsOrdered)
            sorted.Add(one);

        for (int i = 0; i < 25 * 3; i++) {
            int index = i % capacity;
            InventoryTab tab;
            if (i < 25 * 1)
                tab = tab1;
            else if (i < 25 * 2)
                tab = tab2;
            else
                tab = tab3;

            if(i >= sorted.Count) {
                tab.ItemTable[index] = null;
                continue;
            }

            tab.ItemTable[index] = sorted[i];
            sorted[i].Index = index;
            sorted[i].Tab = tab;
            
        }

        SlotManager.RefreshAll();
    }

    List<SlotItem> GetAllItemsInInventory() {
        List<SlotItem> rtv = new List<SlotItem>();
        for(int i = 0; i < capacity; i++)
                rtv.Add(tab1.ItemTable[i]);
        for (int i = 0; i < capacity; i++)
                rtv.Add(tab2.ItemTable[i]);
        for (int i = 0; i < capacity; i++)
                rtv.Add(tab3.ItemTable[i]);

        return rtv;
	}

	// インベントリ表示・非表示
	private void InventoryOnOff()
	{
        if (Time.timeScale == 0) return;
        
        if (Input.GetKeyDown(KeyCode.E))
        {        
            bViewInventoryPanel = !bViewInventoryPanel;
            Cursor.visible = bViewInventoryPanel;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            bViewInventoryPanel = false;
        inventoryPanel.SetActive(bViewInventoryPanel);
        openingInventory = bViewInventoryPanel;
    }

    private void CreateTab1 () {
        tab1 = TabManager.GetTab ("Tab1");
    }

    private void CreateTab2 () {
        tab2 = TabManager.GetTab ("Tab2");
    }

    private void CreateTab3 () {
        tab3 = TabManager.GetTab ("Tab3");
    }

    //視覚的にタブ変更
    public void TabConvert (GameObject tabItem) {
        foreach (var tab in tabList) {
            if (tabItem != tab.gameObject) {
                tabBtnDctn[tab].interactable = true;
                tabImgDctn[tab].color = new Color32 (240, 240, 240, 255);
            } else {
                tabBtnDctn[tab].interactable = false;
                tabImgDctn[tab].color = new Color32 (255, 255, 255, 255);
            }
        }
        slotManager.Refresh (TabManager.GetTab (tabItem.name));
    }

    //アイテムが選択しているときのタブ変更
    public void TabConvertWithItem (GameObject tabItem) {
        if (ItemHandler.SelectedItem != null) {
            TabConvert (tabItem);
        }
    }
}
