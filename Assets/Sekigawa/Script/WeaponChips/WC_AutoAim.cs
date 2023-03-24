using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_AutoAim : WeaponChip {

	float value1, value2;

	void Awake() {
		Name = "タクティカルバイザー";
	}

	protected override void SetupChip() {
		value1 = 2 * base.GetStdMultiply(10f, level);
		value2 = base.GetStdMultiply(9f, level) / 10f;
		string str = "照準から" + value1.ToString("F1") + "m以内が自動的に捕捉されるようになります。";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "照準から" + (2 * base.GetStdMultiply(10f, level)).ToString("F1") + "m以内が自動的に捕捉されるようになります。";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.SetAutoAim(true);
		base.ws.autoAimRange = value1;
		//base.ws.moveSpeedMultiply_activate *= value2;
	}
	protected override void OnChipUnLoad() {
		base.ws.SetAutoAim(false);
		base.ws.autoAimRange = 0;
		//base.ws.moveSpeedMultiply_activate /= value2;
	}
}