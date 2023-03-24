using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_DamageUp : WeaponChip {

	float value;

	void Awake() {
		Name = "�r�b�O�V���b�g";
	}

	protected override void SetupChip() {
		value = base.GetStdMultiply(5f, level);
		string str = "�e�̃_���[�W��" + ConvertPercentMultiply(value) + "���܂��B";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�e�̃_���[�W��" + ConvertPercentMultiply(base.GetStdMultiply(5f, level)) + "���܂��B";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.damage *= value;
	}

	protected override void OnChipUnLoad() {
		base.ws.damage /= value;
	}
}