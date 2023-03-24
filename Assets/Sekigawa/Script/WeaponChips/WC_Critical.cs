using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_Critical : WeaponChip {

	float value1, value2;

	void Awake() {
		Name = "クリティカライズ";
	}

	protected override void SetupChip() {
		value1 = base.GetStdMultiply(18f, level);
		value2 = base.GetStdMultiply(3f, level);
		string str = "クリティカル発生率が通常の" + value1.ToString("F1") + "倍になり、" +
					 "クリティカル時のダメージが" + ConvertPercentMultiply(value2) + "します。";
		base.SetDescription(str);
	}
	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "クリティカル発生率が通常の" + base.GetStdMultiply(18f, level).ToString("F1") + "倍になり、" +
					 "クリティカル時のダメージが" + ConvertPercentMultiply(base.GetStdMultiply(3f, level)) + "します。";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.crit_rate *= value1;
		base.ws.crit_multiply *= value2;
	}

	protected override void OnChipUnLoad() {
		base.ws.crit_rate /= value1;
		base.ws.crit_multiply /= value2;
	}
}