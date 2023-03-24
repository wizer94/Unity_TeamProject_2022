using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

public class WeaponChip_Pickup : MonoBehaviour
{
	public GameObject WC_Inventory;

	public InventoryManager inventoryManager;
	public SlotManager slotManager;

	AudioSource audiosource;
	AudioClip pickupSound;
	GameObject starEffect;

	void Start() {

		audiosource = GetComponent<AudioSource>();
		pickupSound = Resources.Load("SoundEffect/pickup") as AudioClip;
		starEffect = Resources.Load("Prefab/StarEffect") as GameObject;
	}
	
	void Update() {
		
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("WeaponChip")) {
			WeaponChip wc = other.GetComponent<WeaponChip>();
			Item chip = new Item(other.gameObject, wc.Name, wc.description, wc.level, wc.icon, wc.GetColorFromLevel());
			bool takable = AddInventory(chip);
			if (takable) {
				wc.droping = false;
				other.transform.SetParent(WC_Inventory.transform);
				slotManager.Refresh(slotManager.LastRefreshedTab);
			}
		}
	}

	public bool AddInventory(SlotItem si) {
		if (!inventoryManager.tab1.IsFull) {
			inventoryManager.tab1.Add(si);
			Pickup();
			return true;
		}
		if (!inventoryManager.tab2.IsFull) {
			inventoryManager.tab2.Add(si);
			Pickup();
			return true;
		}
		if (!inventoryManager.tab3.IsFull) {
			inventoryManager.tab3.Add(si);
			Pickup();
			return true;
		}

		return false;
	}

	void Pickup() {
		audiosource.PlayOneShot(pickupSound);

		GameObject effect = Instantiate(starEffect, transform.position, Quaternion.identity);
		effect.transform.localScale *= 0.5f;
		ParticleSystem ps = effect.GetComponent<ParticleSystem>();
		ParticleSystem.MainModule main = ps.main;
		main.startColor = new Color(1f, 1f, 0.4f);
	}
}
