using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WC_Critical : WeaponChip {

	float value1, value2;

	void Awake() {
		Name = "�N���e�B�J���C�Y";
	}

	protected override void SetupChip() {
		value1 = base.GetStdMultiply(18f, level);
		value2 = base.GetStdMultiply(3f, level);
		string str = "�N���e�B�J�����������ʏ��" + value1.ToString("F1") + "�{�ɂȂ�A" +
					 "�N���e�B�J�����̃_���[�W��" + ConvertPercentMultiply(value2) + "���܂��B";
		base.SetDescription(str);
	}
	protected override string GetDescription(int level) {
		string rtv = "";
		string str = "�N���e�B�J�����������ʏ��" + base.GetStdMultiply(18f, level).ToString("F1") + "�{�ɂȂ�A" +
					 "�N���e�B�J�����̃_���[�W��" + ConvertPercentMultiply(base.GetStdMultiply(3f, level)) + "���܂��B";
		rtv += str;
		return rtv;
	}

	protected override void OnChipLoad() {
		SetupChip();

		base.ws.crit_rate *= value1;
		base.ws.crit_multiply *= value2;
	}

	protected override void OnChipUnLoad() {
		base.ws.crit_rate /= value1;
		base.ws.crit_multiply /= value2;
	}
}