using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_Homing : WeaponChip {

	float value1, value2;

	void Awake() {
		Name = "グラビトンライン";
	}

	protected override void SetupChip() {
		value1 = level / 25f * 0.4f + 1f;
		value2 = level / 25f * 5f + 1f;
		string str = "ホーミング性能を" + ((value1 - 1f)*100).ToString("F0") + "%付与し、" +
					 "弾の飛ぶ距離が" + ConvertPercentMultiply(value2) + "します。";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "ホーミング性能を" + ((level / 25f * 0.4f) * 100).ToString("F0") + "%付与し、" +
					 "弾の飛ぶ距離が" + ConvertPercentMultiply(level / 25f * 5f + 1f) + "します。";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.homing_Acc = value1 - 1f;
		base.ws.maxRange += value2 - 1f;

	}

	protected override void OnChipUnLoad() {
		base.ws.homing_Acc = 0;
		base.ws.maxRange -= value2 - 1f;

	}
}
