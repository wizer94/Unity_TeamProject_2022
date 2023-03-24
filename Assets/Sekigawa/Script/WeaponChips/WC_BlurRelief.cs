using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_BlurRelief : WeaponChip {

	float valueMin, valueMax;

	void Awake() {
		Name = "スタビライザー";
	}

	protected override void SetupChip() {
		valueMin = 1f / base.GetStdMultiply(10f, level);
		valueMax = 1f / base.GetStdMultiply(5f, level);
		string str = "射撃時のブレが" + ConvertPercentMultiply(valueMax) + "します。";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "射撃時のブレが" + ConvertPercentMultiply(1f / base.GetStdMultiply(5f, level)) + "します。";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.minBlur *= valueMin;
		base.ws.maxBlur *= valueMax;
		base.ws.cohesive += level / 5;
		
	}

	protected override void OnChipUnLoad() {

		base.ws.minBlur /= valueMin;
		base.ws.maxBlur /= valueMax;
		base.ws.cohesive -= level / 5;

	}
}