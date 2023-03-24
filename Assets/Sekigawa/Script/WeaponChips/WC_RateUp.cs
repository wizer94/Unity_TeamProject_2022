using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_RateUp : WeaponChip
{
	float value;

	void Awake() {
		Name = "オーバークロック";
	}

	protected override void SetupChip() {
		value = base.GetStdMultiply(4f, level);
		string str = "発射レートが" + ConvertPercentMultiply(value) + "します。";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "発射レートが" + ConvertPercentMultiply(base.GetStdMultiply(4f, level)) + "します。";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.perShot *= value;
	}
	protected override void OnChipUnLoad() {
		base.ws.perShot /= value;
	}

}
