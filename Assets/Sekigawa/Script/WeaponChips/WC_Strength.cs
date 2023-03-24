using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_Strength : WeaponChip {

	float value1, value2;

	void Awake() {
		Name = "�Ε���";
	}


	protected override void SetupChip() {

		value1 = base.GetStdMultiply(21f, level);
		value2 = 1f / base.GetStdMultiply(8f, level);
		string str = "�e�̃_���[�W��" + ConvertPercentMultiply(value1) + "���܂����A" +
					 "���˃��[�g��" + ConvertPercentMultiply(value2) + "���܂��B";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�e�̃_���[�W��" + ConvertPercentMultiply(base.GetStdMultiply(21f, level)) + "���܂����A" +
					 "���˃��[�g��" + ConvertPercentMultiply(1f / base.GetStdMultiply(8f, level)) + "���܂��B";
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
