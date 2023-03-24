using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_DamageUp : WeaponChip {

	float value;

	void Awake() {
		Name = "ビッグショット";
	}

	protected override void SetupChip() {
		value = base.GetStdMultiply(5f, level);
		string str = "弾のダメージが" + ConvertPercentMultiply(value) + "します。";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "弾のダメージが" + ConvertPercentMultiply(base.GetStdMultiply(5f, level)) + "します。";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.damage *= value;
	}

	protected override void OnChipUnLoad() {
		base.ws.damage /= value;
	}
}