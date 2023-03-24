using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

public class PlayerStat : MonoBehaviour
{
	[SerializeField] List<InventorySlot> inventory_item = new List<InventorySlot>();
	[SerializeField] Sprite defaultsprite;

	public bool[] equip_check;

	// CharaStat
	[Tooltip("変更する数値確認用（入力しないこと）")]
	public float[] check_stat;
	[Tooltip("装備したChipのタイプ確認用（入力しないこと）")]
	public string[] check_type;

	[Tooltip ("初期スピード（必ず入力）")]
	public float Min_speed;         // 初期値（最低限法定）
	public float speed;             // 速度変動値
	[Tooltip("初期HP（必ず入力）")]
	public float Max_HP;            // 初期値
	public float HP;                // HP変動値
	private float Min_HP = 1.0f;	// 最低限HP
	[Tooltip("初期ダッシュ速度（入力不要）")]
	public float dash;              // 初期値
	public float damageCut = 0.0f;
	public float FullHPDamage = 1.0f;
	public float FullHPDamage_HP = 0.98f;
	public float LessHPDamage = 1.0f;
	public float LessHPDamage_HP = 0.35f;
	public float Avoid_up = 0.0f;   // 回飛距離アップ
	public float Invincible;        // 無敵時間増加
	public float Crit_Up;           // クリティカル率アップ
	public float CritDamage_Up;     // クリティカルダメージアップ

	public static int enemy_cnt = 0;		// 撃破数
	public static float ExistTime = 0.0f;   // 終了時間
	public bool inSafeArea = false;

	public int target_id;
	public int selected_id;

	UI ui;



	public float move_stat = 0;
	public string move_type = "";

	// Chip
	/*
	public float Speed_up;          //// 移動速度上昇
	public float HP_up;             //// HP最大量
	public float Damage_down;		// ダメージ軽減
	public bool  FullHP;			// 体力満タン時ダメージ上昇
	public bool  LessHP;            // 瀕死時にダメージ上昇＆連射速度アップ (35％以下)
	public float Crit_per;			// クリティカル率アップ
	public bool muteki;				// 一定時間無敵
	public float Avoid_up;			// 回飛距離アップ
	*/

	//public bool  Avoid_bool;		// 回避クールタイム減少（保留）


	//public bool  AllCooldown;       // クールタイム減少 (回避、無敵など)
	//public bool  Invisible_Time;	// 一定時間透明化

	//public float Drop_per;			// ドロップ率アップ
	//public float Heal_per;			// 回復量上昇（保留）

	// 未定
	//public bool  Avoid_perfect;     // 回避成功後ダメージ上昇（敵の攻撃実装したら）
	// public bool LandEffect;		// 地形効果
	// public int Bomb_up;			// ボム追加

	// public bool Check_Fight;		// 非戦闘時自動回復
	// public float imune;			// 属性耐性
	// public float enhance_damage;	// 属性強化
	// public float damage_counter;	// ダメージカウンター

	void Start()
	{
		HP = Max_HP;
		speed = Min_speed;
		dash = speed + 3.0f;
		Invincible = 0.05f;
		// 基本ステータス
		Stat_Reset();
		for (int i = 0; i < 5; ++i)
		{
			equip_check[i] = false;
			check_stat[i] = 0.0f;
			check_type[i] = null;
		}

		ui = GetComponent<UI>();

		InvokeRepeating("SafeAreaHealing", 0.25f, 0.25f);
	}

	void Update()
	{
		target_id = ItemHandler.target_index;
		selected_id = ItemHandler.selected_index;
		// 終了時間カウンタ
		if (gameObject.activeInHierarchy)
			ExistTime += Time.deltaTime;

		Equip_Check();

		if (scene.Stage_start)
		{
			UI ui = this.GetComponent<UI>();
			HP = Max_HP;
			ui.ChangeHPGauge(HP, Max_HP);
			scene.Stage_start = false;
		}
		if (InventoryManager.remove_check)
		{
			Stat_Reset();
		}
	}

	void Plus_Stat(int slotnumber)
	{
		check_stat[slotnumber] = inventory_item[slotnumber].Item.Stat;
		check_type[slotnumber] = inventory_item[slotnumber].Item.Equip_Type;

		switch (check_type[slotnumber])
		{
			//HP
			case "HP":
				//HPが最大の状態でここに来たら
				if (Max_HP <= HP)
				{
					//少しだけ削る
					ui.DecGauge(HP - 30, Max_HP);
				}

				Max_HP += check_stat[slotnumber];								
				HP += check_stat[slotnumber];

				//UIHPを増やす
				ui.HealHPGauge(HP, Max_HP);
				break;
			//SPEED
			case "SPEED":
				speed += check_stat[slotnumber];
				dash += check_stat[slotnumber];
				break;
			// DamageCut
			case "DamageCut":
				damageCut += check_stat[slotnumber];
				break;
			// Konshin - HPが100％の場合与えるダメージが上がる
			case "Konshin":
				FullHPDamage += check_stat[slotnumber];
				break;
			// Haisui - HPが35％以下の場合与えるダメージが上がる
			case "Haisui":
				LessHPDamage += check_stat[slotnumber];
				break;
			// 回避距離・無敵時間増加
			case "AvoidUp":
				Avoid_up += check_stat[slotnumber];
				Invincible += check_stat[slotnumber];
				break;
			// クリティカル
			case "Critical":
				Crit_Up += check_stat[slotnumber];
				CritDamage_Up += check_stat[slotnumber] * 2;
				break;
		}

		inventory_item[slotnumber].Item.Equip = inventory_item[slotnumber].isEquip;
		equip_check[slotnumber] = inventory_item[slotnumber].Item.Equip;
	}


	void SafeAreaHealing() {
		if (!inSafeArea) return;
		//Debug.Log(HP + "/" + Max_HP);

		if (HP < Max_HP) {
			HP += Max_HP * 0.1f;

		}
		if (HP > Max_HP) {
			HP = Max_HP;
		}
		ui.HealHPGauge(HP, Max_HP);
	}

	void HoldZero(float stat)
	{
		if (stat <= 0.0f)
			stat = 0.0f;
	}

	void Minus_Stat(int slotnumber)
	{
		switch (check_type[slotnumber])
		{
			//HP
			case "HP":
				UI ui = this.GetComponent<UI>();

				Max_HP -= check_stat[slotnumber];
				HP -= check_stat[slotnumber];
				if (HP > Max_HP)
					HP = Max_HP;
				if (HP < 0)
					HP = Min_HP;

				ui.ChangeHPGauge(HP, Max_HP);

				break;
			//SPEED
			case "SPEED":
				speed -= check_stat[slotnumber];
				dash -= check_stat[slotnumber];
				break;
			// DamageCut
			case "DamageCut":
				damageCut -= check_stat[slotnumber];
				HoldZero(damageCut);
				break;
			// Konshin - HPが100％の場合与えるダメージが上がる
			case "Konshin":
				FullHPDamage -= check_stat[slotnumber];
				break;
			// Haisui - HPが35％以下の場合与えるダメージが上がる
			case "Haisui":
				LessHPDamage -= check_stat[slotnumber];
				break;          
			// 回避距離・無敵時間増加
			case "AvoidUp":
				Avoid_up -= check_stat[slotnumber];
				Invincible -= check_stat[slotnumber];
				HoldZero(Avoid_up);
				HoldZero(Invincible);
				break;
			// クリティカル
			case "Critical":
				Crit_Up -= check_stat[slotnumber];
				CritDamage_Up -= check_stat[slotnumber] * 2;
				HoldZero(Crit_Up);
				HoldZero(CritDamage_Up);
				break;

		}
		check_type[slotnumber] = null;

		equip_check[slotnumber] = inventory_item[slotnumber].isEquip;
		check_stat[slotnumber] = 0.0f;
	}

	void Stat_Move(int target_index, int selected_index)
	{
		if (target_index > 5) return;
		if (selected_index > 5) return;
		move_stat = 0;
		move_type = "";

		move_stat = check_stat[target_index];
		check_stat[target_index] = check_stat[selected_index];
		check_stat[selected_index] = move_stat;

		move_type = check_type[target_index];
		check_type[target_index] = check_type[selected_index];
		check_type[selected_index] = move_type;

		equip_check[target_index] = inventory_item[target_id].isEquip;
	}

	public void Equip_Check()
	{

		for (int i = 0; i < 5; ++i)
		{
			// PCT <->PCT
			if (ItemHandler.playerchip_move) {
				Stat_Move(target_id, selected_id);
				ItemHandler.playerchip_move = false;
			}

			// 装備外した時の処理 PCT->Inven
			if (inventory_item[i].Item == null) {
				if (equip_check[i] != inventory_item[i].isEquip)
					Minus_Stat(i);
				continue;
			}
			else
			{
				// Invenchip <->PCT
				if (ItemHandler.playerchip_change) {
					Stat_Move(target_id, selected_id);
					Minus_Stat(i);
					ItemHandler.playerchip_change = false;
					Plus_Stat(i);

					inventory_item[i].Item.Equip = inventory_item[i].isEquip;
					equip_check[i] = inventory_item[i].Item.Equip;
					continue;
				}
			}

			// Inven -> PCT
			if (inventory_item[i].Item.Equip != inventory_item[i].isEquip) {
				Minus_Stat(i);
				Plus_Stat(i);
			}
		}

		if (inventory_item[0].Item == null && inventory_item[1].Item == null && inventory_item[2].Item == null && inventory_item[3].Item == null && inventory_item[4].Item == null)
			Stat_Reset();
	}

	private void Stat_Reset()
	{
		if (!equip_check[0] && !equip_check[1] && !equip_check[2] && !equip_check[3] && !equip_check[4])
		{
			for (int i = 0; i < 5; ++i)
			{
				check_stat[i] = 0.0f;
				check_type[i] = null;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.transform.CompareTag("SavePoint")) {
			inSafeArea = true;
		}
	}
	private void OnTriggerExit2D(Collider2D other) {
		if (other.transform.CompareTag("SavePoint")) {
			inSafeArea = false;
		}
	}
}
