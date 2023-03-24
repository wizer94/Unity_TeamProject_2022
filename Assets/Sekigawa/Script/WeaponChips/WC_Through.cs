using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_Through : WeaponChip
{

	float value;

	void Awake() {
		Name = "シャープバレット";
	}

	protected override void SetupChip() {
		if (level <= 5)
			value = base.GetStdMultiply(1f, 2f, level, 5) / 2f;
		else
			value = base.GetStdMultiply(1f, 3f, level - 5, 20);

		string str = "弾が敵を貫通するようになり";
		if (value != 1) {
			str += value < 1 ? "ますが、" : "、";
			str += "貫通のたびにダメージが" + ConvertPercentMultiply(value) + "します。";
		}
		else {
			str += "ます。";
		}
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "";
		if (level <= 5) {
			str = "弾が敵を貫通するようになり";
			if (level != 5) {
				str += "ますが、貫通のたびにダメージが" + ConvertPercentMultiply(base.GetStdMultiply(1f, 2f, level, 5) / 2f) + "します。";
			}
			else {
				str += "ます。";
			}
		}
		else {
			str = "弾が敵を貫通するようになり、貫通のたびにダメージが" + base.GetStdMultiply(1f, 3f, level - 5, 20) + "します。";
		}
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.through = value;

	}

	protected override void OnChipUnLoad() {
		base.ws.through = 0;

	}
}
