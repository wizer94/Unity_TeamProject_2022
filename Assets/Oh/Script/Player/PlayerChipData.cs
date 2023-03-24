using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

public class PlayerChipData : MonoBehaviour
{ 
    public List<Sprite> spriteList = new List<Sprite>();

	// chipリスト
	public SlotItem ChipList(string _id)
	{
		switch (int.Parse(_id))
		{
			// HP UP
			case 10001:
				Item HP01 = new Item(_id, "最大HP_R", 50.0f, "PlayerChip", "HP", "最大HPを50増加するマテリアル。", spriteList[0]);
				HP01.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return HP01;
			case 10002:
				Item HP02 = new Item(_id, "最大HP_SR", 100.0f, "PlayerChip", "HP", "最大HPを100増加するマテリアル。", spriteList[1]);
				HP02.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Equip);
				return HP02;
			case 10003:
				Item HP03 = new Item(_id, "最大HP_SSR", 200.0f, "PlayerChip", "HP", "最大HPを200増加するマテリアル。", spriteList[2]);
				HP03.UseEvent += (item) => Debug.Log("Item: " + item.Name);
				return HP03;

			// SPEED
			case 10011:
				Item SPEED01 = new Item(_id, "SPEED UP_R", 0.5f, "PlayerChip", "SPEED", "移動速度を0.5増加するマテリアル。", spriteList[3]);
				SPEED01.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return SPEED01;
			case 10012:
				Item SPEED02 = new Item(_id, "SPEED UP_SR", 1.0f, "PlayerChip", "SPEED", "速度を1.0増加するマテリアル。", spriteList[4]);
				SPEED02.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return SPEED02;
			case 10013:
				Item SPEED03 = new Item(_id, "SPEED UP_SSR", 1.8f, "PlayerChip", "SPEED", "速度を1.8増加するマテリアル。", spriteList[5]);
				SPEED03.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return SPEED03;

			// DamageCut
			case 10021:
				Item DamageCut01 = new Item(_id, "Damage Cut_R", 0.13f, "PlayerChip", "DamageCut", "受けるダメージを13%軽減するマテリアル。", spriteList[6]);
				DamageCut01.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return DamageCut01;
			case 10022:
				Item DamageCut02 = new Item(_id, "Damage Cut_SR", 0.27f, "PlayerChip", "DamageCut", "受けるダメージを27%軽減するマテリアル。", spriteList[7]);
				DamageCut02.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return DamageCut02;
			case 10023:
				Item DamageCut03 = new Item(_id, "Damage Cut_SSR", 0.40f, "PlayerChip", "DamageCut", "受けるダメージを40%軽減するマテリアル。", spriteList[8]);
				DamageCut03.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return DamageCut03;

			// HighHP_DamageUp
			case 10031:
				Item Konshin01 = new Item(_id, "渾身_R", 0.1f, "PlayerChip", "Konshin", "HPが100％の場合与えるダメージが10％上がるマテリアル。", spriteList[9]);
				Konshin01.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return Konshin01;
			case 10032:
				Item Konshin02 = new Item(_id, "渾身_SR", 0.22f, "PlayerChip", "Konshin", "HPが100％の場合与えるダメージが22％上がるマテリアル。", spriteList[10]);
				Konshin02.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip + item.Tab.TabName);
				return Konshin02;
			case 10033:
				Item Konshin03 = new Item(_id, "渾身_SSR", 0.4f, "PlayerChip", "Konshin", "HPが100％の場合与えるダメージが40％上がるマテリアル。", spriteList[11]);
				Konshin03.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return Konshin03;

			// LessHP_DamageUP
			case 10041:
				Item Haisui01 = new Item(_id, "背水_R", 0.15f, "PlayerChip", "Haisui", "HPが35％以下の場合与えるダメージが15％上がるマテリアル。", spriteList[12]);
				Haisui01.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return Haisui01;
			case 10042:
				Item Haisui02 = new Item(_id, "背水_R", 0.28f, "PlayerChip", "Haisui", "HPが35％以下の場合与えるダメージが28％上がるマテリアル。", spriteList[13]);
				Haisui02.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return Haisui02;
			case 10043:
				Item Haisui03 = new Item(_id, "背水_R", 0.45f, "PlayerChip", "Haisui", "HPが35％以下の場合与えるダメージが45％上がるマテリアル。", spriteList[14]);
				Haisui03.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return Haisui03;

			// 回避距離・無敵時間
			case 10051:
				Item AvoidUp01 = new Item(_id, "回避効率_R", 0.08f, "PlayerChip", "AvoidUp", "回避距離が8%・無敵時間が13%増加するマテリアル。", spriteList[15]);
				AvoidUp01.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return AvoidUp01;
			case 10052:
				Item AvoidUp02 = new Item(_id, "回避効率_SR", 0.12f, "PlayerChip", "AvoidUp", "回避距離が12%・無敵時間が17%増加するマテリアル。", spriteList[16]);
				AvoidUp02.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return AvoidUp02;
			case 10053:
				Item AvoidUp03 = new Item(_id, "回避効率_SSR", 0.25f, "PlayerChip", "AvoidUp", "回避距離が25%・無敵時間が30%増加するマテリアル。", spriteList[17]);
				AvoidUp03.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return AvoidUp03;

			// クリティカル確率・ダメージアップ
			case 10061:
				Item CritUp01 = new Item(_id, "Critical_R", 0.05f, "PlayerChip", "Critical", "クリティカル確率が5%・クリティカルダメージが10%増加するマテリアル。", spriteList[18]);
				CritUp01.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return CritUp01;
			case 10062:
				Item CritUp02 = new Item(_id, "Critical_R", 0.1f, "PlayerChip", "Critical", "クリティカル確率10%・クリティカルダメージが20%増加するマテリアル。", spriteList[19]);
				CritUp02.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return CritUp02;
			case 10063:
				Item CritUp03 = new Item(_id, "Critical_R", 0.2f, "PlayerChip", "Critical", "クリティカル確率20%・クリティカルダメージが40%増加するマテリアル。", spriteList[20]);
				CritUp03.UseEvent += (item) => Debug.Log("Item: " + item.Name + " " + item.Stat + " " + item.Equip);
				return CritUp03;
		}

		return new Item("", "", 0.0f, "", "", "", null);
	}

}