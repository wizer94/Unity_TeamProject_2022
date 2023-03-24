using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_RangeUp : WeaponChip {


	float value1, value2;

	void Awake() {
		Name = "ロングバレル";
	}

	protected override void SetupChip() {
		value1 = base.GetStdMultiply(20f, level);
		value2 = base.GetStdMultiply(3f, level);
		string str = "弾の飛ぶ距離が" + value1.ToString("F1") + "m増加し、" +
					 "速度が" + ConvertPercentMultiply(value2) + "します。";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "弾の飛ぶ距離が" + base.GetStdMultiply(20f, level).ToString("F1") + "m増加し、" +
					 "速度が" + ConvertPercentMultiply(base.GetStdMultiply(3f, level)) + "します。";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.maxRange += value1;
		base.ws.speed *= value2;

	}

	protected override void OnChipUnLoad() {
		base.ws.maxRange -= value1;
		base.ws.speed /= value2;

	}
}