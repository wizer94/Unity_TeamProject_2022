using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_Mist : WeaponChip
{

	float value1, value2;
	int value3;
	float bfrCount, damageMultiply;

	void Awake() {
		Name = "鉛の霧";
	}

	protected override void SetupChip() {
		value1 = base.GetStdMultiply(3f, level);
		value2 = 1f / base.GetStdMultiply(10f, level);
		value3 = level / 5;
		string str = "弾の発射レートが" + ConvertPercentMultiply(value1);
		if (value3 > 0)
			str += "し、弾が追加で" + value3 + "個発射";
		str += "しますが、速度が" + ConvertPercentMultiply(value2) + "します。";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "弾の発射レートが" + ConvertPercentMultiply(base.GetStdMultiply(3f, level));
		if (level >= 5)
			str += "し、弾が追加で" + (level / 5) + "個発射";
		str += "しますが、速度が" + ConvertPercentMultiply(1f / base.GetStdMultiply(10f, level)) + "します。";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.perShot *= value1;
		base.ws.speed *= value2;


		bfrCount = base.ws.count;
		base.ws.count += value3;
		damageMultiply = bfrCount / base.ws.count;
		base.ws.damage *= damageMultiply;

	}

	protected override void OnChipUnLoad() {
		base.ws.perShot /= value1;
		base.ws.speed /= value2;

		base.ws.count -= value3;
		base.ws.damage /= damageMultiply;
	}

}
