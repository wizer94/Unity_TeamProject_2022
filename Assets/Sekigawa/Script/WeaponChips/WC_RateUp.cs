using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_RateUp : WeaponChip
{
	float value;

	void Awake() {
		Name = "�I�[�o�[�N���b�N";
	}

	protected override void SetupChip() {
		value = base.GetStdMultiply(4f, level);
		string str = "���˃��[�g��" + ConvertPercentMultiply(value) + "���܂��B";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "���˃��[�g��" + ConvertPercentMultiply(base.GetStdMultiply(4f, level)) + "���܂��B";
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
