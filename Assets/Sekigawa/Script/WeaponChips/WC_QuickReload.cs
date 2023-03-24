using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_QuickReload : WeaponChip {

	float value;

	void Awake() {
		Name = "デフトハンド";
	}

	protected override void SetupChip() {
		value = base.GetStdMultiply(8, level);
		string str = "リロード速度が" + ConvertPercentMultiply(value) + "します。";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "リロード速度が" + ConvertPercentMultiply(base.GetStdMultiply(8, level)) + "します。";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.reloadTime /= value;

	}
	protected override void OnChipUnLoad() {
		base.ws.reloadTime *= value;

	}

}
