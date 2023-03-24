using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

public class PlayerChipData : MonoBehaviour
{ 
    public List<Sprite> spriteList = new List<Sprite>();

	// chip���X�g
	public SlotItem ChipList(string _id)
	{
		switch (int.Parse(_id))
		{
			// HP UP
			case 10001:
				Item HP01 = new Item(_id, "�ő�HP_R", 50.0f, "PlayerChip", "HP", "�ő�HP��50��������}�e���A���B", spriteList[0]);
				HP01.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return HP01;
			case 10002:
				Item HP02 = new Item(_id, "�ő�HP_SR", 100.0f, "PlayerChip", "HP", "�ő�HP��100��������}�e���A���B", spriteList[1]);
				HP02.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Equip);
				return HP02;
			case 10003:
				Item HP03 = new Item(_id, "�ő�HP_SSR", 200.0f, "PlayerChip", "HP", "�ő�HP��200��������}�e���A���B", spriteList[2]);
				HP03.UseEvent += (item) => Debug.Log("Item: " + item.Name);
				return HP03;

			// SPEED
			case 10011:
				Item SPEED01 = new Item(_id, "SPEED UP_R", 0.5f, "PlayerChip", "SPEED", "�ړ����x��0.5��������}�e���A���B", spriteList[3]);
				SPEED01.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return SPEED01;
			case 10012:
				Item SPEED02 = new Item(_id, "SPEED UP_SR", 1.0f, "PlayerChip", "SPEED", "���x��1.0��������}�e���A���B", spriteList[4]);
				SPEED02.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return SPEED02;
			case 10013:
				Item SPEED03 = new Item(_id, "SPEED UP_SSR", 1.8f, "PlayerChip", "SPEED", "���x��1.8��������}�e���A���B", spriteList[5]);
				SPEED03.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return SPEED03;

			// DamageCut
			case 10021:
				Item DamageCut01 = new Item(_id, "Damage Cut_R", 0.13f, "PlayerChip", "DamageCut", "�󂯂�_���[�W��13%�y������}�e���A���B", spriteList[6]);
				DamageCut01.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return DamageCut01;
			case 10022:
				Item DamageCut02 = new Item(_id, "Damage Cut_SR", 0.27f, "PlayerChip", "DamageCut", "�󂯂�_���[�W��27%�y������}�e���A���B", spriteList[7]);
				DamageCut02.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return DamageCut02;
			case 10023:
				Item DamageCut03 = new Item(_id, "Damage Cut_SSR", 0.40f, "PlayerChip", "DamageCut", "�󂯂�_���[�W��40%�y������}�e���A���B", spriteList[8]);
				DamageCut03.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return DamageCut03;

			// HighHP_DamageUp
			case 10031:
				Item Konshin01 = new Item(_id, "�Ӑg_R", 0.1f, "PlayerChip", "Konshin", "HP��100���̏ꍇ�^����_���[�W��10���オ��}�e���A���B", spriteList[9]);
				Konshin01.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return Konshin01;
			case 10032:
				Item Konshin02 = new Item(_id, "�Ӑg_SR", 0.22f, "PlayerChip", "Konshin", "HP��100���̏ꍇ�^����_���[�W��22���オ��}�e���A���B", spriteList[10]);
				Konshin02.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip + item.Tab.TabName);
				return Konshin02;
			case 10033:
				Item Konshin03 = new Item(_id, "�Ӑg_SSR", 0.4f, "PlayerChip", "Konshin", "HP��100���̏ꍇ�^����_���[�W��40���オ��}�e���A���B", spriteList[11]);
				Konshin03.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return Konshin03;

			// LessHP_DamageUP
			case 10041:
				Item Haisui01 = new Item(_id, "�w��_R", 0.15f, "PlayerChip", "Haisui", "HP��35���ȉ��̏ꍇ�^����_���[�W��15���オ��}�e���A���B", spriteList[12]);
				Haisui01.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return Haisui01;
			case 10042:
				Item Haisui02 = new Item(_id, "�w��_R", 0.28f, "PlayerChip", "Haisui", "HP��35���ȉ��̏ꍇ�^����_���[�W��28���オ��}�e���A���B", spriteList[13]);
				Haisui02.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return Haisui02;
			case 10043:
				Item Haisui03 = new Item(_id, "�w��_R", 0.45f, "PlayerChip", "Haisui", "HP��35���ȉ��̏ꍇ�^����_���[�W��45���オ��}�e���A���B", spriteList[14]);
				Haisui03.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return Haisui03;

			// ��������E���G����
			case 10051:
				Item AvoidUp01 = new Item(_id, "�������_R", 0.08f, "PlayerChip", "AvoidUp", "���������8%�E���G���Ԃ�13%��������}�e���A���B", spriteList[15]);
				AvoidUp01.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return AvoidUp01;
			case 10052:
				Item AvoidUp02 = new Item(_id, "�������_SR", 0.12f, "PlayerChip", "AvoidUp", "���������12%�E���G���Ԃ�17%��������}�e���A���B", spriteList[16]);
				AvoidUp02.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return AvoidUp02;
			case 10053:
				Item AvoidUp03 = new Item(_id, "�������_SSR", 0.25f, "PlayerChip", "AvoidUp", "���������25%�E���G���Ԃ�30%��������}�e���A���B", spriteList[17]);
				AvoidUp03.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return AvoidUp03;

			// �N���e�B�J���m���E�_���[�W�A�b�v
			case 10061:
				Item CritUp01 = new Item(_id, "Critical_R", 0.05f, "PlayerChip", "Critical", "�N���e�B�J���m����5%�E�N���e�B�J���_���[�W��10%��������}�e���A���B", spriteList[18]);
				CritUp01.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return CritUp01;
			case 10062:
				Item CritUp02 = new Item(_id, "Critical_R", 0.1f, "PlayerChip", "Critical", "�N���e�B�J���m��10%�E�N���e�B�J���_���[�W��20%��������}�e���A���B", spriteList[19]);
				CritUp02.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return CritUp02;
			case 10063:
				Item CritUp03 = new Item(_id, "Critical_R", 0.2f, "PlayerChip", "Critical", "�N���e�B�J���m��20%�E�N���e�B�J���_���[�W��40%��������}�e���A���B", spriteList[20]);
				CritUp03.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return CritUp03;
		}

		return new Item("", "", 0.0f, "", "", "", null);
	}

}