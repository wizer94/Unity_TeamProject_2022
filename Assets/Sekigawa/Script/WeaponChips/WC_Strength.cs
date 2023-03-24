using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_Strength : WeaponChip {

	float value1, value2;

	void Awake() {
		Name = "対物化";
	}


	protected override void SetupChip() {

		value1 = base.GetStdMultiply(21f, level);
		value2 = 1f / base.GetStdMultiply(8f, level);
		string str = "弾のダメージが" + ConvertPercentMultiply(value1) + "しますが、" +
					 "発射レートが" + ConvertPercentMultiply(value2) + "します。";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "弾のダメージが" + ConvertPercentMultiply(base.GetStdMultiply(21f, level)) + "しますが、" +
					 "発射レートが" + ConvertPercentMultiply(1f / base.GetStdMultiply(8f, level)) + "します。";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.damage *= value1;
		base.ws.perShot *= value2;

	}

	protected override void OnChipUnLoad() {
		base.ws.damage /= value1;
		base.ws.perShot /= value2;

	}
}
