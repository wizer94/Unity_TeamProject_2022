using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_ContinuityHit : WeaponChip {

	float value1, value2;
	int missable;

	void Awake() {
		Name = "アンストッパブル";
	}

	protected override void SetupChip() {
		value1 = base.GetStdMultiply(2f, 10f, level, 25);
		value2 = base.GetStdMultiply(1.25f, 2f, level, 25);
		string str = "連続ヒットごとに銃が強化され、" +
					 "最大でダメージが" + ConvertPercentMultiply(value1) + "、" +
					 "発射レートが" + ConvertPercentMultiply(value2) + "します。";
		missable = level / 5;
		if (missable >= 1)
			str += "\n" + missable + "回外しても効果が継続されます。";
		base.SetDescription(str);
	}


	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "連続ヒットごとに銃が強化され、" +
					 "最大でダメージが" + ConvertPercentMultiply(base.GetStdMultiply(2f, 10f, level, 25)) + "、" +
					 "発射レートが" + ConvertPercentMultiply(base.GetStdMultiply(1.25f, 2f, level, 25)) + "します。";
		if (level / 5 >= 1)
			str += "\n" + level / 5 + "回外しても効果が継続されます。";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.continuityDamage = value1;
		base.ws.continuityPerShot = value2;
		base.ws.continuityMissable = missable;

	}

	protected override void OnChipUnLoad() {
		base.ws.continuityDamage = 1f;
		base.ws.continuityPerShot = 1f;
		base.ws.continuityMissable = 0;
	}
}
