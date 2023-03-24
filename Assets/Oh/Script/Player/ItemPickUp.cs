using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

public class ItemPickUp : MonoBehaviour
{
	drop_chip d_c;
	public string _id;
	InventoryManager i_v;
	PlayerChipData playerChip;

	public GameObject Inventory;
	public SlotManager slotManager;

	List<Collider2D> hitList = new List<Collider2D>();

	AudioSource audiosource;
	AudioClip pickupSound;
	GameObject starEffect;

	private void Start() {
		audiosource = GetComponent<AudioSource>();
		pickupSound = Resources.Load("SoundEffect/pickup") as AudioClip;
		starEffect = Resources.Load("Prefab/StarEffect") as GameObject;
	}

	private void Update()
	{
		if (hitList.Count > 0)
		{
			//if (Input.GetKeyDown(KeyCode.F))
			{
				List<Collider2D> removeList = new List<Collider2D>();

				foreach (var col in hitList)
				{
					d_c = col.gameObject.GetComponent<drop_chip>();
					i_v = Inventory.GetComponent<InventoryManager>();   // inventoryåƒÇ—èoÇµ
					playerChip = gameObject.GetComponent<PlayerChipData>();

					_id = d_c.chip_id;

					if (i_v.tab1.IsFull) {
						if (i_v.tab2.IsFull) {
							if (i_v.tab3.IsFull) {
								slotManager.Refresh(slotManager.LastRefreshedTab);
								return;
							}
						}
					}

					if (!i_v.tab1.IsFull) {
						i_v.tab1.Add(playerChip.ChipList(_id));
						Pickup();
					}
					else if (!i_v.tab2.IsFull) {
						i_v.tab2.Add(playerChip.ChipList(_id));
						Pickup();
					}
					else {
						i_v.tab3.Add(playerChip.ChipList(_id));
						Pickup();
					}

					slotManager.Refresh(slotManager.LastRefreshedTab);
					_id = "";
					removeList.Add(col);
				}

				foreach(var col in removeList)
				{
					hitList.Remove(col);
					Destroy(col.gameObject);
				}
			}
		}
	}

	//public void OnTriggerStay2D(Collider2D col)
	//{
	//	if (col.gameObject.tag == "DropChip")
	//	{
	//		isHit = true;
	//	}
	//}

	void Pickup() {
		audiosource.PlayOneShot(pickupSound);

		GameObject effect = Instantiate(starEffect, transform.position, Quaternion.identity);
		effect.transform.localScale *= 0.3f;
		ParticleSystem ps = effect.GetComponent<ParticleSystem>();
		ParticleSystem.MainModule main = ps.main;
		main.startColor = new Color(1f, 1f, 0.4f);
		ParticleSystem.Burst burst = ps.emission.GetBurst(0);
		burst.count = Random.Range(2, 5);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "DropChip")
		{
			hitList.Add(collision);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "DropChip")
		{
			hitList.Remove(collision);
		}
	}
}
