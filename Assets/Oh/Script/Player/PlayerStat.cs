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
	[Tooltip("�ύX���鐔�l�m�F�p�i���͂��Ȃ����Ɓj")]
	public float[] check_stat;
	[Tooltip("��������Chip�̃^�C�v�m�F�p�i���͂��Ȃ����Ɓj")]
	public string[] check_type;

	[Tooltip ("�����X�s�[�h�i�K�����́j")]
	public float Min_speed;         // �����l�i�Œ���@��j
	public float speed;             // ���x�ϓ��l
	[Tooltip("����HP�i�K�����́j")]
	public float Max_HP;            // �����l
	public float HP;                // HP�ϓ��l
	private float Min_HP = 1.0f;	// �Œ��HP
	[Tooltip("�����_�b�V�����x�i���͕s�v�j")]
	public float dash;              // �����l
	public float damageCut = 0.0f;
	public float FullHPDamage = 1.0f;
	public float FullHPDamage_HP = 0.98f;
	public float LessHPDamage = 1.0f;
	public float LessHPDamage_HP = 0.35f;
	public float Avoid_up = 0.0f;   // ��򋗗��A�b�v
	public float Invincible;        // ���G���ԑ���
	public float Crit_Up;           // �N���e�B�J�����A�b�v
	public float CritDamage_Up;     // �N���e�B�J���_���[�W�A�b�v

	public static int enemy_cnt = 0;		// ���j��
	public static float ExistTime = 0.0f;   // �I������
	public bool inSafeArea = false;

	public int target_id;
	public int selected_id;

	UI ui;



	public float move_stat = 0;
	public string move_type = "";

	// Chip
	/*
	public float Speed_up;          //// �ړ����x�㏸
	public float HP_up;             //// HP�ő��
	public float Damage_down;		// �_���[�W�y��
	public bool  FullHP;			// �̗͖��^�����_���[�W�㏸
	public bool  LessHP;            // �m�����Ƀ_���[�W�㏸���A�ˑ��x�A�b�v (35���ȉ�)
	public float Crit_per;			// �N���e�B�J�����A�b�v
	public bool muteki;				// ��莞�Ԗ��G
	public float Avoid_up;			// ��򋗗��A�b�v
	*/

	//public bool  Avoid_bool;		// ����N�[���^�C�������i�ۗ��j


	//public bool  AllCooldown;       // �N�[���^�C������ (����A���G�Ȃ�)
	//public bool  Invisible_Time;	// ��莞�ԓ�����

	//public float Drop_per;			// �h���b�v���A�b�v
	//public float Heal_per;			// �񕜗ʏ㏸�i�ۗ��j

	// ����
	//public bool  Avoid_perfect;     // ��𐬌���_���[�W�㏸�i�G�̍U������������j
	// public bool LandEffect;		// �n�`����
	// public int Bomb_up;			// �{���ǉ�

	// public bool Check_Fight;		// ��퓬��������
	// public float imune;			// �����ϐ�
	// public float enhance_damage;	// ��������
	// public float damage_counter;	// �_���[�W�J�E���^�[

	void Start()
	{
		HP = Max_HP;
		speed = Min_speed;
		dash = speed + 3.0f;
		Invincible = 0.05f;
		// ��{�X�e�[�^�X
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
		// �I�����ԃJ�E���^
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
				//HP���ő�̏�Ԃł����ɗ�����
				if (Max_HP <= HP)
				{
					//�����������
					ui.DecGauge(HP - 30, Max_HP);
				}

				Max_HP += check_stat[slotnumber];								
				HP += check_stat[slotnumber];

				//UIHP�𑝂₷
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
			// Konshin - HP��100���̏ꍇ�^����_���[�W���オ��
			case "Konshin":
				FullHPDamage += check_stat[slotnumber];
				break;
			// Haisui - HP��35���ȉ��̏ꍇ�^����_���[�W���オ��
			case "Haisui":
				LessHPDamage += check_stat[slotnumber];
				break;
			// ��������E���G���ԑ���
			case "AvoidUp":
				Avoid_up += check_stat[slotnumber];
				Invincible += check_stat[slotnumber];
				break;
			// �N���e�B�J��
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
			// Konshin - HP��100���̏ꍇ�^����_���[�W���オ��
			case "Konshin":
				FullHPDamage -= check_stat[slotnumber];
				break;
			// Haisui - HP��35���ȉ��̏ꍇ�^����_���[�W���オ��
			case "Haisui":
				LessHPDamage -= check_stat[slotnumber];
				break;          
			// ��������E���G���ԑ���
			case "AvoidUp":
				Avoid_up -= check_stat[slotnumber];
				Invincible -= check_stat[slotnumber];
				HoldZero(Avoid_up);
				HoldZero(Invincible);
				break;
			// �N���e�B�J��
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

			// �����O�������̏��� PCT->Inven
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
