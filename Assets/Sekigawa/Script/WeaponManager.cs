using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{

	const int weaponKind = 7;
	[SerializeField] GameObject[] guns_ = new GameObject[weaponKind];
	[SerializeField] int weaponNum = 0;
	[SerializeField] int subWeaponNum = 1;
	public GameObject[] guns = new GameObject[weaponKind];
	public bool[] usable_gun = new bool[weaponKind];
	[SerializeField] Image mainImage, subImage;
	Image mainImageParent, subImageParent;
	Color selectedColor, unselectedColor;

	[SerializeField] Transform Main_parent;
	[SerializeField] Transform Storage_parent;
	[SerializeField] Inventory.SlotManager slotManager;

	AudioSource audiosource;
	[SerializeField] AudioClip changedSound;

	ReticleScript reticle_script;

	PlayerStat ps;
	float animationTime = 0;

	GameObject gun_using, gun_main, gun_sub;
	bool using_main = true;

    void Start()
	{
		reticle_script = GameObject.Find("Reticle").GetComponent<ReticleScript>();
		ps = GetComponent<PlayerStat>();
		usable_gun[0] = true;
		usable_gun[1] = true;
		usable_gun[6] = true;

		selectedColor = new Color(0.5f, 1f, 0.5f, 1f);
		unselectedColor = new Color(0.5f, 0.5f, 0.5f, 1f);
		mainImageParent = mainImage.transform.parent.GetComponent<Image>();
		subImageParent = subImage.transform.parent.GetComponent<Image>();
		mainImageParent.color = using_main ? selectedColor : unselectedColor;
		subImageParent.color = !using_main ? selectedColor : unselectedColor;

		audiosource = GetComponent<AudioSource>();

		for (int i = 0; i < guns_.Length; i++) {
			GameObject gun = Instantiate(guns_[i]);
			gun.transform.position += transform.position;
			gun.name = gun.name.Replace("(Clone)", "");
			guns[i] = gun;
			gun.transform.SetParent(Storage_parent);
			gun.SetActive(false);
		}

		gun_main = guns[weaponNum];
		if (weaponNum == subWeaponNum)
			subWeaponNum = (subWeaponNum + 1) % weaponKind;
		gun_sub = guns[subWeaponNum];


		ChangeWeapon(gun_main);
	}

	void Update()
	{
		float dt = Time.deltaTime;

		if (KeyScript.InputDown(KeyScript.Dir.Switch)) {
			using_main = !using_main;
			ChangeWeapon(using_main ? gun_main : gun_sub);
			mainImageParent.color = using_main ? selectedColor : unselectedColor;
			subImageParent.color = !using_main ? selectedColor : unselectedColor;
		}


		if (ps.inSafeArea)
			animationTime += dt;
		else
			animationTime = 0;

		if (UsableWeaponCount() > 2) {
			mainImage.transform.localScale = Vector3.one * (1f + Mathf.Sin(animationTime * 4f) * 0.1f);
			subImage.transform.localScale = Vector3.one * (1f + Mathf.Sin(animationTime * 4f) * 0.1f);
		}

	}

	bool ChangeWeapon(GameObject wp) {
		if (gun_using == wp) return false;

		if (gun_using != null) {
			WeaponScript ws = gun_using.GetComponent<WeaponScript>();
			for(int i=0;i< ws.chipDatas.Length; i++) {
				ws.chipDatas[i] = slotManager.slotList[i].Item;
				if(slotManager.slotList[i].Item != null)
					Inventory.TabManager.GetTab("WeaponTab").Remove(slotManager.slotList[i].Item);

				if (ws.chipDatas[i] != null) {
					if (ws.chipDatas[i].Index >= 0)
						ws.chipDatas[i].Index -= 5;
				}

			}

			Inventory.SlotManager.RefreshAll();

			gun_using.transform.SetParent(Storage_parent);
			gun_using.SetActive(false);
		}


		wp.transform.SetParent(Main_parent);
		wp.SetActive(true);
		reticle_script.weapon = wp;
		gun_using = wp;
		WeaponScript wps = wp.GetComponent<WeaponScript>();
		for (int i = 0; i < wps.chipDatas.Length; i++) {
			if (wps.chipDatas[i] != null) {

				if (wps.chipDatas[i].Index < 0)
					wps.chipDatas[i].Index += 5;
				slotManager.slotList[i].SetSlot(wps.chipDatas[i]);
				Inventory.TabManager.GetTab("WeaponTab").Add(slotManager.slotList[i].Item, slotManager.slotList[i].Index);
			}
			else {
				slotManager.slotList[i].SetSlot(null);
			}
		}
		Inventory.SlotManager.RefreshAll();

		GetComponent<UI>().ChangeW(gun_using.name);
		audiosource.PlayOneShot(changedSound);
		

		return true;
	}

	public void OnClickMainButton() {
		if (!ps.inSafeArea) return;

		do
			weaponNum = (weaponNum + 1) % weaponKind;
		while (weaponNum == subWeaponNum || !usable_gun[weaponNum]);

		gun_main = guns[weaponNum];
		mainImage.sprite = gun_main.GetComponent<SpriteRenderer>().sprite;

		if(using_main)
			ChangeWeapon(gun_main);
	}
	public void OnClickSubButton() {
		if (!ps.inSafeArea) return;

		do
			subWeaponNum = (subWeaponNum + 1) % weaponKind;
		while (weaponNum == subWeaponNum || !usable_gun[subWeaponNum]);

		gun_sub = guns[subWeaponNum];
		subImage.sprite = gun_sub.GetComponent<SpriteRenderer>().sprite;


		if (!using_main)
			ChangeWeapon(gun_sub);
	}
	public int UsableWeaponCount() {
		int rtv = 0;
		for (int i = 0; i < usable_gun.Length; i++) {
			if (usable_gun[i])
				rtv++;
		}
		return rtv;

	}

	public void RecreateGuns() {
		foreach(GameObject gun in guns) {
			Destroy(gun);
		}
		Start();
	}

	public bool ReleaseWeapon(int num) {
		if (!usable_gun[num]) {
			usable_gun[num] = true;
			return true;
		}
		return false;
	}
	public int ReleaseWeaponRandom() {
		List<int> not_usable_gun = new List<int>();
		for(int i = 0; i < usable_gun.Length; i++) {
			if (!usable_gun[i])
				not_usable_gun.Add(i);
		}
		if (not_usable_gun.Count == 0)
			return -1;
		int r = Random.Range(0, not_usable_gun.Count);
		usable_gun[not_usable_gun[r]] = true;
		return not_usable_gun[r];
	}
}
