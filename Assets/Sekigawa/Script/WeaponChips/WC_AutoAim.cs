using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_AutoAim : WeaponChip {

	float value1, value2;

	void Awake() {
		Name = "�^�N�e�B�J���o�C�U�[";
	}

	protected override void SetupChip() {
		value1 = 2 * base.GetStdMultiply(10f, level);
		value2 = base.GetStdMultiply(9f, level) / 10f;
		string str = "�Ə�����" + value1.ToString("F1") + "m�ȓ��������I�ɕߑ������悤�ɂȂ�܂��B";
		base.SetDescription(str);
	}

	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�Ə�����" + (2 * base.GetStdMultiply(10f, level)).ToString("F1") + "m�ȓ��������I�ɕߑ������悤�ɂȂ�܂��B";
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