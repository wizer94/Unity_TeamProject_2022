using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_Fire : WeaponChip {

	int value1, value2;

	void Awake() {
		Name = "メルトダウン";
	}

	protected override void SetupChip() {
		value1 = level / 5 + 1;
		value2 = level % 5 + 1 + value1;

		float sum = 40f * value1 * value1 * value2;

		string str = "敵に当たると発火し、" + value2 + "秒間にわたって継続ダメージ、\n" +
					 "合計で約" + sum + "ダメージを与えます。";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "敵に当たると発火し、" + (level % 5 + 1 + value1) + "秒間にわたって継続ダメージ、\n" +
					 "合計で約" + (40f * (level / 5 + 1) * (level / 5 + 1) * (level % 5 + 1 + (level / 5 + 1))) + "ダメージを与えます。";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.fireLevel = value1;
		base.ws.fireTime = value2;
	}
	protected override void OnChipUnLoad() {
		base.ws.fireLevel = 0;
		base.ws.fireTime = 0;
	}
}