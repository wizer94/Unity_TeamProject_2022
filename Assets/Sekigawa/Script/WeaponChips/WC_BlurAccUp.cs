using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_BlurAccUp : WeaponChip {

	float value1, value2;

	void Awake() {
		Name = "コンペンセイター";
	}

	protected override void SetupChip() {
		value1 = 1f / base.GetStdMultiply(8f, level);
		value2 = base.GetStdMultiply(5f, level);
		string str = "射撃時に加算されるブレが" + ConvertPercentMultiply(value1) + "し、" +
					 "非射撃時にブレが回復する速度が" + ConvertPercentMultiply(value2) + "します。";
		base.SetDescription(str);
	}


	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "射撃時に加算されるブレが" + ConvertPercentMultiply(1f / base.GetStdMultiply(8f, level)) + "し、" +
					 "非射撃時にブレが回復する速度が" + ConvertPercentMultiply(base.GetStdMultiply(5f, level)) + "します。";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.blurAccRate *= value1;
		base.ws.blurRechargeRate *= value2;
		
	}

	protected override void OnChipUnLoad() {
		base.ws.blurAccRate /= value1;
		base.ws.blurRechargeRate /= value2;
	}
}